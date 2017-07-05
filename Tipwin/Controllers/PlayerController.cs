using CaptchaMvc.HtmlHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using Tipwin.Hash;
using Tipwin.Models;
using Tipwin.Repository;
using Tipwin.ViewModel;


namespace Tipwin.Controllers
{

    public class PlayerController : Controller
    {

        PlayerDb db = new PlayerDb();
        List<Player> listPlayers = new List<Player>();

        public ActionResult GetPlayer()
        {
            db = new PlayerDb();

            List<Player> listPlayers = new List<Player>();
            listPlayers = db.GetPlayers();
            return View(listPlayers);
        }



        public ActionResult Create()
        {

            return View();
        }


        [HttpPost]
        public ActionResult Create(Player player)
        {
            db = new PlayerDb();
            List<Player> listPlayer = new List<Player>();


            if (ModelState.IsValid && this.IsCaptchaValid("Correct"))
            {
                try
                {

                    db.InsertPlayer(player);
                    WebMail.Send(player.Email, "Login Link", "http://localhost:60387/Player/Prijava");
                    TempData["novikorisnik"] = player.KorisnickoIme + " je uspješno registriran/a. ";
                    TempData["info"] = "Vaš korisnički račun je uspješno kreiran. Poslana je poruka za aktivaciju na vašu el. poštu";


                    return RedirectToAction("EmailConfirmation", listPlayer);

                }
                catch (Exception)
                {
                    ViewBag.ErrorMessage = "Greška: captcha nije validna.";

                    ModelState.AddModelError("", "Korisnik je već registriran. Korisničko ime ili el. pošta su zauzeti");
                    return View();
                }

            }
            else if (!this.IsCaptchaValid("is not valid"))
            {
                ModelState.AddModelError("", "Captcha not valid");
                return View(player);
            }
            else if (player.KorisnickoIme == player.Lozinka)
            {
                ModelState.AddModelError("", "");
                return View(player);
            }
            else if (player.Lozinka == player.Email)
            {
                ModelState.AddModelError("", "Lozinka mora biti različita od el. pošte");
                return View(player);
            }

            else if (player.Email != player.EmailPonovo)
            {
                ModelState.AddModelError("", "Pogrešno unesena el. pošta");
                return View(player);
            }

            else
            {
                ModelState.AddModelError("", "Neuspješno uneseni podaci!Player nije dodan u bazu");
                return View(player);
            }
        }


        public ActionResult SendPasswordResetEmail(string ToEmail, string UserName, string UniqueId)
        {
            PlayerDb db = new PlayerDb();
            var user = db.Validate();
            var username = (from i in user where i.KorisnickoIme == UserName select i.KorisnickoIme).FirstOrDefault();

            //var token = WebSecurity.GeneratePasswordResetToken(UserName);
            var resetLink = "<a href='" + Url.Action("ResetPassword", "Player", new { un = UserName }, "http") + "'>Reset Password</a>";

            //send mail
            string subject = "Password Reset Token";
            string body = "<b>Please find the Password Reset Token.</b><br/>The below link will be valid till 30 mins<br/>" + resetLink; //edit it
            try
            {
                SendPasswordResetEmail(username, subject, body);
                TempData["Message"] = "Mail Sent.";
            }
            catch (Exception ex)
            {
                TempData["Message"] = "Error occured while sending email." + ex.Message;
            }


            MailMessage mailMessage = new MailMessage("danijel147258@gmail.com", ToEmail);

            StringBuilder sbEmailBody = new StringBuilder();
            sbEmailBody.Append("Dear " + UserName + "<br/> <br/>");
            sbEmailBody.Append("Please click on the following link to reset your password");
            sbEmailBody.Append("<br/>");
            sbEmailBody.Append("http://localhost:60387/Player/SendPasswordResetEmail?uid=" + UniqueId);
            sbEmailBody.Append("<br/>");
            sbEmailBody.Append("<b>Tipwin<b/>");

            mailMessage.IsBodyHtml = true;

            mailMessage.Body = sbEmailBody.ToString();
            mailMessage.Subject = "Reset Your Password";
            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);

