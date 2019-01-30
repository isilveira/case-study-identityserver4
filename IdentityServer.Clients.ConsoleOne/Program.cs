using IdentityModel.Client;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace IdentityServer.Clients.ConsoleOne
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

            var token = client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = disco.TokenEndpoint,

                ClientId = "client_app1",
                ClientSecret = "client_app1_secret",
                Scope = "api1"
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

            var response = client.GetAsync("http://localhost:5100/api/identity").Result;
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
