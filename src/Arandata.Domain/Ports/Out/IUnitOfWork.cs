namespace Arandata.Domain.Ports.Out
{
    public interface IUnitOfWork
    {
        Task CommitAsync();
    }
}
