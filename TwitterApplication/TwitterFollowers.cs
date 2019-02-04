using System;
using System.Data.SqlTypes;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json.Linq;

namespace TwitterApplication
{
    public class TwitterFollowers
    {



            public static string GetTwitterFollowersi(string consumerKey, string consumerKeySecret, string accessToken, string accessTokenSecret, string screenName)
        {
                // oauth application keys
                string oauth_token = accessToken;
                string oauth_token_secret = accessTokenSecret;

                string oauth_consumer_key = consumerKey;
                string oauth_consumer_secret = consumerKeySecret;

                // oauth implementation details
                string oauth_version = "1.0";
                string oauth_signature_method = "HMAC-SHA1";

                // unique request details
                string oauth_nonce = Convert.ToBase64String(new ASCIIEncoding().GetBytes(DateTime.Now.Ticks.ToString()));
                var timeSpan = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
                string oauth_timestamp = Convert.ToInt64(timeSpan.TotalSeconds).ToString();

                // message api details
                string status = "Updating status via REST API if this works";
            //var resource_url = "https://api.twitter.com/1.1/users/show.json";
            var resource_url = "https://api.twitter.com/1.1/followers/ids.json";
            
            string screen_name = screenName;
                // create oauth signature
                string baseFormat = "oauth_consumer_key={0}&oauth_nonce={1}&oauth_signature_method={2}" +
                "&oauth_timestamp={3}&oauth_token={4}&oauth_version={5}&screen_name={6}";

                string baseString = string.Format(baseFormat,
                oauth_consumer_key,
                oauth_nonce,
                oauth_signature_method,
                oauth_timestamp,
                oauth_token,
                oauth_version,
                Uri.EscapeDataString(screen_name)
                );

                baseString =
                    string.Concat("GET&", Uri.EscapeDataString(resource_url), "&", Uri.EscapeDataString(baseString));

                string compositeKey = string.Concat(Uri.EscapeDataString(oauth_consumer_secret),
                "&", Uri.EscapeDataString(oauth_token_secret));

                string oauth_signature;
                using (HMACSHA1 hasher = new HMACSHA1(ASCIIEncoding.ASCII.GetBytes(compositeKey)))
                {
                    oauth_signature = Convert.ToBase64String(
                    hasher.ComputeHash(ASCIIEncoding.ASCII.GetBytes(baseString)));
                }

                // create the request header
                var headerFormat = "OAuth oauth_nonce=\"{0}\", oauth_signature_method=\"{1}\", " +
                "oauth_timestamp=\"{2}\", oauth_consumer_key=\"{3}\", " +
                "oauth_token=\"{4}\", oauth_signature=\"{5}\", " +
                "oauth_version=\"{6}\"";

                string authHeader = string.Format(headerFormat,
                Uri.EscapeDataString(oauth_nonce),
                Uri.EscapeDataString(oauth_signature_method),
                Uri.EscapeDataString(oauth_timestamp),
                Uri.EscapeDataString(oauth_consumer_key),
                Uri.EscapeDataString(oauth_token),
                Uri.EscapeDataString(oauth_signature),
                Uri.EscapeDataString(oauth_version)
                );


            string responseData;
            
             ServicePointManager.Expect100Continue = false;
            string postBody = "screen_name=" + Uri.EscapeDataString(screen_name);
            postBody += "&cursor=1624478381695873342";
            resource_url += "?" + postBody ;
            string url;
            var nextcur = "";
            //do
            //{
                url = resource_url;
                url = nextcur == "" ? resource_url : resource_url + "&cursor=" + nextcur;
                HttpWebRequest request = (HttpWebRequest) WebRequest.Create(url);
                request.Headers.Add("Authorization", authHeader);
                request.Method = "GET";
                request.ContentType = "application/x-www-form-urlencoded";

                WebResponse response = request.GetResponse();
                 responseData =
                    new StreamReader(response.GetResponseStream() ?? throw new InvalidOperationException()).ReadToEnd();
                //response.Close();
                var twitterObj = JObject.Parse(responseData);
                nextcur = twitterObj["next_cursor_str"].ToString();

           // } while (!string.IsNullOrEmpty(nextcur) && nextcur != "0");

            return responseData;
        }
        

    }
}