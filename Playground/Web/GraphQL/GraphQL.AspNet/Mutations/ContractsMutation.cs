using CosmosDb.Shared.Repository;
using GraphQL.AspNet.Entities;

namespace GraphQL.AspNet.Mutations;

public class ContractsMutation(ICosmosDbRepository<Contract> contractsRepository)
{
    public async Task<Contract> AddContractAsync(Contract contract)
    {
        return await contractsRepository.CreateAsync(contract);
    }

    public async Task<Contract> UpdateContractAsync(Contract contract)
    {
        await contractsRepository.UpdateAsync(contract.Id, contract);
        return contract;
    }
    
    public async Task DeleteContractAsync(string contractId)
    {
        await contractsRepository.DeleteAsync(contractId);
    }
}