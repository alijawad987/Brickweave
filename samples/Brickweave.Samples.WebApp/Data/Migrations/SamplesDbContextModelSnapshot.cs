﻿using System;
using Brickweave.Samples.SqlServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Brickweave.Samples.WebApp.Data.Migrations
{
    [DbContext(typeof(SamplesDbContext))]
    partial class SamplesDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.2-rtm-10011")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Brickweave.EventStore.SqlServer.Entities.EventData", b =>
                {
                    b.Property<Guid>("EventId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("CommitSequence");

                    b.Property<DateTime>("Created");

                    b.Property<string>("Json");

                    b.Property<Guid>("StreamId");

                    b.HasKey("EventId");

                    b.ToTable("Event");
                });

            modelBuilder.Entity("Brickweave.Messaging.SqlServer.Entities.MessageFaulireData", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("Enqueued");

                    b.Property<string>("Exception");

                    b.Property<string>("Message");

                    b.HasKey("Id");

                    b.ToTable("MessageFailure");
                });

            modelBuilder.Entity("Brickweave.Samples.Persistence.SqlServer.Entities.PersonSnapshot", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("FirstName");

                    b.Property<string>("LastName");

                    b.HasKey("Id");

                    b.ToTable("Persons");
                });
#pragma warning restore 612, 618
        }
    }
}