            smtpClient.Credentials = new System.Net.NetworkCredential()
            {
                UserName = "danijel147258@gmail.com",
                Password = "Domnet123-"
            };
            smtpClient.EnableSsl = true;
            smtpClient.Send(mailMessage);

            return View();
        }


        //public static IEnumerable<SelectListItem> GetCountries()
        //{
        //    RegionInfo country = new RegionInfo(new CultureInfo("en-US", false).LCID);
        //    List<SelectListItem> countryNames = new List<SelectListItem>();
        //    string cult = CultureInfo.CurrentCulture.EnglishName;
        //    string count = cult.Substring(cult.IndexOf('(') + 1, cult.LastIndexOf(')') - cult.IndexOf('(') - 1);

        //    foreach (CultureInfo cul in CultureInfo.GetCultures(CultureTypes.SpecificCultures))
        //    {
        //        country = new RegionInfo(new CultureInfo(cul.Name, false).LCID);
        //        countryNames.Add(new SelectListItem()
        //        {
        //            Text = country.DisplayName,
        //            Value = country.DisplayName,
        //            Selected = count == country.EnglishName
        //        });
        //    }
        //    IEnumerable<SelectListItem> nameAdded = countryNames.GroupBy(x => x.Text).Select(x => x.FirstOrDefault()).ToList<SelectListItem>().OrderBy(x => x.Text);
        //    return nameAdded;
        //}
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(string empty)
        {
            //if (this.IsCaptchaValid("Captcha is not valid"))
            //{
            //    return View();
            //}
            //ViewBag.ErrorMessage = "Error: captcha is not valid.";
            return View();
        }


        public ActionResult EmailConfirmation()
        {
            return View();
        }


        //public ActionResult Verify(string id)
        //{
        //    if (string.IsNullOrEmpty(id) || (!Regex.IsMatch(id, @"[0-9a-f]{8}\-
        //                             ([0-9a-f]{4}\-){3}[0-9a-f]{12}")))
        //    {
        //        ViewBag.Msg = "Not Good!!!";
        //        return View();
        //    }

        //    else
        //    {
        //        var user = Membership.GetUser(new Guid(id));

        //        if (!user.IsApproved)
        //        {
        //            user.IsApproved = true;
        //            Membership.UpdateUser(user);
        //            FormsAuthentication.SetAuthCookie(user.UserName, false);
        //            return RedirectToAction("EmailConfirmation", "Player");
        //        }
        //        else
        //        {
        //            FormsAuthentication.SignOut();
        //            ViewBag.Msg = "Account Already Approved";
        //            return RedirectToAction("Login");
        //        }
        //    }
        //}
        public ActionResult ForgotPassword()
        {

            return View();
        }

        public ActionResult ForgotUserName()
        {
            Player player = new Player();
            return View(player);

        }

        [HttpPost]
        public ActionResult ForgotUserName(Player player)
        {

            List<UserNameViewModel> userlistPlayer = new List<UserNameViewModel>();
            userlistPlayer = db.ForgotUser();

            try
            {
                if ((userlistPlayer.Any(s => s.Email == player.Email && s.DatumRodjenja == player.DatumRodjenja)) && this.IsCaptchaValid("Correct"))
                {
                    Guid newGuid = new Guid();
                    Random r = new Random(99);
                    // newGuid = r.Next();

                    WebMail.Send(player.Email, "List", $"http://localhost:60387/Player/GetPlayers={newGuid}");
                    TempData["ConfirmMessage"] = $"U el. pošti potvrdite korisničko ime {player.KorisnickoIme}";
                    return RedirectToAction("Login");

                }
                else if (!this.IsCaptchaValid("invalid captcha"))
                {
                    ModelState.AddModelError("", "Captcha is not valid");
                    return View();
                }
                else
                {
                    ViewBag.DataError = "Nema podataka u bazi. Prijavite se.";
                    return View();
                }


            }
            catch (Exception ex)
            {
                TempData["send"] = "Not existing data " + ex.Message;
                return View();
            }
        }

