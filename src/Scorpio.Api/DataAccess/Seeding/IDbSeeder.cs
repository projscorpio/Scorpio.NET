using System.Threading.Tasks;

namespace Scorpio.Api.DataAccess.Seeding
{
    public interface IDbSeeder
    {
        Task Seed();
    }
}