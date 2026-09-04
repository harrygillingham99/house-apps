using System;
using System.Collections.Generic;
using System.Management;
using House.Objects.Objects;

namespace House.HLL.ServerStats
{
    public class MemoryStatusProvider : IMemoryStatusProvider
    {
        public IEnumerable<Status> GetMemoryInfo()
        {
            var search = new ManagementObjectSearcher("root\\CIMV2", "Select TotalVisibleMemorySize, FreePhysicalMemory from Win32_OPeratingSystem");

            foreach (var x in search.Get())
            {

                var totalMemory = (ulong)x["TotalVisibleMemorySize"];
                var freeMemory = (ulong)x["FreePhysicalMemory"];

                // -> KB to MB
                var total = (double)totalMemory / 1024; ;
                var free = Convert.ToInt32(Math.Round(Convert.ToDecimal((((double)freeMemory / 1024)))));
                var used = Convert.ToInt32(Math.Round(Convert.ToDecimal((((total - free))))));

                yield return new Status
                {
                    TotalMb = total,
                    FreeMb = free,
                    UsedMb = used
                };
            }
        }
    }

    public interface IMemoryStatusProvider
    {
        IEnumerable<Status> GetMemoryInfo();
    }
}
