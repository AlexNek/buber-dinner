using BuberDinner.Domain.Menus;

namespace BuberDinner.Application.Common.Interfaces.Persistance;

public interface IMenuRepository
{
    Task AddAsync(Menu menu);
    Task<IEnumerable<Menu>> GetAllAsync();
}