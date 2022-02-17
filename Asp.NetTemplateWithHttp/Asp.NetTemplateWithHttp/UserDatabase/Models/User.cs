using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserDatabase.Models
{
    [Table("Users")]
    public class User
    {
        [PrimaryKey, AutoIncrement, Column("ID")]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public override string ToString()
        {
            return @$"Id:{Id} 
Name:{Name} 
Email:{Email} 
Password:{Password}
---------------------";
        }
    }
}
