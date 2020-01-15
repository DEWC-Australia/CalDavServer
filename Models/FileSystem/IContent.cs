
namespace CalDav.Models.FileSystem
{
    public interface IContent
    {
        
        // size of the file content in bytes
        long ContentLength { get; }
        //media type of the file
        string ContentType { get; }
        //entity tag - string that identifies current state of resource's content.
        // string.Format("{0}-{1}", Modified.ToBinary(), this.serialNumber)
        string Etag { get; }

    }
}
