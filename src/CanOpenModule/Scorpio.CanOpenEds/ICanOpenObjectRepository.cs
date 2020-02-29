using Scorpio.CanOpenEds.DTO;
using System.Threading.Tasks;

namespace Scorpio.CanOpenEds
{
    public interface ICanOpenObjectRepository
    {
        /// <summary>
        /// Gets tree structure of all commands
        /// </summary>
        /// <returns></returns>
        Task<TreeDTO> GetTreeAsync();

        /// <summary>
        /// Gets single command details
        /// </summary>
        /// <returns></returns>
        Task<CanOpenObjectResponseDTO> GetCanOpenObjectAsync(string index, string subIndex);
    } 
}
