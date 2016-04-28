using System;
using System.Collections.Generic;

namespace Core.DataContext
{
    public class Article
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public string ImageUrl { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime PublishedDate { get; set; }
        public bool IsImageParsed { get; set; }
        public bool ContainsPerson { get; set; }
        public virtual ICollection<Face> Faces { get; set; } 
    }
}