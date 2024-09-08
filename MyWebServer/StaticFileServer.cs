using MyWebServer.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace MyWebServer
{
    public class StaticFileServer
    {
        private string _hostUrl; 
        private string _hostDir;
        private Router _router;

        private HttpListener? listener;
        private bool running = false;
        
        public StaticFileServer( string hostUrl, string hostDir) 
        {
            this._hostUrl = hostUrl;
            this._hostDir = hostDir;
            _router = new Router(hostDir);
        }

        public async Task<int> RunAsync(string[] args)
        {
            listener = new HttpListener();
            listener.Prefixes.Add(_hostUrl);
            listener.Start();
            Console.WriteLine($"Listening on {_hostUrl}");
            running = true;


            // LISTENING LOOP !!!!!
            while (running)
            {
                HttpListenerContext ctx = await listener.GetContextAsync();

                Console.WriteLine($"New request: ###################");
                Console.WriteLine($"Url: {ctx.Request.Url}");
                Console.WriteLine($"Url.AbsolutePath: {ctx.Request.Url?.AbsolutePath}");
                Console.WriteLine($"Url.AbsoluteUri: {ctx.Request.Url?.AbsoluteUri}");
                Console.WriteLine($"Headers: {ctx.Request.Headers}");
                Console.WriteLine($"RemoteEndPoint: {ctx.Request.RemoteEndPoint}");
                Console.WriteLine($"RawUrl: {ctx.Request.RawUrl}");
                //ctx.Request.HttpMethod



                //this method will take care of responding

                //[get Response Packet]
                string? route = ctx.Request.Url?.AbsolutePath.TrimStart('/');
                string filePath;

                if (string.IsNullOrEmpty(route))
                {
                    filePath = GetFilePath("index.html");
                }
                else
                {
                    filePath = GetFilePath(route);
                }

                RouterResponsePacket packet = _router.Route(filePath);

                if (string.IsNullOrEmpty(packet.Redirect))
                {
                    ctx.Response.StatusCode = (int)HttpStatusCode.OK;

                }
                else
                {
                    ctx.Response.StatusCode = (int)HttpStatusCode.NotFound;

                }

                await SendFileAsync(ctx.Response, packet);




                //here we just out put what we sent.
                Console.WriteLine("New Response: ###################");
                Console.WriteLine($"Status Code: {ctx.Response.StatusCode}");
                Console.WriteLine($"Content Type: {ctx.Response.ContentType}");
                Console.WriteLine($"ContentLength64: {ctx.Response.ContentLength64}");

                ctx.Response.Close();

            }
            // LISTENING LOOP STOPED HERE

            //e close the listener and we return. 
            listener.Close();
            return 0;
        }

        public string GetFilePath(string route)
        {
            return Path.Combine(_hostDir, route);
        }

        

        private async Task SendFileAsync(HttpListenerResponse response, RouterResponsePacket packet)
        {
            

            try
            {
                response.ContentType = packet.ContentType;
                response.ContentLength64 = packet.Data.Length;
                response.ContentEncoding = packet.Encoding;
                await response.OutputStream.WriteAsync(packet.Data, 0, packet.Data.Length);
                response.OutputStream.Close();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }



    }
}







////////////////////////////////////////////////////////////////////////
//private async Task SendErrorFileAsync(HttpListenerResponse response, RouterResponsePacket packet)
//{


//    try
//    {
//        response.ContentType = packet.ContentType;
//        response.ContentLength64 = packet.Data.Length;
//        response.ContentEncoding = packet.Encoding;
//        response.StatusCode = (int)HttpStatusCode.OK;
//        await response.OutputStream.WriteAsync(packet.Data, 0, packet.Data.Length);
//        response.OutputStream.Close();

//    }
//    catch (Exception ex)
//    {
//        throw new Exception(ex.Message);
//    }

//}

//private async Task ProccessRequest(HttpListenerContext ctx, RouterResponsePacket packet)

//{
//    if (File.Exists(filePath))
//    {
//        try
//        {
//            ctx.Response.StatusCode = (int)HttpStatusCode.OK;
//            await SendFileAsync(ctx.Response, filePath);

//            return;

//        }
//        catch(HttpListenerException ex) 
//        {
//            ctx.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
//            Console.WriteLine($"HttpException: {ex.Message}");


//        }catch(Exception ex)
//        {
//            ctx.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

//            Console.WriteLine($"{ex.GetType()}: {ex.Message}");

//        }

//    }


//    //THIS SNIPPET HANDLES NOT FOUND FILES
//    {
//        try
//        {
//            ctx.Response.StatusCode = (int)HttpStatusCode.NotFound;
//            await SendFileAsync(ctx.Response, Path.Combine(hostDir, "notFound.html"));

//            return;

//        }
//        catch (HttpListenerException ex)
//        {
//            ctx.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
//            Console.WriteLine($"HttpException: {ex.Message}");


//        }
//        catch (Exception ex)
//        {
//            ctx.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

//            Console.WriteLine($"{ex.GetType()}: {ex.Message}");

//        }
//    }


//}