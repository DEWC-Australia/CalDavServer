
namespace CalDav.Models.FileSystem
{
    public interface IHttpSettings
    {
        // whether to buffer the data sent to server
        bool AllowWriteStreamBuffering { get; set; }
        // whether HttpChunked is supported - I would like to get here
        bool SendChunked { get; set; }
        //time-out in milliseconds.
        int TimeOut { get; set; }
    }
}
