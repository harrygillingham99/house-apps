using System.Collections.Generic;
using House.Objects.Attributes;

namespace House.Objects.Objects
{
    [NSwagInclude]
    public class WallboardInfo
    {
        public string CatUrl { get; set; }
        public string RandomImageUri { get; set; }
        public List<Status> ServerStatus { get; set; }
        public List<ProcessInfoResult> Processes { get; set; }
    }
}