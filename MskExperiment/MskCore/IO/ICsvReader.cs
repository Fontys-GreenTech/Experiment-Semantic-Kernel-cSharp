using System.Collections.Generic;
using System.Threading.Tasks;

namespace MskCore.IO;

public interface ICsvReader
{
    public Task<List<Dictionary<string, string>>> ReadCsvAsync();
}