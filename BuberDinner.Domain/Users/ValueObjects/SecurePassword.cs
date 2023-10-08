using BuberDinner.Domain.Common.Models;

namespace BuberDinner.Domain.Users.ValueObjects
{
    public sealed class SecurePassword : ValueObject
    {
        private SecurePassword(string value, bool encrypt = true)
        {
            if (encrypt)
            {
                Value = BCrypt.Net.BCrypt.HashPassword(value);
            }
            else
            {
                Value = value;
            }
        }

        public static SecurePassword CreateNew(string value)
        {
            return new(value);
        }
        public static SecurePassword CreateAsIs(string value)
        {
            return new(value, false);
        }

        public override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }

        public string Value { get; private set; }
    }
}
