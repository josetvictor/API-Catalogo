# Anotações

## Fundamentos

### Roteamento

Nas Web APIs dotnet realizamos o roteamento utilizando as classes da camada de controller, onde realizamos a utilização do atributo ApiController, definimos o roteamento base com o atributo Route e fazemos uma herança da classe ControllerBase

CategoriaCOntroller:
```cs
[Route("[Controller]")]
[ApiController]
public class CategoriaController : ControllerBase
{
    // instância do contexto via injeção de dependência

    // métodos Actions atravez dos atributos: HttpGet, HttpPost, HttpPut e HttpDelete
    // que compoe o atributo Route
}
```

Na AspNet Core existe alguns padrões de roteamento como:

- Controllers
- Razor Pages
- Middleware habilitado para o endpoint
- Delegates e mabdas registrados no roteamento

Algumas outras configurações que podemos fazer nos roteamentos é definir paramentros e adicionar alguns padrões de nomeclatura para as rotas, além de adicionar alguns indicativos do que aquela rota irá fazer,como adicionar um nome ou uma descrição.
### Restrições de Rotas

No cenario onde temos uma rota que retorna um produto pelo Id, as restrições ajudam a filtrar ou impedir que valores indesejados atinjam os requests, assim, no caso de na URL vir um o valor 0 ao inves de um numero valido é possivel verificar com antecendencia antes de realizar um request para nossa API.

É possível ver no codigo abaixo que foi adicionando uma restrição na rota de retornar um produto pelo ID, assim, caso a rota recebesse um numero menor que 0 ele não iria nem realizar a requisição para poder retornar o 404.

Existes diversas restrições que podemos adicionar as nossas rotas e a vantagem disso é poupar que a nossa API realizar alguma requisição desnecessaria.
```cs
// api/produtos/1
[HttpGet("{id:int:min(1)}", Name ="ObterProduto")]
public ActionResult<Produto> Get(int id)
{}
```

### Tipos de retorno

Os tipos de retorno que temos na ASPNET são:

- Um tipo específico;
- Uma interface de `IActionResult`;
- Uma `ActionResult<T>` de um tipo

## Data Annotations

Os Atributos `Data Annotations` permitem aplicar a validação no modelo de domínio, assim, quando o modelo é usando nas Actions, o framework verifica se o objeto é valido com base nos atributos `Data Annotations`.

Um exemplo pratico é usar acima de uma propriedade de um modelo entre colchetes: `[Atributo]`

## Validações
É possivel criar validações e usar como Data Annotations como no exemplo feito no diretorio de `Validations`.

Outra forma de realizar validações é a nivel de modelo, incrementando o metodo da interface `IValidataObject`. Esse exemplo pode ser visto no modelo de Produtos no diretorio de produtos desse projeto.

## Middleware

De forma bem resumida, middleware são trechos de codigo que encapsula uma determinada logica necessarias, podendo ter varios trechos desses codigos a serem executados entre um request e um response de uma API Aspnet. Esses trechos de codigos geralmente precisam ter uma ordem correta de execução. No exemplo de um projeto Aspnet MVC existe um middleware chamado de MVC que define o mapeamento e envio dos requests para o controlador, mas antes da execução desse middleware é preciso ser executado o middleware de Authorization, que habilita a funcionada de delegar o acesso de um recurso a um perfil. Nesse caso é preciso se precaver quanto a ordem de execução desses trechos de codigo.


## Padrão Repository

No curso é apresentado o padrão repository, padrão esse que utilizo em quase todos meus projetos. Uma das maiores vantagens desse padrão, é criar uma camada na aplicação onde não ficamos presos a uma ferramenta de persistência, assim podemos ter ate mais de uma opção de persistência de dados e ate mesmo facilitando a construção de alguns testes, onde podemos criar uma camada de dados fakes para agilizar nossos testes e se por ventura no futuro o ORM for descontinuado por exemplo, podemos realizar a migração dessa persistencia sem ter que fazer nenhuma interferencia com outras camadas do projeto.

Além disso foi me apresentado um outro padrão que não conhecia, o `Unit of Work`, que junto ao padrão `Repository` forma uma otima pratica para evitar concorrencia de varias instancias da classe dbcontext que pode ser gerado utilizando apenas o padrão `Repository`, além de evitar algumas repetições de codigos desnecessarios e facilitando uma manutenção quando em um projeto grande.

## DTO e AutoMapper

Em algumas experiências passadas já utilizei a pratica de DTO utilizando o AutoMapper. A nomeclatura que usava-se no projeto era diferente mas o conceito é o mesmo. Quando utilizamos algum ORM como o EF por exemplo, que utilizamos dos nossos modelos de dominio para construir nosso banco de dados, e não é uma boa pratica utilizarmos os modelos de dominio para apresentarmos os dados a um cliente, assim é preciso utilizar um modelo, que seria nosso Data Transfer Object para fazermos o mapeamento das informações que desejamos mostrar ao cliente.

No exemplo de um modelo de produto, onde é construido por, nome, preço e quantidade em estoque, não queremos mostrar quantos desse produto tem no estoque por exemplo, nesse caso, nosso DTO só tera apenas as informações de Nome e preço por exemplo. Como pra realizar esse mapeamento de forma manual, é massante e bem repetitivo, podemos usar a biblioteca AutoMapper para fazer um mapeamento de forma mais dinamica e pratica para um projeto grande.

## Segurança

Autenticação x Autorização.

### Implementações de Autenticação e Autorização

- Identity (forma nativa da plataforma);
- Provedores Externos;
- Autenticação com servidor;
- Autenticação baseada em Tokens

No projeto API Catalogo, realizei a implementação de uma autenticação baseada em tokens usando o conceito de JWT.

