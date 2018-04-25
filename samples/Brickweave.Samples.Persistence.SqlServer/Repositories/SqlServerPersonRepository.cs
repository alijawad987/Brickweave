﻿using System.Threading.Tasks;
using Brickweave.EventStore.Factories;
using Brickweave.EventStore.Serialization;
using Brickweave.EventStore.SqlServer;
using Brickweave.Samples.Domain.Persons.Models;
using Brickweave.Samples.Domain.Persons.Services;
using Brickweave.Samples.Persistence.SqlServer.Entities;
using Microsoft.EntityFrameworkCore;

namespace Brickweave.Samples.Persistence.SqlServer.Repositories
{
    public class SqlServerPersonRepository : SqlServerAggregateRepository<Person, SamplesDbContext>, 
        IPersonRepository, IPersonInfoRepository
    {
        private readonly SamplesDbContext _dbContext;

        public SqlServerPersonRepository(SamplesDbContext dbContext, SamplesDbContext samplesContext, 
            IDocumentSerializer serializer, IAggregateFactory aggregateFactory) 
            : base(dbContext, serializer, aggregateFactory)
        {
            _dbContext = samplesContext;
        }
        
        public async Task SavePersonAsync(Person person)
        {
            await SaveUncommittedEventsAsync(person, person.Id.Value,
                () => AddSnapshotAsync(person));
        }

        public async Task<Person> GetPersonAsync(PersonId id)
        {
            return await TryFindAsync(id.Value);
        }

        public async Task<PersonInfo> GetPersonInfoAsync(PersonId personId)
        {
            var data = await _dbContext.Persons
                .FirstOrDefaultAsync(p => p.Id == personId.Value);

            return data?.ToInfo();
        }

        private void AddSnapshotAsync(Person person)
        {
            _dbContext.Persons.Add(new PersonSnapshot
            {
                Id = person.Id.Value,
                FirstName = person.Name.FirstName,
                LastName = person.Name.LastName
            });
        }
    }
}
