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
                    LozinkaPonovo = Convert.ToString(dr["lozinka_ponovo"])
                };
                listPlayers.Add(p1);
            }
            return listPlayers;
        }
        public bool InsertPlayer(Player player)
        {
            string saltHashReturned = HashedPassword.Encrypt(player.Lozinka, HashedPassword.SupportedHashAlgorithms.SHA256, null);
            connection = new MySqlConnection(conString);
            string insQuery = "INSERT INTO players VALUES (@id,@oslovljavanje,@ime,@prezime,@datum_rodjenja,@email,@email_ponovo,@ulica,@kucni_broj,@grad_mjesto,@postanski_broj,@drzava,@jezik_za_kontakt,@broj_telefona,@broj_mobitela,@korisnicko_ime,@lozinka,@lozinka_ponovo)";
            MySqlCommand cmd = new MySqlCommand(insQuery, connection);
            cmd.Parameters.AddWithValue("@id", null);
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
            cmd.Parameters.AddWithValue("@lozinka_ponovo", saltHashReturned);
            connection.Open();
            int i = cmd.ExecuteNonQuery();
            connection.Close();
            if (i >= 1) return true; else return false;
        }


        public string FetchUserId(string email)
        {
            MySqlCommand cmd = new MySqlCommand();
            cmd = new MySqlCommand("SELECT id FROM players WHERE email=@email", connection);
            cmd.Parameters.AddWithValue("@email", email);
            if (connection.State == ConnectionState.Closed)
            {
                connection.Open();
            }
            string UserID = Convert.ToString(cmd.ExecuteScalar());
            connection.Close();
            cmd.Dispose();
            return UserID;
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
        public List<UserNameViewModel> GetForgotUser()
        {
            connection = new MySqlConnection(conString);
            string selQuery = "select email,datum_rodjenja from players";
            MySqlCommand cmd = new MySqlCommand(selQuery, connection);
            MySqlDataAdapter da = new MySqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            List<UserNameViewModel> userlistPlayers = new List<UserNameViewModel>();
            da.Fill(dt);
            foreach (DataRow dr in dt.Rows)
            {
                UserNameViewModel p1 = new UserNameViewModel()
                {
                    Email = Convert.ToString(dr["email"]),
                    DatumRodjenja = Convert.ToDateTime(dr["datum_rodjenja"])

                };
                userlistPlayers.Add(p1);
            }
            return userlistPlayers;
        }
        public List<PasswordViewModel> GetForgotPassword()
        {
            connection = new MySqlConnection(conString);
            string selQuery = "select korisnicko_ime,datum_rodjenja from players";
            MySqlCommand cmd = new MySqlCommand(selQuery, connection);
            MySqlDataAdapter da = new MySqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            List<PasswordViewModel> userlistPlayers = new List<PasswordViewModel>();
            da.Fill(dt);
            foreach (DataRow dr in dt.Rows)
            {
                PasswordViewModel p1 = new PasswordViewModel()
                {
                    KorisnickoIme = Convert.ToString(dr["korisnicko_ime"]),
                    DatumRodjenja = Convert.ToDateTime(dr["datum_rodjenja"])

                };
                userlistPlayers.Add(p1);
            }
            return userlistPlayers;
        }

        public String GenCode()
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var stringChars = new char[8];
            var random = new Random();

            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }

            var finalString = new String(stringChars);
            return finalString;
        }

        public string SendActivationEmail(ActivationViewModel activationvm, string email)
        {
            var code = GenCode();
            activationvm.ActivationCode = code;
            connection = new MySqlConnection(conString);
            string selQuery = "insert into activations values (@id,@activation_code,@players_id,@email,@provjeren)";

            MySqlCommand cmd = new MySqlCommand(selQuery, connection);
            cmd.Parameters.AddWithValue("@id", null);
            cmd.Parameters.AddWithValue("@activation_code", activationvm.ActivationCode);
            cmd.Parameters.AddWithValue("@players_id", activationvm.PlayersId);
            cmd.Parameters.AddWithValue("@email", email);
            cmd.Parameters.AddWithValue("@provjeren", activationvm.Provjeren);

            connection.Open();
            cmd.ExecuteNonQuery();
            connection.Close();
            return activationvm.ActivationCode;

        }
        public void SavePlayerActivationId(int activationCodeId, int playerId, bool provjeren)
        {
            connection = new MySqlConnection(conString);
            string selQuery = "update activations set players_id = @players_id,provjeren=true where id = @id";

            MySqlCommand cmd = new MySqlCommand(selQuery, connection);
            cmd.Parameters.AddWithValue("@id", activationCodeId);
            cmd.Parameters.AddWithValue("@players_id", playerId);
            cmd.Parameters.AddWithValue("@provjeren", provjeren);

            connection.Open();
            cmd.ExecuteNonQuery();
            connection.Close();
        }

        public void NewActivationCode(int activationCodeId, int playerId)
        {
            connection = new MySqlConnection(conString);
            string selQuery = "update activations set players_id = @players_id where id = @id";

            MySqlCommand cmd = new MySqlCommand(selQuery, connection);
            cmd.Parameters.AddWithValue("@id", activationCodeId);
            cmd.Parameters.AddWithValue("@players_id", playerId);

            connection.Open();
            cmd.ExecuteNonQuery();
            connection.Close();
        }

        //public void Verificiran(int provjeren)
        //{
        //    connection = new MySqlConnection(conString);
        //    string selQuery = "update activations set provjeren = 1";

        //    MySqlCommand cmd = new MySqlCommand(selQuery, connection);
        //    cmd.Parameters.AddWithValue("@provjeren", provjeren);

        //    connection.Open();
        //    cmd.ExecuteNonQuery();
        //    connection.Close();
        //}

        public void UpdatePassword(string email, string newPassword, string passwordRepeat)
        {
            connection = new MySqlConnection(conString);
            string hashPass = HashedPassword.Encrypt(newPassword, HashedPassword.SupportedHashAlgorithms.SHA256, null);

            string selQuery = "update players set lozinka=@lozinka,lozinka_ponovo=@lozinka_ponovo where  email = @email";

            MySqlCommand cmd = new MySqlCommand(selQuery, connection);
            cmd.Parameters.AddWithValue("@email", email);
            cmd.Parameters.AddWithValue("@lozinka", hashPass);
            cmd.Parameters.AddWithValue("@lozinka_ponovo", hashPass);

            connection.Open();
            cmd.ExecuteNonQuery();
            connection.Close();
        }

        public List<ActivationViewModel> GetActivationEmail()
        {
            connection = new MySqlConnection(conString);
            string selQuery = "select * from activations;";
            MySqlCommand cmd = new MySqlCommand(selQuery, connection);
            MySqlDataAdapter da = new MySqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            List<ActivationViewModel> listPlayers = new List<ActivationViewModel>();
            da.Fill(dt);
            foreach (DataRow dr in dt.Rows)
            {
                ActivationViewModel p1 = new ActivationViewModel()
                {
                    Id = Convert.ToInt32(dr["id"]),
                    ActivationCode = Convert.ToString(dr["activation_code"]),
                    Email = Convert.ToString(dr["email"]),
                    PlayersId = Convert.ToInt32(dr["players_id"]),
                    Provjeren = Convert.ToBoolean(dr["provjeren"])
                };
                listPlayers.Add(p1);
            }
            return listPlayers;

        }
    }
}
