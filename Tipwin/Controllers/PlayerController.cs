using CaptchaMvc.HtmlHelpers;
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

            ViewBag.ErrorMessage = "Greška: captcha nije validna.";
            if (ModelState.IsValid && this.IsCaptchaValid("Captcha nije validna"))
            {
                WebMail.Send(player.Email, "Login Link", "https://app1.4tipnet.com/hr/registracija");

                db.InsertPlayer(player);
                ViewBag.Message = player.KorisnickoIme + " " + player.Prezime + " je uspješno registriran. ";
                return RedirectToAction("Login");
            }
            else if (player.KorisnickoIme == player.Lozinka)
            {
                ModelState.AddModelError("", "");
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
        //public JsonResult IsUserNameAvailable(string korisnickoIme)
        //{
        //    return Json(!db.GetPlayers().Any(user => user.KorisnickoIme == korisnickoIme), JsonRequestBehavior.AllowGet);

        //}


        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Player player)
        {



            if (player != null)
            {
                Session["korisnik"] = player.KorisnickoIme;
                return View("LoginSuccess");
            }

            return View();



            // db = player.KorisnickoIme.Any(korisnickoIme));
            //var user = IsUserNameAvailable(korisnickoIme);

            //Session["korisnik"] = player.KorisnickoIme;
            //return RedirectToAction("LoginSuccess", user);




            //List<Player> players = db.GetPlayers();

            //if (players.Any(x => x.KorisnickoIme != null))
            //{
            //    return View(players);
            //}




            //var userdetail = db.GetPlayers();
            //if (!userdetail.Exists(x=>x.KorisnickoIme))
            //{
            //    Session["korisnik"] = player.KorisnickoIme;
            //    return View("LoginSuccess");



            //var newUser = db.CheckRecord(player);

            //if (newUser)
            //{
            //    Session["korisnik"] = player.KorisnickoIme;
            //    return View("LoginSuccess");
            //}







        }




        public ActionResult LoginSuccess()
        {
            Player player = new Player();
            ViewBag.Message = "Login Success";
            return View(player);
        }

        public ActionResult Logout()
        {
            Session.Remove("korisnicko_ime");
            return RedirectToAction("GetPlayer");
        }




    }
}
