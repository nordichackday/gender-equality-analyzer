using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.DataContext
{
    public class Article
    {
        public int Id { get; set; }
        public string Title { get; set; }
        [Index]
        [MaxLength(200)]
        public string Url { get; set; }
        public string ImageUrl { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime PublishedDate { get; set; }
        public bool IsImageParsed { get; set; }
        public bool ContainsPerson { get; set; }
        public virtual ICollection<Face> Faces { get; set; } 
        public virtual Site Site { get; set; }
    }
}