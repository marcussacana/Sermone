using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.WebAssembly.Http;
using Newtonsoft.Json;
using Sermone.Tools;

namespace Sermone.Pastes
{

    public class CadenceCreator : IPasteCreator
    {
        public string Name => "Cadence";
        public async Task<IPaste> Create(string Username, string Password, HttpClient Client)
        {
            if (Client == null)
            {
                Client = new HttpClient();
            }

            Cadence Cadence = new Cadence(Username, Password, Client);
            await Cadence.GetToken();
            return Cadence;
        }

        class Cadence : IPaste
        {
            readonly HttpClient Client;
            readonly string Username;
            readonly string Password;

            string Token;

            public Cadence(string Username, string Password, HttpClient Client)
            {
                this.Username = Username;
                this.Password = Password;
                this.Client = Client;
            }

            public async Task<Dictionary<long, string>> EnumPastes()
            { 
                var Rst = await DoRequest("GET", API: $"pastes/?preview=256&author={Username}");
                var Pastes = JsonConvert.DeserializeObject<PasteInfo[]>(Rst);

                Dictionary<long, string> Results = new Dictionary<long, string>();
                foreach (var Paste in  Pastes)
                {
                    string Title = Paste.content.Split('\n').First();
                    Results.Add(Paste.pasteID, Title);
                }

                return Results;
            }

            public async Task<string[]> GetPaste(long PasteId)
            {
                var Rst = await DoRequest("GET", ID: PasteId);
                var PasteInfo = JsonConvert.DeserializeObject<PasteInfo>(Rst);

                return PasteInfo.content.Replace("\r\n", "\n")
                    .Split('\n').Skip(1).ToArray();
            }

            public string GetPasteUrl(long PasteId)
            {
                return $"https://cadence.moe/api/pastes/{PasteId}/raw";
            }

            public async Task<long> CreatePaste(string Title, string[] Value)
            {
                StringBuilder Builder = new StringBuilder();
                Builder.AppendLine(Title);
                if (Value != null)
                    foreach (string String in Value)
                    {
                        if (String.Contains("\n") || String.Contains("\r"))
                        {
                            Builder.AppendLine(String.Replace("\r", "").Replace("\n", " "));
                        }
                        else
                            Builder.AppendLine(String);
                    }


                var Rst = await DoRequest("POST", new PostRequest
                {
                    content = Builder.ToString(),
                    username = Username,
                    token = Token
                });

                return JsonConvert.DeserializeObject<PostResponse>(Rst).pasteID;
            }

            public async Task SetPaste(string Title, string[] Value, long PasteId)
            {
                StringBuilder Builder = new StringBuilder();
                Builder.AppendLine(Title);
                if (Value != null)
                    foreach (string String in Value)
                    {
                        if (String.Contains("\n") || String.Contains("\r"))
                        {
                            Builder.AppendLine(String.Replace("\r", "").Replace("\n", " "));
                        }
                        else
                            Builder.AppendLine(String);
                    }


                await DoRequest("PATCH", new PatchRequest
                {
                    content = Builder.ToString(),
                    token = Token
                }, PasteId);
            }

            public async Task GetToken()
            {
                if (!string.IsNullOrEmpty(Token)) return;

                string Rst = await DoRequest("POST", new LoginRequest
                {
                    username = Username,
                    password = Password
                }, API: "accounts/tokens");

                LoginRespose Response = JsonConvert.DeserializeObject<LoginRespose>(Rst);

                Token = Response.token;
            }
            
            async Task<string> DoRequest(string Method, long ID = 0, string API = "pastes")
            {
                return await DoRequest<object>(Method, null, ID, API);
            }

            async Task<string> DoRequest<T>(string Method, T Data, long ID = 0, string API = "pastes")
            {
                try
                {                    
                    var URL = $"https://cadence.moe/api/{API}{(ID != 0 ? $"/{ID}" : "")}";
                    HttpContent Content = null;
                    if (Method != "GET")  {
                        var JSON = JsonConvert.SerializeObject(Data);
                        Content = new StringContent(JSON, Encoding.UTF8, "application/json");
                    }
                    
                    var Result = Method switch
                    {
                        "GET" => await Client.GetAsync(URL),
                        "POST" => await Client.PostAsync(URL, Content),
                        "PATCH" => await Client.PatchAsync(URL, Content),
                        "PUT" => await Client.PutAsync(URL, Content),
                        "DELETE" => await Client.DeleteAsync(URL),
                        _ => throw new HttpRequestException("Invalid HTTP Method")
                    };                    

                    return await Result.Content.ReadAsStringAsync();
                }
                catch (HttpRequestException ex)
                {
                    Console.Write(ex.ToString());
                    return null;
                }
            }

#pragma warning disable 649
            struct PostRequest
            {
                public string username;
                public string token;
                public string content;
            }

            struct PostResponse
            {
                public long pasteID;
            }

            struct PatchRequest
            {
                public string content;
                public string token;
            }

            struct LoginRequest
            {
                public string username;
                public string password;
            }

            struct LoginRespose
            {
                public string token;
                public long expires;
            }

            struct PasteInfo
            {
                public long pasteID;
                public string author;
                public long creationTime;
                public long? expiryTime;
                public string content;
            }
#pragma warning restore 649
        }
    }
}