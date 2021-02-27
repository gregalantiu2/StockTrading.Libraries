using Alpaca.Markets;
using StockTrading.Libraries.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockTrading.Libraries
{
    public class AlpacaMarketData : IMarketData
    {
        private IAlpacaTradingClient _alpacaTradingClient;
        private IAlpacaDataClient _alpacaDataClient;
        public AlpacaMarketData(string apikey, string secretKey)
        {
            _alpacaTradingClient = Alpaca.Markets.Environments.Paper.GetAlpacaTradingClient(new SecretKey(apikey, secretKey));
            _alpacaDataClient = Environments.Paper.GetAlpacaDataClient(new SecretKey(apikey, secretKey));
        }

        //Checks to see if market is open and when the next close time is 
        public async Task<Tuple<bool, DateTime>> MarketTimesGet()
        {
            var clock = await _alpacaTradingClient.GetClockAsync();

            return Tuple.Create(clock.IsOpen, clock.NextCloseUtc);
        }
        //Takes a list of stock symbols and gets their current price
        public async Task<Dictionary<string, decimal>> GetStonkPriceChange(List<string> symbols)
        {
            Dictionary<string, decimal> stonkPriceChanges = new Dictionary<string, decimal>();

            foreach (var symbol in symbols)
            {
                var bars = await _alpacaDataClient.GetBarSetAsync(new BarSetRequest(symbol, TimeFrame.FifteenMinutes) { Limit = 1 });

                var startPrice = bars[symbol].First().Open;
                var endPrice = bars[symbol].Last().Close;

                var priceChange = endPrice - startPrice;

                stonkPriceChanges.Add(symbol, priceChange);
            }

            return stonkPriceChanges;
        }
    }
}