        [AllowAnonymous]
        public ActionResult Login()
        {
            Player player = new Player();
            return View(player);
        }


        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(AccountViewModel playervm)
        {

            List<AccountViewModel> listPlayer = new List<AccountViewModel>();
            listPlayer = db.Validate();


            try
            {

                if (listPlayer.Any(s => s.KorisnickoIme == playervm.KorisnickoIme && HashedPassword.Confirm(playervm.Lozinka, s.Lozinka, HashedPassword.SupportedHashAlgorithms.SHA256))
                    && !MvcApplication.LoginCounter.ReturnIfLockedUsername(playervm.KorisnickoIme))
                {


                    //listPlayer.SingleOrDefault(s => s.KorisnickoIme == playervm.KorisnickoIme && s.Lozinka == playervm.Lozinka);
                    // System.Web.Security.FormsAuthentication.SetAuthCookie(playervm.KorisnickoIme, false);
                    Session["korisnickoIme"] = playervm.KorisnickoIme.ToString();
                    Session["lozinka"] = playervm.Lozinka.ToString();

                    MvcApplication.LoginCounter.ClearLockedLoginsAfterSuccessfull(playervm.KorisnickoIme);


                    return RedirectToAction("LoginSuccess");

                }
                else
                {
                    MvcApplication.LoginCounter.CheckLogin(playervm.KorisnickoIme);
                    if (MvcApplication.LoginCounter.ReturnIfLockedUsername(playervm.KorisnickoIme))
                    {
                        ViewBag.ErrorMessageLogin = MvcApplication.LoginCounter.LoginErrorMessage;
                        return View();
                    }

                }
                return View();

            }
            catch (Exception ex)
            {
                TempData["Message"] = "Login failed.Error - " + ex.Message;
            }
            return View();
        }






        public ActionResult LoginSuccess()
        {
            AccountViewModel playervm = new AccountViewModel();

            //if (Request.Cookies["cookie"] != null)
            //{
            //    Response.Cookies["cookie"].Expires = DateTime.Now.AddMinutes(1);
            //}


            HttpCookie hc = Request.Cookies["cookie"];
            if (hc != null)
            {
                playervm.KorisnickoIme = hc["cookie"];
                playervm.Lozinka = hc["cookie"];

                Response.Cookies["cookie"].Expires = DateTime.Now.AddMinutes(1);
                Response.Cookies.Add(hc);

                Response.Redirect("LoginSuccess");
            }
            if (Session["korisnickoIme"] != null)
            {
                TempData["korisnickoime"] = "Login Success";
                return View();
            }
            else
            {
                return RedirectToAction("Login");
            }



        }

        public ActionResult Logout()
        {
            if (Session != null)
            {
                Session.Remove("korisnickoIme");
                Session.Remove("lozinka");

            }

            return View();
        }

        public ActionResult LockedAccount()
        {
            return View();
        }

        public ActionResult Delete(int id)
        {
            db = new PlayerDb();
            if (db.DeletePlayer(id))
            {
                return RedirectToAction("GetPlayer");

            }


            return Content("Greška pri brisanju iz baze");
        }

        public JsonResult UsernameExists(string username)
        {
            PlayerDb db = new PlayerDb();


            if (ModelState.IsValid)
            {
                List<AccountViewModel> lista = new List<AccountViewModel>();

                lista = db.Validate();
                foreach (var item in lista)
                {
                    if (String.Equals(username, Convert.ToString(item.KorisnickoIme), StringComparison.OrdinalIgnoreCase))
                    {
                        return Json(false);
                    }
                }
            }
            return Json(true, JsonRequestBehavior.AllowGet);
        }


    }
}
