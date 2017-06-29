using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using Tipwin.Hash;
using Tipwin.Models;
using Tipwin.ViewModel;

namespace Tipwin.Repository
{
    public class PlayerDb
    {
        MySqlConnection connection;
        string conString = "SERVER = 'localhost'; "
            + "DATABASE = 'player'; "
            + "UID = 'root'; "
            + "PASSWORD='root'; ";


        //public List<AccountViewModel> GetValidatePlayers()
        //{
        //    connection = new MySqlConnection(conString);
        //    string selQuery = "SELECT korisnicko_ime,lozinka FROM players;";
        //    MySqlCommand cmd = new MySqlCommand(selQuery, connection);
        //    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
        //    DataTable dt = new DataTable();
        //    List<AccountViewModel> listPlayers = new List<AccountViewModel>();
        //    da.Fill(dt);
        //    foreach (DataRow dr in dt.Rows)
        //    {
        //        AccountViewModel p1 = new AccountViewModel()
        //        {
        //            KorisnickoIme = Convert.ToString(dr["korisnicko_ime"]),
        //            Lozinka = Convert.ToString(dr["lozinka"])

        //        };
        //        listPlayers.Add(p1);
        //    }
        //    return listPlayers;
        //}



        public List<Player> GetPlayers()
        {


            connection = new MySqlConnection(conString);
            string selQuery = "SELECT * FROM players;";
            MySqlCommand cmd = new MySqlCommand(selQuery, connection);
            MySqlDataAdapter da = new MySqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            List<Player> listPlayers = new List<Player>();
            da.Fill(dt);
            foreach (DataRow dr in dt.Rows)
            {
                Player p1 = new Player()
                {
                    Id = Convert.ToInt32(dr["id"]),
                    Oslovljavanje = Convert.ToString(dr["oslovljavanje"]),
                    Ime = Convert.ToString(dr["ime"]),
                    Prezime = Convert.ToString(dr["prezime"]),
                    DatumRodjenja = Convert.ToDateTime(dr["datum_rodjenja"]),
                    Email = Convert.ToString(dr["email"]),
                    EmailPonovo = Convert.ToString(dr["email_ponovo"]),
                    Ulica = Convert.ToString(dr["ulica"]),
                    KucniBroj = Convert.ToString(dr["kucni_broj"]),
                    GradMjesto = Convert.ToString(dr["grad_mjesto"]),
                    PostanskiBroj = Convert.ToInt32(dr["postanski_broj"]),
                    Drzava = Convert.ToString(dr["drzava"]),
                    JezikZaKontakt = Convert.ToString(dr["jezik_za_kontakt"]),
                    BrojTelefona = Convert.ToInt32(dr["broj_telefona"] as int? ?? null),
                    BrojMobitela = Convert.ToInt32(dr["broj_mobitela"] as int? ?? null),
                    KorisnickoIme = Convert.ToString(dr["korisnicko_ime"]),
                    Lozinka = Convert.ToString(dr["lozinka"]),
                    LozinkaPonovo = Convert.ToString(dr["lozinka_ponovo"]),
                    // PogresnaLozinka = Convert.ToString(dr["pogresnaLozinka"])

                };
                listPlayers.Add(p1);
            }
            return listPlayers;
        }
        public bool InsertPlayer(Player player)
        {

            string saltHashReturned = HashedPassword.Sha256(player.Lozinka);

            connection = new MySqlConnection(conString);
            string insQuery = "INSERT INTO players VALUES (@id,@oslovljavanje,@ime,@prezime,@datum_rodjenja,@email,@email_ponovo,@ulica,@kucni_broj,@grad_mjesto,@postanski_broj,@drzava,@jezik_za_kontakt,@broj_telefona,@broj_mobitela,@korisnicko_ime,@lozinka,@lozinka_ponovo)";
            MySqlCommand cmd = new MySqlCommand(insQuery, connection);
            cmd.Parameters.AddWithValue("@id", player.Id);
            cmd.Parameters.AddWithValue("@oslovljavanje", player.Oslovljavanje);
            cmd.Parameters.AddWithValue("@ime", player.Ime);
            cmd.Parameters.AddWithValue("@prezime", player.Prezime);
            cmd.Parameters.AddWithValue("@datum_rodjenja", player.DatumRodjenja);
            cmd.Parameters.AddWithValue("@email", player.Email);
            cmd.Parameters.AddWithValue("@email_ponovo", player.EmailPonovo);
            cmd.Parameters.AddWithValue("@ulica", player.Ulica);
            cmd.Parameters.AddWithValue("@kucni_broj", player.KucniBroj);
            cmd.Parameters.AddWithValue("@grad_mjesto", player.GradMjesto);
            cmd.Parameters.AddWithValue("@postanski_broj", player.PostanskiBroj);
            cmd.Parameters.AddWithValue("@drzava", player.Drzava);
            cmd.Parameters.AddWithValue("@jezik_za_kontakt", player.JezikZaKontakt);
            cmd.Parameters.AddWithValue("@broj_telefona", player.BrojTelefona);
            cmd.Parameters.AddWithValue("@broj_mobitela", player.BrojMobitela);
            cmd.Parameters.AddWithValue("@korisnicko_ime", player.KorisnickoIme);
            cmd.Parameters.AddWithValue("@lozinka", saltHashReturned);
            //  cmd.Parameters.AddWithValue("@lozinka", player.Lozinka);
            cmd.Parameters.AddWithValue("@lozinka_ponovo", saltHashReturned);
            //cmd.Parameters.AddWithValue("@pogresnaLozinka", player.PogresnaLozinka);
            //cmd.Parameters.AddWithValue("@rememberme", player.RememberMe);
            connection.Open();
            int i = cmd.ExecuteNonQuery();
            connection.Close();

            if (i >= 1) return true; else return false;
        }

