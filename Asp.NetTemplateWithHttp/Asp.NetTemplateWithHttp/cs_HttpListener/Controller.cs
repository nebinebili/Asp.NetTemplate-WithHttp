using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using UserDatabase.DAL.Context;
using UserDatabase.DAL.Repositories.Abstract;
using UserDatabase.DAL.Repositories.Concret;
using UserDatabase.Models;

namespace cs_HttpListener
{
    public class Controller
    {

        public static UserDbContext userDb { get; set; } = new UserDbContext();
        public IRepository<User> stockusers { get; set; } = new Repository<User>(userDb.Db);

        public void ControllerStart(HttpListener listener)
        {
            while (true)
            {

                var context = listener.GetContext();

                var request = context.Request;
                var response = context.Response;

                var reader = new StreamReader(request.InputStream);
                var writer = new StreamWriter(response.OutputStream);

                if (request.HttpMethod == "GET")
                {
                    GetMethod(writer, response);
                }
                else if (request.HttpMethod == "POST")
                {
                    PostMethod(reader, writer, response);
                }
                else if (request.HttpMethod == "PUT")
                {
                    PutMethod(reader, writer, response);
                }
                else if (request.HttpMethod == "DELETE")
                {
                    DeleteMethod(request,response);
                }

                response.Close();
            }
        }
        public void GetMethod(StreamWriter writer, HttpListenerResponse response)
        {
            var json = JsonSerializer.Serialize(stockusers.Get());
            writer.WriteLine(json);
            writer.Flush();
            response.StatusCode = 200;
            response.ContentType = "application/json";
        }

        public void PostMethod(StreamReader reader, StreamWriter writer, HttpListenerResponse response)
        {
            var userJson = reader.ReadToEnd();
            var user = JsonSerializer.Deserialize<User>(userJson);

            if (stockusers.Get().Any(u => u.Email == user.Email))
            {
                response.StatusCode = 404;
            }
            else
            {
                var newuser = new User
                {
                    Name = user.Name,
                    Email = user.Email,
                    Password = user.Password
                };

                stockusers.Insert(newuser);

                response.StatusCode = 201; // Created
                response.ContentType = "application/json";
            }

           
        }

        public void PutMethod(StreamReader reader, StreamWriter writer, HttpListenerResponse response)
        {
            var userJson = reader.ReadToEnd();
            var user = JsonSerializer.Deserialize<User>(userJson);

            stockusers.Update(user);
            response.StatusCode = 200;
            response.ContentType = "application/json";
        }

        public void DeleteMethod(HttpListenerRequest request, HttpListenerResponse response)
        {
            var email = request.QueryString["Email"];
            var password = request.QueryString["Password"];

            if(stockusers.Get().Any(u=>u.Email==email)&& stockusers.Get().Any(u => u.Password == password))
            {
                var user = stockusers.Get().Where(u => u.Email == email).Where(u => u.Password == password).FirstOrDefault();
                stockusers.Delete(user);
                response.StatusCode = 200;
            }
            else
            {
                response.StatusCode = 404;              
            }
            
        }
    }
}
