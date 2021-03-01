using StockTrading.Libraries.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

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
        public async void RunDayTradingProgram(List<string> symbols)
        {
            var response = await _marketData.MarketTimesGet();

            if(response.Item1)
            {
                var now = DateTime.Now;

                while(now < response.Item2)
                {
                    await _algorithm.ExecuteAlgo(symbols);
                    now = DateTime.Now;
                }
            }
        }
    }
}
