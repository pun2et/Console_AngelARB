//using Newtonsoft.Json;
//using System.Collections.Generic;

//// --- Login Models ---
//public class LoginRequest
//{
//    public string ClientID { get; set; }
//    public string Password { get; set; }
//}

//public class LoginResponse
//{
//    public bool Status { get; set; }
//    public string Message { get; set; }
//    public LoginData Data { get; set; }
//    public string ErrorCode { get; set; }
//}

//public class LoginData
//{
//    public string JwtToken { get; set; }
//    public string RefreshToken { get; set; }
//    public string FeedToken { get; set; }
//    public string ClientID { get; set; }
//}

//// --- Quote Models ---
//public class QuoteRequest
//{
//    public string Exchange { get; set; }
//    public string Token { get; set; }
//    public string Symbol { get; set; }
//}

//public class QuoteResponse
//{
//    public bool Status { get; set; }
//    public string Message { get; set; }
//    public QuoteData Data { get; set; }
//    public string ErrorCode { get; set; }
//}

//public class QuoteData
//{
//    public string Exchange { get; set; }
//    public string Token { get; set; }
//    public string Symbol { get; set; }
//    public string Price { get; set; }
//    // Add more fields as per the actual quote response (e.g., open, high, low, close, volume)
//    // You'll need to inspect the API documentation or a sample response for the full structure.
//}

//// --- Market Data Request ---
//public class MarketDataRequest
//{
//    public List<MarketDataInstrument> Mode { get; set; }
//    public List<string> ExchangeTokens { get; set; }
//}

//public class MarketDataInstrument
//{
//    public string Exchange { get; set; }
//    public List<string> Tokens { get; set; }
//}

//public class MarketDataResponse
//{
//    public bool Status { get; set; }
//    public string Message { get; set; }
//    public Dictionary<string, MarketDataDetail> Data { get; set; }
//    public string ErrorCode { get; set; }
//}

//public class MarketDataDetail
//{
//    public string Exchange { get; set; }
//    public string Token { get; set; }
//    public string Symbol { get; set; }
//    public string LTP { get; set; } // Last Traded Price
//    // Add other relevant fields like open, high, low, close, volume, etc.
//}