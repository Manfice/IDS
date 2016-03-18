using System.Collections.Generic;
using IndDev.Domain.Entity;

namespace IndDev.Models
{
    public class NewsViewModel
    {
        public IEnumerable<News> News { get; set; }
        public News CurrentNews { get; set; }
        public PageInfo PageInfo { get; set; }
        public string Category { get; set; }
        public string ReturnUrl { get; set; }
    }
}