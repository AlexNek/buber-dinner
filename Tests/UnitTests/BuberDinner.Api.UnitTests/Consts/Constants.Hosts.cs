using BuberDinner.Domain.Hosts.ValueObjects;

namespace BuberDinner.Api.UnitTests.Consts
{
    public static partial class Constants
    {
        public static class Host
        {
            public static readonly HostId Id = HostId.Create(Guid.Empty);
        }
    }
}
