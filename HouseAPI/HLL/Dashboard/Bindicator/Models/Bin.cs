using System;

namespace House.HLL.Dashboard.Bindicator.Models
{
    public class Bin
    {

        public DateTime Subsequent { get; set; }
        public DateTime Next { get; set; }
        public string PdfLink { get; set; }
        public bool Communal { get; set; }

        // For some reason the council has next and previous wrong
        // previous refers to the nearest collection in future
        // next means the collection after previous
        public Bin(BinLookupDto dto)
        {
            Subsequent = dto.Subsequent;
            Next = dto.Next;
            PdfLink = dto.PdfLink;
            Communal = dto.Communal;
        }

    }
}
