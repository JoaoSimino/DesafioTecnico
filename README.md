📝 Desafio Técnico – Backend Developer (C#/.NET)
Contexto
Você foi contratado para desenvolver uma API RESTful para gerenciar um sistema de controle de pedidos e clientes para uma loja virtual. Esse sistema permitirá que os clientes realizem pedidos e acompanhem seus status.

Requisitos principais
Funcionalidades obrigatórias:

CRUD de Clientes [X]
Cliente: Id, Nome, Email, Telefone, DataCadastro

CRUD de Produtos[X]
Produto: Id, Nome, Descrição, Preço, Estoque

CRUD de Pedidos[X]
Pedido: Id, ClienteId, DataPedido, Status (Aberto, Pago, Cancelado), Itens


ItemPedido: ProdutoId, Quantidade, PreçoUnitario[X]

------------

Ao criar um Pedido:

Validar se o Cliente existe. [X]

Validar se todos os Produtos existem e possuem estoque suficiente.[X]

Deduzir o estoque dos produtos.[X]

Ao cancelar um Pedido: [X]

Repor o estoque dos produtos.[X]

Endpoint para alterar o status do pedido (Aberto -> Pago, Aberto -> Cancelado).[X]

Endpoint para listar todos os pedidos de um cliente específico.[X]

Requisitos técnicos
✅ Utilizar .NET 8 ou superior
✅ Utilizar Entity Framework Core (com SQLite ou PostgreSQL local)
✅ Estruturar utilizando Clean Architecture (Domain, Application, Infrastructure, API)
✅ Aplicar o princípio de Injeção de Dependência
✅ Seguir boas práticas de SOLID
✅ Criar pelo menos 3 testes unitários com xUnit ou NUnit
✅ Utilizar AutoMapper para mapeamento entre DTOs e entidades
✅ Criar validações com FluentValidation
✅ Documentar a API com Swagger 

O que será avaliado:
Estrutura e organização do código

Clareza e objetividade na modelagem

Uso adequado de padrões e boas práticas

Escrita de testes unitários

Qualidade da documentação da API

Uso eficiente de Git (estrutura de commits)

Extras (não obrigatório, mas será um diferencial):
✨ Implementar padrão MediatR para comandos e queries[X]
✨ Publicar a aplicação usando Docker [X]
✨ Implementar autenticação (ex: JWT) [todo]
✨ Criar um pipeline de CI com GitHub Actions [todo]
tratamento de erros -> IExceptionHandler[X]

Como entregar?
Suba o código em um repositório público (GitHub ou GitLab).

Inclua no README:

Descrição do projeto

Como executar localmente

Como executar os testes

⚠️ Importante:
Não se preocupe em entregar algo "perfeito". O foco é demonstrar sua capacidade de resolver o problema com qualidade, clareza e boas práticas.

Use os recursos que normalmente usaria no dia a dia.

---
autenticacao jwt[]
usar boas praticas, para excessoes[X]
testes unitarios[todo]
orientado a eventos?mediatR[X]
questao de ciclo infinito nos endpoints, tirar[X]
---

etapas, configuracao relacao entre entidades  no context o que eh oneToMany...,etc[X]
validar dtos endpoints e ajustar tudo[X]
ajustar o preco unitario do item com base no preco do produto[X]
criar endpoints para gerencia de status do pedido[X]
criar eventos que limpa os Items da Lista de pedido cancelado, e atualiza tambem os obejtos em estoque[X]
começar estruturar testes unitarios[X], criar alguns testes persistentes[]
colocar o projeto no git[X]
Implementar autenticação (ex: JWT) []
colocar no github com actions[]
a cada push deve executar os testes e me gerar um package[]

