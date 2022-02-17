using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserDatabase.Models;

namespace UserDatabase.DAL.Context
{
    public class UserDbContext
    {
        public SQLiteConnection Db { get; set; }
        public UserDbContext()
        {
            Db = new SQLiteConnection("UserDb.db");
            Db.CreateTable<User>();
        }
    }
}
