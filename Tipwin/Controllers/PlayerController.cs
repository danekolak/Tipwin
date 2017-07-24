using CaptchaMvc.HtmlHelpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        static string trenutniPlayerEmail = "";

        PlayerDb db = new PlayerDb();
        List<Player> listPlayers = new List<Player>();

        public ActionResult GetPlayer()
        {
            db = new PlayerDb();

            List<Player> listPlayers = new List<Player>();
            listPlayers = db.GetPlayers();
            return View(listPlayers);
        }

        public ActionResult Confirmed()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Confirmed(ActivationViewModel activationvm)
        {
            db = new PlayerDb();
            List<Player> listPlayers = db.GetPlayers();
            List<ActivationViewModel> activationlistGetActivationEmail = db.GetActivationEmail();
            if (ModelState.IsValid)
            {
                try
                {
                    var verificiran = activationlistGetActivationEmail.Any(s => s.Provjeren == activationvm.Provjeren);

                    var activVM = activationlistGetActivationEmail.SingleOrDefault(d => d.ActivationCode == activationvm.ActivationCode);
                    var p = listPlayers.SingleOrDefault(e => e.Email == activVM.Email);
                    if (activationvm.ActivationCode.Any() && activVM != null)
                    {
                        db.SavePlayerActivationId(activVM.Id, p.Id, verificiran);
                        TempData["odblokiran"] = "Vaš račun je verificiran";
                        trenutniPlayerEmail = p.Email;
                        TempData["trenutni"] = trenutniPlayerEmail;

                        if (activVM.PlayersId == 0)
                        {
                            return RedirectToAction("Login");
                        }
                        else
                            return RedirectToAction("ChangePassword");
                    }
                    else if (activationvm.ActivationCode.Any() && activVM == null)
                    {
                        return RedirectToAction("Login");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Pogrešan verifikacijski kod");
                        return View();
                    }
                }
                catch (Exception e)
                {
                    ModelState.AddModelError("", "Unesite kod za odblokiranje računa" + e.Message);
                    return View();
                }
            }
            return View();
        }

        public ActionResult NewCode()
        {
            return View();
        }
        [HttpPost]
        public ActionResult NewCode(ActivationViewModel avm)
        {
            List<ActivationViewModel> avmList = new List<ActivationViewModel>();
            //var activVM = avmList.SingleOrDefault(a => a.ActivationCode == avm.ActivationCode);
            var p = listPlayers.SingleOrDefault(e => e.Email == avm.Email);


            var random = db.SendActivationEmail(avm, avm.Email);
            WebMail.Send(avm.Email, $"Activation code: {random}", "http://localhost:60387/Player/Confirmed");

            db.NewActivationCode(avm.Id, avm.PlayersId);
            return RedirectToAction("Confirmed");
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(Player player)
        {
            db = new PlayerDb();
            ActivationViewModel activationvm = new ActivationViewModel();
            List<Player> listPlayer = new List<Player>();
            listPlayer = db.GetPlayers();

            if (ModelState.IsValid && this.IsCaptchaValid("Correct"))
            {
                try
                {
                    db.InsertPlayer(player);
                    var random = db.SendActivationEmail(activationvm, player.Email);

                    WebMail.Send(player.Email, $"Activation code: {random}", "http://localhost:60387/Player/Confirmed");

                    TempData["novikorisnik"] = player.KorisnickoIme + " je uspješno registriran/a. ";
                    TempData["info"] = "Vaš korisnički račun je uspješno kreiran. Poslana je poruka za aktivaciju na vašu el. poštu";

                    return RedirectToAction("EmailConfirmation", listPlayer);

                }
                catch (Exception e)
                {
                    ViewBag.ErrorMessage = "Greška: captcha nije validna.";

                    ModelState.AddModelError("", $"Korisnik je već registriran. Korisničko ime ili el. pošta su zauzeti - {e.Message}");
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

        public ActionResult Index()
        {

            return View();
        }
        public ActionResult EmailConfirmation()
        {
            return View();
        }
        public ActionResult ForgotPassword()
        {

            return View();
        }

        [HttpPost]
        public ActionResult ForgotPassword(Player player)
        {
            db = new PlayerDb();
            var _player = db.GetPlayers().SingleOrDefault(p => p.KorisnickoIme == player.KorisnickoIme && p.DatumRodjenja == player.DatumRodjenja);
            List<ActivationViewModel> GetListActivationEmail = new List<ActivationViewModel>();
            GetListActivationEmail = db.GetActivationEmail();                //dohvatimo iz baze polje activation_code

            List<PasswordViewModel> passwordDateList = new List<PasswordViewModel>();   //lista email i datum
            ActivationViewModel activationCode = new ActivationViewModel();

            passwordDateList = db.GetForgotPassword();
            try
            {
                if (_player != null && this.IsCaptchaValid("Correct"))
                {

                    var random = GetListActivationEmail.SingleOrDefault(d => d.Email == _player.Email);
                    WebMail.Send(_player.Email, $"Activation code:{random.ActivationCode}", $"http://localhost:60387/Player/Confirmed");
                    TempData["ConfirmSuccessfull"] = "Uspješno verificiran aktivacijski kod. Prijavite se!";
                    TempData["GetCode"] = "Provjerite el. poštu. Dobili ste aktivacijski kod";
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
        public ActionResult ChangePassword()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ChangePassword(Player player)
        {
            db = new PlayerDb();

            List<ActivationViewModel> GetListActivationEmail = new List<ActivationViewModel>();
            GetListActivationEmail = db.GetActivationEmail();
            List<Player> listPlayers = db.GetPlayers();
            List<ChangePasswordViewModel> changePasswordList = new List<ChangePasswordViewModel>();

            var p = listPlayers.SingleOrDefault(e => e.Email == trenutniPlayerEmail);
            if (p != null)
            {
                try
                {
                    db.UpdatePassword(p.Email, player.Lozinka, player.LozinkaPonovo);
                }
                catch (Exception e)
                {
                    ModelState.AddModelError("", "Unesite novu lozinku" + e.Message);
                    return View();
                }
            }
            return RedirectToAction("Login");
        }
        public ActionResult AccountSuccess()
        {
            return View();
        }
        public ActionResult ForgotUserName()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ForgotUserName(Player player)
        {
            db = new PlayerDb();
            List<ActivationViewModel> GetListActivationEmail = new List<ActivationViewModel>();
            GetListActivationEmail = db.GetActivationEmail();                //dohvatimo iz baze polje activation_code

            List<UserNameViewModel> emailDateList = new List<UserNameViewModel>();   //lista email i date
            ActivationViewModel activationCode = new ActivationViewModel();
            emailDateList = db.GetForgotUser();

            try
            {
                if ((emailDateList.Any(s => s.Email == player.Email && s.DatumRodjenja == player.DatumRodjenja)) && this.IsCaptchaValid("Correct"))
                {
                    var random = db.SendActivationEmail(activationCode, player.Email);
                    WebMail.Send(player.Email, $"Activation code:{random}", "http://localhost:60387/Player/Confirmed");
                    TempData["ConfirmSuccessfull"] = "Dobili ste activacijski kod ";
                    return RedirectToAction("Confirmed");
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

            List<ActivationViewModel> activationListViewModel = new List<ActivationViewModel>();
            activationListViewModel = db.GetActivationEmail();

            var activateAccount = activationListViewModel.Any();
            try
            {
                if (activateAccount && listPlayer.Any(s => s.KorisnickoIme == playervm.KorisnickoIme && HashedPassword.Confirm(playervm.Lozinka, s.Lozinka, HashedPassword.SupportedHashAlgorithms.SHA256))
                && !MvcApplication.LoginCounter.ReturnIfLockedUsername(playervm.KorisnickoIme))
                {
                    Session["korisnickoIme"] = playervm.KorisnickoIme.ToString();
                    Session["lozinka"] = playervm.Lozinka.ToString();

                    MvcApplication.LoginCounter.ClearLockedLoginsAfterSuccessfull(playervm.KorisnickoIme);
                    return RedirectToAction("LoginSuccess");

                }
                else if (!listPlayer.Any(s => s.KorisnickoIme == playervm.KorisnickoIme && HashedPassword.Confirm(playervm.Lozinka, s.Lozinka, HashedPassword.SupportedHashAlgorithms.SHA256)))
                {
                    ModelState.AddModelError("", "Pogrešno korisničko ime i/ili lozinka");
                    return View();
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


        public ActionResult UploadFiles()
        {
            return View();
        }

        private bool IsValidContentType(string contentType)
        {
            return contentType.Equals("image/png") || contentType.Equals("image/gif") || contentType.Equals("image/jpg") || contentType.Equals("image/jpeg");
        }

        private bool IsValidContentLength(double contentLength)
        {
            return ((contentLength / 1024) / 1024) > 0.1 && (((contentLength / 1024) / 1024) < 6); //od 100kb do 5MB
        }

        [HttpPost]
        public ActionResult Process(HttpPostedFileBase photo)
        {
            db = new PlayerDb();
            List<ActivationViewModel> activationlistGetProvjeren = db.GetActivationEmail();
            ActivationViewModel activationvm = new ActivationViewModel();

            try
            {
                var verificiran = activationlistGetProvjeren.Any(s => s.Provjeren);

                if (!IsValidContentType(photo.ContentType))
                {
                    ViewBag.Error = "Jedino JPG, JPEG, PNG & GIF su dopušteni";
                    return View("UploadFiles");
                }
                else if (!IsValidContentLength(photo.ContentLength))
                {
                    ViewBag.Error = "Slike nisu u rasponu od 100kb do 5 mb";
                    return View("UploadFiles");
                }
                else if (verificiran)
                {
                    ViewBag.SendErrorMessage = "Korisnički račun je već verificiran. Ne možete poslati dokument";
                    return View("UploadFiles");
                }
                else
                {
                    if (photo.ContentLength > 0)
                    {
                        var fileName = Path.GetFileName(photo.FileName);
                        var path = Path.Combine(Server.MapPath("~/Content/Images"), fileName);
                        photo.SaveAs(path);
                        ViewBag.fileName = photo.FileName;
                    }
                    return View("Success");
                }
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", $"Error: {e}");
            }
            return View();
        }


        public ActionResult EditUserAccount(int id)
        {
            UserNameData p = new UserNameData();
            p = db.SelectByIdUserNameData(id);
            return View(p);              //dohvatimo podatke
        }
        [HttpPost]
        public ActionResult EditUserAccount(int id, UserNameData player)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.UpdatePlayer(player);
                    return RedirectToAction("GetPlayer");
                }
                catch (Exception e)
                {
                    ModelState.AddModelError("", $"{e.Message}");
                }
            }
            return View();
        }

    }
}
