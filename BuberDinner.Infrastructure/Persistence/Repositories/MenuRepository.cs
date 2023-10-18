using BuberDinner.Application.Common.Interfaces.Persistance;
using BuberDinner.Domain.Menus;

using Microsoft.EntityFrameworkCore;

namespace BuberDinner.Infrastructure.Persistence.Repositories;

public sealed class MenuRepository : IMenuRepository
{
    private readonly BuberDinnerDbContext _dbContext;

    public MenuRepository(BuberDinnerDbContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    public async Task AddAsync(Menu menu)
    {
        if (menu == null)
        {
            throw new ArgumentNullException(nameof(menu));
        }

        await _dbContext.Menus.AddAsync(menu); 
        await _dbContext.SaveChangesAsync();
    }

    public async Task<IEnumerable<Menu>> GetAllAsync()
    {
        return await _dbContext.Menus.ToListAsync();
    }
}
