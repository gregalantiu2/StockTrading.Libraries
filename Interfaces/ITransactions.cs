using Alpaca.Markets;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace StockTrading.Libraries.Interfaces
{
    public interface ITransactions
    {
        public Task<IOrder> TrailOrderBuy(string stonkSymbol, int numberOfShares, decimal trailAmount);
        public Task<IOrder> TrailOrderSell(string stonkSymbol, int numberOfShares, decimal trailAmount);
    }
}
