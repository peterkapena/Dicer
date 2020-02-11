using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dicer.Core.Utl;
using Microsoft.AspNetCore.Mvc;
using Dicer.Model.Dicer;

namespace Dicer.Controllers
{
    public class MainController : ControllerBase
    {
        #region 'Properties'
        public Dictionary<string, object> ResponseData;

        enum svc
        {
            Play = 0,
            Register = 1
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
                ResponseData = new Dictionary<string, object>();
                Service = Fmt.Var.ToInteger(data["s"]);
                return Process(data);
            }
            catch (Exception ex)
            {
                ResponseData.Add("error", ex.Message);
                return StatusCode(500, ResponseData);
            }
        }
        #endregion

        private ActionResult Process(Dictionary<string, object> data)
        {

            switch (Service)
            {
                case (int)svc.Play:
                    if (Play(data))
                        return Ok(ResponseData);
                    else return StatusCode(500, ResponseData);

                case (int)svc.Register:
                    if (Register(data))
                        return Ok(ResponseData);
                    else return StatusCode(500, ResponseData);

                default:
                    return Ok(ResponseData);
            }
        }

        private bool Register(Dictionary<string, object> data)
        {
            try
            {
                Repository repo = new Repository();

                repo.AddPerson(
                      new Person
                      {
                          PersonID = repo.People.Count.ToString(),
                          PhoneNumber = Fmt.Var.ToString(data["prsnNmbr"]),
                          FirstName = Fmt.Var.ToString(data["frstNme"]),
                          LastName = Fmt.Var.ToString(data["lstNme"]),
                          IDNumber = Fmt.Var.ToString(data["idNmbr"]),
                          PassPortNumber = Fmt.Var.ToString(data["pssPrtNmbr"]),
                          OtherIdentityNumber = Fmt.Var.ToString(data["othrIdNmbr"])
                      });

                repo.AddDevice(
                    new Device
                    {
                        DeviceID = repo.Devices.Count.ToString(),
                        mcAddress = Fmt.Var.ToString(data["mcAddrss"]),
                        deviceTypeName = Fmt.Var.ToString(data["dviceTypeNme"]),
                        deviceOwnerName = Fmt.Var.ToString(data["dviceOwnrNme"])
                    }
                    );

                ajSetReturnValue("data", data);
                return true;
            }
            catch (Exception ex)
            {
                ajSetReturnValue("error", ex.Message);
                return false;
            }

        }

        private bool Play(Dictionary<string, object> data)
        {
            try
            {
                Repository repo = new Repository();
                repo.AddGamed(
                      new Gamed
                      {
                          gamedID = repo.gamed.Count.ToString(),
                          DeviceID = repo.Devices.Count.ToString(),
                          PersonID = repo.People.Count.ToString()
                      });
                ajSetReturnValue("data", data);
                return true;
            }
            catch (Exception ex)
            {
                ajSetReturnValue("error", ex.Message);
                return false;
            }
        }
        public virtual void ajSetReturnValue(string key, object value)
        {
            if (ResponseData == null)
                ResponseData = new Dictionary<string, object>();
            if (ResponseData.ContainsKey(key))
                ResponseData.Remove(key);
            ResponseData.Add(key, value);
        }

    }
}