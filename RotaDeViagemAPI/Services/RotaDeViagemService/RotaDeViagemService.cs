using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using RotaDeViagemAPI.Data;
using Azure.Core;

namespace RotaDeViagemAPI.Services.RotaDeViagemService
{
    public class RotaDeViagemService : IRotaDeViagemService
    {
        //modelo default caso o BD não tenha sido configurado.
        private readonly List<RotaDeViagem> rotas = new List<RotaDeViagem> 
        {
            new RotaDeViagem {Id = 1, Origem = "GRU", Destino = "BRC", ValorViagemInt = 10 },
            new RotaDeViagem {Id = 2, Origem = "BRC", Destino = "SCL", ValorViagemInt = 5 },
            new RotaDeViagem {Id = 3, Origem = "GRU", Destino = "CDG", ValorViagemInt = 75 },
            new RotaDeViagem {Id = 4, Origem = "GRU", Destino = "SCL", ValorViagemInt = 20 },
            new RotaDeViagem {Id = 5, Origem = "GRU", Destino = "ORL", ValorViagemInt = 56 },
            new RotaDeViagem {Id = 6, Origem = "ORL", Destino = "CDG", ValorViagemInt = 5 },
            new RotaDeViagem {Id = 7, Origem = "SCL", Destino = "ORL", ValorViagemInt = 20 }
        };

        private readonly DataContext _dbContext;

        public RotaDeViagemService(DataContext dbContext)
        {
            _dbContext = dbContext;
        }   
       
        public List<RotaDeViagem> GetAllRoutes()
        {
            List<RotaDeViagem> rotasBd = new();
            try
            {
                using (_dbContext)
                {
                    rotasBd =  _dbContext.RotaDeViagem.ToList();

                    if (rotas.Count() == 0)
                    {   
                        //se por algum motivo não veio dados do BD, uso o modelo interno.
                        return rotas;
                    }
                       
                }
            }
            catch
            {
                return rotas;
            }

            return rotasBd;
        }

        public async Task<RotaDeViagemResponse?> GetRouteById(int id)
        {
            RotaDeViagemResponse response = new();
            var rota = await GetRoute(id);

            if (rota is not null)
            {
                response.Rota = $"{rota.Origem} - {rota.Destino}";
                response.ValorViagemInt = rota.ValorViagemInt;
            }
            else
            {
                return null;
            }

            return  response;
        }

        public async Task<RotaDeViagem?> GetRoute(int id)
        {
            RotaDeViagem? rotaDeViagem = new();
            try
            {
                using (_dbContext)
                {
                    rotaDeViagem = await _dbContext.RotaDeViagem.Where(x => x.Id == id).FirstOrDefaultAsync();
                    
                    if (rotaDeViagem is null)
                    {
                        rotaDeViagem = rotas.Find(x => x.Id == id);
                    }

                }
            }
            catch
            { 
                return rotas.Find(x => x.Id == id); ;
            }

            return rotaDeViagem;

        }

        public RotaDeViagemResponse? GetBestRouteByOrigemDestino(string origem, string destino)
        {
            var listaDeRotas = GetAllRoutes();

            var melhorRota = CalcularRotaComMenorCusto(origem, destino, listaDeRotas);

            if (melhorRota is null)
                return null;

            return melhorRota;
        }

