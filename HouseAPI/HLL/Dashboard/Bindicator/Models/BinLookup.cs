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

        public BinLookup(ApiResponse apiResponse)
        {
            if (apiResponse?.Data == null || apiResponse.Data.Count == 0)
            {
                InitializeDefaultBins();
                return;
            }

            BinLookupDto ResolveBin(string binTypeToMatch) =>
                apiResponse.Data
                    .Where(item => item.WasteContainerUsageTypeDescription != null)
                    .FirstOrDefault(bin =>
                        bin.WasteContainerUsageTypeDescription.Equals(binTypeToMatch, StringComparison.InvariantCultureIgnoreCase))
                    ?.ToBinLookupDto()
                ?? new BinLookupDto
                {
                    BinType = binTypeToMatch,
                    PdfLink = string.Empty,
                    Communal = false,
                    Next = DateTime.MinValue,
                    Subsequent = DateTime.MinValue
                };

            Rubbish = new Bin(ResolveBin("Rubbish"));
            Recycling = new Bin(ResolveBin("Recycling"));
            FoodWaste = new Bin(ResolveBin("Food waste"));
        }

        public BinLookup(List<BinLookupDto> dto)
        {
            BinLookupDto ResolveBin(string binType) =>
                dto.FirstOrDefault(bin =>
                bin.BinType.Equals(binType, StringComparison.InvariantCultureIgnoreCase)) 
                ?? new BinLookupDto
                {
                    BinType = binType,
                    PdfLink = string.Empty,
                    Communal = false,
                    Next = DateTime.MinValue,
                    Subsequent = DateTime.MinValue
                };

            Rubbish = new Bin(ResolveBin("rubbish"));
            Recycling = new Bin(ResolveBin("recycling"));
            FoodWaste = new Bin(ResolveBin("food waste"));
        }

        public BinLookup()
        {
            InitializeDefaultBins();
        }

        private void InitializeDefaultBins()
        {
            var emptyDto = new BinLookupDto
            {
                BinType = string.Empty,
                PdfLink = string.Empty,
                Communal = false,
                Next = DateTime.MinValue,
                Subsequent = DateTime.MinValue
            };

            Rubbish = new Bin(emptyDto);
            Recycling = new Bin(emptyDto);
            FoodWaste = new Bin(emptyDto);
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

    // New API Response models
    public class ApiResponse
    {
        public PageInfo PageInfo { get; set; }
        public List<WasteContainerData> Data { get; set; }
    }

    public class PageInfo
    {
        public int Total { get; set; }
        public int Count { get; set; }
        public int PageSize { get; set; }
        public int PageNum { get; set; }
        public int Pages { get; set; }
    }

    public class WasteContainerData
    {
        public string UniqueReference { get; set; }
        public string Address { get; set; }
        public string Uprn { get; set; }
        public string WasteContainerDescription { get; set; }
        public string WasteContainerNLPGPremiseId { get; set; }
        public string WasteContainerUsageTypeDescription { get; set; }
        public string WasteContainerUsageTypeItemId { get; set; }
        public string WasteContainerRoundName { get; set; }
        public string WasteContainerRoundItemId { get; set; }
        public string WasteContainerDomesticSchedule { get; set; }
        public string WasteContainerDomesticScheduleItemId { get; set; }
        public string CollectionDay { get; set; }
        public string CollectionWeek { get; set; }
        public List<DateTime> ScheduleDateRange { get; set; }
        public string PdfLink { get; set; }

        public BinLookupDto ToBinLookupDto()
        {
            var binType = WasteContainerUsageTypeDescription switch
            {
                "Rubbish" => "Rubbish",
                "Recycling" => "Recycling",
                "Food waste" => "Food waste",
                "Garden Waste" => "Garden waste",
                _ => WasteContainerUsageTypeDescription
            };

            DateTime nextDate = DateTime.MinValue;
            DateTime subsequentDate = DateTime.MinValue;

            if (ScheduleDateRange != null && ScheduleDateRange.Count >= 1)
            {
                nextDate = ScheduleDateRange[0];
                if (ScheduleDateRange.Count >= 2)
                {
                    subsequentDate = ScheduleDateRange[1];
                }
            }

            return new BinLookupDto
            {
                BinType = binType,
                PdfLink = PdfLink ?? string.Empty,
                Communal = false,
                Next = nextDate,
                Subsequent = subsequentDate
            };
        }
    }

}
