using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using Newtonsoft.Json;
using RestSharp;

namespace Platxe.Areas.HR.Controllers.api
{

    [Area("HR")]
    [Authorize]
    [Produces("application/json")]
    [Route("api/hr/[Controller]/[Action]")]
    public class icardsController : Controller
    {
        private readonly string DB;
        private readonly int UserID;
        private readonly string xLan;
        private readonly Guid? Subscribe;
        private readonly string CID;

        private readonly IWebHostEnvironment root;

        public icardsController(IHttpContextAccessor httpContextAccessor, IWebHostEnvironment hostingEnvironment)
        {
            root = hostingEnvironment;

            DB = httpContextAccessor.HttpContext.User.FindFirstValue("aiwDB");
            UserID = Convert.ToInt32(httpContextAccessor.HttpContext.User.FindFirstValue("uID"));
            xLan = httpContextAccessor.HttpContext.User.FindFirstValue("aiwLan");
            Subscribe = Guid.Parse(httpContextAccessor.HttpContext.User.FindFirstValue("aiwSubscribe"));
            CID = httpContextAccessor.HttpContext.User.FindFirstValue("aiwCID");
        }

        [HttpGet]
        public JsonResult Employee(DataSourceLoadOptions loadOptions)
        {
            return Json(DataSourceLoader.Load(new CLiHR.Cards.Employee().GetList(DB), loadOptions));
        }

