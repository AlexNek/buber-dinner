using BuberDinner.Application.Common.Interfaces.Persistence;
using BuberDinner.Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace BuberDinner.Infrastructure.Persistence.Repositories;

public class UserRepository : IUserRepository
{
    private readonly BuberDinnerDbContext _dbContext;

    public UserRepository(BuberDinnerDbContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    public User? GetUserByEmail(string email)
    {
        if (string.IsNullOrEmpty(email))
        {
            throw new ArgumentNullException(nameof(email));
        }

        return _dbContext.Users.SingleOrDefault(u => u.Email == email);
    }

    public void Add(User user)
    {
        if (user == null)
        {
            throw new ArgumentNullException(nameof(user));
        }

        _dbContext.Users.Add(user);
        _dbContext.SaveChanges();
    }

    public async Task<IEnumerable<User>> GetAllAsync()
    {
        return await _dbContext.Users.ToListAsync();
    }
}