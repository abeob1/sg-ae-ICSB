using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

namespace FileUpload
{
    public static class MyExtensions
    {
        public static string AppendTimeStamp(this string fileName)
        {
            return string.Concat(
                Path.GetFileNameWithoutExtension(fileName),
                DateTime.Now.ToString("yyyyMMddHHmmss"),
                Path.GetExtension(fileName)
                );
        }
    }

    public class Stats
    {

    }
}