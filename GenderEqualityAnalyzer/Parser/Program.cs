using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;
using Core.DataContext;
using Microsoft.ProjectOxford.Face;

namespace Parser
{
    class Program
    {
        static void Main(string[] args)
        {
            GetSR();
        }

        static void GetSR()
        {
            var xml = GetSRXML().Result;

            XNamespace _xmlns = "http://www.sitemaps.org/schemas/sitemap/0.9";


            var urls = from x in xml.Elements(_xmlns + "urlset").Elements(_xmlns + "url")
                select x.Element(_xmlns + "loc").Value;


            var images = Parse(urls.Take(1));
            var faces = Task.Run(() => DetectFaces(images.First().ImageUrl)).Result;

        }

        private static Gender DetermineGender (string gender)
        {
            if(gender == "male")
            {
                return Gender.Male;
            }
            else if(gender == "female")
            {
                return Gender.Female;
            }

            return Gender.Unknown;
        }

        static async Task<IEnumerable<Face>> DetectFaces(string imageUrl)
        {
            var client = new FaceServiceClient("12d8c6e300bb43dfaddfac5b62d9cde8");

            var faces = await client.DetectAsync(imageUrl, true, true, new FaceAttributeType[] { FaceAttributeType.Age, FaceAttributeType.FacialHair,
                                                                                                    FaceAttributeType.Gender, FaceAttributeType.Glasses,
                                                                                                    FaceAttributeType.HeadPose, FaceAttributeType.Smile});
            return faces.Select(x => new Face()
            {
                Article = null,
                Age = x.FaceAttributes.Age,
                BeardFactor = x.FaceAttributes.FacialHair.Beard,
                FaceId = x.FaceId,
                Gender = DetermineGender(x.FaceAttributes.Gender),
                HeadPitch = x.FaceAttributes.HeadPose.Pitch,
                HeadRoll = x.FaceAttributes.HeadPose.Roll,
                HeadYaw = x.FaceAttributes.HeadPose.Yaw,
                MoustacheFactor = x.FaceAttributes.FacialHair.Moustache,
                SideburnsFactor = x.FaceAttributes.FacialHair.Sideburns,
                SmileFactor = x.FaceAttributes.Smile
            });
        } 


        static IEnumerable<Article> Parse(IEnumerable<string> urls)
        {
            var tasks = new List<Task<Article>>();

            foreach (var url in urls)
            {
                var task = Parse(url);
                tasks.Add(task);
            }

            Task.WaitAll(tasks.ToArray());

            return tasks.Select(x => x.Result);
        }

        static async Task<Article> Parse(string url)
        {
            var httpClient = new HttpClient();

            var html = await httpClient.GetStringAsync(url);

            var regex = new Regex(@"<meta property=\""og:image\"" content=\""([\S]*)\""", RegexOptions.IgnoreCase);

            var match = regex.Match(html);
            if (match.Success)
            {
                var img = match.Groups[1].Value;
                Console.WriteLine(img);

                return new Article()
                {
                    ImageUrl = img
                };
            }
            return new Article();

        }

        private static async Task<XDocument> GetSRXML()
        {
            var httpClient = new HttpClient();

            return  XDocument.Parse(await httpClient.GetStringAsync("http://sverigesradio.se/sida/newssitemap.aspx"));

            
        }
    }
}
