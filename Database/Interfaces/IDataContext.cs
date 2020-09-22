namespace Database.Interfaces
{
    public interface IDataContext
    {
        void BeginTransaction();
        void Commit();
        void Rollback();
    }
}