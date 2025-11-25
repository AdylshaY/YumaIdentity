namespace YumaIdentity.Application.Interfaces
{
    using System.Threading.Tasks;

    public interface IDatabaseSeeder
    {
        Task InitializeAsync();
    }
}
