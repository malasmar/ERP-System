using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Platxe.Controllers.api
{
    [Produces("application/json")]
    [Route("api/[Controller]/[Action]")]
    public class apiaccountController : Controller
    {
        private readonly IWebHostEnvironment root;

        public apiaccountController(IWebHostEnvironment hostingEnvironment)
        {
            root = hostingEnvironment;

        }

        private void LoadSubscribeModuals(List<Claim> claims, Guid? Subscribe)
        {
            List<string> values = new List<string>();
            values = CLiCore.Account.core.SubscribeModuals(Subscribe);
            foreach (string item in values)
            {
                Claim claim = new Claim(ClaimTypes.Role, item);
                claims.Add(claim);
            }
        }
        private void LoadUserModuals(List<Claim> claims, string DB, Guid? Key)
        {
            List<string> values = new List<string>();
            values = CLiCore.Account.core.UserAccessPermissions(DB, Key);
            foreach (string item in values)
            {
                Claim claim = new Claim(ClaimTypes.Role, item);
                claims.Add(claim);
            }
        }
        [HttpPost]
        public async Task<JsonResult> Login(string ClientID, string Username, string Password)
        {
            string Lan;
            Lan = Request.Cookies["PlatxeLan"] ?? "en";
            CLiCore.Account.Subscribe subscribe = new CLiCore.Account.Subscribe().GetItem(ClientID);
            if (subscribe == null || subscribe.Key == null)
                return Json("NoSubscribe");
            CLiCore.Account.User user = new CLiCore.Account.User().GetList(Username, subscribe.Key);
            if (subscribe == null || subscribe.Key == null)
            {
                return Json("NoSubscribe");
            }
            else
            {
                if (subscribe.EndDate <= DateTime.Now)
                {
                    return Json("Expiry");
                }
                else
                {
                    if (user != null && user.Key != null)
                    {
                        if (user.Password == Password)
                        {
                            var claims = new List<Claim>
                            {
                        new Claim(ClaimTypes.NameIdentifier,user.Key.ToString()),
                        new Claim(ClaimTypes.Name,user.Name1.ToString()),
                        new Claim("aiwSubscribe", user.Subscribe.ToString()),
                        new Claim("aiwDB", user.Database.ToString()),
                        new Claim("aiwDBKey", user.DbKey.ToString()),
                        new Claim("aiwDBD1", user.DbDisplay1.ToString()),
                        new Claim("aiwDBD2", user.DbDisplay2.ToString()),
                        new Claim("aiwCID", subscribe.ID),
                        new Claim("aiwYear", DateTime.Now.Year.ToString()),
                        new Claim("uID", user.No.ToString()),
                        new Claim("eID", user.No.ToString()),
                        new Claim("eName1", user.Name1),
                        new Claim("eName2",user.Name2),
                        new Claim(ClaimTypes.Email,user.Email),
                        new Claim("Key",user.Key.ToString()),
                        new Claim("aiwLan",Lan),
                        new Claim("LoginDate",DateTime.Now.ToString("dddd ,MMM dd.MM.yyyy HH:mm"))
                    };
                            //Add admin role
                            if (user.IsAdmin)
                            {
                                Claim claim = new Claim(ClaimTypes.Role, "PL-Admin");
                                claims.Add(claim);
                            }
                            LoadSubscribeModuals(claims, user.Subscribe);
                            LoadUserModuals(claims, user.Database, user.Key);
                            ClaimsIdentity userIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                            ClaimsPrincipal principal = new ClaimsPrincipal(userIdentity);
                            await HttpContext.SignInAsync(principal);
                            return Json("Ok");
                        }
                        else
                        {
                            return Json("Password");
                        }
                    }
                    else
                    {
                        return Json("NoUser");
                    }
                }
            }
        }

        [HttpPost]
        public async Task<JsonResult> ChangeLanguage(string Lan)
        {
            CookieOptions option = new CookieOptions();
            if (option.Expires.HasValue == false)
                option.Expires = DateTime.Now.AddDays(10);
            Response.Cookies.Append("PlatxeLan", Lan, option);


            CLiCore.Account.User user = new CLiCore.Account.User().GetList(User.FindFirstValue(ClaimTypes.Email), Guid.Parse(User.FindFirstValue("aiwSubscribe")));
            string db = HttpContext.User.FindFirstValue("aiwDB");
            var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.NameIdentifier,user.Key.ToString()),
                        new Claim(ClaimTypes.Name,user.Name1.ToString()),
                        new Claim("aiwSubscribe", user.Subscribe.ToString()),
                        new Claim("aiwDB",  db),
                        new Claim("aiwDBKey", HttpContext.User.FindFirstValue("aiwDBKey")),
                        new Claim("aiwDBD1", HttpContext.User.FindFirstValue("aiwDBD1")),
                        new Claim("aiwDBD2", HttpContext.User.FindFirstValue("aiwDBD2") ),
                        new Claim("aiwYear", HttpContext.User.FindFirstValue("aiwYear")),
                        new Claim("aiwCID", HttpContext.User.FindFirstValue("aiwCID")),
                        new Claim("uID", user.No.ToString()),
                        new Claim("eID", user.No.ToString()),
                        new Claim("eName1", user.Name1),
                        new Claim("eName2",user.Name2),
                        new Claim(ClaimTypes.Email,user.Email),
                        new Claim("Key",user.Key.ToString()),
                        new Claim("aiwLan",Lan),
                        new Claim("LoginDate",DateTime.Now.ToString("dddd ,MMM dd.MM.yyyy HH:mm"))
                    };
            if (user.IsAdmin)
            {
                Claim claim = new Claim(ClaimTypes.Role, "CI-Admin");
                claims.Add(claim);
            }
            LoadSubscribeModuals(claims, user.Subscribe);
            LoadUserModuals(claims, db, user.Key);
            await HttpContext.SignOutAsync();
            ClaimsIdentity userIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            ClaimsPrincipal principal = new ClaimsPrincipal(userIdentity);
            await HttpContext.SignInAsync(principal);
            return Json(true);
        }
        [HttpPost]
        public async Task<JsonResult> ChangeFile(Guid? Target)
        {
            CLiCore.Account.Selections.Database db = new CLiCore.Account.Selections.Database().GetItem(Target);
            CLiCore.Account.User user = new CLiCore.Account.User().GetList(User.FindFirstValue(ClaimTypes.Email), Guid.Parse(User.FindFirstValue("aiwSubscribe")));

            var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.NameIdentifier,user.Key.ToString()),
                        new Claim(ClaimTypes.Name,user.Name1.ToString()),
                        new Claim("aiwSubscribe", user.Subscribe.ToString()),
                        new Claim("aiwDB",  db.DatabaseName),
                        new Claim("aiwDBKey", db.Key.ToString()),
                        new Claim("aiwDBD1", db.Name1),
                        new Claim("aiwDBD2", db.Name2 ),
                         new Claim("aiwCID", HttpContext.User.FindFirstValue("aiwCID")),
                        new Claim("aiwYear", HttpContext.User.FindFirstValue("aiwYear")),
                        new Claim("uID", user.No.ToString()),
                        new Claim("eID", user.No.ToString()),
                        new Claim("eName1", user.Name1),
                        new Claim("eName2",user.Name2),
                        new Claim(ClaimTypes.Email,user.Email),
                        new Claim("Key",user.Key.ToString()),
                        new Claim("aiwLan",HttpContext.User.FindFirstValue("aiwLan")),
                        new Claim("LoginDate",DateTime.Now.ToString("dddd ,MMM dd.MM.yyyy HH:mm"))
                    };
            if (user.IsAdmin)
            {
                Claim claim = new Claim(ClaimTypes.Role, "CI-Admin");
                claims.Add(claim);
            }
            LoadSubscribeModuals(claims, user.Subscribe);
            LoadUserModuals(claims, db.DatabaseName, user.Key);
            await HttpContext.SignOutAsync();
            ClaimsIdentity userIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            ClaimsPrincipal principal = new ClaimsPrincipal(userIdentity);
            await HttpContext.SignInAsync(principal);
            return Json(true);
        }
        [HttpPost]
        public async Task<JsonResult> ChangeYear(int Year)
        {

            CLiCore.Account.User user = new CLiCore.Account.User().GetList(User.FindFirstValue(ClaimTypes.Email), Guid.Parse(User.FindFirstValue("aiwSubscribe")));
            string db = HttpContext.User.FindFirstValue("aiwDB");
            var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.NameIdentifier,user.Key.ToString()),
                        new Claim(ClaimTypes.Name,user.Name1.ToString()),
                        new Claim("aiwSubscribe", user.Subscribe.ToString()),
                        new Claim("aiwDB",  HttpContext.User.FindFirstValue("aiwDB")),
                        new Claim("aiwDBKey", HttpContext.User.FindFirstValue("aiwDBKey")),
                        new Claim("aiwDBD1", HttpContext.User.FindFirstValue("aiwDBD1")),
                        new Claim("aiwDBD2", HttpContext.User.FindFirstValue("aiwDBD2") ),
                         new Claim("aiwCID", HttpContext.User.FindFirstValue("aiwCID")),
                        new Claim("aiwYear", Year.ToString()),
                        new Claim("uID", user.No.ToString()),
                        new Claim("eID", user.No.ToString()),
                        new Claim("eName1", user.Name1),
                        new Claim("eName2",user.Name2),
                        new Claim(ClaimTypes.Email,user.Email),
                        new Claim("Key",user.Key.ToString()),
                        new Claim("aiwLan",HttpContext.User.FindFirstValue("aiwLan")),
                        new Claim("LoginDate",DateTime.Now.ToString("dddd ,MMM dd.MM.yyyy HH:mm"))
                    };
            if (user.IsAdmin)
            {
                Claim claim = new Claim(ClaimTypes.Role, "CI-Admin");
                claims.Add(claim);
            }
            LoadSubscribeModuals(claims, user.Subscribe);
            LoadUserModuals(claims, db, user.Key);
            await HttpContext.SignOutAsync();
            ClaimsIdentity userIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            ClaimsPrincipal principal = new ClaimsPrincipal(userIdentity);
            await HttpContext.SignInAsync(principal);
            return Json(true);
        }


        [HttpPost]
        public JsonResult SendPasswordRecovery(string ClientID, string UserName)
        {
            CLiCore.Account.Subscribe subscribe = new CLiCore.Account.Subscribe().GetItem(ClientID);
            if (subscribe == null || subscribe.Key == null)
                return Json("NoSubscribe");

            CLiCore.Account.User user = new CLiCore.Account.User().GetList(UserName, subscribe.Key);
            if (subscribe == null || subscribe.Key == null)
            {
                return Json("NoSubscribe");
            }
            else
            {
                if (subscribe.EndDate <= DateTime.Now)
                {
                    return Json("Expiry");
                }
                else
                {
                    if (user != null && user.Key != null)
                    {
                        CLiCore.Platx.PlatxeMails sender = new CLiCore.Platx.PlatxeMails().GetItem("no-reply");

                        if (CLiCore.iCore.IsValidEmail(user.Email) == false)
                        {
                            return Json("NoUser");
                        }
                        Thread T1 = new Thread(delegate ()
                        {
                            StringBuilder strBody = new StringBuilder();
                            string Pathx = root.WebRootPath + @"/mails/Password.html";
                            FileStream fileStream = new FileStream(Pathx, FileMode.Open, FileAccess.Read);
                            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
                            {
                                string line;
                                while ((line = streamReader.ReadLine()) != null)
                                {

                                    strBody.AppendLine(line);
                                }

                                // strBody.Replace("@Password", Password);
                            }
                            using (var smtp = new SmtpClient())
                            {
                                smtp.UseDefaultCredentials = sender.UseDefaultCredentials;
                                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                                smtp.Port = sender.Port;
                                smtp.EnableSsl = sender.EnableSSL;
                                var message = new MailMessage();

                                message.To.Add(new MailAddress(user.Email));
                                message.From = new MailAddress(sender.Email, "Platx Support");  // replace with valid value
                                message.Subject = "Password Recovery";
                                message.Body = strBody.ToString();
                                message.IsBodyHtml = sender.IsBodyHtml;
                                message.Priority = MailPriority.High;
                                message.BodyEncoding = System.Text.Encoding.GetEncoding("utf-8");
                                var credential = new NetworkCredential
                                {
                                    UserName = sender.UserName,  // replace with valid value
                                    Password = sender.Password  // replace with valid value
                                };
                                smtp.Credentials = credential;
                                smtp.Host = sender.SMTP;
                                smtp.Send(message);
                            }
                        });
                        T1.Start();
                        return Json("Ok");
                    }
                    else
                    {
                        return Json("NoUser");
                    }
                }
            }


        }


    }
}
