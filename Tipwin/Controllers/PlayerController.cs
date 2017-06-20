using CaptchaMvc.HtmlHelpers;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using Tipwin.Models;
using Tipwin.Repository;


namespace Tipwin.Controllers
{

    public class PlayerController : Controller
    {
        PlayerDb db = new PlayerDb();
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

            var korisnik = (!listPlayer.Exists(s => s.KorisnickoIme.Contains(player.KorisnickoIme))
           || !listPlayer.Exists(s => s.Lozinka.Contains(player.Lozinka)));

            ViewBag.ErrorMessage = "Greška: captcha nije validna.";
            if (ModelState.IsValid && this.IsCaptchaValid("Captcha nije validna"))
            {
                WebMail.Send(player.Email, "Login Link", "https://app1.4tipnet.com/hr/registracija");

                db.InsertPlayer(player);
                TempData["novikorisnik"] = player.KorisnickoIme + " je uspješno registriran/a. ";
                return RedirectToAction("Login", listPlayer);
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
            else if (korisnik)
            {
                ModelState.AddModelError("", "Korisničko ime je zauzeto");
                return View(player);
            }

            else
            {
                ModelState.AddModelError("", "Neuspješno uneseni podaci!Player nije dodan u bazu");
                return View(player);
            }
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(string empty)
        {
            if (this.IsCaptchaValid("Captcha is not valid"))
            {
                return View();
            }
            ViewBag.ErrorMessage = "Error: captcha is not valid.";
            return View();
        }

        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }


        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Player player)
        {

            List<Player> listPlayer = new List<Player>();
            listPlayer = db.GetPlayers();

            var korisnik = (listPlayer.Exists(s => s.KorisnickoIme.Contains(player.KorisnickoIme))
            && listPlayer.Exists(s => s.Lozinka.Contains(player.Lozinka)));

            if (korisnik)
            {
                Response.Cookies["KorisnickoIme"].Value = player.KorisnickoIme;
                Response.Cookies["KorisnickoIme"].Expires = DateTime.Now.AddSeconds(40);
                Response.Cookies["Lozinka"].Value = player.Lozinka;
                Response.Cookies["Lozinka"].Expires = DateTime.Now.AddSeconds(40);

                if (Request.Cookies["KorisnickoIme"] != null)
                {
                    string cvalue = Request.Cookies["KorisnickoIme"].Value.ToString();
                    ViewData["Value"] = cvalue;
                }

                string cookieValue;
                if (Request.Cookies["cookie"] != null)
                {
                    cookieValue = Request.Cookies["cookie"].ToString();
                }
                else
                {
                    Response.Cookies["cookie"].Value = "cookie value";
                }

                ////Cookie
                //HttpCookie hc = new HttpCookie("userInfo", player.KorisnickoIme);
                ////Expire
                //hc.Expires = DateTime.Now.AddSeconds(15);
                ////Save data u Cookie
                //HttpContext.Response.SetCookie(hc);
                ////Get data iz Cookie
                //HttpCookie nc = Request.Cookies["userInfo"];
                //return View("LoginSuccess");

                //Cookie
                //HttpCookie hc = new HttpCookie("userInfo");
                //hc["KorisnickoIme"] = player.KorisnickoIme;
                //hc["Lozinka"] = player.Lozinka;

                //hc.Expires = DateTime.Now.AddSeconds(10);
                //Response.Cookies.Add(hc);

                //   Response.Redirect("Login");

                //Session
                Session["korisnickoime"] = player.KorisnickoIme.ToString();
                Session["lozinka"] = player.Lozinka.ToString();

                Session["korisnik"] = player.KorisnickoIme + " je uspjesno prijavljen/a";


                //FormsAuthenticationTicket fat = new FormsAuthenticationTicket(1, "Player", DateTime.Now, DateTime.Now.AddMinutes(2), false, JsonConvert.SerializeObject(korisnik));
                //HttpCookie hc = new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(fat));
                //hc.Expires = DateTime.Now.AddMinutes(2);
                //Response.Cookies.Add(hc);

                return View("LoginSuccess", listPlayer);
            }

            else
            {
                //int max = 3, count = 0;
                //while (max > count)
                //{
                //    count++;

                //    ViewBag.Count = $"broj pokušaja prijave {count}";
                //    db.InsertPlayer(player);
                //    return RedirectToAction("LoginInvalid", listPlayer);
                //}

                ModelState.AddModelError("", $"Pogrešno korisničko ime ili lozinka ");

                return View();

            }
        }

        public ActionResult LoginInvalid()
        {
            ViewBag.ErrorLogin = "Broj neuspjelih pokusaja logiranja ";
            return View();
        }





        public ActionResult LoginSuccess()
        {
            Player player = new Player();

            if (Request.Cookies["cookie"] != null)
            {
                Response.Cookies["cookie"].Expires = DateTime.Now.AddSeconds(20);
            }


            HttpCookie hc = Request.Cookies["userInfo"];
            if (hc != null)
            {
                player.KorisnickoIme = hc["KorisnickoIme"];
                player.Lozinka = hc["Lozinka"];


                Response.Cookies.Add(hc);

                Response.Redirect("LoginSuccess");
            }
            if (Session["korisnickoime"] != null)
            {
                ViewBag.Message = "Login Success";
                return View();
            }
            else
            {
                return RedirectToAction("Login");
            }




            // var timeSpan = new TimeSpan(0, 0, 20);
            //if (TimeSpan.FromSeconds(20) != null)
            //{
            //    return RedirectToAction("Create");
            //}
            //return View(player);
        }

        public ActionResult Logout()
        {
            Session.Remove("korisnik");
            return View();
        }

        public ActionResult LockedAccount()
        {
            return View();
        }


        //[HttpPost]
        //public JsonResult IsValidDateOfBirth(string dob)
        //{
        //    var min = DateTime.Now.Date.AddYears(-18);
        //    var max = DateTime.Now.Date.AddYears(-110);
        //    var msg = string.Format("Please enter a value between {0:dd/MM/yyyy} and {1:dd/MM/yyyy}", max, min);
        //    try
        //    {
        //        var date = DateTime.Parse(dob);
        //        if (date > min || date < max)
        //            return Json(msg);
        //        else
        //            return Json(true);
        //    }
        //    catch (Exception)
        //    {
        //        return Json(msg);
        //    }
        //}

        //public class ValidateDateRange : ValidationAttribute
        //{
        //    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        //    {
        //        // your validation logic
        //        if (value >= Convert.ToDateTime("01/10/2008") && value <= Convert.ToDateTime("01/12/2008"))
        //        {
        //            return ValidationResult.Success;
        //        }
        //        else
        //        {
        //            return new ValidationResult("Date is not in given range.");
        //        }
        //    }
        //}
    }
}
