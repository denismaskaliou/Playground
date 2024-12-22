using CosmosDb.Shared.Repository;
using GraphQL.AspNet.Entities;

namespace GraphQL.AspNet.Queries;

public class ContractsQuery(ICosmosDbRepository<Contract> contractsRepository)
{
    public async Task<IList<Contract>> GetContracts(string? id)
    {
        if (id == null)
        {
            return await contractsRepository.GetAllAsync();
        }

        return new List<Contract> { await contractsRepository.GetByIdAsync(id) };
    }
}