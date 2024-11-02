using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using WebMaker.Categories;
using WebMaker.EntityFrameworkCore;

namespace WebMaker.Repositories;

public class CategoryTranslationRepository : EfCoreRepository<WebMakerDbContext, CategoryTranslation, Guid>, ICategoryTranslationRepository
{
    public CategoryTranslationRepository(IDbContextProvider<WebMakerDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }

    public async Task<List<CategoryTranslation>> GetListAsync(bool includeDetails = false, CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();
        var query = dbContext.Set<CategoryTranslation>().AsQueryable();

        if (includeDetails)
        {
            query = WithDetails(query);
        }

        return await query.ToListAsync(cancellationToken);
    }

    public async Task<List<CategoryTranslation>> GetPagedListAsync(
        int skipCount, 
        int maxResultCount, 
        string sorting,
        bool includeDetails = false,
        CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();
        var query = dbContext.Set<CategoryTranslation>().AsQueryable();

        if (includeDetails)
        {
            query = WithDetails(query);
        }

        return await query
            .Skip(skipCount)
            .Take(maxResultCount)
            .ToListAsync(cancellationToken);
    }

    public IQueryable<CategoryTranslation> WithDetails()
    {
        return GetQueryable().Include(x => x.Category);
    }

    public IQueryable<CategoryTranslation> WithDetails(params Expression<Func<CategoryTranslation, object>>[] propertySelectors)
    {
        var query = GetQueryable();

        if (propertySelectors != null)
        {
            foreach (var propertySelector in propertySelectors)
            {
                query = query.Include(propertySelector);
            }
        }

        return query;
    }

    public async Task<IQueryable<CategoryTranslation>> WithDetailsAsync()
    {
        return WithDetails();
    }

    public async Task<IQueryable<CategoryTranslation>> WithDetailsAsync(params Expression<Func<CategoryTranslation, object>>[] propertySelectors)
    {
        return WithDetails(propertySelectors);
    }

    public async Task<IQueryable<CategoryTranslation>> GetQueryableAsync()
    {
        return (await GetDbContextAsync()).Set<CategoryTranslation>();
    }

    public async Task<List<CategoryTranslation>> GetListAsync(
        Expression<Func<CategoryTranslation, bool>> predicate,
        bool includeDetails = false,
        CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();
        var query = dbContext.Set<CategoryTranslation>().Where(predicate);

        if (includeDetails)
        {
            query = WithDetails(query);
        }

        return await query.ToListAsync(cancellationToken);
    }

    public async Task<CategoryTranslation> InsertAsync(
        CategoryTranslation entity,
        bool autoSave = false,
        CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();

        await dbContext.Set<CategoryTranslation>().AddAsync(entity, cancellationToken);

        if (autoSave)
        {
            await dbContext.SaveChangesAsync(cancellationToken);
        }

        return entity;
    }

    public async Task InsertManyAsync(
        IEnumerable<CategoryTranslation> entities,
        bool autoSave = false,
        CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();

        await dbContext.Set<CategoryTranslation>().AddRangeAsync(entities, cancellationToken);

        if (autoSave)
        {
            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task<CategoryTranslation> UpdateAsync(
        CategoryTranslation entity,
        bool autoSave = false,
        CancellationToken cancellationToken = default) 
    {
        var dbContext = await GetDbContextAsync();

        dbContext.Set<CategoryTranslation>().Update(entity);

        if (autoSave)
        {
            await dbContext.SaveChangesAsync(cancellationToken);
        }

        return entity;
    }

    public async Task UpdateManyAsync(
        IEnumerable<CategoryTranslation> entities,
        bool autoSave = false,
        CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();

        dbContext.Set<CategoryTranslation>().UpdateRange(entities);

        if (autoSave)
        {
            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task DeleteAsync(
        CategoryTranslation entity,
        bool autoSave = false,
        CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();

        dbContext.Set<CategoryTranslation>().Remove(entity);

        if (autoSave)
        {
            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task DeleteManyAsync(
        IEnumerable<CategoryTranslation> entities,
        bool autoSave = false,
        CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();

        dbContext.Set<CategoryTranslation>().RemoveRange(entities);

        if (autoSave)
        {
            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task<CategoryTranslation?> FindAsync(
        Expression<Func<CategoryTranslation, bool>> predicate,
        bool includeDetails = true,
        CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();
        var query = dbContext.Set<CategoryTranslation>().AsQueryable();

        if (includeDetails)
        {
            query = WithDetails(query);
        }

        return await query.FirstOrDefaultAsync(predicate, cancellationToken);
    }

    public async Task<CategoryTranslation> GetAsync(
        Expression<Func<CategoryTranslation, bool>> predicate,
        bool includeDetails = true,
        CancellationToken cancellationToken = default)
    {
        var entity = await FindAsync(predicate, includeDetails, cancellationToken);

        if (entity == null)
        {
            throw new EntityNotFoundException(typeof(CategoryTranslation));
        }

        return entity;
    }

    public async Task DeleteAsync(
        Expression<Func<CategoryTranslation, bool>> predicate,
        bool autoSave = false,
        CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();
        var entities = await dbContext.Set<CategoryTranslation>()
            .Where(predicate)
            .ToListAsync(cancellationToken);

        await DeleteManyAsync(entities, autoSave, cancellationToken);
    }

    public async Task DeleteDirectAsync(
        Expression<Func<CategoryTranslation, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();
        await dbContext.Set<CategoryTranslation>()
            .Where(predicate)
            .ExecuteDeleteAsync(cancellationToken);
    }

    public async Task<CategoryTranslation> GetAsync(
        Guid id,
        bool includeDetails = true,
        CancellationToken cancellationToken = default)
    {
        var entity = await FindAsync(id, includeDetails, cancellationToken);

        if (entity == null)
        {
            throw new EntityNotFoundException(typeof(CategoryTranslation), id);
        }

        return entity;
    }

    public async Task<CategoryTranslation?> FindAsync(
        Guid id,
        bool includeDetails = true,
        CancellationToken cancellationToken = default)
    {
        return await FindAsync(e => e.Id == id, includeDetails, cancellationToken);
    }

    public async Task<bool> Test(string slug, Guid? excludeCategoryId = null)
    {
        var dbContext = await GetDbContextAsync();
        var query = dbContext.Set<CategoryTranslation>().AsQueryable();

        if (excludeCategoryId.HasValue)
        {
            query = query.Where(x => x.Category.Id != excludeCategoryId.Value);
        }

        return await query.AnyAsync(x => x.Category.SeoSlug == slug);
    }

    private static IQueryable<CategoryTranslation> WithDetails(IQueryable<CategoryTranslation> query)
    {
        return query.Include(x => x.Category);
    }
}