using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace StockTrading.Libraries.Interfaces
{
    public interface IAlgorithm
    {
        public Task<Dictionary<string, string>> ExecuteAlgo(List<string> symbols);
    }
}
