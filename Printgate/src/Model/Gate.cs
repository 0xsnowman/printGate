using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Printgate.Model
{
    public class Gate
    {
        private GateSettings settings;

        public Gate()
        {
        }

        public Gate(GateSettings settings)
        {
            this.settings = settings;
        }

        public string GetDataTimeFromTimeStamp(double timestamp)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return origin.AddSeconds(timestamp).ToString();
        }

        public async Task<List<TableReservation>> GetTableDataFromServer(string url)
        {
            var result = new List<TableReservation>();
            
            var data = await GetDataFromServer(url);
            if (data != null && data.Count() > 0)
            {
                foreach (var e in data)
                {
                    var temp = JsonConvert.DeserializeObject<TableReservation>(e.ToString());
                    result.Add(temp);
                }
            }

            return result;
        }

        public async Task<List<TakeAwayReservation>> GetTakeAwayDataFromServer(string url)
        {
            var result = new List<TakeAwayReservation>();

            var data = await GetDataFromServer(url);
            if (data != null && data.Count() > 0)
            {
                foreach (var e in data)
                {
                    var temp = JsonConvert.DeserializeObject<TakeAwayReservation>(e.ToString());
                    result.Add(temp);
                }
            }

            return result;
        }

        public async Task<List<RoomReservation>> GetRoomDataFromServer(string url)
        {
            var result = new List<RoomReservation>();
            
            var data = await GetDataFromServer(url);
            if (data != null && data.Count() > 0)
            {
                foreach (var e in data)
                {
                    var temp = JsonConvert.DeserializeObject<RoomReservation>(e.ToString());
                    result.Add(temp);
                }
            }

            return result;
        }

        public async Task<string> SetTableReservationToServer(string url, bool get = false)
        {
            var result = await GetDataFromServer(url, get);
            return result == null ? null : result.ToString();
        }

        public async Task<string> SetTakeAwayReservationToServer(string url, bool get = false)
        {
            var result = await GetDataFromServer(url, get);
            return result == null ? null : result.ToString();
        }

        private async Task<JToken> GetDataFromServer(string url, bool get = true)
        {
            JToken result;
            WebRequest request = WebRequest.Create(url);
            if (!get)
                request.Method = "PUT";

            request.Credentials = CredentialCache.DefaultCredentials;
            System.Net.ServicePointManager.ServerCertificateValidationCallback = (senderX, certificate, chain, sslPolicyErrors) => { return true; };
            WebResponse response = await request.GetResponseAsync();
            using (var dataStream = response.GetResponseStream())
            {
                var reader = new StreamReader(dataStream);
                string responseFromServer = reader.ReadToEnd();
                Console.WriteLine(responseFromServer);
                try
                {
                    if (get)
                        result = JObject.Parse(responseFromServer)["data"]["data"];
                    else
                        result = JObject.Parse(responseFromServer)["data"];

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return null;
                }
            }
            response.Close();
            return result;
        }
    }
}