        public bool DeletePlayer(int id)
        {
            connection = new MySqlConnection(conString);
            string delQuery = "DELETE FROM players WHERE id=@id";
            MySqlCommand cmd = new MySqlCommand(delQuery, connection);
            cmd.Parameters.AddWithValue("@id", id);

            connection.Open();
            int i = cmd.ExecuteNonQuery();
            connection.Close();
            if (i >= 1) return true; else return false;
        }

        //public bool Validate(AccountViewModel playervm)
        //{
        //    connection = new MySqlConnection(conString);
        //    string Query = "select korisnicko_ime from players WHERE korisnicko_ime=@korisnicko_ime AND lozinka =@lozinka";
        //    MySqlCommand cmd = new MySqlCommand(Query, connection);
        //    cmd.Parameters.AddWithValue("@korisnicko_ime", playervm.KorisnickoIme);
        //    cmd.Parameters.AddWithValue("@lozinka", playervm.Lozinka);

        //    connection.Open();
        //    string userName = (string)cmd.ExecuteScalar();
        //    connection.Close();
        //    if (userName != null) return true; else return false;         
        //}

        public List<AccountViewModel> Validate()
        {
            connection = new MySqlConnection(conString);
            string selQuery = "select korisnicko_ime,lozinka from players";
            MySqlCommand cmd = new MySqlCommand(selQuery, connection);
            MySqlDataAdapter da = new MySqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            List<AccountViewModel> accountlistPlayers = new List<AccountViewModel>();
            da.Fill(dt);
            foreach (DataRow dr in dt.Rows)
            {
                AccountViewModel p1 = new AccountViewModel()
                {
                    KorisnickoIme = Convert.ToString(dr["korisnicko_ime"]),
                    Lozinka = Convert.ToString(dr["lozinka"])

                };
                accountlistPlayers.Add(p1);
            }
            return accountlistPlayers;
        }

        //[HttpPost]
        //public ActionResult Validate(User user)
        //{
        //    try
        //    {
        //        string cs = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
        //        using (var connection = new SqlConnection(cs))
        //        {
        //            string commandText = "SELECT Username FROM [User] WHERE Username=@Username AND Password = @Password";
        //            using (var command = new SqlCommand(commandText, connection))
        //            {
        //                command.Parameters.AddWithValue("@Username", user.Username);
        //                command.Parameters.AddWithValue("@Password", user.Password);
        //                connection.Open();

        //                string userName = (string)command.ExecuteScalar();

        //                if (!String.IsNullOrEmpty(userName))
        //                {
        //                    System.Web.Security.FormsAuthentication.SetAuthCookie(user.Username, false);
        //                    return RedirectToAction("Index", "Home");
        //                }

        //                TempData["Message"] = "Login failed.User name or password supplied doesn't exist.";

        //                connection.Close();
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        TempData["Message"] = "Login failed.Error - " + ex.Message;
        //    }
        //    return RedirectToAction("Index");
        //}






        //public bool FindEmail(string email)
        //{
        //    connection = new MySqlConnection(conString);
        //    string emailQuery = "select email as email from players where email = @email";
        //    MySqlCommand cmd = new MySqlCommand(emailQuery, connection);
        //    cmd.Parameters.AddWithValue("@email", email);

        //    connection.Open();
        //    int i = cmd.ExecuteNonQuery();
        //    connection.Close();

