using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dicer.Core.Utl;
using Microsoft.AspNetCore.Mvc;

namespace Dicer.Controllers
{
    public class MainController : ControllerBase
    {
        #region 'Properties'
        public Dictionary<string, object> ResponseData;

        enum svc
        {
            SaveFibonacci = 0
        }


        public int Service
        {
            get; set;
        }
        #endregion

        #region 'HTTP METHOD'   
        [HttpGet]
        [HttpPost]
        [HttpPut]
        [HttpDelete]
        public ActionResult OnRequest([FromBody]  Dictionary<string, object> data)
        {
            try
            {
                if (LoadParam(data))
                    return Process(Request.Method, data);
                else return StatusCode(500);

            }
            catch (Exception ex)
            {
                ResponseData.Add("error", ex.Message);
                return NotFound(ResponseData);
            }
        }
        #endregion

        #region 'Utilities'
        private bool LoadParam(Dictionary<string, object> data)
        {
            ResponseData = new Dictionary<string, object>();
            Service = Fmt.Var.ToInteger(data["s"]);
            return true;
        }


        private ActionResult Process(string method, Dictionary<string, object> data)
        {
            switch (method)
            {
                case "GET":
                    return Ok(ResponseData);

                case "POST":
                    //save the data from here
                    ResponseData.Add("data", data);
                    return Ok(ResponseData);

                case "PUT":
                    return Ok(ResponseData);

                case "DELETE":
                    return Ok(ResponseData);

                default:
                    ResponseData.Add("error", $"{method} method not implemented yet");
                    return Ok(ResponseData);
            }
        }
        #endregion
    }
}