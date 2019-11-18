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
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class AddUserActivity : Activity
    {
        List<string> usernames = new List<string>();
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Verifier verifier = new Verifier();

            Repository repo = new Repository();
            repo.initializeDb();
            usernames = repo.getUsernames();

            // Create your application here
            SetContentView(Resource.Layout.Login);

            //Initializing from layout
            Button login = FindViewById<Button>(Resource.Id.login);
            EditText usernameField = FindViewById<EditText>(Resource.Id.userName);
            EditText passwordField = FindViewById<EditText>(Resource.Id.password);
            TextView errorText = FindViewById<TextView>(Resource.Id.errorLabel);

            string username = string.Empty;
            string password = string.Empty;

            //Login button click action
            login.Click += (object sender, EventArgs e) => {
                username = usernameField.Text;
                password = passwordField.Text;
                errorText.Text = "";
                if (verifier.validEntry(username, password))
                {
                    usernames.Add(username);

                    var newAccount = new Account();
                    newAccount.Username = username;
                    newAccount.Password = password;
                    repo.addAccount(newAccount);

                    var intent = new Intent(this, typeof(UsersListActivity));
                    intent.PutStringArrayListExtra("users", usernames);
                    StartActivity(intent);
                }
                else
                {
                    if (!verifier.isUniqueName(username))
                    {
                        errorText.Text = "Username already taken!";
                    }
                    else if(verifier.nullUsername(username))
                    {
                        errorText.Text = "Username cannot be blank";
                    }
                    else if(!verifier.isAlphaNum(password))
                    {
                        errorText.Text = "Invalid Password: Must contain alphanumeric characters - at least one of each";
                    }
                    else if(!verifier.isNotRepeating(password))
                    {
                        errorText.Text = "Invalid Password: Repeating sequence found";
                    }
                    else if(!verifier.isCorrectLength(password))
                    {
                        errorText.Text += "Invalid Password: Must be between 5-12 characters";
                    }
                }
            };
        }
    }
}