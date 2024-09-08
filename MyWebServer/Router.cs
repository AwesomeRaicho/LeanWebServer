using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using MyWebServer.Models;
using static System.Net.Mime.MediaTypeNames;

namespace MyWebServer
{

    /// <summary>
    /// This router class is meant to serve small files, if the end goal is to serve larger files, a sender 
    /// class may be needed to be able to use Outstreamwritter (send chunks) this router is expexting to encode small files
    /// </summary>
    public class Router
    {
        string _hostDir;
        private Dictionary<string, ExtensionInfo> _extFolderMap;
        
        public Router(string hostDir)
        {
            _hostDir = hostDir;
            _extFolderMap = new Dictionary<string, ExtensionInfo>()
            {
              {"ico", new ExtensionInfo() {Loader=ImageLoader, ContentType="image/ico"}},
              {"png", new ExtensionInfo() {Loader=ImageLoader, ContentType="image/png"}},
              {"jpg", new ExtensionInfo() {Loader=ImageLoader, ContentType="image/jpg"}},
              {"gif", new ExtensionInfo() {Loader=ImageLoader, ContentType="image/gif"}},
              {"bmp", new ExtensionInfo() {Loader=ImageLoader, ContentType="image/bmp"}},
              {"html", new ExtensionInfo() {Loader=FileLoader, ContentType="text/html"}},
              {"css", new ExtensionInfo() {Loader=FileLoader, ContentType="text/css"}},
              {"js", new ExtensionInfo() {Loader=FileLoader, ContentType="text/javascript"}},
              {"", new ExtensionInfo() {Loader=FileLoader, ContentType="text/html"}},
            };

        }


        public RouterResponsePacket Route(string filePath)
        {
            string extension = RightOfRightmostOf(filePath, '.');
            if(string.IsNullOrEmpty(extension)) 
            {
                extension = "html";
                filePath += ".html";
            }
            ExtensionInfo? extInfo;
            RouterResponsePacket responsePacket;

            if(_extFolderMap.TryGetValue(extension, out extInfo))
            {
                if (File.Exists(filePath))
                {
                    responsePacket = extInfo.Loader(filePath, extension, extInfo);
                }
                else
                {
                    string errorPath = Path.Combine(_hostDir, "notFound.html");
                    string errorText = File.ReadAllText(errorPath);


                    responsePacket = new RouterResponsePacket()
                    {
                        Data = Encoding.UTF8.GetBytes(errorText),
                        Encoding = Encoding.UTF8,
                        ContentType = "text/html",
                        Redirect = "notFound",
                    };
                };
                

            }
            else
            {
                string errorPath = Path.Combine(_hostDir, "notFound.html");
                string errorText = File.ReadAllText(errorPath);


                responsePacket = new RouterResponsePacket()
                {
                    Data = Encoding.UTF8.GetBytes(errorText),
                    Encoding = Encoding.UTF8,
                    ContentType = "text/html",
                    Redirect = "notFound",
                };
            }

            return responsePacket;


        }


        private RouterResponsePacket FileLoader(string filePath, string extension, ExtensionInfo extInfo)
        {
            string text = File.ReadAllText(filePath);
            RouterResponsePacket packet = new RouterResponsePacket()
            {
                ContentType = extInfo.ContentType,
                Data = Encoding.UTF8.GetBytes(text),
                Encoding = Encoding.UTF8
            };

            return packet;
        }
        private RouterResponsePacket ImageLoader(string filePath, string extension, ExtensionInfo extInfo)
        {
            FileStream fStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fStream);

            RouterResponsePacket packet = new RouterResponsePacket()
            {
                Data = br.ReadBytes((int)fStream.Length),
                ContentType = extInfo.ContentType,
            };
            br.Close();
            fStream.Close();

            return packet;

        }


        private string RightOfRightmostOf(string src, char c)
        {
            string ret = String.Empty;
            int idx = src.LastIndexOf(c);

            if (idx != -1)
            {
                ret = src.Substring(idx + 1);
            }

            return ret;
        }

    }
}
