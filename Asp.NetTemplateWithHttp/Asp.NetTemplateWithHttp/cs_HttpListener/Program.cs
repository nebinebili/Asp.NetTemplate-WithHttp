using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text.Json;
using UserDatabase.DAL.Context;
using UserDatabase.DAL.Repositories.Abstract;
using UserDatabase.DAL.Repositories.Concret;
using UserDatabase.Models;

namespace cs_HttpListener
{
    class Program
    {
        static void Main(string[] args)
        {
            //FillData();
            StartServer();
        }
        static void FillData()
        {
            UserDbContext userDb = new UserDbContext();
            IRepository<User> stockusers = new Repository<User>(userDb.Db);

            var file = File.ReadAllText("./USER_DATA.json");
            var users = JsonSerializer.Deserialize<List<User>>(file);

            foreach (var user in users)
            {
                stockusers.Insert(user);
            }
        }
        static void StartServer()
        {
            var listener = new HttpListener();

            listener.Prefixes.Add("http://localhost:4545/");
            listener.Start();
            Controller controller = new Controller();
            controller.ControllerStart(listener);
        }
    }
}
