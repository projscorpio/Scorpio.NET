using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Scorpio.CanOpenEds.DTO;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Scorpio.CanOpenEds.FileRepository
{
    public class CanObjectFileRepository : ICanOpenObjectRepository
    {
        private readonly ILogger<CanObjectFileRepository> _logger;
        private readonly IMemoryCache _cache;
        private readonly string _edsPath;
        private string TreeCacheKey => $"{this.GetType().Name}__tree"; // Include type to distinguish derived types

        public CanObjectFileRepository(ILogger<CanObjectFileRepository> logger, IMemoryCache cache, string edsPath)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(ILogger<CanObjectFileRepository>));
            _cache = cache ?? throw new ArgumentNullException(nameof(IMemoryCache));
            _edsPath = edsPath ?? throw new ArgumentNullException(nameof(FileRepositoryConfiguration));
        }
        
        public Task<CanOpenObjectResponseDTO> GetCanOpenObjectAsync(string index, string subIndex)
        {
            if (_cache.TryGetValue<CanOpenObjectResponseDTO>(GetCanObjectCacheKey(index, subIndex), out var o))
            {
                _logger.LogDebug("CanObject cache hit");
                return Task.FromResult(o);
            }

            try
            {
                var domainModel = ReadFileToDomainModel();
                var obj = domainModel.CanOpenObjects.FirstOrDefault(x => string.Equals(x.Index, index));
                var subObj = obj?.SubObjects.FirstOrDefault(x => string.Equals(x.SubIndex, subIndex));

                if (obj != null && (subObj != null || string.Equals(subIndex, "00")))
                {
                    var key = GetCanObjectCacheKey(index, subIndex);
                    _logger.LogDebug($"Putting CanObject: {key} to cache...");

                    return Task.FromResult(_cache.Set(key, MapCanObject(subObj, obj)));
                }

                return Task.FromResult<CanOpenObjectResponseDTO>(null);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while getting tree from json file.");
                throw;
            }
        }

        private string GetCanObjectCacheKey(string index, string subIndex) => $"{this.GetType().Name}_{index}__{subIndex ?? ""}";

        private static CanOpenObjectResponseDTO MapCanObject(CanOpenSubObjectDTO subObj, CanOpenObjectDTO obj)
        {
            var name = subObj?.Name is null ? obj.Name : $"{obj.Name} - {subObj.Name}";

            return new CanOpenObjectResponseDTO
            {
                Description = subObj?.Description ?? obj.Description,
                Name = name,
                Label = obj.Label,
                Index = obj.Index,
                SubIndex = subObj?.SubIndex,
                AccessType = subObj?.AccessType,
                DataType = subObj?.DataType,
                DefaultValue = subObj?.DefaultValue,
                HighValue = subObj?.HighValue,
                LowValue = subObj?.LowValue,
                ObjectType = subObj?.ObjectType,
                PDOmapping = subObj?.PDOmapping,
                TPDOdetectCOS = subObj?.TPDOdetectCOS
            };
        }

        public Task<TreeDTO> GetTreeAsync()
        {
            if (_cache.TryGetValue<TreeDTO>(TreeCacheKey, out var tree))
            {
                _logger.LogDebug("Tree cache hit");
                return Task.FromResult(tree);
            }

            try
            {
                var domainModel = ReadFileToDomainModel();
                var newTree = MapToTree(domainModel);
                _logger.LogDebug("Putting tree to cache...");

                return Task.FromResult(_cache.Set(TreeCacheKey, newTree));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while getting tree from json file.");
                throw;
            }
        }

        private CanOpenObjectRootDTO ReadFileToDomainModel()
        {
            var text = File.ReadAllText(_edsPath);
            return JsonConvert.DeserializeObject<CanOpenObjectRootDTO>(text);
        }

        private static TreeDTO MapToTree(CanOpenObjectRootDTO json)
        {
            return new TreeDTO
            {
                Count = json.CanOpenObjects.Count,
                Items = json.CanOpenObjects.Select(obj => new TreeItemDTO
                {
                    Name = obj.Name,
                    Index = obj.Index,
                    SubItems = obj.SubObjects.Select(sub => new TreeSubItemDTO
                    {
                        Name = sub.Name,
                        Index = sub.SubIndex
                    }).ToList()
                }).ToList()
            };
        }
    }
}
