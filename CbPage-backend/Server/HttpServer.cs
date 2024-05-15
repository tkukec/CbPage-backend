using CbPage_backend.DataBase;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Unicode;

namespace CbPage_backend.Server
{
    public class HttpServer
    {
        public int Port = 8080;

        private HttpListener _listener;

        public void Start()
        {
            _listener = new HttpListener();
            _listener.Prefixes.Add("http://*:" + Port.ToString() + "/");
            _listener.Start();
            Receive();
        }

        public void Stop()
        {
            _listener.Stop();
        }

        private void Receive()
        {
            _listener.BeginGetContext(new AsyncCallback(ListenerCallback), _listener);
        }

        private void ListenerCallback(IAsyncResult result)
        {
            if (_listener.IsListening)
            {
                var context = _listener.EndGetContext(result);
                var request = context.Request;

                Console.WriteLine($"{request.HttpMethod} {request.Url}");

                //if (request.HasEntityBody)
                //{
                //    var body = request.InputStream;
                //    var encoding = request.ContentEncoding;
                //    var reader = new StreamReader(body, encoding);
                //    if (request.ContentType != null)
                //    {
                //        Console.WriteLine("Client data content type {0}", request.ContentType);
                //    }
                //    Console.WriteLine("Client data content length {0}", request.ContentLength64);

                //    Console.WriteLine("Start of data:");
                //    string s = reader.ReadToEnd();
                //    Console.WriteLine(s);
                //    Console.WriteLine("End of data:");
                //    reader.Close();
                //    body.Close();
                //}
                context.Response.AddHeader("Access-Control-Allow-Origin", "*");
                context.Response.AddHeader("Access-Control-Allow-Methods", "GET, POST, OPTIONS, PUT, PATCH, DELETE");
                context.Response.AddHeader("Access-Control-Allow-Headers", "Content-Type, X-Requested-With");
                context.Response.AddHeader("Access-Control-Allow-Credentials", "true");

                switch (request.Url.AbsolutePath)
                {
                    case "/":
                        context.Response.StatusCode = (int)HttpStatusCode.OK;

                        context.Response.ContentType = "text/html";
                        context.Response.OutputStream.Write(Encoding.UTF8.GetBytes("<h1>Hello World!</h1>"));
                        break;
                    case "/register":
                        context.Response.StatusCode = (int)HttpStatusCode.OK;
                        context.Response.ContentType = "application/json";
                        //context.Response.ContentType = "application/octet-stream";
                        UserForm userForm = null;
                        //var data = Encoding.UTF8.GetBytes("{\"Response\" : 1}");
                        //context.Response.OutputStream.Write(data, 0, data.Length);
                        //break;
                        try
                        {
                            using (var stream = context.Request.InputStream)
                            {
                                using (StreamReader r = new StreamReader(stream, context.Request.ContentEncoding))
                                {

                                    string json = r.ReadToEnd();
                                    Console.WriteLine(json);
                                    Console.WriteLine("Got Here");
                                    Console.WriteLine(context.Request.ContentType);
                                    userForm = JsonSerializer.Deserialize<UserForm>(json);
                                }
                            }
                        }catch(Exception e)
                        {

                            var data = Encoding.UTF8.GetBytes("{\"Response\" : 0}");
                            context.Response.OutputStream.Write(data, 0, data.Length);
                        }

                        if (File.Exists($".users/{userForm.Username}"))
                        {
                            var data = Encoding.UTF8.GetBytes("{\"Response\" : 0}");
                            context.Response.OutputStream.Write(data, 0, data.Length);
                        }
                        else
                        {

                            User user = new User(userForm.Username, userForm.RealName, userForm.RealLastName, userForm.Email);
                            user.GenerateSalt();
                            user.PasswordHash = Encoders.PasswordHasher.Hash(userForm.Password, user.salt);
                            user.PushToDatabase();
                            int rnd = (new Random()).Next();
                            keyValuePairs.Add(rnd, userForm.Username);
                            var data = Encoding.UTF8.GetBytes("{\"Response\" : }" + rnd.ToString());
                            context.Response.OutputStream.Write(data, 0, data.Length);
                        }
                        break;
                    case "/login":
                        context.Response.StatusCode = (int)HttpStatusCode.OK;
                        context.Response.ContentType = "application/json";

                        UserForm userFormLogin = null;
                        using (StreamReader r = new StreamReader("file.json"))
                        {
                            string json = r.ReadToEnd();
                            userFormLogin = JsonSerializer.Deserialize<UserForm>(json);
                        }
                        if (File.Exists($".users/{userFormLogin.Username}"))
                        {
                            User user = User.PullFromDatabase(userFormLogin.Username);
                            byte[] passwordHash = Encoders.PasswordHasher.Hash(userFormLogin.Password, user.salt);
                            if (passwordHash == user.PasswordHash)
                            {
                                int rnd = (new Random()).Next();
                                keyValuePairs.Add(rnd, userFormLogin.Username);

                                var data = Encoding.UTF8.GetBytes("{\"Response\" : }" + rnd.ToString());
                                context.Response.OutputStream.Write(data, 0, data.Length);
                            } 
                        }
                        else
                        {

                            var data = Encoding.UTF8.GetBytes("{\"Response\" : }" + 0);
                            context.Response.OutputStream.Write(data, 0, data.Length);
                        }
                        break;

                    default:
                        context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                        break;
                }

                //var response = context.Response;
                //response.StatusCode = (int)HttpStatusCode.OK;
                //response.ContentType = "text/plain";
                //response.OutputStream.Write(new byte[] { }, 0, 0);
                context.Response.OutputStream.Close();



                Receive();
            }

            
        }
        private Dictionary<int, string> keyValuePairs = new Dictionary<int, string>();
    }
}
