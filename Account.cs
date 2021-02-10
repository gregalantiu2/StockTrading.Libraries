using Alpaca.Markets;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace StockTrading.Libraries
{
    public class Account
    {
        private static String ApiKey { get; set; }
        private static String SecretKey { get; set; }
        private IAlpacaTradingClient alpacaTradingClient;
        private IAlpacaDataClient alpacaDataClient;
        public Decimal buyingPower { get; set; }
        public Decimal portfolioValue { get; set; }
        public Account(string apikey, string secretKey)
        {
            ApiKey = apikey;
            SecretKey = secretKey;
        }

        public async Task Run()
        {
            alpacaTradingClient = Environments.Paper.GetAlpacaTradingClient(new SecretKey(ApiKey, SecretKey));

            alpacaDataClient = Environments.Paper.GetAlpacaDataClient(new SecretKey(ApiKey, SecretKey));

            // Get information about current account value.
            while (true)
            {
                var account = await alpacaTradingClient.GetAccountAsync();
                buyingPower = account.BuyingPower;
                portfolioValue = account.Equity;
            }

        }
    }
}
