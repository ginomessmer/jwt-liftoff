using RestSharp;
using RestSharp.Extensions.MonoHttp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;

namespace JwtLiftoff.Client.Console
{
    // Just a quick console application, nothing fancy though
    public class Program
    {
        public const string JWT_TOKEN_RELATIVE_PATH = "/api/jwt";
        public const string JWT_BOX_RELATIVE_PATH = "/api/WhatsInTheBox";

        static void Main(string[] args)
        {
            ForegroundColor = ConsoleColor.White;
            WriteLine("\nTell me the base remote URI of your LwtLiftoff endpoint:");

            string endpointUrl = ReadLine();
            Uri endpointUriResult;

            if(Uri.TryCreate(endpointUrl, UriKind.Absolute, out endpointUriResult))
            {
                WriteLine("Tell me your username:");
                string username = ReadLine();

                WriteLine("And your password:");
                string password = ReadLine();

                WriteLine("Let's try to generate a JSON Web Token for you and see what's inside the box.");

                var jwtClient = new RestClient(endpointUriResult.ToString() + JWT_TOKEN_RELATIVE_PATH);
                var jwtRequest = new RestRequest(Method.POST);
                jwtRequest.AddHeader("content-type", "application/x-www-form-urlencoded");
                jwtRequest.AddParameter("application/x-www-form-urlencoded", 
                    $"username={HttpUtility.UrlEncode(username)}&password={HttpUtility.UrlEncode(password)}", 
                    ParameterType.RequestBody);
                IRestResponse jwtResponse = jwtClient.Execute(jwtRequest);

                if(jwtResponse.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var jwt = jwtResponse.Content;
                    WriteLine("Looking good, we got your JWT to authenticate. "
                        + $"Let's try to find out what's inside {endpointUriResult.ToString() + JWT_BOX_RELATIVE_PATH}");

                    var boxClient = new RestClient(endpointUriResult.ToString() + JWT_BOX_RELATIVE_PATH);
                    var boxRequest = new RestRequest(Method.GET);
                    boxRequest.AddHeader("authorization", $"Bearer {jwt}");
                    IRestResponse boxResponse = boxClient.Execute(boxRequest);

                    if(boxResponse.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        ForegroundColor = ConsoleColor.Green;
                        WriteLine("We received a response:\n\n" + boxResponse.Content);
                        ReadLine();                        
                    }
                    else
                    {
                        ForegroundColor = ConsoleColor.Red;
                        WriteLine("Something went wrong while, try again...");
                        Main(args);
                    }
                }
                else
                {
                    ForegroundColor = ConsoleColor.Red;
                    WriteLine("Something went wrong while, try again...");
                    Main(args);
                }
            }
            else
            {
                ForegroundColor = ConsoleColor.Red;
                WriteLine("Not a valid URI, try again...");
                Main(args);
            }
        }
    }
}