        [HttpPost]
        public JsonResult UpdateEmployee(CLiHR.Cards.Employee data)
        {
            try
            {
                if (data.Key == null)
                {
                    Guid? Key;
                    Key= CLiHR.Cards.Employee.Insert(DB, data, Subscribe);
                    if (data.SelfService == false)
                    {
                        string pass = CLiCore.iRandom.RandomDigits(6);
                        string message = System.IO.File.ReadAllText(Path.Combine(root.WebRootPath, "PL", "Whatsapp-ar.txt"));
                        message = message.Replace("##Username", data.Mobile);
                        message = message.Replace("##ComID", CID);
                        message = message.Replace("##Email", data.Email);
                        message = message.Replace("##Password", pass);
                        SendWhatsSelfService(data.Mobile, message);
                        CLiHR.Cards.Employee.UpdatePassword(Key, pass);
                    }
                }
                else
                {
                    CLiHR.Cards.Employee.Update(DB, data, Subscribe);
               
                }
                return Json(true);
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }

        [HttpDelete]
        public JsonResult DeleteEmployee(Guid? Key)
        {
            int res = CLiHR.Cards.Employee.Delete(DB, Key);
            return Json(res);
        }

        [HttpPut]
        public JsonResult UpdateEmployees(Guid? key, string values)
        {
            Guid? Subscribe = Guid.Parse(User.FindFirstValue("aiwSubscribe"));
            CLiHR.Cards.Employee employee = new CLiHR.Cards.Employee().GetItem(DB, key);

            JsonConvert.PopulateObject(values, employee);
            CLiHR.Cards.Employee.Update(DB, employee, Subscribe);
            return Json(Ok());
        }
        [HttpGet]
        public JsonResult Department(DataSourceLoadOptions loadOptions)
        {
            return Json(DataSourceLoader.Load(new CLiHR.Cards.Department().GetList(DB), loadOptions));
        }

        [HttpPost]
        public JsonResult UpdateDepartment(CLiHR.Cards.Department data)
        {
            if (data.Key == null)
            {
                CLiHR.Cards.Department.Insert(DB, data);
            }
            else
            {
                CLiHR.Cards.Department.Update(DB, data);
            }
            return Json(true);
        }

        [HttpDelete]
        public JsonResult DeleteDepartment(Guid? Key)
        {
            int res = CLiHR.Cards.Department.Delete(DB, Key);
            return Json(res);
        }


        [HttpGet]
        public JsonResult JobTitle(DataSourceLoadOptions loadOptions)
        {
            return Json(DataSourceLoader.Load(new CLiHR.Cards.JobTitle().GetList(DB), loadOptions));
        }

        [HttpPost]
        public JsonResult UpdateJobTitle(CLiHR.Cards.JobTitle data)
        {
            if (data.Key == null)
            {
                CLiHR.Cards.JobTitle.Insert(DB, data);
            }
            else
            {
                CLiHR.Cards.JobTitle.Update(DB, data);
            }
            return Json(true);
        }

        [HttpDelete]
        public JsonResult DeleteJobTitle(Guid? Key)
        {
            int res = CLiHR.Cards.JobTitle.Delete(DB, Key);
            return Json(res);
        }

        [HttpGet]
        public JsonResult BankNames(DataSourceLoadOptions loadOptions)
        {
            return Json(DataSourceLoader.Load(new CLiHR.Cards.BankNames().GetList(DB), loadOptions));
        }

        [HttpPost]
        public JsonResult UpdateBankNames(CLiHR.Cards.BankNames data)
        {
            if (data.Key == null)
            {
                CLiHR.Cards.BankNames.Insert(DB, data);
            }
            else
            {
                CLiHR.Cards.BankNames.Update(DB, data);
            }
            return Json(true);
        }

        [HttpDelete]
        public JsonResult DeleteBankNames(Guid? Key)
        {
            int res = CLiHR.Cards.BankNames.Delete(DB, Key);
            return Json(res);
        }

        [HttpGet]
        public JsonResult Biometric(DataSourceLoadOptions loadOptions)
        {
            return Json(DataSourceLoader.Load(new CLiHR.Cards.Biometric().GetList(DB), loadOptions));
        }

        [HttpPost]
        public JsonResult UpdateBiometric(CLiHR.Cards.Biometric data)
        {
            if (data.Key == null)
            {
                CLiHR.Cards.Biometric.Insert(DB, data);
            }
            else
            {
                CLiHR.Cards.Biometric.Update(DB, data);
            }
            return Json(true);
        }

        [HttpDelete]
        public JsonResult DeleteBiometric(Guid? Key)
        {
            int res = CLiHR.Cards.Biometric.Delete(DB, Key);
            return Json(res);
        }
        //Shift
        [HttpGet]
        public JsonResult ShiftCardList(DataSourceLoadOptions loadOptions)
        {
            return Json(DataSourceLoader.Load(new CLiHR.Cards.ShiftList().GetList(DB), loadOptions));
        }
        [HttpDelete]
        public void DeleteShift(string key)
        {
            CLiHR.Cards.ShiftList.Delete(DB, key);
        }
        [HttpGet]
        public JsonResult GetListShiftCard(DataSourceLoadOptions loadOptions)
        {
            return Json(DataSourceLoader.Load(new CLiHR.Cards.ShiftList().GetList(DB), loadOptions));
        }
        [HttpPost]
        public JsonResult InsertShiftCard(string values)
        {
            var cls = new CLiHR.Cards.ShiftList();
            JsonConvert.PopulateObject(values, cls);
            CLiHR.Cards.ShiftList.Insert(DB, cls);
            return Json(Ok());
        }
        [HttpPut]
        public JsonResult UpdateShiftCard(string Key, string values)
        {
            var cls = new CLiHR.Cards.ShiftList().GetItem(DB, Key);
            JsonConvert.PopulateObject(values, cls);
            CLiHR.Cards.ShiftList.Insert(DB, cls);
            return Json(Ok());
        }
        [HttpDelete]
        public JsonResult DeleteShiftCard(string key)
        {
            CLiHR.Cards.ShiftList.Delete(DB, key);
            return Json(true);
        }
        [HttpPost]
        public JsonResult SaveShift(CLiHR.Cards.Shift data)
        {
            CLiHR.Cards.Shift.Insert(DB, data);
            return Json(true);
        }




        //Categories
        [HttpGet]
        public JsonResult CatRequest(DataSourceLoadOptions loadOptions)
        {
            return Json(DataSourceLoader.Load(new CLiHR.Categories.Request().GetList(DB), loadOptions));
        }

        [HttpPost]
        public JsonResult UpdateCatRequest(CLiHR.Categories.Request data)
        {
            if (data.Key == null)
            {
                CLiHR.Categories.Request.Insert(DB, data);
            }
            else
            {
                CLiHR.Categories.Request.Update(DB, data);
            }
            return Json(true);
        }

        [HttpDelete]
        public JsonResult DeleteCatRequest(Guid? Key)
        {
            int res = CLiHR.Categories.Request.Delete(DB, Key);
            return Json(res);
        }

        [HttpGet]
        public JsonResult Reward(DataSourceLoadOptions loadOptions)
        {
            return Json(DataSourceLoader.Load(new CLiHR.Categories.Reward().GetList(DB), loadOptions));
        }

        [HttpPost]
        public JsonResult UpdateReward(CLiHR.Categories.Reward data)
        {
            if (data.Key == null)
            {
                CLiHR.Categories.Reward.Insert(DB, data);
            }
            else
            {
                CLiHR.Categories.Reward.Update(DB, data);
            }
            return Json(true);
        }

        [HttpDelete]
        public JsonResult DeleteReward(Guid? Key)
        {
            int res = CLiHR.Categories.Reward.Delete(DB, Key);
            return Json(res);
        }

        [HttpGet]
        public JsonResult Penalty(DataSourceLoadOptions loadOptions)
        {
            return Json(DataSourceLoader.Load(new CLiHR.Categories.Penalty().GetList(DB), loadOptions));
        }

        [HttpPost]
        public JsonResult UpdatePenalty(CLiHR.Categories.Penalty data)
        {
            if (data.Key == null)
            {
                CLiHR.Categories.Penalty.Insert(DB, data);
            }
            else
            {
                CLiHR.Categories.Penalty.Update(DB, data);
            }
            return Json(true);
        }

        [HttpDelete]
        public JsonResult DeletePenalty(Guid? Key)
        {
            int res = CLiHR.Categories.Penalty.Delete(DB, Key);
            return Json(res);
        }

        [HttpGet]
        public JsonResult Location(DataSourceLoadOptions loadOptions)
        {
            return Json(DataSourceLoader.Load(new CLiHR.Cards.Location().GetList(DB), loadOptions));
        }

        [HttpPost]
        public JsonResult UpdateLocation(CLiHR.Cards.Location data)
        {
            if (data.Key == null)
            {
                CLiHR.Cards.Location.Insert(DB, data);
            }
            else
            {
                CLiHR.Cards.Location.Update(DB, data);
            }
            return Json(true);
        }

        [HttpDelete]
        public JsonResult DeleteLocation(Guid? Key)
        {
            int res = CLiHR.Cards.Location.Delete(DB, Key);
            return Json(res);
        }


        private void  SendWhatsSelfService(string Mobile,string Message)
        {
            Thread T1 = new Thread(async delegate ()
            {
               

                CLiCore.PL.Whatsapp whatsapp = new CLiCore.PL.Whatsapp().GetItem();
                if (whatsapp == null || whatsapp.APIURL == "" || whatsapp.Status == false)
                    return ;

                try
                {
                    var phoneUtil = PhoneNumbers.PhoneNumberUtil.GetInstance();
                    var phoneNumber = phoneUtil.Parse(Mobile, "SA");
                    var internationalNumber = phoneUtil.Format(phoneNumber, PhoneNumbers.PhoneNumberFormat.INTERNATIONAL);
                    string mob = internationalNumber.Replace(" ", "");
                    var url = "https://api.ultramsg.com/" + whatsapp.InstanceID + "/messages/chat";
                    var client = new RestClient(url);

                    var request = new RestRequest(url, Method.Post);
                    request.AddHeader("content-type", "application/x-www-form-urlencoded");
                    request.AddParameter("token", whatsapp.Token);
                    request.AddParameter("to", mob);
                    request.AddParameter("body", Message);


                    RestResponse response = await client.ExecuteAsync(request);
                    var output = response.Content;
                    return;
                }
                catch (Exception ex)
                {
                    return  ;
                }

              
            });
            T1.Start();

          
        }
    }
}
