//using System;
//using System.Net.Http;
//using System.Text;
//using System.Threading.Tasks;
//using Newtonsoft.Json;
//using System.IO;
//public class AngelBrokingApiService
//{
//    private readonly HttpClient _httpClient;
//    private const string BaseUrl = "https://apiconnect.angelbroking.com/rest/secure/angelbroking/"; // Base URL for the SmartAPI
//    private const string ApiKey = "MGHSg3OI"; // Replace with your actual API Key from Angel Broking

//    private string _jwtToken;
//    private string _refreshToken;
//    private string _feedToken;
//    private string _clientId;

//    public AngelBrokingApiService()
//    {
//        _httpClient = new HttpClient();
//        _httpClient.DefaultRequestHeaders.Add("X-API-KEY", ApiKey);
//        //_httpClient.DefaultRequestHeaders.Add("Content-Type", "application/json");
//        _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
//    }

//    public async Task<bool> Login(string clientId, string password)
//    {
//        var loginRequest = new LoginRequest
//        {
//            ClientID = clientId,
//            Password = password
//        };

//        var jsonContent = JsonConvert.SerializeObject(loginRequest);
//        var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

//        var response = await _httpClient.PostAsync($"{BaseUrl}user/v1/loginByPassword", content);
//        response.EnsureSuccessStatusCode();

//        var responseString = await response.Content.ReadAsStringAsync();
//        var loginResponse = JsonConvert.DeserializeObject<LoginResponse>(responseString);

//        if (loginResponse.Status && loginResponse.Data != null)
//        {
//            _jwtToken = loginResponse.Data.JwtToken;
//            _refreshToken = loginResponse.Data.RefreshToken;
//            _feedToken = loginResponse.Data.FeedToken;
//            _clientId = loginResponse.Data.ClientID;

//            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _jwtToken);

//            Console.WriteLine("Login Successful!");
//            Console.WriteLine($"JWT Token: {_jwtToken}");
//            return true;
//        }
//        else
//        {
//            Console.WriteLine($"Login Failed: {loginResponse.Message} - {loginResponse.ErrorCode}");
//            return false;
//        }
//    }

//    public async Task<MarketDataResponse> GetMarketData(string exchange, List<string> tokens)
//    {
//        if (string.IsNullOrEmpty(_jwtToken))
//        {
//            Console.WriteLine("Not logged in. Please login first.");
//            return null;
//        }

//        var marketDataRequest = new MarketDataRequest
//        {
//            Mode = new List<MarketDataInstrument>
//            {
//                new MarketDataInstrument { Exchange = exchange, Tokens = tokens }
//            },
//            ExchangeTokens = new List<string>() // This might be required for specific modes, adjust as per API docs
//        };

//        var jsonContent = JsonConvert.SerializeObject(marketDataRequest);
//        var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

//        // The endpoint for market data is often different or requires a specific path.
//        // According to the docs, it might be /rest/secure/angelbroking/market/v1/quote
//        // or /rest/secure/angelbroking/market/v1/market_data for specific data.
//        // Let's assume a generic "market_data" for now, but confirm with API docs.
//        var response = await _httpClient.PostAsync($"{BaseUrl}market/v1/market_data", content);
//        response.EnsureSuccessStatusCode();

//        var responseString = await response.Content.ReadAsStringAsync();
//        var marketDataResponse = JsonConvert.DeserializeObject<MarketDataResponse>(responseString);

//        if (marketDataResponse.Status)
//        {
//            Console.WriteLine($"Market Data for {exchange} ({string.Join(",", tokens)}):");
//            if (marketDataResponse.Data != null)
//            {
//                foreach (var entry in marketDataResponse.Data)
//                {
//                    Console.WriteLine($"  Token: {entry.Key}, Symbol: {entry.Value.Symbol}, LTP: {entry.Value.LTP}");
//                }
//            }
//            return marketDataResponse;
//        }
//        else
//        {
//            Console.WriteLine($"Failed to get market data: {marketDataResponse.Message} - {marketDataResponse.ErrorCode}");
//            return null;
//        }
//    }

//    // You might also need a method to refresh the token
//    public async Task RefreshToken()
//    {
//        // Implement token refresh logic here using the refresh token
//        // Refer to the Angel Broking API documentation for the refresh token endpoint and payload.
//    }
//}