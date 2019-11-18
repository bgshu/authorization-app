using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using SQLite;

namespace Auth
{
    class Repository
    {
        private SQLiteConnection db;

        public void initializeDb()
        {
            // SQLite Creation
            string dbPath = Path.Combine(
            System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal),
            "database.db3");
            db = new SQLiteConnection(dbPath);
            db.CreateTable<Account>();
        }

        public List<String> getUsernames()
        {
            var table = db.Table<Account>();
            return table.Select(account => account.Username).ToList();
        }
        
        public void addAccount(Account account)
        {
            db.Insert(account);
        }

        public bool isUsernameExisting(string username)
        {
            return db.Table<Account>().Where(account => account.Username == username).ToList().Count > 0;
        }
    }
}