        //    if (i >= 1) return true; else return false;

        //}

        //public bool EmailConfirmation(string korisnickoIme)
        //{
        //    bool flag = false;
        //    string res = null;
        //    connection = new MySqlConnection(conString);
        //    string emailQuery = "select emailConfirmed as emailConfirmed from players where id = @id";
        //    MySqlCommand cmd = new MySqlCommand(emailQuery, connection);
        //    cmd.Parameters.AddWithValue("@korisnickoIme", korisnickoIme);
        //    connection.Open();
        //    using (MySqlDataReader reader = cmd.ExecuteReader())
        //    {
        //        if (reader.HasRows)
        //        {
        //            if (reader.Read())
        //            {
        //                res = reader["emailConfirmed"].ToString();
        //                if (res == "false")  //ovdje moguca greska
        //                {
        //                    flag = false;
        //                }
        //                else
        //                {
        //                    flag = true;
        //                }
        //            }
        //        }
        //        connection.Close();
        //    }
        //    return flag;

        //}

        //public bool EmailConfirmationById(string korisnickoIme)
        //{
        //    connection = new MySqlConnection(conString);
        //    string query = "select emailConfirmed as emailConfirmed from players where id = @id";
        //    MySqlCommand cmd = new MySqlCommand(query, connection);
        //    cmd.Parameters.AddWithValue("@korisnickoIme", korisnickoIme);

        //    connection.Open();
        //    int i = cmd.ExecuteNonQuery();
        //    connection.Close();

        //    if (i >= 1) return true; else return false;

        //}

        //public bool FindUserName(string korisnickoIme)
        //{
        //    connection = new MySqlConnection(conString);
        //    string emailQuery = "select korisnickoIme as korisnickoIme from players where korisnickoIme = @korisnickoIme";
        //    MySqlCommand cmd = new MySqlCommand(emailQuery, connection);
        //    cmd.Parameters.AddWithValue("@korisnickoIme", korisnickoIme);

        //    connection.Open();
        //    int i = cmd.ExecuteNonQuery();
        //    connection.Close();

        //    if (i >= 1) return true; else return false;
        //}

        //public bool UpdateDatabase(string korisnickoIme)
        //{
        //    connection = new MySqlConnection(conString);
        //    string query = "update players set emailLinkDate = '" + DateTime.Now + "'  where korisnickoIme=@korisnickoIme";
        //    MySqlCommand cmd = new MySqlCommand(query, connection);
        //    cmd.Parameters.AddWithValue("@korisnickoIme", korisnickoIme);

        //    connection.Open();
        //    int i = cmd.ExecuteNonQuery();
        //    connection.Close();

        //    if (i >= 1) return true; else return false;
        //}

        //public bool UpdateLastLoginDate(string korisnickoIme)
        //{
        //    connection = new MySqlConnection(conString);
        //    string query = "update players set lastLoginDate = '" + DateTime.Now + "'  where korisnickoIme=@korisnickoIme";
        //    MySqlCommand cmd = new MySqlCommand(query, connection);
        //    cmd.Parameters.AddWithValue("@korisnickoIme", korisnickoIme);

        //    connection.Open();
        //    int i = cmd.ExecuteNonQuery();
        //    connection.Close();

        //    if (i >= 1) return true; else return false;
        //}





        //public bool InvalidLogin(Player player)
        //{



        //    connection = new MySqlConnection(conString);
        //    string insQuery = "INSERT INTO players VALUES (@pogresnaLozinka)";
        //    MySqlCommand cmd = new MySqlCommand(insQuery, connection);

        //    cmd.Parameters.AddWithValue("@pogresnaLozinka", player.PogresnaLozinka);



        //    connection.Open();
        //    int i = cmd.ExecuteNonQuery();
        //    connection.Close();

        //    if (i >= 1)
        //        return true;
        //    else
        //        return false;
        //}


        //public bool InvalidLogin(string korisnickoIme, string lozinka)
        //{
        //    string selString = "SELECT korisnicko_ime,lozinka FROM players";

        //    using (MySqlConnection conn = new MySqlConnection(conString))
        //    {
        //        conn.Open();
        //        using (MySqlCommand cmd = new MySqlCommand(selString, conn))
        //        {


        //            cmd.Parameters.AddWithValue("@korisnicko_ime", korisnickoIme);
        //            cmd.Parameters.AddWithValue("@lozinka", lozinka);

        //            MySqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
        //            if (reader.Read())
        //            {
        //                return true;  // data exist
        //            }
        //            else
        //            {
        //                return false; //data not exist
        //            }
        //        }
        //    }
        //}



    }
}
