
namespace CalDav.Models.FileSystem
{
    public interface IFileSystemAccess
    {
        bool AccessControl { get; }
        bool CalendarAccess { get; }
        bool CalendarServerSharing { get; }

        bool Owner { get; }
        bool Read { get; }
        bool Write { get; }
    }
}
