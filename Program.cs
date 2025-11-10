using AngelArbApp;
using AngelArbApp.Models;
using AngelBroking;
using AngelOne.SmartApi.Clients;
using OtpNet;
using OtpNet;
using System.Text;
//using AngelOne.SmartApi.Models;

namespace SmartApiLoginDemo
{
    class Program
    {
        static async Task Main(string[] args)
        {
            
            var LoginResp = await HelperClass.LoginToAngelAsync();

            if (LoginResp.status != true)
            {
                Console.WriteLine("Login Failed");
                return;
                throw new Exception("Login Failed");
            }
            Console.WriteLine("Login Successful");
            ////var TokenResp = await HelperClass.GenerateTakenAsync(LoginResp.data.refreshToken);
            //var token = "eyJhbGciOiJIUzUxMiJ9.eyJ1c2VybmFtZSI6IlNDQ1QxMDg1Iiwicm9sZXMiOjAsInVzZXJ0eXBlIjoiVVNFUiIsInRva2VuIjoiZXlKaGJHY2lPaUpTVXpJMU5pSXNJblI1Y0NJNklrcFhWQ0o5LmV5SjFjMlZ5WDNSNWNHVWlPaUpqYkdsbGJuUWlMQ0owYjJ0bGJsOTBlWEJsSWpvaWRISmhaR1ZmWVdOalpYTnpYM1J2YTJWdUlpd2laMjFmYVdRaU9qUXNJbk52ZFhKalpTSTZJak1pTENKa1pYWnBZMlZmYVdRaU9pSTJZVEV4TURGbU5TMDBaV1poTFRNME5Ea3RZbUpoTkMwd1pUVTVZV1V5T0dWbE9Ua2lMQ0pyYVdRaU9pSjBjbUZrWlY5clpYbGZkaklpTENKdmJXNWxiV0Z1WVdkbGNtbGtJam8wTENKd2NtOWtkV04wY3lJNmV5SmtaVzFoZENJNmV5SnpkR0YwZFhNaU9pSmhZM1JwZG1VaWZTd2liV1lpT25zaWMzUmhkSFZ6SWpvaVlXTjBhWFpsSW4wc0ltNWlkVXhsYm1ScGJtY2lPbnNpYzNSaGRIVnpJam9pWVdOMGFYWmxJbjE5TENKcGMzTWlPaUowY21Ga1pWOXNiMmRwYmw5elpYSjJhV05sSWl3aWMzVmlJam9pVTBORFZERXdPRFVpTENKbGVIQWlPakUzTmpFME56WXdPVGtzSW01aVppSTZNVGMyTVRNNE9UVXhPU3dpYVdGMElqb3hOell4TXpnNU5URTVMQ0pxZEdraU9pSXpPREl6WkRFeFpDMHdZMlJoTFRRd09XTXRPVE5sTWkwek1HVmpaalppT0RabFkyWWlMQ0pVYjJ0bGJpSTZJaUo5LngtUHozOExZOWxhNjFHR0tWSkdyT1k3c1FTbV9iLUx2ck9XSWlyVUxYVXZYQVB6VlRaWnI3ZU5PQkowejhxalQtaGhPd0k3Y0pOSzFKVklyTHg1VWQwejRsNlc4LU5zVl95cG56NHQtbE9DZE1CV00wY0NDUjBsWU1OcUpkV3V1bGxveWZVVzFDSk1naE1MbjJqTTlKbUQ2T2dtY3J0U29sLW5nY2dRaGd3OCIsIkFQSS1LRVkiOiJiWDY0bllaeCIsIlgtT0xELUFQSS1LRVkiOnRydWUsImlhdCI6MTc2MTM4OTY5OSwiZXhwIjoxNzYxNDE3MDAwfQ.1igsE11P8ajEODJJ_aJGkXGiIKqwkrJ8ROKcMNWfZEcYRuINZLNxHT1RG3iV6l39itMXS2P95vMhuINQ2hGGLQ";
            //var pp = await HelperClass.GetMarketData(token, "634", "NSE");
            var listOfAll = Utilities.GetNSEFutList();
            await FutDiffCalulator.GetAndDisplayForEachStk(listOfAll, LoginResp.data.jwtToken);
        }

       
    }
}
