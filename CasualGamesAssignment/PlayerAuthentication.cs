using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Formatting;
using System.Text;
using System.Threading.Tasks;

namespace CasualGamesAssignment
{
    public enum AUTHSTATUS { NONE,OK,INVALID,FAILED }


    static public class PlayerAuthentication
    {
        static public string baseWebAddress = "http://localhost:53824/";


        static public string PlayerToken = "";
        static public AUTHSTATUS PlayerStatus = AUTHSTATUS.NONE;
        // public PlayerAuthentication()
        //  {
        //    baseWebAddress = ConfigurationManager.AppSettings["AzureEndPoint"] as string ?? "";

        //}

        //static async public Task<List<GameScoreObject>>(int count, string GameName)
        //    {
        //    return null;
        //    }

        static public List<Score> getScores(int count)
            {
            using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var response = client.GetAsync(baseWebAddress + "api/HighScores/" + count.ToString() + "/").Result;
                    var resultContent = response.Content.ReadAsAsync<List<Score>>(
                        new[] { new JsonMediaTypeFormatter() }).Result;
                    return resultContent;
                }
            }

    static async public Task<bool> login(string username, string password)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("grant_type", "password"),
                        new KeyValuePair<string, string>("username", username),
                        new KeyValuePair<string, string>("password", password),
                    });
                var result = client.PostAsync(baseWebAddress + "Token", content).Result;
                try
                {
                    var resultContent = result.Content.ReadAsAsync<Token>(
                        new[] { new JsonMediaTypeFormatter() }
                        ).Result;
                    string ServerError = string.Empty;
                    if (!(String.IsNullOrEmpty(resultContent.AccessToken)))
                    {
                        Console.WriteLine(resultContent.AccessToken);
                        PlayerToken = resultContent.AccessToken;
                        PlayerStatus = AUTHSTATUS.OK;
                        return true;
                    }
                    else
                    {
                        PlayerToken = "Invalid Login" ;
                        PlayerStatus = AUTHSTATUS.INVALID;
                        Console.WriteLine("Invalid credentials");
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    PlayerStatus = AUTHSTATUS.FAILED;
                    PlayerToken = "Server Error -> " + ex.Message;
                    Console.WriteLine(ex.Message);
                    return false;
                }
                PlayerStatus = AUTHSTATUS.INVALID;
                return false;

            }
        }

        

}
}
