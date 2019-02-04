using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Xml.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TweetSharp;

namespace TwitterApplication
{
    public class Program
    {


        private static readonly string ConsumerKey = "hwmqUviS8JQnYvOuf6rQNjMHx";
        private static readonly string ConsumerKeySecret = "Vcki4kPXwp1JoQIaxhWUnpiWZgDuMiBQ0SoYCoK8mnUqs5RrBH";
        private static readonly string AccessToken = "184886104-o8FIj1bAdCMjh1GczMJ9xfrzf4STeWjPOk7O4rIY";
        private static readonly string AccessTokenSecret = "2ue2vtthMnI3GQKaTICCq090tOp4vcFX2CwY6f6yBAyE6";


        private static void Main(string[] args)
        {
            try
            {
                //TweetMessage().Wait();
                //FindFollowers();
                TestAPI();
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc);
            }
            finally
            {
                Console.ReadLine();
            }
        }
        
        //private static string tags = "#TripleTalaqBill #BJPRocks ";
        //private static string tags = "#மூத்திரச்சட்டி #திராவிடம் @Suba_Vee @mathimaran ";
        //private static string tags = "#CongressLies #CongressRuinedIndia ";
        //private static string tags = "#IfCongressWins #SoniaLoots #AccidentalPM ";
        //private static string tags = "#NamoEmpowersMiddleClass ";
        //private static string tags = "#SaveKeralaFromCommunists ";
        //private static string tags = "#TNWelcomesModi #திருக்குறள் #இழிமகன்சுடலை  ";
        //private static string tags = "#RamMandir #Ayodhya @narendramodi @myogiadityanath ";
        private static string tags = "#Test ";
        //#TNWelcomesModi


        public static string FindFollowers()
        {
            string handle = "narendramodi";
            String response = TwitterFollowers.GetTwitterFollowersi(ConsumerKey, ConsumerKeySecret, AccessToken, AccessTokenSecret, handle);
            Console.WriteLine(response);
            JObject oj = JObject.Parse(response);


            XDocument doc = JsonConvert.DeserializeObject<XDocument>(response);
            Console.WriteLine(doc.ToString());
            return response;
        }


        public static void TestAPI()
        {
            TwitterService twitterService = new TwitterService(ConsumerKey, ConsumerKeySecret);
            
            ListFollowersOptions userID = new ListFollowersOptions();
            //options.UserId = tuSelf.Id;
            userID.ScreenName = "narendramodi";
            userID.IncludeUserEntities = true;
            userID.SkipStatus = false;
            userID.Cursor = -1;
            TwitterCursorList<TwitterUser> followers = twitterService.ListFollowers(userID);
            while (followers != null)
            {
                foreach (TwitterUser follower in followers)
                {
                    //Do something with the user profile here
                }

                userID.Cursor = followers.NextCursor;
                followers = twitterService.ListFollowers(userID);
            }
        }

        private static async Task TweetMessage()
        {
            TwitterApi twitter = new TwitterApi(ConsumerKey, ConsumerKeySecret, AccessToken, AccessTokenSecret);
            string[] txtTweets = File.ReadAllText("Tweets\\SriRamMandir.txt").Split(('\n'));
            int count = 0;
            List<int> lst = new List<int>();
            foreach (string twt in txtTweets)
            {
                string msg = $"{tags} {twt}";
                if (msg.Length > 280)
                {
                    Console.WriteLine("Message {0} is too long.", ++count);
                    lst.Add(count);
                    //Console.WriteLine("{0} is more than 280 chars.", msg);
                    //msg = msg.Substring(279);
                }
                else
                {
                    string response = await twitter.Tweet(msg);
                    Console.WriteLine(response);
                }
            }
            int[] array = lst.ToArray();
        }
    }
}
