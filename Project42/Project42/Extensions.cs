using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project42
{
    static class Extensions
    {
        public static void AddAll<T>(this List<T> list, params T[] objs)
        {
            foreach (T obj in objs)
                list.Add(obj);
        }

        public static void AddAll<T>(this List<T> list, List<T> list2)
        {
            foreach (T obj in list2)
                list.Add(obj);
        }

        public static void AddAll<T>(this ObservableCollection<T> list, List<T> list2)
        {
            foreach (T obj in list2)
                list.Add(obj);
        }

        public static byte StringToByte(this string hex)
        {
            int NumberChars = hex.Length;
            byte @byte;

            @byte = Convert.ToByte(hex.Substring(0, 2), 16);

            return @byte;
        }
    }
}
