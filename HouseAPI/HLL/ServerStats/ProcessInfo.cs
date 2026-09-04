using System;
using System.Collections.Generic;
using System.Management;
using House.Objects.Objects;

namespace House.HLL.ServerStats
{
    public class ProcessInfo : IProcessInfo
    {
        public IEnumerable<ProcessInfoResult> GetProcessInfo()
        {
            var search = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_Process");

            foreach (var x in search.Get())
            {
                var wss = Convert.ToInt64(x["WorkingSetSize"]);
                yield return new ProcessInfoResult
                {
                    Name = x["Name"].ToString(),
                    MemoryMbUsed = wss / 1024 / 1024
                };
            }
        }
    }

    public interface IProcessInfo
    {
        IEnumerable<ProcessInfoResult> GetProcessInfo();
    }
}
