using System;
using System.Collections.Generic;
using System.Configuration;

namespace Tipwin
{

    public class LoginCounter
    {
        Dictionary<string, int> _loginCount;
        Dictionary<string, DateTime> _lockedLogin;
        int loginPreventedTime = Convert.ToInt32(ConfigurationManager.AppSettings["loginPreventedTime"]);
        public string LoginErrorMessage { get; private set; }
        public LoginCounter()
        {
            _loginCount = new Dictionary<string, int>();
            _lockedLogin = new Dictionary<string, DateTime>();
        }

        public bool CheckLogin(string username)
        {
            if (_lockedLogin.ContainsKey(username))
            {
                if (_lockedLogin[username].AddMinutes(1) >= DateTime.Now)
                {
                    LoginErrorMessage = $"{username} je premašio/la broj pokušaja.Novi pokušaj od: {_lockedLogin[username].AddMinutes(loginPreventedTime)}";
                    return false;
                }
                else
                {
                    _lockedLogin.Remove(username); // isteklo vrijeme, dopusti pokusaje
                    _loginCount.Remove(username);
                }

            }

            if (_loginCount.ContainsKey(username))
            {
                if (_loginCount[username] < 2)
                {
                    _loginCount[username]++;

                    return false;
                }
                else
                {
                    if (_lockedLogin.ContainsKey(username)) return false;
                    _lockedLogin.Add(username, DateTime.Now);
                }
            }
            else
            {
                _loginCount.Add(username, 1);
            }
            return true;
        }

        public void ClearLockedLoginsAfterSuccessfull(string username)
        {

            if (_lockedLogin.ContainsKey(username))
            {
                _lockedLogin.Remove(username);
            }
            if (_loginCount.ContainsKey(username))
            {
                _loginCount.Remove(username);
            }
        }

        public bool ReturnIfLockedUsername(string username)
        {
            return _lockedLogin.ContainsKey(username);
        }
    }

}
