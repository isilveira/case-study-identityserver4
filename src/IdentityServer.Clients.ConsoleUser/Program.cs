using IdentityModel.Client;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;

namespace IdentityServer.Clients.ConsoleUser
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            Console.ReadLine();

            var client = new HttpClient();
            var disco = client.GetDiscoveryDocumentAsync("http://localhost:5000").Result;
            if (disco.IsError)
            {
                Console.WriteLine(disco.Error);
                Console.ReadLine();
                return;
            }
            Console.WriteLine("Enter user data...");
            Console.WriteLine("\nUser name: ");
            string userName = Console.ReadLine();
            Console.WriteLine("\nPassword: ");
            string userPassword = Console.ReadLine();

            Console.WriteLine($"\n\nRequest user ({userName}) token...\n");
            var token = client.RequestPasswordTokenAsync(new PasswordTokenRequest
            {
                Address = disco.TokenEndpoint,

                ClientId = "client.console_user",
                ClientSecret = "4264F8BA-51D0-D271-E49E-E4C2E1B31744",
                Scope = "api1",
                UserName = userName,
                Password = userPassword
            }).Result;

            if (token.IsError)
            {
                Console.WriteLine(token.Error);
                Console.ReadLine();
                return;
            }

            Console.WriteLine(token.Json);
            Console.ReadLine();
            // call api
            client.SetBearerToken(token.AccessToken);

            var response = client.GetAsync("http://localhost:6100/api/identity").Result;
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            else
            {
                var content = response.Content.ReadAsStringAsync().Result;
                Console.WriteLine(JArray.Parse(content));
            }

            Console.ReadLine();
        }
    }
}
