using System.Collections.Generic;
using System.Threading.Tasks;

namespace House.HLL.TerrariaRunner.Interfaces
{
    public interface ITerrariaRunner
    {
        List<string> GetCurrentLogsTail(int secondsToCapture);
        bool Start();
        void InputCommand(string command);
    }
}