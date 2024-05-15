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

                if (request.HasEntityBody)
                {
                    var body = request.InputStream;
                    var encoding = request.ContentEncoding;
                    var reader = new StreamReader(body, encoding);
                    if (request.ContentType != null)
                    {
                        Console.WriteLine("Client data content type {0}", request.ContentType);
                    }
                    Console.WriteLine("Client data content length {0}", request.ContentLength64);

                    Console.WriteLine("Start of data:");
                    string s = reader.ReadToEnd();
                    Console.WriteLine(s);
                    Console.WriteLine("End of data:");
                    reader.Close();
                    body.Close();
                }

                switch(request.Url.AbsolutePath)
                {
                    case "/":
                        context.Response.StatusCode = (int)HttpStatusCode.OK;
                        context.Response.ContentType = "text/html";
                        context.Response.OutputStream.Write(Encoding.UTF8.GetBytes("<h1>Hello World!</h1>"));
                        break;
                    case "/Register":
                        context.Response.StatusCode = (int)HttpStatusCode.OK;
                        context.Response.ContentType = "application/octet-stream";
                        UserForm userForm = null;
                        
                        using (StreamReader r = new StreamReader("file.json"))
                        {
                            string json = r.ReadToEnd();
                            userForm = JsonSerializer.Deserialize<UserForm>(json);
                        }

                        if (File.Exists($"./{userForm.Username}"))
                        {
                            context.Response.OutputStream.Write(new byte[] { 0 }, 0, 1);
                        }
                        else
                        {

                            User user = new User();
                            user.Username = userForm.Username;
                            user.RealName = userForm.RealName;
                            user.RealLastName = userForm.RealLastName;
                            user.Email = userForm.Email;
                            user.GenerateSalt();
                            user.PasswordHash = Encoders.PassowrdHasher.Hash(userForm.Password, user.salt);
                            user.PushToDatabase();
                            int rnd = (new Random()).Next();
                            keyValuePairs.Add(rnd, userForm.Username);
                            context.Response.OutputStream.Write(BitConverter.GetBytes(rnd), 0, 1);
                        }
                        break;
                    case "/Login":
                        context.Response.StatusCode = (int)HttpStatusCode.OK;
                        context.Response.ContentType = "application/octet-stream";
                        UserForm userFormLogin = null;
                        using (StreamReader r = new StreamReader("file.json"))
                        {
                            string json = r.ReadToEnd();
                            userFormLogin = JsonSerializer.Deserialize<UserForm>(json);
                        }
                        if (File.Exists($"./{userFormLogin.Username}"))
                        {
                            User user = User.PullFromDatabase();
                            byte[] passwordHash = Encoders.PassowrdHasher.Hash(userFormLogin.Password, user.salt);
                            if (passwordHash == user.PasswordHash)
                            {
                                int rnd = (new Random()).Next();
                                keyValuePairs.Add(rnd, userFormLogin.Username);
                                context.Response.OutputStream.Write(BitConverter.GetBytes(rnd), 0, 1);
                            } 
                            break;
                        }
                        else
                        {
                            context.Response.OutputStream.Write(new byte[] { 0 }, 0, 1);
                        }
                    default:
                        context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                        break;
                }

                var response = context.Response;
                response.StatusCode = (int)HttpStatusCode.OK;
                response.ContentType = "text/plain";
                response.OutputStream.Write(new byte[] { }, 0, 0);
                response.OutputStream.Close();

                Receive();
            }

            
        }
        private Dictionary<int, string> keyValuePairs = new Dictionary<ulong, string>();
    }
}
