using CaptchaMvc.HtmlHelpers;
using System;
using System.Collections.Generic;
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

            var korisnik = (!listPlayer.Exists(s => s.KorisnickoIme.Contains(player.KorisnickoIme))
            || !listPlayer.Exists(s => s.Lozinka.Contains(player.Lozinka)));




            if (!korisnik)
            {
                Session["korisnik"] = player.KorisnickoIme + " je uspjesno prijavljen/a";





                //FormsAuthenticationTicket fat = new FormsAuthenticationTicket(1, "Player", DateTime.Now, DateTime.Now.AddMinutes(2), false, JsonConvert.SerializeObject(korisnik));
                //HttpCookie hc = new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(fat));
                //hc.Expires = DateTime.Now.AddMinutes(2);
                //Response.Cookies.Add(hc);


                return View("LoginSuccess", listPlayer);
            }




            else
            {
                ModelState.AddModelError("", "Pogrešno korisničko ime ili lozinka");
                return View();

            }


        }




        public ActionResult LoginSuccess()
        {
            Player player = new Player();

            ViewBag.Message = "Login Success";


            // var timeSpan = new TimeSpan(0, 0, 20);
            if (TimeSpan.FromSeconds(20) != null)
            {
                return RedirectToAction("Create");
            }
            return View(player);
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




    }
}
