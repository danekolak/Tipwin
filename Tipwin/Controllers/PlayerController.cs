﻿using CaptchaMvc.HtmlHelpers;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
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


        //public JsonResult IsUserExists(string UserName)
        //{
        //    //check if any of the UserName matches the UserName specified in the Parameter using the ANY extension method.  
        //    return Json(!listPlayers.Any(x => x.KorisnickoIme == UserName), JsonRequestBehavior.AllowGet);
        //}


        public ActionResult Create()
        {

            return View();
        }

        //[HttpPost]
        //public ActionResult Create(Player player)
        //{
        //    db = new PlayerDb();
        //    List<Player> listPlayer = new List<Player>();

        //    var korisnik = (!listPlayer.Exists(s => s.KorisnickoIme.Contains(player.KorisnickoIme))
        //   || !listPlayer.Exists(s => s.Lozinka.Contains(player.Lozinka)));

        //    ViewBag.ErrorMessage = "Greška: captcha nije validna.";
        //    if (ModelState.IsValid && this.IsCaptchaValid("Captcha nije validna"))
        //    {
        //        WebMail.Send(player.Email, "Login Link", "https://app1.4tipnet.com/hr/registracija");

        //        db.InsertPlayer(player);
        //        TempData["novikorisnik"] = player.KorisnickoIme + " je uspješno registriran/a. ";
        //        return RedirectToAction("Login", listPlayer);
        //    }

        //    else if (player.KorisnickoIme == player.Lozinka)
        //    {
        //        ModelState.AddModelError("", "");
        //        return View(player);
        //    }
        //    else if (player.Lozinka == player.Email)
        //    {
        //        ModelState.AddModelError("", "Lozinka mora biti različita od el. pošte");
        //        return View(player);
        //    }
        //    else if (player.Email != player.EmailPonovo)
        //    {
        //        ModelState.AddModelError("", "Pogrešno unesena el. pošta");
        //        return View(player);
        //    }
        //    else if (korisnik)
        //    {
        //        ModelState.AddModelError("", "Korisničko ime je zauzeto");
        //        return View(player);
        //    }

        //    else
        //    {
        //        ModelState.AddModelError("", "Neuspješno uneseni podaci!Player nije dodan u bazu");
        //        return View(player);
        //    }
        //}

        [HttpPost]
        public ActionResult Create(Player player)
        {
            db = new PlayerDb();
            List<Player> listPlayer = new List<Player>();


            if (ModelState.IsValid && this.IsCaptchaValid("Correct"))
            {
                try
                {


                    WebMail.Send(player.Email, "Login Link", "http://localhost:60387/Player/Login");

                    db.InsertPlayer(player);
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
            else if (player.Email == "email")
            {
                ModelState.AddModelError("", "El. pošta je zauzeta");
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


        [AllowAnonymous]
        public ActionResult Login()
        {
            Player player = new Player();
            return View(player);
        }


        //[HttpPost]
        //public ActionResult LoginValidate(AccountViewModel playervm)
        //{
        //    List<AccountViewModel> listPlayer = new List<AccountViewModel>();
        //    listPlayer = db.Validate();

        //    try
        //    {


        //        if (!String.IsNullOrEmpty(playervm.KorisnickoIme))
        //        {
        //            System.Web.Security.FormsAuthentication.SetAuthCookie(playervm.KorisnickoIme, false);
        //            return View();
        //        }

        //        TempData["Message"] = "Login failed.User name or password supplied doesn't exist.";



        //    }
        //    catch (Exception ex)
        //    {
        //        TempData["Message"] = "Login failed.Error - " + ex.Message;
        //    }
        //    return View();
        //}


        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(AccountViewModel playervm)
        {

            List<AccountViewModel> listPlayer = new List<AccountViewModel>();
            listPlayer = db.Validate();

            try
            {

                var korisnik = listPlayer.Exists(s => s.KorisnickoIme.Equals(playervm.KorisnickoIme)) || listPlayer.Exists(l => l.Lozinka.Equals(playervm.Lozinka));

                if (!String.IsNullOrEmpty(playervm.KorisnickoIme) && korisnik)
                {
                    // System.Web.Security.FormsAuthentication.SetAuthCookie(playervm.KorisnickoIme, false);
                    Session["korisnickoIme"] = playervm.KorisnickoIme.ToString();
                    Session["lozinka"] = playervm.Lozinka.ToString();
                    return RedirectToAction("LoginSuccess");

                }

                else if (!korisnik)
                {
                    if (Session["count"] == null)
                    {
                        Session["count"] = 1;
                    }
                    else
                    {
                        int count = (int)Session["count"];
                        count++;
                        Session["count"] = count;
                        ;
                    }
                    ViewBag.Count = Session["count"];
                    return View();
                }

                TempData["Message"] = "Login failed.User name or password supplied doesn't exist.";



            }
            catch (Exception ex)
            {
                TempData["Message"] = "Login failed.Error - " + ex.Message;
            }
            return View();

            //List<AccountViewModel> listPlayer = new List<AccountViewModel>();
            //listPlayer = db.Validate();

            //var korisnik = listPlayer.Exists(s => s.KorisnickoIme.Equals(player.KorisnickoIme)) && listPlayer.Exists(l => l.Lozinka.Equals(player.Lozinka));


            //var korisnik = listPlayer.Single(u => u.KorisnickoIme == player.KorisnickoIme && u.Lozinka == player.Lozinka);


            //if (korisnik)
            //{
            //    Session["id"] = player.KorisnickoIme.ToString();
            //    Session["korisnickoIme"] = player.Lozinka.ToString();
            //    return RedirectToAction("LoginSuccess");

            //}
            //else
            //{
            //    ModelState.AddModelError("", "Pogrešno korisničko ime i/ili lozinka");
            //    return View();

            //}




            //if (korisnik)
            //{
            //    //FormsAuthentication.SetAuthCookie(player.KorisnickoIme, player.RememberMe);
            //    //if (this.Url.IsLocalUrl(returnUrl))
            //    //{
            //    //    return Redirect(returnUrl);
            //    //}
            //    //else
            //    //{
            //    //    return RedirectToAction("GetPlayer", "Player");
            //    //}


            //    //Response.Cookies["KorisnickoIme"].Value = player.KorisnickoIme;
            //    //Response.Cookies["KorisnickoIme"].Expires = DateTime.Now.AddMinutes(2);
            //    //Response.Cookies["Lozinka"].Value = player.Lozinka;
            //    //Response.Cookies["Lozinka"].Expires = DateTime.Now.AddMinutes(2);

            //    //if (Request.Cookies["KorisnickoIme"] != null)
            //    //{
            //    //    string cvalue = Request.Cookies["KorisnickoIme"].Value.ToString();
            //    //    ViewData["Value"] = cvalue;
            //    //}

            //    string cookieValue;
            //    if (Request.Cookies["cookie"] != null)
            //    {
            //        cookieValue = Request.Cookies["cookie"].ToString();
            //        ViewData["cookie"] = cookieValue;

            //    }
            //    else
            //    {
            //        Response.Cookies["cookie"].Value = "cookie value is empty";
            //    }

            //    //Session["korisnickoime"] = player.KorisnickoIme.ToString();
            //    //Session["lozinka"] = player.Lozinka.ToString();

            //    //Session["korisnik"] = player.KorisnickoIme + " je uspjesno prijavljen/a";




            //    ////Cookie
            //    //HttpCookie hc = new HttpCookie("userInfo", player.KorisnickoIme);
            //    ////Expire
            //    //hc.Expires = DateTime.Now.AddSeconds(15);
            //    ////Save data u Cookie
            //    //HttpContext.Response.SetCookie(hc);
            //    ////Get data iz Cookie
            //    //HttpCookie nc = Request.Cookies["userInfo"];
            //    //return View("LoginSuccess");

            //    //// Cookie
            //    //HttpCookie hc = new HttpCookie("userInfo");
            //    //hc["KorisnickoIme"] = player.KorisnickoIme;
            //    //hc["Lozinka"] = player.Lozinka;

            //    //hc.Expires = DateTime.Now.AddSeconds(10);
            //    //Response.Cookies.Add(hc);

            //    //Response.Redirect("Login");

            //    //// Session

            //    //FormsAuthenticationTicket fat = new FormsAuthenticationTicket(1, "Player", DateTime.Now, DateTime.Now.AddMinutes(2), false, JsonConvert.SerializeObject(korisnik));
            //    //HttpCookie hc = new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(fat));
            //    //hc.Expires = DateTime.Now.AddMinutes(2);
            //    //Response.Cookies.Add(hc);


            //    Guid newGuid = Guid.NewGuid();


            //    Session["korisnickoime"] = player.KorisnickoIme.ToString();
            //    Session["lozinka"] = player.Lozinka.ToString();

            //    Session["korisnik"] = player.KorisnickoIme + " je uspjesno prijavljen/a";
            //    Session["korisnik"] = newGuid;


            //    return View("LoginSuccess", listPlayer);
            //}

            //else
            //{

            //    ViewBag.Error = $"Pogrešno korisničko ime ili lozinka";
            //    ModelState.AddModelError("", $"Pogrešno korisničko ime ili lozinka ");

            //    return View();

            //}

        }

        public ActionResult LoginInvalid()
        {
            ViewBag.ErrorLogin = "Broj neuspjelih pokušaja logiranja ";
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


    }
}
