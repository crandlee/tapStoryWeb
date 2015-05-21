using System;
using System.Globalization;

namespace tapStoryWebApi.Extensions
{
    public static class CoreExtensions
    {
        public static Boolean IsNumeric(this String input, NumberStyles numberStyle)
        {
            Double temp;
            return Double.TryParse(input, numberStyle, CultureInfo.CurrentCulture, out temp);
        }

        public static void Empty(this System.IO.DirectoryInfo directory)
        {
            foreach (var file in directory.GetFiles()) file.Delete();
            foreach (var subDirectory in directory.GetDirectories()) subDirectory.Delete(true);
        }
    }
}