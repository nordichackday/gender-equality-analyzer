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
            ProcessUnParsedImages();
            var image = @"https://media.licdn.com/mpr/mpr/shrinknp_200_200/p/3/000/115/1e1/3122221.jpg";
        }

        private static void ProcessUnParsedImages()
        {
            var articleRepo = new ArticleRepository();
            var articles = articleRepo.GetUnParsed().Take(10);
            foreach (var article in articles)
            {
                var faces = Task.Run(() => DetectFaces(article.ImageUrl)).Result;
                article.Faces =  faces.ToList();
                articleRepo.Update(article);
            }
            articleRepo.Save();
        }
    }
}
