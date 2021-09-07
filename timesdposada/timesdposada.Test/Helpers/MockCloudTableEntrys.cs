using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using System;
using System.Threading.Tasks;

namespace timesdposada.Test.Helpers
{
    public class MockCloudTableEntrys : CloudTable
    {

        public MockCloudTableEntrys(Uri tableAddress) : base(tableAddress)
        { 
        }

        public MockCloudTableEntrys(Uri tableAbsoluteUri, StorageCredentials credentials) : base(tableAbsoluteUri, credentials)
        {
        }

        public MockCloudTableEntrys(StorageUri tableAddress, StorageCredentials credentials) : base(tableAddress, credentials)
        { 
        }

        public override async Task<TableResult> ExecuteAsync(TableOperation operation)
        {
            return await Task.FromResult(new TableResult
            {
                HttpStatusCode = 200,
                Result = TestFactory.GetEntryEntity()
            });
        }
    }
}
