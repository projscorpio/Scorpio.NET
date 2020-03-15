using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Scorpio.Api.Models;
using Scorpio.Api.Paging;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Scorpio.Api.DataAccess
{
    public class MongoRepository<TEntity> : IGenericRepository<TEntity, string> where TEntity : EntityBase
    {
        private IMongoDatabase _database;
        protected IOptions<MongoDbConfiguration> Options;
        protected IMongoCollection<TEntity> Collection;
        private readonly object _lock = new object();

        public MongoRepository(IOptions<MongoDbConfiguration> options)
        {
            lock (_lock)
            {
                var settings = MongoClientSettings.FromConnectionString(options.Value.ConnectionString);
                settings.ServerSelectionTimeout = TimeSpan.FromMilliseconds(options.Value.ConnectionTimeoutMs);
                var mongoClient = new MongoClient(settings);

                _database = mongoClient.GetDatabase(options.Value.Database);
                var collectionName = typeof(TEntity).Name;
                Collection = _database.GetCollection<TEntity>(collectionName);
                Options = options;
            }
        }

        public virtual async Task<TEntity> CreateAsync(TEntity entity)
        {
            var insertOptions = new InsertOneOptions
            {
                BypassDocumentValidation = false
            };

            await Collection.InsertOneAsync(entity, insertOptions);
            return entity;
        }

        public virtual async Task DeleteAsync(TEntity entity)
        {
            await Collection.DeleteOneAsync(x => x.Id == entity.Id);
        }

        public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await Collection.Find(_ => true).ToListAsync();
        }

        public async Task<PagedList<TEntity>> GetPaged(PageParam pageParam)
        {
            var toSkip = (pageParam.PageNumber - 1) * pageParam.ItemsPerPage;
            var query = Collection.Find(_ => true);
            var totalTask = query.CountDocumentsAsync();
            var itemsTask = query.Skip(toSkip).Limit(pageParam.ItemsPerPage).ToListAsync();
            await Task.WhenAll(totalTask, itemsTask);

            return PagedList<TEntity>.Build(itemsTask.Result, totalTask.Result, pageParam.ItemsPerPage, pageParam.PageNumber);
        }

        public async Task<PagedList<TEntity>> GetManyFilteredAndPaged(Expression<Func<TEntity, bool>> predicate, PageParam pageParam)
        {
            var toSkip = (pageParam.PageNumber - 1) * pageParam.ItemsPerPage;
            var query = Collection.Find(predicate);
            var totalTask = query.CountDocumentsAsync();
            var itemsTask = query.Skip(toSkip).Limit(pageParam.ItemsPerPage).ToListAsync();
            await Task.WhenAll(totalTask, itemsTask);

            return PagedList<TEntity>.Build(itemsTask.Result, totalTask.Result, pageParam.ItemsPerPage, pageParam.PageNumber);
        }

        public async Task<TEntity> GetLatestFiltered(Expression<Func<TEntity, bool>> predicate)
        {
            return await Collection
                .Find(predicate)
                .SortByDescending(x => x.Id)
                .FirstOrDefaultAsync();
        }

        public virtual async Task<TEntity> GetByIdAsync(string id)
        {
            return await Collection.Find(x => x.Id == id).FirstOrDefaultAsync();
        }

        public virtual async Task<TEntity> GetFiltered(Expression<Func<TEntity, bool>> predicate)
        {
            return await Collection.Find(predicate).FirstOrDefaultAsync();
        }

        public virtual async Task<IEnumerable<TEntity>> GetManyFiltered(Expression<Func<TEntity, bool>> predicate)
        {
            return await Collection.Find(predicate).ToListAsync();
        }

        public virtual async Task<TEntity> UpdateAsync(TEntity entity)
        {
            await Collection.ReplaceOneAsync(x => x.Id == entity.Id, entity);
            return entity;
        }

        #region IDisposable Support
        private bool _disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    _database = null;
                    Collection = null;
                }

                _disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}
