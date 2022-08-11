using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace StudentreBackend.Data.Extensions
{
    public abstract class BaseService
    {
        protected DefaultDbContext Context { get; }
        protected IMapper Mapper { get; }

        #region BaseService()
        public BaseService(DefaultDbContext context, IMapper mapper)
        {
            Context = context;
            Mapper = mapper;
        }
        #endregion

        #region Find()
        public virtual T Find<T>(long id) where T : class, IEntity
        {
            return Context.Find<T>(id);
        }
        #endregion

        #region FindAsync()
        public virtual async Task<T> FindAsync<T>(long id) where T : class, IEntity
        {
            return await Context.FindAsync<T>(id);
        }
        #endregion

        #region Attach()
        public virtual T Attach<T>(T entity) where T : class, IEntity
        {
            if (Context.Entry(entity).State == EntityState.Detached)
            {
                Context.Attach<T>(entity);
            }

            return entity;
        }

        public virtual List<T> Attach<T>(params T[] entities) where T : class, IEntity
        {
            Context.AttachRange(entities.Where(p => Context.Entry(p).State == EntityState.Detached));

            return new List<T>(entities);
        }
        #endregion


        #region Create()
        public virtual T Create<T>(T entity) where T : class, IEntity
        {
            Context.Add<T>(entity);
            SaveChanges();

            return entity;
        }

        public virtual List<T> Create<T>(params T[] entities) where T : class, IEntity
        {
            if (entities.Length > 0)
            {
                Context.AddRange(entities);
                SaveChanges();
            }

            return new List<T>(entities);
        }
        #endregion

        #region CreateAsync()
        public virtual async Task<T> CreateAsync<T>(T entity) where T : class, IEntity
        {
            await Context.AddAsync<T>(entity);
            await SaveChangesAsync();

            return entity;
        }

        public virtual async Task<List<T>> CreateAsync<T>(params T[] entities) where T : class, IEntity
        {
            if (entities.Length > 0)
            {
                await Context.AddRangeAsync(entities);
                await SaveChangesAsync();
            }

            return new List<T>(entities);
        }
        #endregion
 
        #region Update()
        public virtual T Update<T>(T entity) where T : class, IEntity
        {
            if (entity != null)
            {
                if (Context.Entry(entity).State == EntityState.Detached)
                {
                    Context.Update(entity);
                }

                SaveChanges();
            }

            return entity;
        }

        public virtual List<T> Update<T>(params T[] entities) where T : class, IEntity
        {
            if (entities.Length > 0)
            {
                Context.UpdateRange(entities.Where(p => Context.Entry(p).State == EntityState.Detached));
                SaveChanges();
            }

            return new List<T>(entities);
        }
        #endregion

        #region UpdateAsync()
        public virtual async Task<T> UpdateAsync<T>(T entity) where T : class, IEntity
        {
            if (entity != null)
            {
                if (Context.Entry(entity).State == EntityState.Detached)
                {
                    Context.Update(entity);
                }

                await SaveChangesAsync();
            }

            return entity;
        }

        public virtual async Task<List<T>> UpdateAsync<T>(params T[] entities) where T : class, IEntity
        {
            if (entities.Length > 0)
            {
                Context.UpdateRange(entities.Where(p => Context.Entry(p).State == EntityState.Detached));
                await SaveChangesAsync();
            }

            return new List<T>(entities);
        }
        #endregion


        #region Remove()
        public virtual T Remove<T>(long id) where T : class, IEntity
        {
            return Remove(Find<T>(id));
        }

        public virtual T Remove<T>(T entity) where T : class, IEntity
        {
            if (entity != null)
            {
                Context.Remove(entity);
                SaveChanges();
            }

            return entity;
        }

        public virtual List<T> Remove<T>(params T[] entities) where T : class, IEntity
        {
            if (entities.Length > 0)
            {
                Context.RemoveRange(entities);
                SaveChanges();
            }

            return new List<T>(entities);
        }
        #endregion

        #region RemoveAsync()
        public virtual async Task<T> RemoveAsync<T>(long id) where T : class, IEntity
        {
            return await RemoveAsync(await FindAsync<T>(id));
        }

        public virtual async Task<T> RemoveAsync<T>(T entity) where T : class, IEntity
        {
            if (entity != null)
            {
                Context.Remove(entity);
                await SaveChangesAsync();
            }
            return entity;
        }

        public virtual async Task<List<T>> RemoveAsync<T>(params T[] entities) where T : class, IEntity
        {
            if (entities.Length > 0)
            {
                Context.RemoveRange(entities);
                await SaveChangesAsync();
            }
            return new List<T>(entities);
        }
        #endregion

        #region SaveChanges()
        public int SaveChanges()
        {
            return Context.SaveChanges();
        }

        public int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            return Context.SaveChanges(acceptAllChangesOnSuccess);
        }
        #endregion

        #region SaveChangesAsync()
        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            return await Context.SaveChangesAsync(cancellationToken);
        }

        public async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await Context.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }
        #endregion
    }
}