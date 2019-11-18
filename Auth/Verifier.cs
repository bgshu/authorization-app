using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Auth
{
    class Verifier
    {
        Repository repo = new Repository();
        private Regex r = new Regex("^(?=.*\\d)(?=.*[a-zA-Z]).{1,}$"); //Checks for alphanum one of each required
        private Regex s = new Regex("^([a-zA-z0-9]{2})\\1{1,}$"); //Will return true if there's a repeating sequence
        private Regex t = new Regex("^\\w{5,12}$"); //Checks for length of 5-12 chars

        public bool isUniqueName(string name)
        {
            if (repo.getUsernames().Contains(name))
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public bool validEntry(string username, string password)
        {
            if (r.IsMatch(password) && !s.IsMatch(password) && !username.Equals("") && isUniqueName(username))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool nullUsername(string username)
        {
            if (username.Equals(""))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool isAlphaNum(string password)
        {
            if (r.IsMatch(password))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool isNotRepeating(string password)
        {
            if (s.IsMatch(password))
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public bool isCorrectLength(string password)
        {
            if (t.IsMatch(password))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}