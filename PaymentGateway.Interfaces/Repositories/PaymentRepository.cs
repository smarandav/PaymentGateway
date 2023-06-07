using System;
using System.Threading.Tasks;
using FluentResults;
using PaymentGateway.Domain;
using PaymentGateway.Domain.Errors;
using PaymentGateway.Domain.Repositories;

namespace PaymentGateway.Interfaces.Repositories
{
    public class PaymentRepository : CosmosRepository<Payment>, IPaymentRepository
    {
        public async Task<Result<Payment>> Get(string paymentId, string merchantId)
        {
            var merchant = await GetItem(paymentId, merchantId);
            if (merchant != null)
            {
                return Result.Ok(merchant);
            }

            return Result.Fail(new PaymentNotFoundError());
        }

        public PaymentRepository(IDataStoreConfiguration dataStoreConfiguration) 
            : base(dataStoreConfiguration.ConnectionString, dataStoreConfiguration.DatabaseName, dataStoreConfiguration.PaymentCollectionName)
        {
        }

        protected override string PartitionKeyValue(Payment item)
        {
            return item.MerchantId;
        }

        protected override string PrimaryKeyValue(Payment item)
        {
            return item.Id;
        }

        public async Task<Result<Payment>> Upsert(Payment payment)
        {
            var storedPayment = await UpsertAsync(payment);
            return Result.Ok(storedPayment);
        }
    }
}