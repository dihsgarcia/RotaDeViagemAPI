# RotaDeViagemAPI
 A Api tem como finalidade calcular a rota de viagem com o menor custo possível, dado um conjunto de origens, destinos e valores. 
 Para efetuar o cálculo principal proposto, foi utilizado o algoritmo de Dijkstra.
 
### Como Iniciar ?
Para os endpoints que compõem o CRUD (incluir/alterar/excluir) é necessário a execução prévia do script de banco de dados (**SCRIPT_BD_RotaDeViagem**), que se encontra na raiz do projeto.
Para o endpoint principal, que é quem calcula a melhor rota, existe um objeto já populado com as rotas propostas, então se por algum motivo não existir os dados no banco de dados, mesmo assim será possível a execução deste endpoint.

