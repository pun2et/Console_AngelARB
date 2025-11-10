using AngelArbApp.Models;
using Clientupstoxone.Models;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AngelArbApp
{

    public static class GoogleUpdater
    {
        static readonly string[] Scopes = { SheetsService.Scope.Spreadsheets };
        static readonly string ApplicationName = "AngelArbApp";
        static readonly string SpreadsheetId = "1r4hyObm9oL2m72IifY_lEMHHyFHQI2sNpJBvXFgJeCU";
        static SheetsService _sheetsService;
        static readonly string SheetName = "ARBSheet";

        public static void GoogleAuth()
        {           
            var serviceAccountJson = Path.Combine(AppContext.BaseDirectory, "Data\\credentials.json");
            InitializeSheetsService(serviceAccountJson);
        }

        static void InitializeSheetsService(string serviceAccountJsonPath)
        {
            if (!File.Exists(serviceAccountJsonPath))
                throw new FileNotFoundException("Service account JSON not found.", serviceAccountJsonPath);

            var credential = GoogleCredential.FromFile(serviceAccountJsonPath)
                                             .CreateScoped(Scopes);

            _sheetsService = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName
            });
        }

        public static void UpdateSheetFromDisplayModels(List<DisplayModel> items, SettingsModel settingsModel)
        {
            if (_sheetsService == null) throw new InvalidOperationException("Sheets service not initialized.");

            // Build rows: header + data
            var values = new List<IList<object>>();

            // Header row (keep order consistent with DisplayModel)
            values.Add(new List<object>
        {
            "Symbol",
            "Current_Fut_price",
            "Next_Fut_price",
            "FUT_Diff_%_"+settingsModel.CurrentMonth+"_"+settingsModel.NextMonth,
            "Lot_Size",           
            "Execution_Cost_FUT",
            "Updated_time"
        });

            foreach (var d in items)
            {
                values.Add(new List<object>
            {
                d.Symbol ?? string.Empty,    
                d.NearFUT_Price ?? string.Empty,
                d.NextFUT_Price ?? string.Empty,
                d.FUT_Difference_per ?? string.Empty,
                d.Lot_Size ?? string.Empty,                
                d.Total_Sprade ?? string.Empty,
                d.Updated_time ?? string.Empty
            });
            }

            // Clear existing sheet content (optional but keeps sheet clean)
            var clearRequest = _sheetsService.Spreadsheets.Values.Clear(new ClearValuesRequest(), SpreadsheetId, SheetName);
            clearRequest.Execute();


            // Write new values starting at A1
            var valueRange = new ValueRange { Values = values };
            var updateRequest = _sheetsService.Spreadsheets.Values.Update(valueRange, SpreadsheetId, $"{SheetName}!A1");
            updateRequest.ValueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.USERENTERED;
            updateRequest.Execute();
        }
    }

}
