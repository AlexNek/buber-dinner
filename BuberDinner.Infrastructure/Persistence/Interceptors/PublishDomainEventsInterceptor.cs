using BuberDinner.Domain.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace BuberDinner.Infrastructure.Persistence.Interceptors;

public class PublishDomainEventsInterceptor : SaveChangesInterceptor
{
    private readonly IPublisher _mediator;

    public PublishDomainEventsInterceptor(IPublisher mediator)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        if (eventData == null)
        {
            throw new ArgumentNullException(nameof(eventData));
        }

        PublishDomainEvents(eventData.Context).GetAwaiter().GetResult();
        return base.SavingChanges(eventData, result);
    }

    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        if (eventData == null)
        {
            throw new ArgumentNullException(nameof(eventData));
        }

        await PublishDomainEvents(eventData.Context);
        return await base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private async Task PublishDomainEvents(DbContext? dbContext)
    {
        if (dbContext is null)
        {
            return;
        }

        var entitiesWithDomainEvents = dbContext.ChangeTracker.Entries<IHasDomainEvents>()
            .Where(entry => entry.Entity.DomainEvents.Any())
            .Select(entry => entry.Entity)
            .ToList();

        var domainEvents = entitiesWithDomainEvents
            .SelectMany(entry => entry.DomainEvents)
            .ToList();

        entitiesWithDomainEvents.ForEach(
            entity => entity.ClearDomainEvents()
        );

        foreach (var domainEvent in domainEvents)
        {
            await _mediator.Publish(domainEvent);
        }
    }
}