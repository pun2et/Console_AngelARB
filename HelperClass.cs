using AngelArbApp.Models;
using AngelOne.SmartApi.Clients.Requests;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AngelArbApp.Models;

namespace AngelArbApp
{
    public static class HelperClass
    {
        private static string APIKey = "";
        private static string APISecret = "";
        private static string redirectUri = "";
        public static async Task<LoginResponse> LoginToAngelAsync()
        {
            var settingFileread = System.IO.File.ReadAllText("Data\\Settings.Json").ToString();
            var settingData = JsonConvert.DeserializeObject<SettingsModel>(settingFileread);

            Console.WriteLine("Starting Login Process...");
            Console.WriteLine("Checking Existing Token...");
            Console.WriteLine("ClientCode : " + settingData.ClientCode);
            Console.WriteLine("CurrentMonth : " + settingData.CurrentMonth);
            Console.WriteLine("NextMonth : " + settingData.NextMonth);
            Console.WriteLine("Max Execution Cost : " + settingData.Spread_LessThan);
            


            var tokenstate = HelperClass.CheckToken();
            var readtotp = true;
            if (tokenstate == true)
            {
                var token = System.IO.File.ReadAllText("Data\\AccesTokenData.Json").ToString();
                var tokendata = JsonConvert.DeserializeObject<LoginResponse>(token.ToString());
                return tokendata;
            }
            
            
           


            while (readtotp)
            {
                Console.WriteLine("Enter New TOTP:");
                string newtotp = Console.ReadLine();
                int totpcode = Convert.ToInt32(newtotp);
                var totplght = newtotp.Length;
                if (totplght == 6)
                {
                    readtotp = false;
                    settingData.TOTPCode = totpcode.ToString();
                }
                else
                {
                    Console.WriteLine("Invalid TOTP, Please enter 6 digit TOTP");
                }
            }           

           

            LoginReq loginRequest = new LoginReq();
            loginRequest.clientcode = settingData.ClientCode;
            loginRequest.password = settingData.ClientPIN;
            loginRequest.totp = settingData.TOTPCode;

            var req = JsonConvert.SerializeObject(loginRequest);

            var client = new RestClient("https://apiconnect.angelone.in/rest/auth/angelbroking/user/v1/loginByPassword");
            var request = new RestRequest("", Method.Post);

            request.AddBody(req);

            request.AddHeader("Accept", "application/json");
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("X-UserType", "USER");
            request.AddHeader("X-SourceID", "WEB");
            request.AddHeader("X-PrivateKey", settingData.APIKey);
            request.AddHeader("X-ClientLocalIP", "");
            request.AddHeader("X-ClientPublicIP", "");
            request.AddHeader("X-MACAddress", "");
            request.AddHeader("Accept-Encoding", "gzip, deflate, br");

            var response = await client.ExecuteAsync(request);

            if (response.IsSuccessful)
            {
                Console.WriteLine("Access Token Response:\n" + response.Content);
            }
            else
            {
                Console.WriteLine($"Error ({response.StatusCode}):\n{response.Content}");
            }
            var tokenResponse = new LoginResponse();
            try
            {
                tokenResponse = System.Text.Json.JsonSerializer.Deserialize<LoginResponse>(response.Content);
                tokenResponse.token_reciviewed_at = DateTime.Now;
                if (tokenResponse != null)
                {
                    Console.WriteLine($"Access Token: {tokenResponse.data.jwtToken}");

                    System.IO.File.WriteAllText("Data\\AccesTokenData.Json", JsonConvert.SerializeObject(tokenResponse));

                }
                else
                {
                    Console.WriteLine("Failed to deserialize response.");
                }
                return JsonConvert.DeserializeObject<LoginResponse>(response.Content);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error deserializing response: " + ex.Message);

            }

            return JsonConvert.DeserializeObject<LoginResponse>(response.Content);

        }

