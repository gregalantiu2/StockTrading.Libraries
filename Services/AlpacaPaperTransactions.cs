using Alpaca.Markets;
using StockTrading.Libraries.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace StockTrading.Libraries.Services
{
    public class AlpacaPaperTransactions : ITransactions
    {
        private IAlpacaTradingClient _alpacaTradingClient;
        public AlpacaPaperTransactions(string apikey, string secretKey)
        {
            _alpacaTradingClient = Alpaca.Markets.Environments.Paper.GetAlpacaTradingClient(new SecretKey(apikey, secretKey));
        }
        public Task<IOrder> TrailOrderBuy(string stonkSymbol,int numberOfShares,decimal trailAmount)
        {
            return _alpacaTradingClient.PostOrderAsync(TrailingStopOrder.Buy(stonkSymbol, numberOfShares, TrailOffset.InDollars(trailAmount)));
        }
        public Task<IOrder> TrailOrderSell(string stonkSymbol, int numberOfShares, decimal trailAmount)
        {
            return _alpacaTradingClient.PostOrderAsync(TrailingStopOrder.Sell(stonkSymbol, numberOfShares, TrailOffset.InDollars(trailAmount)));
        }
    }
}
