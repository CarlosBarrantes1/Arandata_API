namespace Arandata.Domain.Ports.Out
{
    public interface IUnitOfWork
    {
        void Commit();
    }
}
