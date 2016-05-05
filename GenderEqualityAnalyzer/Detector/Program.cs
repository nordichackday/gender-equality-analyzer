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
            try
            {
                var client = new FaceServiceClient("d389c8fbaf95432d80f8ba99504ee2c3");

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
            catch(Exception e)
            {
                Console.WriteLine("Something went very wrong: " + e.Message);
                return new List<Face>();
            }
            
        }


        static async Task<Article> DetectAndUpdateArticle(Article article)
        {
            if(article.ImageUrl != null)
            {
                var image = article.ImageUrl.TrimEnd(new char[] { '"', '>', '/' });
                Console.WriteLine("Processing image: " + image + " \nfrom article: " + article.Title);
                var faces = await DetectFaces(image);
                Console.WriteLine("Faces found: " + faces.Count());
                article.IsImageParsed = true;
                article.ContainsPerson = (faces.Count() > 0);
                return article;
            }
            //article.IsImageParsed = true;
            //article.ContainsPerson = false;
            return article;
        }

        private static void ProcessUnParsedImages()
        {
            var articleRepo = new ArticleRepository();
            var taskList = new List<Task<Article>>();
            var articles = articleRepo.GetUnParsed().Take(20);
            Console.WriteLine("Number of articles unparsed: " + articles.Count());

            foreach (var article in articles)
            {
                taskList.Add(DetectAndUpdateArticle(article));
            }

            Task.WaitAll(taskList.ToArray());
            foreach (var article in taskList)
            {
                articleRepo.Update(article.Result);
            }

            //foreach (var article in articles)
            //{
            //    var image = article.ImageUrl.TrimEnd(new char[] { '"', '>', '/' });
            //    Console.WriteLine("Processing image: " + image + " \nfrom article: " + article.Title);
            //    var faces = Task.Run(() => DetectFaces(image)).Result;
            //    article.Faces = faces.ToList();
            //    Console.WriteLine("Faces found: " + faces.Count());
            //    article.IsImageParsed = true;
            //    article.ContainsPerson = (faces.Count() > 0);
            //    articleRepo.Update(article);
            //}
            Console.WriteLine("Saving faces to db");
            articleRepo.Save();
        }
        static void Main(string[] args)
        {
            Console.WriteLine("Starting processing unparsed images");
            while(true)
            {
                ProcessUnParsedImages();
                Console.WriteLine("Going to sleep now at: " + DateTime.Now.ToShortTimeString()+ ":" + DateTime.Now.Second.ToString() );
                //System.Threading.Thread.Sleep(1000 * 45); 
            }

        }
    }
}
