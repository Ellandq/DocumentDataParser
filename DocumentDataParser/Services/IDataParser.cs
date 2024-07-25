using System.IO;
using System.Threading.Tasks;

namespace DocumentDataParser.Services
{
    public interface IDataParser
    {
        Task<bool> ParseDataAsync(Stream fileStream);
    }
}
