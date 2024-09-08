using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWebServer.Models
{
    /// <summary>
    /// DTO that gets returned by the router to the Static Server
    /// </summary>
    public class RouterResponsePacket
    {
        public string? Redirect { get; set; }
        public required byte[] Data { get; set; }
        public string? ContentType { get; set; }
        public Encoding? Encoding { get; set; }
    }
}
