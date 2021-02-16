using Alpaca.Markets;
using System;
using System.Collections.Generic;
using System.Linq;
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
            //Dictionary<string, decimal> transactions = new Dictionary<string, decimal>();

            //transactions = await GetStonkPriceChange(symbols);

            //foreach(var )


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
