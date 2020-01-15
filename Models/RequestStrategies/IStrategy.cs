
using CalDav.Models.FileSystem.Folder;
using CalDav.Models.Requests;

namespace CalDav.Models.RequestStrategies
{
    public interface IStrategy
    {
        Result DoOperation();
    }
    
}
