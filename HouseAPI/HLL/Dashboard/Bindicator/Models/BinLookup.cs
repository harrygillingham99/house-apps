using System;
using System.Collections.Generic;
using System.Linq;

namespace House.HLL.Dashboard.Bindicator.Models
{
    public class BinLookup
    {
        public Bin Rubbish { get; set; }
        public Bin Recycling { get; set; }
        public Bin FoodWaste { get; set; }

        public BinLookup(List<BinLookupDto> dto)
        {
            BinLookupDto ResolveBin(string binType) =>
                dto.FirstOrDefault(bin =>
                bin.BinType.Equals(binType, StringComparison.InvariantCultureIgnoreCase));

            Rubbish = new Bin(ResolveBin("rubbish"));
            Recycling = new Bin(ResolveBin("recycling"));
            FoodWaste = new Bin(ResolveBin("food waste"));
        }

        public BinLookup()
        {
        }
    }

    public class BinLookupDto
    {
        public string BinType { get; set; }
        public string PdfLink { get; set; }
        public bool Communal { get; set; }
        public DateTime Next { get; set; }
        public DateTime Subsequent { get; set; }
    }

}
