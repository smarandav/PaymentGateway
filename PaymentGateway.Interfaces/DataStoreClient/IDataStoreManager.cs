using System.Threading.Tasks;

namespace PaymentGateway.Interfaces.DataStoreClient
{
    public interface IDataStoreManager
    {
        Task EnsureDataStoreIsCreated();
    }
}