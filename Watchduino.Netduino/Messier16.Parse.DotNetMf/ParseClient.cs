using System;
//using Microsoft.SPOT;
using System.Text;
using System.Net;
using System.Collections;
using System.IO;


namespace Messier16.Parse.DotNetMf
{
    public class ParseClient
    {
        private readonly string _restKey;
        private readonly string _appId;
        const string CurrentVersion = "/1/";
        const string BaseUri = "https://api.parse.com" + CurrentVersion;
        public ParseClient(string appId, string restKey)
        {
            _restKey = restKey;
            _appId = appId;
        }

        public void SendPushToChannel(Hashtable data, string[] channels = null)
        {
            string chnl = ChannelTransformer(channels);
            string dta = DataTransformer(data);
            string uri = BaseUri + "push";

            using (HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri))
            {
                AddAppKeysHeaders(request);
                request.Method = "POST";

                string pushContent = "{\"channels\":[" + chnl + "],\"data\":{" + dta + "}}";
                byte[] pushBytes = Encoding.UTF8.GetBytes(pushContent);

                request.ContentType = "application/json";
                request.ContentLength = pushBytes.Length;

                using (Stream requestStream = request.GetRequestStream())
                {
                    requestStream.Write(pushBytes, 0, pushBytes.Length);
                    requestStream.Flush();
                }

                // Get the response and response stream
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    using (Stream responseStream = response.GetResponseStream())
                    {
                        var statusCode = response.StatusCode;

                        // Set up a response buffer and read in the response
                        byte[] responseBytes = new byte[(int)response.ContentLength];

                        int index = 0;
                        int count = 100;
                        while (true)
                        {
                            index += responseStream.Read(responseBytes, index, count);
                            if (responseBytes.Length - index < count)
                                count = responseBytes.Length - index;
                            if (count <= 0)
                                break;
                        }

                        // Convert the response to a string
                        var cr = Encoding.UTF8.GetChars(responseBytes);
                    }
                }
            }
        }

        public void SendPushAlertToChanel(string data, string[] channels = null)
        {
            Hashtable htData = new Hashtable();
            htData.Add("alert", data);
            SendPushToChannel(htData, channels);
        }

        private void AddAppKeysHeaders(HttpWebRequest request)
        {
            request.Headers.Add("X-Parse-Application-Id", _appId);
            request.Headers.Add("X-Parse-REST-API-Key", _restKey);
        }

        public string ChannelTransformer(string[] channels = null)
        {
            if (channels == null || channels.Length == 0)
            {
                return "\"" + String.Empty + "\"";
            }
            else
            {
                StringBuilder sb = new StringBuilder("\"" + channels[0] + "\"");
                for (int i = 1; i < channels.Length; i++)
                {
                    sb.Append(',').Append('\"').Append(channels[i]).Append('\"');
                }
                return sb.ToString();
            }
        }

        public string DataTransformer(Hashtable data)
        {
            string transformedData = "";
            if (data != null && data.Count > 0)
            {
                int count = data.Count;
                int index = 0;
                foreach (DictionaryEntry de in data)
                {
                    transformedData += "\"" + de.Key + "\":\"" + de.Value + "\"";
                    if (++index < count)
                        transformedData += ",";
                }
            }
            return transformedData;
        }


    }
}
