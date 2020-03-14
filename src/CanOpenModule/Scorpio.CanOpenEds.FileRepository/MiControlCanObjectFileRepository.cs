using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace Scorpio.CanOpenEds.FileRepository
{
    public class MiControlCanObjectFileRepository : CanObjectFileRepository
    {
        public MiControlCanObjectFileRepository(ILogger<CanObjectFileRepository> logger, IMemoryCache cache, string edsPath) : base(logger, cache, edsPath)
        {
        }
    }
}
