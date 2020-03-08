using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace Scorpio.CanOpenEds.FileRepository
{
    public class ScorpioCanObjectFileRepository : CanObjectFileRepository, IScorpioCanOpenObjectRepository
    {
        public ScorpioCanObjectFileRepository(ILogger<CanObjectFileRepository> logger, IMemoryCache cache, string edsPath) : base(logger, cache, edsPath)
        {
        }
    }
}