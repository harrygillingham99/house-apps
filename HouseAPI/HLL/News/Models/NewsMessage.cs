using System;

namespace House.HLL.News.Models
{
    public class NewsMessage
    {
        public string Message { get; set; }

        public string CreatedBy { get; set; }
        public DateTime PublishedAt { get; set; }
    }
}
