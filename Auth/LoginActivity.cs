using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;

using SQLite;

namespace Auth
{
    [Table("Accounts")]
    public class Account
    {
        [PrimaryKey, AutoIncrement, Column("_id")]
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
    [Activity(Label = "LoginActivity", MainLauncher = true)]
    public class LoginActivity : Activity
    {
        static readonly List<string> data = new List<string>();
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // SQLite Creation
            string dbPath = Path.Combine(
            System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal),
            "database.db3");
            var db = new SQLiteConnection(dbPath);
            db.CreateTable<Account>();
            var table = db.Table<Account>();
            foreach (var a in table)
            {
                data.Add(a.Username);
            }

            // Create your application here
            SetContentView(Resource.Layout.Login);

            //Initializing from layout
            Button login = FindViewById<Button>(Resource.Id.login);
            EditText usernameField = FindViewById<EditText>(Resource.Id.userName);
            EditText passwordField = FindViewById<EditText>(Resource.Id.password);
            TextView errorText = FindViewById<TextView>(Resource.Id.errorLabel);

            string username = string.Empty;
            string password = string.Empty;

            Regex r = new Regex("^(?=.*\\d)(?=.*[a-zA-Z]).{5,12}$"); //Checks for alphanum one of each required between 5-12 chars
            Regex s = new Regex("^([a-zA-z0-9]{2})\\1{1,}$"); //Checks for repeating sequence -- will return true

            //Login button click action
            login.Click += (object sender, EventArgs e) => {
                username = usernameField.Text;
                password = passwordField.Text;
                errorText.Text = "";
                if (r.IsMatch(password) && !s.IsMatch(password) && !username.Equals(""))
                {
                    Console.Out.Write("data added");
                    data.Add(username);

                    var newAccount = new Account();
                    newAccount.Username = username;
                    newAccount.Password = password;
                    db.Insert(newAccount);

                    var intent = new Intent(this, typeof(UsersListActivity));
                    intent.PutStringArrayListExtra("users", data);
                    StartActivity(intent);
                }
                else
                {
                    Console.Out.Write("failed");
                    errorText.Text = "Invalid Password";
                }
            };
        }
    }
}