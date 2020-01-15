
namespace CalDav.Models.FileSystem
{
    public interface ICalendar: ICalendarRepository, IFileSystemInfo, IFileSystemAccess, IContent, IHttpSettings
    {
    }
}
