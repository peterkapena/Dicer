using Dicer.Core.Utl;
using Dicer.Model.Dicer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dicer.Controllers
{
    public class MainController : ControllerBase
    {
        #region 'Properties'
        public Dictionary<string, object> ResponseData;
        private DicerContext Context;
        public MainController(DicerContext cntxt)
        {
            Context = cntxt;
            ResponseData = new Dictionary<string, object>();
        }
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
        public async Task<ActionResult> OnRequestAsync([FromBody]  Dictionary<string, object> data)
        {
            try
            {
                Service = Fmt.Var.ToInteger(data["s"]);
                ActionResult x = await ProcessAsync(data); ;
                return x;
            }
            catch (Exception ex)
            {
                ResponseData.Add("error", ex.Message);
                return StatusCode(500, ResponseData);
            }
        }
        #endregion

        private async Task<ActionResult> ProcessAsync(Dictionary<string, object> data)
        {

            switch (Service)
            {
                case (int)svc.Play:
                    bool bGamed = await PlayAsync(data);
                    if (bGamed)
                        return Ok(ResponseData);
                    else return StatusCode(500, ResponseData);

                case (int)svc.Register:
                    bool bRgstd = await RegisterAsync(data);
                    if (bRgstd)
                    {
                        return Ok(ResponseData);
                    }

                    else return StatusCode(500, ResponseData);

                default:
                    return Ok(ResponseData);
            }
        }

        private bool Play1(Dictionary<string, object> data)
        {
            try
            {
                Repository repo = new Repository();
                repo.AddGamed(
                      new Gamed
                      {
                          //gamedID = repo.gamed.Count.ToString(),
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

        #region "Async functions"

        private async Task<bool> RegisterAsync(Dictionary<string, object> data)
        {
            try
            {
                Person person = new Person
                {
                    PhoneNumber = Fmt.Var.ToString(data["phneNmbr"]),
                    FirstName = Fmt.Var.ToString(data["frstNme"]),
                    LastName = Fmt.Var.ToString(data["lstNme"]),
                    IDNumber = Fmt.Var.ToString(data["idNmbr"]),
                    PassPortNumber = Fmt.Var.ToString(data["pssPrtNmbr"]),
                    OtherIdentityNumber = Fmt.Var.ToString(data["othrIdNmbr"])
                };

                Device device =
                     new Device
                     {
                         mcAddress = Fmt.Var.ToString(data["mcAddrss"]),
                         deviceTypeName = Fmt.Var.ToString(data["dviceTypeNme"]),
                         deviceOwnerName = Fmt.Var.ToString(data["dviceOwnrNme"])
                     };

                var addedPerson = await Context.Person.AddAsync(person);
                var addedDevice = await Context.Device.AddAsync(device);
                await Context.SaveChangesAsync();

                ajSetReturnValue("PersonID", addedPerson.Entity.PersonID);
                ajSetReturnValue("DeviceID", addedDevice.Entity.DeviceID);

                return true;
            }
            catch (Exception ex)
            {
                ajSetReturnValue("error", ex.Message);
                return false;
            }
        }
        private async Task<bool> PlayAsync(Dictionary<string, object> data)
        {
            try
            {
                var date = Fmt.Var.ToDate(DateTime.Now);
                Gamed gamed = new Gamed
                {
                    //gamedID = repo.gamed.Count.ToString(),
                    DeviceID = Fmt.Var.ToString(data["DeviceID"]),
                    PersonID = Fmt.Var.ToString(data["PersonID"]),
                    Date = date
                };

                var addedGamed = await Context.Gamed.AddAsync(gamed);

                await Context.SaveChangesAsync();

                ajSetReturnValue("GamedID", addedGamed.Entity.GamedID);
                ajSetReturnValue("DeviceID", data["DeviceID"]);
                ajSetReturnValue("PersonID", data["PersonID"]);
                ajSetReturnValue("Date", date);

                return true;
            }
            catch (Exception ex)
            {
                ajSetReturnValue("error", ex.Message);
                return false;
            }
        }
        public async Task<List<Person>> GetPersonsAsync()
        {
            var Persons = await Context.Person.ToListAsync();
            return Persons;
        }

        public async Task<List<Gamed>> GetGamedAsync()
        {
            var Gamed = await Context.Gamed.ToListAsync();
            return Gamed;
        }

        public async Task<List<Device>> GetDevicesAsync()
        {
            var Device = await Context.Device.ToListAsync();
            return Device;
        }
        #endregion

    }
}