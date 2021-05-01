using Alpaca.Markets;
using StockTrading.Libraries.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace StockTrading.Libraries
{
    public class Algorithm1 : IAlgorithm
    {
        private IMarketData _marketData;
        private ITransactions _transactions;
        public Algorithm1(IMarketData marketData,ITransactions transactions)
        {
            _marketData = marketData;
            _transactions = transactions;
        }
        public async Task<Dictionary<string, string>> ExecuteAlgo(List<string> symbols, int trailAmount, int upswingAmount)
        {
            Dictionary<string, string> transactions = new Dictionary<string, string>();

            Dictionary<string, decimal> priceChanges = new Dictionary<string, decimal>();

            priceChanges = await _marketData.GetStonkPriceChange(symbols);

            foreach(var stonk in priceChanges)
            {
                if(stonk.Value < (trailAmount < 0 ? trailAmount : trailAmount * -1))
                {
                    try
                    {
                        var order = await _transactions.TrailOrderBuy(stonk.Key, 25, 0.80M);
                        transactions.Add("Buy", "Bought " + stonk.Key + " at " + stonk.Value.ToString());
                    }
                    catch(HttpRequestException e)
                    {
                        transactions.Add("Error", e.Message);
                    }
                }
                else if(stonk.Value > upswingAmount)
                {
                    try
                    {
                        var order = await _transactions.TrailOrderSell(stonk.Key, 25, 0.80M);
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
            return transactions;
        }
        public async Task<Dictionary<string, string>> ExecuteAlgoTEST(List<string> symbols)
        {
            Dictionary<string, string> transactions = new Dictionary<string, string>();

            Dictionary<string, decimal> priceChanges = new Dictionary<string, decimal>();

            priceChanges = await _marketData.GetStonkPriceChange(symbols);

            foreach (var stonk in priceChanges)
            {
                var order = await _transactions.TrailOrderBuy(stonk.Key, 25, 0.80M);
            }
            return transactions;
        }
    }
}
