// Global using directives

global using Ardalis.Result;
global using Bogus;
global using Core.Application;
global using Core.Application.Auth.Commands;
global using Core.Application.Auth.Exceptions;
global using Core.Application.Contracts;
global using Core.Application.Invitations.Commands;
global using Core.Application.Invitations.Queries;
global using Core.Application.Organisations.Commands;
global using Core.Application.Organisations.Queries;
global using Core.Application.Users.Commands;
global using Core.Application.Users.Queries;
global using Core.Domain;
global using Core.Domain.Common;
global using Core.Domain.Users;
global using Core.IntegrationTests.Fixtures;
global using FluentAssertions;
global using MediatR;
global using Microsoft.Extensions.DependencyInjection;
global using Polly;
global using Xunit.Abstractions;