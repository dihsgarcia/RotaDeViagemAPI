using System.Threading.Tasks;

namespace RotaDeViagemAPI.Services.RotaDeViagemService
{
    public interface IRotaDeViagemService
    {
        public Task<RotaDeViagemResponse?> GetRouteById(int id);
        public RotaDeViagemResponse? GetBestRouteByOrigemDestino(string origem, string destino);
        public Task<RotaDeViagemResponse?> AddRoute(RotaDeViagem rota);
        public Task<RotaDeViagemResponse?> UpdateRoute(int id, RotaDeViagem request);
        public Task<bool> DeleteRoute(int id);

    }
}
