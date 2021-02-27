using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace StockTrading.Libraries.Interfaces
{
    public interface IMarketData
    {
        public Task<Tuple<bool, DateTime>> MarketTimesGet();
        public Task<Dictionary<string, decimal>> GetStonkPriceChange(List<string> symbols);
    }
}
