// Global using directives

global using System.Net;
global using Core.Application.Contracts;
global using Core.Domain.Common;
global using FluentAssertions;
global using MartinCostello.Logging.XUnit;
global using MediatR;
global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.Extensions.Logging;
global using Web.IntegrationTests.Fixtures;
global using Xunit;
global using Xunit.Abstractions;
using System.Diagnostics.CodeAnalysis;

[assembly: ExcludeFromCodeCoverage]
[assembly: AssemblyTrait("Type", "IntegrationWeb")]