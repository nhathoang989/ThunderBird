using System.Collections.Generic;
using System.Web.Http;
using CockFighting.ViewModels;
using CockFighting.Repositories;
using System.Web.Http.Cors;
using System;
using System.IO;
using System.Web;
using OfficeOpenXml.Table;
using OfficeOpenXml;
using System.Data;
using OfficeOpenXml.Style;
using System.Linq;

namespace CockFighting.OnePage.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("api/CockFighting")]
    public class CockFightingController : BaseApiController
    {
        // GET: api/Default
        [HttpGet]
        [Route("")]
        public ApiResult<List<DerbyRegisterViewModel>> Get()
        {
            var data = SWUserRepository<DerbyRegisterViewModel>.Instance.GetModelList();

            return GetResult(1, data);
        }

        // GET: api/Match
        [HttpGet]
        [Route("Export")]
        public ApiResult<string> Export()
        {
            var data = SWUserRepository<DerbyRegisterViewModel>.Instance.GetModelList();
            string error;
            string SavedPath = ExportToExcel(data, "Teams", "/Excels", "Derby_Teams", out error);
            return GetResult(1, SavedPath);
        }

        public static string ExportToExcel(List<DerbyRegisterViewModel> lstData, string sheetName, string folderPath, string fileName, out string errorMsg)
        {
            errorMsg = string.Empty;

            try
            {
                if (lstData.Count > 0)
                {
                    var filenameE = fileName + "-" + DateTime.Now.ToString("yyyyMMdd") + ".xlsx";

                    // create new data table
                    var dtable = new DataTable();

                    List<string> headers = new List<string>() { "STT", "Tên đội", "Chủ đội", "Gà đá 1", "Gà đá 2", "Gà đá 3", "Gà đá 4" };
                    string[] cols = { "stt", "team", "user", "cock1W", "cock1LB", "cock2W", "cock2LB", "cock3W", "cock3LB", "cock4W", "cock4LB" };

                    foreach (var item in cols)
                    {
                        dtable.Columns.Add(item, typeof(string));
                    }

                    // Row value
                    int stt = 0;
                    foreach (var user in lstData)
                    {
                        foreach (var team in user.Teams)
                        {
                            var r = dtable.NewRow();
                            stt++;
                            r["stt"] = stt;
                            r["team"] = team.Name;
                            r["user"] = team.UserName;

                            r["cock1W"] = team.Cocks[0]?.Code;
                            r["cock1LB"] = team.Cocks[0]?.Weight;

                            r["cock2W"] = team.Cocks[1]?.Code;
                            r["cock2LB"] = team.Cocks[1]?.Weight;

                            r["cock3W"] = team.Cocks[2]?.Code;
                            r["cock3LB"] = team.Cocks[2]?.Weight;

                            r["cock4W"] = team.Cocks[3]?.Code;
                            r["cock4LB"] = team.Cocks[3]?.Weight;

                            dtable.Rows.Add(r);
                        }

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
                        for (int i = 0; i < headers.Count; i++)
                        {
                            ExcelRange modelTable = null;
                            int index = i + 1;
                            if (i > 2)
                            {
                                index = (2 * i) - 2;
                                modelTable = wsDt.Cells[1, index, 1, index + 1];
                                modelTable.Merge = true;
                               
                            }
                            else
                            {
                                modelTable = wsDt.Cells[1, index];
                            }
                            modelTable.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                            modelTable.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                            modelTable.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                            modelTable.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                            modelTable.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                            modelTable.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                            modelTable.Value = headers[i];

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

        // GET: api/Default/5
        public ApiResult<DerbyRegisterViewModel> Get(string id)
        {
            var data = SWUserRepository<DerbyRegisterViewModel>.Instance.GetSingleModel(u => u.Phone == id);
            return GetResult(1, data); ;
        }

        // POST: api/Default
        [HttpPost]
        [Route("")]
        public ApiResult<DerbyRegisterViewModel> Post([FromBody]DerbyRegisterViewModel view)
        {
            if (view.Team != null)
            {
                var team = view.Teams.FirstOrDefault(t => t.Id == view.Team.Id);
                if (team != null)
                {
                    view.Teams.Remove(team);
                }
                
            }
            view.Teams.Add(view.Team);
            SWUserViewModel<DerbyRegisterViewModel> register = new SWUserViewModel<DerbyRegisterViewModel>(view);

            var saveResult = register.SaveModel(true);
            return GetResult(1, saveResult.Data);
        }

        // PUT: api/Default/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Default/5
        [HttpGet]
        [Route("Delete/{id}")]
        public ApiResult<bool> Delete(string id)
        {
            SWMatchRepository<MatchViewModel>.Instance.RemoveListModel(m => m.CreatedDate < DateTime.UtcNow);
            var result = SWUserRepository<DerbyRegisterViewModel>.Instance.RemoveModel(u => u.Phone == id);
            return GetResult(1, result.IsSucceed);
        }


    }
}
