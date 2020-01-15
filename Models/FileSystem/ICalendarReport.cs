
namespace CalDav.Models.FileSystem
{
    public interface ICalendarReport
    {
        // supportMultiGet - Gets a list of calendar files that correspont to the specified list of item paths
        // supportQuery - find calendars that match a specified filter
        void MultiGet();
        void Query(string filter);
    }
}
