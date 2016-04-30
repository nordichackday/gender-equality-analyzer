using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DataContext
{
    public class BlacklistedImageUrl
    {
        public int Id { get; set; }
        [Index]
        [MaxLength(200)]
        public string ImageUrl { get; set; }
    }
}
