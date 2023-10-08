using BuberDinner.Domain.Users.ValueObjects;

using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace BuberDinner.Infrastructure.Persistence.Converters
{
    public class SecurePasswordConverter : ValueConverter<SecurePassword, string>
    {
        public SecurePasswordConverter()
            : base(
                v => v.Value,
                v => SecurePassword.CreateAsIs(v))
        {
        }
    }
}
