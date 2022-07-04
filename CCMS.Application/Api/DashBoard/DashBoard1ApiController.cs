using CCMS.Application.Dtos;
using CCMS.Application.Dtos.Dashboard1Dtos;
using CCMS.Application.Dtos.notice;
using CCMS.Application.Enum;
using CCMS.Application.Services;
using CCMS.Application.Services.Notice;
using CCMS.Application.Utils;
using CCMS.Core.Filter;
using CsvHelper;
using CsvHelper.Configuration;
using Dapper;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCMS.Application.Api.DashBoard
{
    [Route("api/[controller]")]
    [ApiDescriptionSettings("Dashboard Sersor",Tag = "API DashBoard sensor", Order =109)]
    
    public class DashBoard1Api : ControllerBase
    {
        private readonly IDapperRepository _dapper;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ISysNotifyUploadService _notifyUploadService;
        public DashBoard1Api(IDapperRepository dapperRepository, ISysNotifyUploadService notifyUploadService, IWebHostEnvironment webHostEnvironment)
        {
            _dapper = dapperRepository;
            _webHostEnvironment = webHostEnvironment;
            _notifyUploadService = notifyUploadService;
        }

        [HttpGet("get-all-machine")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllMachines()
        {

            var list = await _dapper.Context.QueryAsync<dynamic>(@"select equipment_code,equipment_name from SD_Equipment");
            return Ok(list);
        }


        [HttpGet("get-all-location")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllLocations()
        {

            var list = await _dapper.Context.QueryAsync<dynamic>(@"select location_code,location_name from SD_location");
            return Ok(list);
        }

        [HttpPost("dashboard-inspector-search")]
        [AllowAnonymous]
        public async Task<IActionResult> DashboardSearch([FromQuery] DateTime? datestart, [FromQuery] DateTime? enddate)
        {

            var query = @"
                           SELECT 
                                FORMAT(CAST(Created_Date AS DATE),'yyyy-MM-dd')  as Created_Date,
                          
                         SUM(CASE WHEN Status='NG' THEN 1 ELSE 0 end) CountNg,
                          SUM(CASE WHEN Status='OK' THEN 1 ELSE 0 end) CountOk,
                           SUM(CASE WHEN Status='FIXED' THEN 1 ELSE 0 end) CountFixed
                          FROM dbo.Data_Sensor
                         /**where**/
                          GROUP BY  CAST(Created_Date AS DATE)
                        ";
            SqlBuilder builder = new SqlBuilder();
            var selector = builder.AddTemplate(query);

            if (datestart !=null)
            {
                builder.Where("Created_Date >= @datestart", new { datestart });
            }
            if (enddate != null)
            {
                builder.Where("Created_Date <= @enddate", new { enddate });
            }

            var rows =await _dapper.Context.QueryAsync<Status_Sensor_Model>(selector.RawSql, selector.Parameters);
            var totalOk = rows?.Sum(a => a.CountOk);
            var totalNg = rows?.Sum(a => a.CountNg);
            var totalFixed = rows?.Sum(a => a.CountFixed);

            var dict = rows?.ToDictionary(a => a.Created_Date, a => a);

            return Ok(new { totalOk, totalNg, totalFixed, data = dict });
        }

        [HttpGet("get-all-status")]
        [AllowAnonymous]
        public  IActionResult GetAllStatus()
        {
            return Ok(new List<object>
            {
                new {value="ok",title="OK"},
                 new {value="ng",title="NG"}
            });
        }

        [HttpPost("test-notify")]
        [AllowAnonymous]
        public async Task< IActionResult> Test_Notify()
        {
            await _notifyUploadService.PushNotify_UploadedFile(new NotifyMessage
            {
                type = MessageType.notifyupload,
                data = new
                {
                    id = 888,
                    Staff="Mr G",
                    Size=200,
                    Status="OK",
                    Location="XXX",
                    Machine="YYY",
                    
                    DateCreated = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    addrow = true

                }

            });
            return Ok();
        }



        [HttpGet("get-list-data")]
        [AllowAnonymous]
        public IActionResult GetList(int? page, int? pagesize, string search_name, string search_status, DateTime? start_date, DateTime? end_date, string inspector)
        {
            const string selectQuery = @" ;WITH _data AS (
                      SELECT a.*,
                                FORMAT( a.Created_Date, 'yyyy-MM-dd HH:mm:ss' ) AS DateCreated 
                            from  Data_Sensor a
                           
                      /**where**/
                    ),
                      _count AS (
                        SELECT COUNT(1) AS TotalCount FROM _data
                    )
                    SELECT * FROM _data CROSS APPLY _count /**orderby**/ OFFSET @PageIndex * @PageSize ROWS FETCH NEXT @PageSize ROWS ONLY";

            SqlBuilder builder = new SqlBuilder();
            var selector = builder.AddTemplate(selectQuery, new { PageIndex = page ?? 0, PageSize = pagesize ?? 1000 });
           
            if (!string.IsNullOrEmpty(inspector))
            {
                var inspector_param = "%" + inspector + "%";
                builder.Where("a.Staff Like @inspector ", new { inspector= inspector_param });
            }
            if (!string.IsNullOrEmpty(search_name))
            {
                var searchname_param = "%" + search_name + "%";
                builder.Where("a.Machine Like @searchname or a.Location Like @searchname", new { searchname = searchname_param });
            }
            if (!string.IsNullOrEmpty(search_status))
            {
                builder.Where("a.Status = @search_status ", new { search_status  });
            }
            if (start_date!=null)
            {
                builder.Where("a.[Created_Date] >= @start_date ", new { start_date });
            }
            if (end_date != null)
            {
                builder.Where("a.[Created_Date] <= @end_date ", new { end_date });
            }
            builder.OrderBy("id desc");
            var rows = _dapper.Context.Query<dynamic>(selector.RawSql, selector.Parameters).ToList(); ;

            return Ok(new PagedResults<dynamic>
            {
                Items = rows,
                TotalCount = rows.Count == 0 ? 0 : rows[0].TotalCount
            });

        }

        [HttpPost("get-detail")]
        [AllowAnonymous]
        public async Task<IActionResult> GetDetail([FromBody] upload_model_input input )
        {

            var item = await _dapper.Context.QueryFirstOrDefaultAsync<upload_model_input>(@"select * from Data_Sensor where id=@id ", input);
            if (item!=null  && !string.IsNullOrWhiteSpace(item.PathFile))
            {
                var filename=System.IO.Path.GetFileName(item.PathFile);
                
                string webRootPath = _webHostEnvironment.WebRootPath;
                string uploads = Path.Combine(webRootPath, "Uploadfiles");
                var file_path = Path.Combine(uploads, filename);
                if (System.IO.File.Exists(file_path))
                {
                    var config = new CsvConfiguration(CultureInfo.InvariantCulture)
                    {
                        HasHeaderRecord = false,
                    };
                    using (var reader = new StreamReader(file_path))
                    using (var csv = new CsvReader(reader, config))
                    {
                        var records = csv.GetRecords<DataSensor>();
                        var dict = new Dictionary<string, List<decimal>>();
                        var dict_bars = new Dictionary<string, List<decimal>>();

                        if (records != null)
                        {

                            var lst2 = new List<decimal>();
                            foreach (var w in records)
                            {
                                lst2.Clear();
                                lst2.Add(w.sensor_0);
                                lst2.Add(w.sensor_1);
                                lst2.Add(w.sensor_2);
                                lst2.Add(w.sensor_3);
                                lst2.Add(w.sensor_4);
                                lst2.Add(w.sensor_5);
                                lst2.Add(w.sensor_6);
                                lst2.Add(w.sensor_7);

                                for (var i = 0; i < 8; i++)
                                {
                                    if (dict.TryGetValue("sensor_" + i.ToString(), out List<decimal> lst))
                                    {
                                        lst.Add(lst2[i]);
                                    }
                                    else
                                    {
                                        dict.Add("sensor_" + i.ToString(), new List<decimal> { lst2[i] });
                                    }
                                }

                            }
                        }

                        foreach(var w in dict)
                        {
                            dict_bars.Add(w.Key, w.Value.OrderByDescending(a => a).ToList());
                        }
                        return Ok(new {linedata= dict , bardata=dict_bars});

                    }
                }
                else return BadRequest("File upload not found");
            }
            return BadRequest("File upload not found");
          
        }

        [HttpPost("upload")]
        [AllowAnonymous]
        public async Task<IActionResult> Upload([FromForm] upload_model input)
        {

            //xử lý file
            string _fileName = null;
            if (input.filetext != null )
            {
                _fileName = Path.GetFileNameWithoutExtension(input.filetext.FileName) + "_" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + Path.GetExtension(input.filetext.FileName);

                string webRootPath = _webHostEnvironment.WebRootPath;
                string uploads = Path.Combine(webRootPath, "Uploadfiles");
                var file_path = Path.Combine(uploads, _fileName);

                input.PathFile = file_path;

                using (Stream fileStream = new FileStream(file_path, FileMode.Create))
                {
                    await input.filetext.CopyToAsync(fileStream);
                }

              var id=  await _dapper.Context.QuerySingleAsync<int>(@"

                                                    INSERT INTO [dbo].[Data_Sensor]
                                                               ([Location]
                                                               ,[Machine]
                                                               ,[Staff]
                                                               ,[Left]
                                                               ,[Size]
                                                               ,[PathFile]
                                                               ,[Created_Date]
                                                                ,[Status] )
                                                         VALUES
                                                               (@Location
                                                               ,@Machine
                                                               ,@Staff
                                                               ,@Left
                                                               ,@Size
                                                               ,@PathFile
                                                               ,getdate()
                                                                ,@Status)
                                                            SELECT CAST(SCOPE_IDENTITY() as int)
                                                        ", input);



                await _notifyUploadService.PushNotify_UploadedFile(new NotifyMessage
                {
                     type= MessageType.notifyupload,
                     data=new
                     {
                         id= id,
                         input.Staff,
                         input.Size,
                         input.Status,
                         input.Location,
                         input.Machine,
                         input. Left,
                         DateCreated=DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                         addrow=true

                     }

                });
            }
            return Ok();
        }

    }
}
