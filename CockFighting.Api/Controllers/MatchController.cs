using System.Collections.Generic;
using System.Web.Http;
using CockFighting.ViewModels;
using CockFighting.Repositories;
//using System.Web.Http.Cors;
using System;
using System.Linq;
using System.Web;
using System.IO;
using OfficeOpenXml;
using OfficeOpenXml.Table;
using System.Data;

namespace CockFighting.OnePage.Controllers
{
    //[EnableCors(origins: "*", headers: "*", methods: "*")]
    public class MatchController : BaseApiController
    {
        // GET: api/Match/true
        [Route("api/Match/{isRegen}/{diff:int?}")]
        public ApiResult<List<MatchViewModel>> Get(bool isRegen, int diff = 25)
        {
            List<MatchViewModel> data = new List<MatchViewModel>();
            if (isRegen)
            {
                SWMatchRepository<MatchViewModel>.Instance.RemoveListModel(m => m.CreatedDate < DateTime.UtcNow);
                List<CockViewModel> processedCocks = new List<CockViewModel>();
                List<CockViewModel> freeCocks = new List<CockViewModel>();
                List<CockViewModel> cocks = SWCockRepository<CockViewModel>.Instance.GetModelList();
                Shuffle(cocks);
                foreach (var cock in cocks)
                {

                    var other = cocks.FirstOrDefault(
                        c =>
                        processedCocks.Count(pc => pc.Id == c.Id) == 0
                        && c.UserPhone != cock.UserPhone && Math.Abs(c.Weight - cock.Weight) < diff);

                    if (other == null)
                    {
                        freeCocks.Add(cock);
                        other = new CockViewModel();
                    }

                    if (processedCocks.Count(c => c.Id == cock.Id || c.Id == other.Id) > 0)
                    {
                        continue;
                    }

                    processedCocks.Add(cock);
                    if (other.Id > 0)
                    {
                        processedCocks.Add(other);
                    }

                    MatchViewModel match = new MatchViewModel()
                    {
                        CockId1 = cock.Id,
                        CockId2 = other.Id > 0 ? other.Id : default(int?),
                        Cock1 = cock,
                        Cock2 = other,
                        CreatedDate = DateTime.UtcNow
                    };

                    SWMatchViewModel<MatchViewModel> vmMatch = new SWMatchViewModel<MatchViewModel>(match);
                    vmMatch.SaveModel();
                    data.Add(match);

                }
            }
            else
            {
                data = SWMatchRepository<MatchViewModel>.Instance.GetModelList();
            }

            return GetResult(1, data.OrderByDescending(d => d.CockId2).ToList());
        }

        // GET: api/Match
        [Route("api/Match")]
        public ApiResult<string> Get()
        {
            List<MatchViewModel> data = SWMatchRepository<MatchViewModel>.Instance.GetModelList();
            string error;
            string SavedPath = ExportToExcel(data.OrderByDescending(d=>d.CockId2).ToList(), "Derby", "/Excels", "Derby_Matches", out error);
            return GetResult(1, SavedPath);
        }

        public static void Shuffle<T>(IList<T> list)
        {
            Random rng = new Random();
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        public static string ExportToExcel(List<MatchViewModel> lstData, string sheetName, string folderPath, string fileName, out string errorMsg)
        {
            errorMsg = string.Empty;

            try
            {
                if (lstData.Count > 0)
                {
                    var filenameE = fileName + "-" + DateTime.Now.ToString("yyyyMMdd") + ".xlsx";

                    // create new data table
                    var dtable = new DataTable();

                    // //get first item
                    //var listColumn = lstData[0].GetType().GetProperties();

                    //// add column name to table
                    //foreach (var item in listColumn)
                    //{
                    //    dtable.Columns.Add(item.Name, typeof(string));
                    //}


                    List<string> headers = new List<string>() { "STT", "Tên đội", "Chủ đội", "LB","Weight", "Weight", "LB", "Chủ đội", "Tên đội" };
                    string[] cols = { "stt", "team1", "user1", "lb1","w1", "w2", "lb2", "user2", "team2" };

                    foreach (var item in cols)
                    {
                        dtable.Columns.Add(item, typeof(string));
                    }

                    // Row value
                    int stt = 0;
                    foreach (var a in lstData)
                    {
                        stt++;

                        var r = dtable.NewRow();
                        r["stt"] = stt;
                        r["team1"] = a.Cock1.TeamName;
                        r["user1"] = a.Cock1.UserName;
                        r["lb1"] = a.Cock1.Code;
                        r["w1"] = a.Cock1.Weight;

                        r["team2"] = a.Cock2?.TeamName;
                        r["user2"] = a.Cock2?.UserName;
                        r["lb2"] = a.Cock2?.Code;
                        r["w2"] = a.Cock2?.Weight;
                        dtable.Rows.Add(r);
                    }

                    // Create file Path
                    string serverPath = HttpContext.Current.Server.MapPath(folderPath);
                    if (!Directory.Exists(serverPath))
                    {
                        Directory.CreateDirectory(serverPath);
                    }
                    string SavePath = Path.Combine(folderPath, filenameE);

                    // Save Excel file
                    using (var pck = new ExcelPackage())
                    {
                        string SheetName = sheetName != string.Empty ? sheetName : "Report";
                        var wsDt = pck.Workbook.Worksheets.Add(SheetName);
                        for (int i = 1; i <= headers.Count; i++)
                        {
                            wsDt.Cells[1, i].Value = headers[i - 1];
                        }


                        wsDt.Cells["A2"].LoadFromDataTable(dtable, false, TableStyles.Light1);
                        wsDt.Cells[wsDt.Dimension.Address].AutoFitColumns();



                        WriteBytesToFile(SavePath, pck.GetAsByteArray());
                        return SavePath;
                    }
                }
                else
                {
                    errorMsg = "Can not export data of empty list";
                }
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
            }

            return string.Empty;
        }

        public static void WriteBytesToFile(string filePath, byte[] content)
        {
            string fullPath = HttpContext.Current.Server.MapPath(filePath);
            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }

            FileStream fs = new FileStream(fullPath, FileMode.Create);
            BinaryWriter w = new BinaryWriter(fs);
            try
            {
                w.Write(content);
            }
            finally
            {
                fs.Close();
                w.Close();
            }
        }

        // GET: api/Match/5
        public ApiResult<MatchViewModel> Get(int id)
        {
            var data = SWMatchRepository<MatchViewModel>.Instance.GetSingleModel(m => m.Id == id);
            return GetResult(1, data); ;
        }

        // POST: api/Match
        public ApiResult<MatchViewModel> Post([FromBody]MatchViewModel view)
        {
            SWMatchViewModel<MatchViewModel> register = new SWMatchViewModel<MatchViewModel>(view);
            var saveResult = register.SaveModel(true);
            return GetResult(1, saveResult.Data);
        }

        // PUT: api/Match/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Match/5
        public ApiResult<bool> Delete(int id)
        {
            var result = SWMatchRepository<MatchViewModel>.Instance.RemoveModel(m => m.Id == id);
            return GetResult(1, result.IsSucceed);
        }

    }
}
