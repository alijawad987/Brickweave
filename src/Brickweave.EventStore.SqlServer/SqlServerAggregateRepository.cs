﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Brickweave.EventStore.Factories;
using Brickweave.EventStore.Serialization;
using Brickweave.EventStore.SqlServer.Entities;
using Microsoft.EntityFrameworkCore;

namespace Brickweave.EventStore.SqlServer
{
    public abstract class SqlServerAggregateRepository<TAggregate, TContext>
        where TAggregate : EventSourcedAggregateRoot
        where TContext : DbContext, IEventStore
    {
        private readonly TContext _dbContext;
        private readonly IDocumentSerializer _serializer;
        private readonly IAggregateFactory _aggregateFactory;

        protected SqlServerAggregateRepository(TContext dbContext, IDocumentSerializer serializer,
            IAggregateFactory aggregateFactory)
        {
            _dbContext = dbContext;
            _aggregateFactory = aggregateFactory;
            _serializer = serializer;
        }
        
        protected async Task SaveUncommittedEventsAsync(EventSourcedAggregateRoot aggregate, Guid streamId,
            Action onBeforeSaveChanges = null)
        {
            var created = DateTime.UtcNow;
            
            var uncommittedEvents = aggregate.GetUncommittedEvents()
                .Select((e, i) => CreateEventData(streamId, e, created, i))
                .ToList();

            uncommittedEvents.ForEach(e => _dbContext.Events.Add(e));
            
            if (onBeforeSaveChanges != null)
                onBeforeSaveChanges.Invoke();

            await _dbContext.SaveChangesAsync();

            aggregate.ClearUncommittedEvents();
        }

        protected async Task<bool> ExistsAsync(Guid streamId)
        {
            var aggregate = await _dbContext.Events
                .FirstOrDefaultAsync(i => i.StreamId.Equals(streamId));

            return aggregate != null;
        }

        protected async Task<TAggregate> TryFindAsync(Guid streamId)
        {
            var eventData = await _dbContext.Events
                .Where(e => e.StreamId.Equals(streamId))
                .OrderBy(e => e.Created)
                .ThenBy(e => e.CommitSequence)
                .ToListAsync();

            var events = eventData
                .Select(d => _serializer.DeserializeObject<IAggregateEvent>(d.Json))
                .ToList();

            return events.Any() ? _aggregateFactory.Create<TAggregate>(events) : null;
        }

        protected async Task DeleteAsync(Guid streamId, Action onBeforeSaveChanges = null)
        {
            var eventData = await _dbContext.Events
                .Where(e => e.StreamId.Equals(streamId))
                .ToListAsync();

            _dbContext.Events.RemoveRange(eventData);

            if (onBeforeSaveChanges != null)
                onBeforeSaveChanges.Invoke();

            await _dbContext.SaveChangesAsync();
        }

        private EventData CreateEventData(Guid streamId, IAggregateEvent @event, DateTime created, int commitSequence)
        {
            return new EventData
            {
                Id = Guid.NewGuid(),
                StreamId = streamId,
                Json = _serializer.SerializeObject(@event),
                Created = created,
                CommitSequence = commitSequence
            };
        }
    }
}
