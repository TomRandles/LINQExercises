using System;
using System.Collections.Generic;
using System.Text;

namespace LINQExercises
{
    public static class ExtensionMethods
    {
        public static int MyCount <T>(this IEnumerable<T> collection)
        {
            int count = 0;
            foreach (var i in collection)
            {
                count++;
            }
            return count;
        }
    }
}
