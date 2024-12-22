using CosmosDb.Shared.Extensions;
using GraphQL.AspNet.Entities;
using GraphQL.AspNet.Mutations;
using GraphQL.AspNet.Options;
using GraphQL.AspNet.Queries;

var builder = WebApplication.CreateBuilder(args);
RegisterDependencies(builder.Services);

var app = builder.Build();
app.MapGraphQL();
app.Run();

void RegisterDependencies(IServiceCollection services)
{
    services.AddCosmosDbRepository<Contract, CosmosDbOptions>("contracts");
    
    services
        .AddOptions<CosmosDbOptions>()
        .Bind(builder.Configuration.GetSection(CosmosDbOptions.SectionName));

    services
        .AddGraphQLServer()
        .AddQueryType<ContractsQuery>()
        .AddMutationType<ContractsMutation>();
}