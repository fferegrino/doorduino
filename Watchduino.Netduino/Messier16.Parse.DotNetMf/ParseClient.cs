using System;
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

        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="appId">Your Parse Application ID</param>
        /// <param name="restKey">Your Parse application REST API Key</param>
        public ParseClient(string appId, string restKey)
        {
            _restKey = restKey;
            _appId = appId;
        }

        /// <summary>
        /// Sends a push notification to the given channels
        /// </summary>
        /// <param name="data">A key-value dictionary that will go into the <code>data</code> section of the request.</param>
        /// <param name="channels">A collection of channels, if no channels are specified the push notification will be sent to the broadcast channel.</param>
        /// <returns>A boolean value indicating wether the push was succesfully sent.</returns>
        public bool SendPushToChannel(Hashtable data, string[] channels = null)
        {
            string chnl = ChannelTransformer(channels);
            string dta = DataTransformer(data);
            string uri = BaseUri + "push";

            HttpStatusCode statusCode = HttpStatusCode.Unauthorized;

            using (HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri))
            {
                request.Method = "POST";
                AddAppKeysHeaders(request);

                string pushContent = "{\"channels\":[" + chnl + "],\"data\":{" + dta + "}}";
                byte[] pushBytes = Encoding.UTF8.GetBytes(pushContent);

                request.ContentType = "application/json";
                request.ContentLength = pushBytes.Length;

                using (Stream requestStream = request.GetRequestStream())
                {
                    requestStream.Write(pushBytes, 0, pushBytes.Length);
                    requestStream.Flush();
                }
                var r = pushBytes.Length;

                // Get the response and response stream
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    using (Stream responseStream = response.GetResponseStream())
                    {
                        statusCode = response.StatusCode;

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
            return statusCode == HttpStatusCode.OK;
        }

        /// <summary>
        /// Sends a push notification to the given channels
        /// </summary>
        /// <param name="alert"></param>
        /// <param name="channels">A collection of channels, if no channels are specified the push notification will be sent to the broadcast channel.</param>
        /// <returns>A boolean value indicating wether the push was succesfully sent.</returns>
        public bool SendPushToChannel(string alert, string[] channels = null)
        {
            Hashtable htData = new Hashtable();
            htData.Add("alert", alert);
            return SendPushToChannel(htData, channels);
        }

        /// <summary>
        /// Adds authentication headers to the web request.
        /// </summary>
        /// <param name="request"></param>
        private void AddAppKeysHeaders(HttpWebRequest request)
        {
            request.Headers.Add("X-Parse-Application-Id", _appId);
            request.Headers.Add("X-Parse-REST-API-Key", _restKey);
        }

        /// <summary>
        /// Transforms a channels array into a formatted json string
        /// </summary>
        /// <param name="channels"></param>
        /// <returns></returns>
        private string ChannelTransformer(string[] channels = null)
        {
            if (channels == null || channels.Length == 0)
            {
                return "\"\"";
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

        /// <summary>
        /// Turns the hashtable into a json formatted string
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private string DataTransformer(Hashtable data)
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
