namespace demoMdat
{
    public interface IAccesBD
    {
        Task<TEntity> ObtenirAsync<TEntity>(object parametres);
    }
}