        public static async Task<TokenResp> GenerateTakenAsync(string refreshtoken)
        {
            var settingFileread = System.IO.File.ReadAllText("Data\\Settings.Json").ToString();
            var settingData = JsonConvert.DeserializeObject<SettingsModel>(settingFileread);

            TokenRequest tokenRequest = new TokenRequest();
            tokenRequest.refreshToken = refreshtoken;

            var req = JsonConvert.SerializeObject(tokenRequest);


            var client = new RestClient("https://apiconnect.angelone.in/rest/auth/angelbroking/jwt/v1/generateTokens");
            var request = new RestRequest("", Method.Post);

            request.AddBody(req);

            request.AddHeader("Accept", "application/json");
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("X-UserType", "USER");
            request.AddHeader("X-SourceID", "WEB");
            request.AddHeader("X-PrivateKey", settingData.APIKey);
            request.AddHeader("X-ClientLocalIP", "");
            request.AddHeader("X-ClientPublicIP", "");
            request.AddHeader("X-MACAddress", "");

            var response = await client.ExecuteAsync(request);

            return JsonConvert.DeserializeObject<TokenResp>(response.Content);
        }

        public static async Task<InstrumentResp> GetMarketData(string jwtToken, string stkToken, string exchange = "NFO")
        {
            var settingFileread = System.IO.File.ReadAllText("Data\\Settings.Json").ToString();
            var settingData = JsonConvert.DeserializeObject<SettingsModel>(settingFileread);
            var req = "";
            if (exchange == "NFO")
            {
                NFODataReq marketDataReq = new NFODataReq();
                AngelArbApp.Exchangetokens exchangetokens = new AngelArbApp.Exchangetokens();
                marketDataReq.mode = "FULL";
                exchangetokens.NFO = new string[] { stkToken };
                marketDataReq.exchangeTokens = exchangetokens;
                req = JsonConvert.SerializeObject(marketDataReq).ToString();
            }
            else 
            {
                NSEEQDataReq marketDataReq = new NSEEQDataReq();
                AngelArbApp.Models.Exchangetokens exchangetokens = new AngelArbApp.Models.Exchangetokens();
                marketDataReq.mode = "FULL";
                exchangetokens.NSE = new string[] { stkToken };
                marketDataReq.exchangeTokens = exchangetokens;
                req = JsonConvert.SerializeObject(marketDataReq).ToString();

            }                    

            var client = new RestClient("https://apiconnect.angelone.in/rest/secure/angelbroking/market/v1/quote/");
            var request = new RestRequest("", Method.Post);

            request.AddBody(req);

            var authcode = "Bearer " + jwtToken;

            request.AddHeader("X-PrivateKey", settingData.APIKey);
            request.AddHeader("Accept", "application/json");
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("X-SourceID", "WEB");
            request.AddHeader("X-UserType", "USER");            
            request.AddHeader("X-ClientLocalIP", "");
            request.AddHeader("X-ClientPublicIP", "");
            request.AddHeader("X-MACAddress", "");
            request.AddHeader("Authorization", authcode);
            request.AddHeader("X-MACAddress", "");
            request.AddHeader("Accept-Encoding", "gzip, deflate, br");

            RestResponse response = new RestResponse();

            try
            {
                response = await client.ExecuteAsync(request);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error while Get " + e.Message);
            }
            if (response.StatusCode == System.Net.HttpStatusCode.Forbidden)
            {
                Console.WriteLine("Error while Get, due to too many request. restart after a while " );
                throw new Exception("Error while Get, due to too many request. restart after a while ");
            }
            return JsonConvert.DeserializeObject<InstrumentResp>(response.Content);
        }

        public static bool CheckToken()
        {
            string token = "";
            LoginResponse tokendata = new LoginResponse();
            try
            {
                token = System.IO.File.ReadAllText("Data\\AccesTokenData.Json").ToString();
                tokendata = JsonConvert.DeserializeObject<LoginResponse>(token.ToString());


                if (tokendata.data.jwtToken.ToString() == null)
                {
                    return false;
                }
                else
                {
                    var currentDateTime = DateTime.Now;
                    var refreshDateTime = tokendata.token_reciviewed_at;
                    var timeDifference = currentDateTime - refreshDateTime;
                    // Check if the difference is greater than 360 minutes
                    if ((timeDifference.TotalMinutes > 360))
                    {
                        return false; // Token needs to be refreshed
                    }
                    else
                    {
                        return true; // Token is still valid
                    }
                }
            }
            catch (Exception)
            {
                return false;

            }
        }

    }
}