        public RotaDeViagemResponse CalcularRotaComMenorCusto(string origem, string destino, List<RotaDeViagem> listaDeRotas)
        {
            //Aqui foi implementatdo o algoritimo de Dijkstra
            //algoritimo já conhecido para encontrar a melhor rota
            //dado um conjunto de origens e destinos e suas possiveis interações
            Dictionary<string, int> minValorViagem = new Dictionary<string, int>();
            Dictionary<string, string> previousNode = new Dictionary<string, string>();
            HashSet<string> processedNodes = new HashSet<string>();
            PriorityQueue<string, int> nodesToProcess = new PriorityQueue<string, int>();

            minValorViagem[origem] = 0;
            previousNode[origem] = null;
            nodesToProcess.Enqueue(origem, 0);

            #region Dijkstra
            while (nodesToProcess.Count > 0)
            {
                //remove o elemento com menor valor da fila
                string currentNode = nodesToProcess.Dequeue();

                //se o node na vez é == ao destino, então todas as possiveis interações ja foram feitas
                //então fim.
                if (currentNode == destino)
                {
                    break;
                }

                //se o node da vez ja foi processado, pula
                if (processedNodes.Contains(currentNode))
                {
                    continue;
                }

                //marca o node da vez como processado
                processedNodes.Add(currentNode);

                //percorre todos as RotaDeViagem onde a origem é == ao node da vez.
                foreach (RotaDeViagem rota in listaDeRotas.Where(r => r.Origem == currentNode))
                {
                    //se o destino dessa rota já foi processado (feito as possiveis interações), pula
                    if (processedNodes.Contains(rota.Destino))
                    {
                        continue;
                    }

                    //Calcula o valor da rota da vez
                    int valorViagem = minValorViagem[currentNode] + rota.ValorViagemInt;

                    //se a rota da vez, tem um valor menor, do que o valor minimo entre suas possiveis interações
                    //atualiza o valor minimo, o node anterior recebe o node da vez e adicona na fila a rota  
                    if (!minValorViagem.ContainsKey(rota.Destino) || valorViagem < minValorViagem[rota.Destino])
                    {
                        minValorViagem[rota.Destino] = valorViagem;
                        previousNode[rota.Destino] = currentNode;
                        nodesToProcess.Enqueue(rota.Destino, valorViagem);
                    }
                }
            }

            //adiciona o nome do destino passado por parametro no inicio da lista, a partir dos dados do previousNode
            //busca o node anterior ao atual e adiciona na lista, apor ser adicionado todos os "destinos",
            //reverte a lista para que a rota seja exibida do inicio ao fim
            List<string> path = new List<string>();
            string destinoAtual = destino;
            while (destinoAtual != null)
            {
                path.Add(destinoAtual);
                destinoAtual = previousNode[destinoAtual];
            }
            path.Reverse();
            #endregion

            //no objeto de response a string Rota é escrita com todos os itens da lista path
            //separando cada item por " - "
            return new RotaDeViagemResponse
            {
                Rota = string.Join(" - ", path),
                ValorViagemInt = minValorViagem[destino]
            };
        }

        public async Task<RotaDeViagemResponse?> AddRoute(RotaDeViagem rota)
        {
            RotaDeViagemResponse? response = new();
            try
            {
                using (_dbContext)
                {
                    _dbContext.RotaDeViagem.Add(rota);
                    var nChanges = await _dbContext.SaveChangesAsync();

                    if(nChanges == 1)
                    {
                        response = await GetRouteById(rota.Id);
                    }

                }
            }
            catch
            {
                return null;
            }

            return response;
        }

        public async Task<RotaDeViagemResponse?> UpdateRoute(int id, RotaDeViagem request)
        {

            RotaDeViagemResponse? response = new();

            try
            {              
                using (_dbContext)
                {
                    var rota = await _dbContext.RotaDeViagem.Where(x => x.Id == id).FirstOrDefaultAsync();
                    if (rota is null)
                        return null;

                    rota.Origem = request.Origem;
                    rota.Destino = request.Destino;
                    rota.ValorViagemInt = request.ValorViagemInt;
       
                    var nChanges = await _dbContext.SaveChangesAsync();

                    if (nChanges == 1)
                    {
                        response = await GetRouteById(rota.Id);
                    }
                }
            }
            catch
            {
                return null;
            }
            
            return response;
        }

        public async Task<bool> DeleteRoute(int id)
        {
            RotaDeViagemResponse? response = new();

            try
            {
                using (_dbContext)
                {
                    var rota = await _dbContext.RotaDeViagem.Where(x => x.Id == id).FirstOrDefaultAsync();
                    if (rota is null)
                        return false;

                    _dbContext.RotaDeViagem.Remove(rota);

                    var nChanges = await _dbContext.SaveChangesAsync();

                    if (nChanges == 1)
                        return true;

                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

    }
}
