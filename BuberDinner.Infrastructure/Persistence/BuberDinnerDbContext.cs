using BuberDinner.Domain.Bills;
using BuberDinner.Domain.Common.Models;
using BuberDinner.Domain.Dinners;
using BuberDinner.Domain.Guests;
using BuberDinner.Domain.Hosts;
using BuberDinner.Domain.MenuReview;
using BuberDinner.Domain.Menus;
using BuberDinner.Domain.Users;
using BuberDinner.Domain.Users.ValueObjects;
using BuberDinner.Infrastructure.Persistence.Converters;
using BuberDinner.Infrastructure.Persistence.Interceptors;
using Microsoft.EntityFrameworkCore;

namespace BuberDinner.Infrastructure.Persistence;

public sealed class BuberDinnerDbContext : DbContext
{
    private readonly PublishDomainEventsInterceptor? _publishDomainEventsInterceptor;

    public BuberDinnerDbContext(
        DbContextOptions<BuberDinnerDbContext> options,
        PublishDomainEventsInterceptor? publishDomainEventsInterceptor
    ) : base(options)
    {
        _publishDomainEventsInterceptor = publishDomainEventsInterceptor;
    }

    public DbSet<Bill> Bills { get; set; } = null!;
    public DbSet<Dinner> Dinners { get; set; } = null!;
    //todo?
    // table reservation ConfigureReservationsTable?
    //public DbSet<DinnerReservations> DinnerReservations { get; set; } = null!;
    //public DbSet<GuestBill> GuestBillIds { get; set; } = null!;
    //public DbSet<GuestMenuReview> GuestMenuReviewIds { get; set; } = null!;
    //public DbSet<GuestPastDinner> GuestPastDinnerIds { get; set; } = null!;
    //public DbSet<GuestPendingDinner> GuestPendingDinnerIds { get; set; } = null!;
    //public DbSet<GuestRating> GuestRatings { get; set; } = null!;
    public DbSet<Guest> Guests { get; set; } = null!;
    //todo?
    //public DbSet<GuestUpcommingDinner> GuestUpcommingDinnerIds { get; set; } = null!;
    //public DbSet<HostDinner> HostDinnerIds { get; set; } = null!;
    //public DbSet<HostMenu> HostMenuIds { get; set; } = null!;

    public DbSet<Host> Hosts { get; set; } = null!;
    public DbSet<Menu> Menus { get; set; } = null!;
    //public DbSet<MenuDinner> MenuDinnerIds { get; set; } = null!;

    // see ConfigureMenuSectionsTable
    //public DbSet<MenuItems> MenuItems { get; set; } = null!;
    //BuildModel for MenuReviewIds
    public DbSet<MenuReview> MenuReviews { get; set; } = null!;
    public DbSet<User> Users { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .Ignore<List<IDomainEvent>>()
            .ApplyConfigurationsFromAssembly(typeof(BuberDinnerDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (_publishDomainEventsInterceptor != null)
        {
            optionsBuilder
                .AddInterceptors(_publishDomainEventsInterceptor);
        }

        base.OnConfiguring(optionsBuilder);
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder
            .Properties<SecurePassword>()
            .HaveConversion<SecurePasswordConverter>();
    }
}