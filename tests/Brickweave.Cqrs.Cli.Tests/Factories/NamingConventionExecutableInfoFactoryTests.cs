﻿using System.Linq;
using Brickweave.Cqrs.Cli.Factories;
using FluentAssertions;
using Xunit;

namespace Brickweave.Cqrs.Cli.Tests.Factories
{
    public class NamingConventionExecutableInfoFactoryTests
    {
        [Fact]
        public void Parse_ReturnsExecutableInfo()
        {
            var factory = new NamingConventionExecutableInfoFactory();

            var result = factory.Create(new[] { "foo", "create", "--id", "12345" });

            result.Name.Should().Be("CreateFoo");
            result.Parameters.Should().HaveCount(1);
            result.Parameters.First(p => p.Name.Equals("id"))
                .Values.First().Should().Be("12345");
        }

        [Fact]
        public void Parse_WhenHasParameterWithNoValue_DefaultsValueToTrue()
        {
            var factory = new NamingConventionExecutableInfoFactory();

            var result = factory.Create(new[] { "foo", "create", "--iscool", "--id", "12345" });

            result.Name.Should().Be("CreateFoo");
            result.Parameters.Should().HaveCount(1);
            result.Parameters.First(p => p.Name.Equals("iscool"))
                .Values.First().Should().Be("true");
        }

        [Fact]
        public void Parse_WhenArgHasMultipleParameterValuesInSeparateArgs_ReturnsExecutableInfoWithMultipleParameterValues()
        {
            var factory = new NamingConventionExecutableInfoFactory();

            var result = factory.Create(new[] { "foo", "create", "--ids", "1", "2" });

            result.Name.Should().Be("CreateFoo");
            result.Parameters.Should().HaveCount(1);

            result.Parameters.First(p => p.Name.Equals("ids"))
                .Values.Should().BeEquivalentTo("1", "2");
        }

        [Fact]
        public void Parse_WhenArgHasMultipleMultiParameterValuesInSeparateArgs_ReturnsExecutableInfoWithMultipleParameterValues()
        {
            var factory = new NamingConventionExecutableInfoFactory();

            var result = factory.Create(new[] { "foo", "create", "--ids", "1", "2", "--other" , "a", "b" });

            result.Name.Should().Be("CreateFoo");
            result.Parameters.Should().HaveCount(2);
            result.Parameters.First(p => p.Name.Equals("ids"))
                .Values.Should().BeEquivalentTo("1", "2");
            result.Parameters.First(p => p.Name.Equals("other"))
                .Values.Should().BeEquivalentTo("a", "b");
        }
    }
}
