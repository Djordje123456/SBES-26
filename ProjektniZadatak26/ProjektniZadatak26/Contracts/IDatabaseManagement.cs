using System.ServiceModel;

namespace Contracts
{
    /// <summary>
    /// Interface that implements the required operations with the database.
    /// </summary>
    [ServiceContract]
    public interface IDatabaseManagement
    {
        [OperationContract]
        void CreateDatabase();
        [OperationContract]
        void ArchiveDatabase();
        [OperationContract]
        void DeleteDatabase();
        [OperationContract]
        void AddEntry(DatabaseEntry entry);
        [OperationContract]
        void ModifyEntry(DatabaseEntry entry);
        [OperationContract]
        double AvgCityConsumption(string city);
        [OperationContract]
        double AvgRegionConsumption(string region);
        [OperationContract]
        DatabaseEntry HighestRegionConsumer(string region);
    }
}
