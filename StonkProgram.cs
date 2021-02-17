using Alpaca.Markets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace StockTrading.Libraries
{
    public class StonkProgram
    {
        private static String ApiKey { get; set; }
        private static String SecretKey { get; set; }
        private IAlpacaTradingClient alpacaTradingClient;
        private IAlpacaDataClient alpacaDataClient;
        public Decimal buyingPower { get; set; }
        public Decimal portfolioValue { get; set; }
        public StonkProgram(string apikey, string secretKey)
        {
            ApiKey = apikey;
            SecretKey = secretKey;
            alpacaTradingClient = Alpaca.Markets.Environments.Paper.GetAlpacaTradingClient(new SecretKey(ApiKey, SecretKey));
            alpacaDataClient = Environments.Paper.GetAlpacaDataClient(new SecretKey(ApiKey, SecretKey));
        }

        public async Task RunDayTradingProgram(List<string> symbols)
        {
            Dictionary<string, string> transactions = new Dictionary<string, string>();

            Dictionary<string, decimal> priceChanges = new Dictionary<string, decimal>();

            priceChanges = await GetStonkPriceChange(symbols);

            foreach(var stonk in priceChanges)
            {
                if(stonk.Value < -3)
                {
                    try
                    {
                        var order = await alpacaTradingClient.PostOrderAsync(TrailingStopOrder.Buy(stonk.Key, 25, TrailOffset.InDollars(0.80M)));
                        transactions.Add("Buy", "Bought " + stonk.Key + " at " + stonk.Value.ToString());
                    }
                    catch(HttpRequestException e)
                    {
                        transactions.Add("Error", e.Message);
                    }
                }
                else if(stonk.Value > 1)
                {
                    try
                    {
                        var order = await alpacaTradingClient.PostOrderAsync(TrailingStopOrder.Sell(stonk.Key, 25, TrailOffset.InDollars(0.80M)));
                        transactions.Add("Trailing Sell Posted", "Trailing stop order posted for " + order.Symbol + "trailing at "  + order.TrailOffsetInDollars.ToString());
                    }
                    catch (HttpRequestException e)
                    {
                        transactions.Add("Error", e.Message);
                    }
                }
                else
                {
                    continue;
                }
            }

        }
        public async Task<Tuple<bool,DateTime>> MarketTimesGet()
        {
            var clock = await alpacaTradingClient.GetClockAsync();

            return Tuple.Create(clock.IsOpen, clock.NextCloseUtc);
        }
        public async Task<Dictionary<string, decimal>> GetStonkPriceChange(List<string> symbols)
        {
            Dictionary<string, decimal> stonkPriceChanges = new Dictionary<string, decimal>();

            foreach(var symbol in symbols)
            {
                var bars = await alpacaDataClient.GetBarSetAsync(new BarSetRequest(symbol, TimeFrame.FifteenMinutes) {Limit = 1 });
                
                var startPrice = bars[symbol].First().Open;
                var endPrice = bars[symbol].Last().Close;

                var priceChange = endPrice - startPrice;

                stonkPriceChanges.Add(symbol, priceChange);
            }

            return stonkPriceChanges;
        }
    }
}
