using Alpaca.Markets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace StockTrading.Libraries
{
    public class AlpacaAlgorithm1
    {
        private IAlpacaTradingClient alpacaTradingClient;
        public AlpacaAlgorithm1(string apikey, string secretKey)
        {
            alpacaTradingClient = Alpaca.Markets.Environments.Paper.GetAlpacaTradingClient(new SecretKey(apikey, secretKey));
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
    }
}
