using Alpaca.Markets;
using StockTrading.Libraries.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace StockTrading.Libraries.Programs
{
    public class AlpacaProgram
    {
        private IAlgorithm _algorithm;
        private IMarketData _marketData;
        public AlpacaProgram(IAlgorithm algorithm, IMarketData marketData)
        {
            _algorithm = algorithm;
            _marketData = marketData;
        }
        public async IAsyncEnumerable<Dictionary<string,string>> RunDayTradingProgram(List<string> symbols)
        {
            var response = await _marketData.MarketTimesGet();

            if(response.Item1)
            {
                var now = DateTime.Now;

                while(now < response.Item2)
                {
                    yield return await _algorithm.ExecuteAlgo(symbols);
                    now = DateTime.Now;
                    System.Threading.Thread.Sleep(6000);
                }
            }
        }
        public async Task<Dictionary<string, string>> RunDayTradingProgramTEST(List<string> symbols)
        {
            var response = await _marketData.MarketTimesGet();

            if (response.Item1)
            {
                var now = DateTime.Now;

                return await _algorithm.ExecuteAlgoTEST(symbols);
            }

            return new Dictionary<string, string>();
        }
    }
}
