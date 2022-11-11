namespace Practices.ML.Net.Abstractions.Repository;

public interface ITableCreator
{
    Task CreateIfNotExist();
}