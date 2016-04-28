using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DataContext;
using Microsoft.ProjectOxford.Face;
using Core.Repositories;

namespace Detector
{
    class Program
    {
        private static Gender DetermineGender(string gender)
        {
            if (gender == "male")
            {
                return Gender.Male;
            }
            else if (gender == "female")
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

        static void Main(string[] args)
        {
            Console.WriteLine("Starting processing unparsed images");
            while(true)
            {
                ProcessUnParsedImages();
                Console.WriteLine("Going to sleep now at: " + DateTime.Now.ToShortTimeString()+ ":" + DateTime.Now.Second.ToString() );
                System.Threading.Thread.Sleep(1000 * 45); 
            }

        }

        private static void ProcessUnParsedImages()
        {
            var articleRepo = new ArticleRepository();
            var articles = articleRepo.GetUnParsed().Take(20);
            Console.WriteLine("Number of articles unparsed: " + articles.Count());
            foreach (var article in articles)
            {
                Console.WriteLine("Processing image: " + article.ImageUrl + " from article: " + article.Title);
                var faces = Task.Run(() => DetectFaces(article.ImageUrl)).Result;
                article.Faces =  faces.ToList();
                article.IsImageParsed = true;
                article.ContainsPerson = (faces.Count() > 0);
                articleRepo.Update(article);
            }
            Console.WriteLine("Saving faces to db");
            articleRepo.Save();
        }
    }
}
