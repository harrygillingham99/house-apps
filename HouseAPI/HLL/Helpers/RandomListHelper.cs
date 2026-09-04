using System;
using System.Collections.Generic;

namespace House.HLL.Helpers
{
    public static class RandomListHelper
    {
        public static T GetRandomItemFromList<T>(this List<T> list, Random random) => list[random.Next(list.Count)];
    }
}
