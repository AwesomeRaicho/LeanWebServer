using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWebServer.Models
{
    public class ExtensionInfo
    {
        public string? ContentType { get; set; }
        /// <summary>
        /// arg1: filePath, arg2: extension, arg3: ExtensionInfo 
        /// </summary>
        public required Func<string, string, ExtensionInfo, RouterResponsePacket> Loader { get; set; }
    }
}
