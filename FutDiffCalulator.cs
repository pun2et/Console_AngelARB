using AngelArbApp.Models;
using AngelBroking;
using Clientupstoxone.Models;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AngelArbApp
{
    public static class FutDiffCalulator
    {
        public static async Task GetAndDisplayForEachStk(List<STKFutList> stkFutLists, string jswToken)
        {
            
            List<DisplayModel> displayModels = new List<DisplayModel>();
            List<DisplayModel> goodlowlist = new List<DisplayModel>();
            List<DisplayModel> goodhighlist = new List<DisplayModel>();

            while (true)
            {
                var settingFileread = System.IO.File.ReadAllText("Data\\Settings.Json").ToString();
                var settingData = JsonConvert.DeserializeObject<SettingsModel>(settingFileread);

                var Firstpp = "<!DOCTYPE html>\r\n<html>\r\n<head>\r\n<style>\r\ntable {\r\n  font-family: arial, sans-serif;\r\n  border-collapse: collapse;\r\n  width: 100%;\r\n}\r\n\r\ntd, th {\r\n  border: 1px solid #dddddd;\r\n  text-align: left;\r\n  padding: 8px;\r\n}\r\n\r\ntr:nth-child(even) {\r\n  background-color: #dddddd;\r\n}\r\n</style>\r\n</head>\r\n<body>\r\n\r\n<h2>Arbitration Table</h2>\r\n\r\n<table>\r\n  <tr>\r\n    <th>Symbol</th>\r\n    <th>Diff Percentage </th>\r\n    <th>Total Sprade</th>\r\n  <th>Updated Time</th>\r\n </tr>";
                var Secondpp = "</table>\r\n\r\n</body>\r\n</html>\r\n";
                var tablerows = "";
                var txtgoodlow = "";
                var txtgoodhigh = "";
                var txtgoodlowsprd = "";
                var txtgoodhighsprd = "";
                RestResponse eqstock = new RestResponse();

                GoogleUpdater.GoogleAuth();

                foreach (var item in stkFutLists)
                {
                    try
                    {

                        var eqstockdata = await HelperClass.GetMarketData(jswToken, item.StockToken, "NSE");
                        System.Threading.Thread.Sleep(int.Parse(settingData.TimeDelayInMiliSec));
                        var nrFutData = await HelperClass.GetMarketData(jswToken, item.Cntrs.ToList().Find(a => a.symbol.Contains(settingData.CurrentMonth)).token);
                        System.Threading.Thread.Sleep(int.Parse(settingData.TimeDelayInMiliSec));
                        var nxtFutData = await HelperClass.GetMarketData(jswToken, item.Cntrs.ToList().Find(a => a.symbol.Contains(settingData.NextMonth)).token);
                        System.Threading.Thread.Sleep(int.Parse(settingData.TimeDelayInMiliSec));

                        DisplayModel displayModel = new DisplayModel();
                        displayModel.Symbol = item.StockSymbol;
                        displayModel.Price = eqstockdata.data.fetched[0].ltp.ToString();
                        displayModel.NearFUT_Price = nrFutData.data.fetched[0].ltp.ToString();
                        displayModel.NextFUT_Price = nxtFutData.data.fetched[0].ltp.ToString();

                        var per_diff = (Math.Abs(float.Parse(displayModel.NearFUT_Price) - float.Parse(displayModel.NextFUT_Price)) / float.Parse(displayModel.NearFUT_Price) * 100);
                        displayModel.FUT_Difference_per = Math.Round(per_diff, 2).ToString();
                        displayModel.Lot_Size = item.Cntrs[0].lotsize.ToString();
                        displayModel.NearFUT_Sprade = (int.Parse(displayModel.Lot_Size) * Math.Abs(nrFutData.data.fetched[0].depth.buy[0].price - nrFutData.data.fetched[0].depth.sell[0].price)).ToString();
                        displayModel.NextFUT_Sprade = (int.Parse(displayModel.Lot_Size) * Math.Abs(nxtFutData.data.fetched[0].depth.buy[0].price - nxtFutData.data.fetched[0].depth.sell[0].price)).ToString();
                        displayModel.Total_Sprade = (Math.Round(float.Parse(displayModel.NearFUT_Sprade) + float.Parse(displayModel.NextFUT_Sprade), 2)).ToString();
                        displayModel.Updated_time = DateTime.Now.ToString("hh:mm:ss");

                        var itemExist = displayModels.Exists(b => b.Symbol == displayModel.Symbol);

                        if (itemExist)
                        {
                            displayModels.Find(b => b.Symbol == displayModel.Symbol).FUT_Difference_per = displayModel.FUT_Difference_per;
                            displayModels.Find(b => b.Symbol == displayModel.Symbol).Updated_time = displayModel.Updated_time;
                            displayModels.Find(b => b.Symbol == displayModel.Symbol).Total_Sprade = displayModel.Total_Sprade;
                        }
                        Console.WriteLine($"Symbol: {displayModel.Symbol}, Price: {displayModel.Price}, Lot Size: {displayModel.Lot_Size}, Per_Diff_inFut: {displayModel.FUT_Difference_per}, Total_Sprade: {displayModel.Total_Sprade}");
                        tablerows += $"<tr>\r\n<td>{displayModel.Symbol}</td>\r\n<td>{displayModel.FUT_Difference_per}</td>\r\n<td>{displayModel.Total_Sprade}</td>\r\n </tr>";

                        bool lowlistupdated = false;
                        bool highlistupdated = false;

                        var lowExist = goodlowlist.Exists(b => b.Symbol == displayModel.Symbol);

                        if (lowExist)
                        {
                            if (float.Parse(displayModel.FUT_Difference_per) < 0.3 && float.Parse(displayModel.FUT_Difference_per) > -0.3 && float.Parse(displayModel.Total_Sprade) < float.Parse(settingData.Spread_LessThan))
                            {
                                goodlowlist.Find(b => b.Symbol == displayModel.Symbol).FUT_Difference_per = displayModel.FUT_Difference_per;
                                goodlowlist.Find(b => b.Symbol == displayModel.Symbol).Updated_time = displayModel.Updated_time;
                                goodlowlist.Find(b => b.Symbol == displayModel.Symbol).Total_Sprade = displayModel.Total_Sprade;
                                lowlistupdated = true;
                            }
                            else
                            {
                                goodlowlist.RemoveAll(b => b.Symbol == displayModel.Symbol);
                                lowlistupdated = true;
                            }
                        }
                        else
                        {
                            if (float.Parse(displayModel.FUT_Difference_per) < 0.3 && float.Parse(displayModel.FUT_Difference_per) > -0.3 && float.Parse(displayModel.Total_Sprade) < float.Parse(settingData.Spread_LessThan))
                            {
                                goodlowlist.Add(displayModel);
                                lowlistupdated = true;

                            }
                        }


                        var highExist = goodhighlist.Exists(b => b.Symbol == displayModel.Symbol);

                        if (highExist)
                        {
                            if ((float.Parse(displayModel.FUT_Difference_per) > 0.8 || float.Parse(displayModel.FUT_Difference_per) < -0.8) && float.Parse(displayModel.Total_Sprade) < float.Parse(settingData.Spread_LessThan))
                            {
                                goodhighlist.Find(b => b.Symbol == displayModel.Symbol).FUT_Difference_per = displayModel.FUT_Difference_per;
                                goodhighlist.Find(b => b.Symbol == displayModel.Symbol).Updated_time = displayModel.Updated_time;
                                goodhighlist.Find(b => b.Symbol == displayModel.Symbol).Total_Sprade = displayModel.Total_Sprade;
                                highlistupdated = true;
                            }
                            else
                            {
                                goodhighlist.RemoveAll(b => b.Symbol == displayModel.Symbol);
                                highlistupdated = true;

                            }


                        }
                        else
                        {
                            if ((float.Parse(displayModel.FUT_Difference_per) > 0.8 || float.Parse(displayModel.FUT_Difference_per) < -0.8) && float.Parse(displayModel.Total_Sprade) < float.Parse(settingData.Spread_LessThan))
                            {
                                goodhighlist.Add(displayModel);
                                highlistupdated = true;

                            }
                        }

                        switch (settingData.DisplaySortBy)
                        {
                            case "Name":
                                goodlowlist = goodlowlist.OrderBy(o => o.Symbol).ToList();
                                goodhighlist = goodhighlist.OrderBy(o => o.Symbol).ToList();
                                break;
                            case "DiffPercent":
                                goodlowlist = goodlowlist.OrderBy(o => o.FUT_Difference_per).ToList();
                                goodhighlist = goodhighlist.OrderBy(o => o.FUT_Difference_per).ToList();
                                break;
                            case "Spread":
                                goodlowlist = goodlowlist.OrderBy(o => o.Total_Sprade).ToList();
                                goodhighlist = goodhighlist.OrderBy(o => o.Total_Sprade).ToList();
                                break;
                            default:
                                break;
                        }

                        System.IO.File.WriteAllText("Data\\LowSpreadList.json", JsonConvert.SerializeObject(goodlowlist));
                        System.IO.File.WriteAllText("Data\\HighSpreadList.json", JsonConvert.SerializeObject(goodhighlist));
                        var updatelist = goodlowlist.Concat(goodhighlist).ToList();
                        if (updatelist.Count > 0 && (highlistupdated || lowlistupdated))
                        {
                            GoogleUpdater.UpdateSheetFromDisplayModels(updatelist,settingData);
                        }                  



                        foreach (var remitem in goodlowlist)
                        {
                            txtgoodlowsprd += $"<tr>\r\n<td>{remitem.Symbol + "  -  " + remitem.Lot_Size}</td>\r\n<td>{remitem.FUT_Difference_per}</td>\r\n<td>{remitem.Total_Sprade}</td>\r\n  <td>{remitem.Updated_time}</td>\r\n</tr>";
                        }
                        foreach (var remitem in goodhighlist)
                        {
                            txtgoodhighsprd += $"<tr>\r\n<td>{remitem.Symbol + "  -  " + remitem.Lot_Size}</td>\r\n<td>{remitem.FUT_Difference_per}</td>\r\n<td>{remitem.Total_Sprade}</td>\r\n  <td>{remitem.Updated_time}</td>\r\n</tr>";
                        }
                        System.IO.File.WriteAllText("Data\\AbrList.html", Firstpp + txtgoodlowsprd + Secondpp + Firstpp + txtgoodhighsprd + Secondpp);
                        System.IO.File.WriteAllText("Data\\Index.html", Firstpp + tablerows + Secondpp);
                        txtgoodlowsprd = "";
                        txtgoodhighsprd = "";
                    }
                    catch (Exception e)
                    {

                        Console.WriteLine("Error " + e.Message);
                        //Console.WriteLine("Error " + eqstock.Content.ToString());
                    }


                }

                Console.WriteLine("New data read started");


            }
        }
    }
}
