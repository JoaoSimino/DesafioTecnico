# Desafio Técnico - API de Gerenciamento de Dados

O desafio foi criado por uma IA, com a finalidade de treinamento de desenvolvimento backend e tecnologiais atuais de mercado. Todos os requisitos de tecnologia que deveriam conter foram passados no prompt.

## 📋 Descrição do Projeto

Desenvolver uma API RESTful para gerenciar um sistema de controle de pedidos e clientes para uma loja virtual. Esse sistema permitirá que os clientes realizem pedidos e acompanhem seus status.

Requisitos principais Funcionalidades obrigatórias:

CRUD de Clientes - Cliente: Id, Nome, Email, Telefone, DataCadastro

CRUD de Produtos - Produto: Id, Nome, Descrição, Preço, Estoque

CRUD de Pedidos - Pedido: Id, ClienteId, DataPedido, Status (Aberto, Pago, Cancelado), Itens

ItemPedido: ProdutoId, Quantidade, Preço Unitario
### Principais Funcionalidades
- **Criação de um Pedido**: Validar se o Cliente existe, e se todos os Produtos existem e possuem estoque suficiente. E por fim deduzir o estoque dos produtos
- **Endpoint para alterar o status do pedido**: Aberto -> Pago, Aberto -> Cancelado.
- **Endpoint para listar pedidos**:  Listar todos os pedidos de um cliente específico.


### Requisitos de Projeto
- .NET 8.0 SDK ou superior
-  Utilizar Entity Framework Core, SQL Server (local ou via Docker)
- Visual Studio 2022 ou outro IDE compatível (como Rider ou VS Code)
- Postman (recomendado para testar endpoints da API)
-  Clean Architecture (Domain, Application, Infrastructure, API) 
- Seguir boas práticas de SOLID 
- Criar pelo menos 3 testes unitários com xUnit ou NUnit
- Utilizar AutoMapper para mapeamento entre DTOs e entidades
- Criar validações com FluentValidation
- Documentar a API com Swagger
- Utilizar jwt

## 🚀 Como Executar o Projeto

Siga os passos abaixo para configurar e executar a aplicação localmente.
### Instalação
1. **Clone o repositório**:
   ```bash
   git clone https://github.com/JoaoSimino/DesafioTecnico.git
   cd DesafioTecnico
   ```

2. **Restaure as dependências**:
   ```bash
   dotnet restore
   ```

3. **Configure o banco de dados**:
   - Crie um banco de dados no SQL Server chamado `DesafioTecnico`.
   - Atualize a string de conexão no arquivo `appsettings.json`:
     ```json
     {
       "ConnectionStrings": {
         "DefaultConnection": "Server=localhost;Database=DesafioTecnico;Trusted_Connection=True;"
       }
     }
     ```
   - Aplique as migrações para criar as tabelas:
     ```bash
     dotnet ef database update
     ```

4. **Inicie a aplicação**:
   ```bash
   dotnet run
   ```

5. Acesse a API em `https://localhost:5001` (ou a porta configurada).

## 🛠️ Tecnologias Utilizadas

- **C#**: Linguagem principal do projeto.
- **ASP.NET Core**: Framework para construção da API REST.
- **Entity Framework Core**: ORM para interação com o banco de dados.
- **SQL Server**: Banco de dados relacional para armazenamento de dados.
- **Swagger**: Documentação interativa da API.
- **xUnit**: Framework para testes unitários (se aplicável).

## 📚 Estrutura do Projeto

```plaintext
DesafioTecnico/
├── Endpoints/         # Controladores da API
├── Model/             # Modelos de dados
├── Data/               # Contexto do Entity Framework e migrações
├── Docker/           # Lógica de negócios
├── Program.cs          # Configuração da aplicação
├── DTOs          # Configuração de serviços e middleware
├── appsettings.json    # Configurações da aplicação
├── DesafioTecnico.sln  # Solução do Visual Studio
└── README.md           # Este arquivo
```

## 🤝 Contribuição

Contribuições são bem-vindas! Para contribuir:

1. Faça um fork do repositório.
2. Crie uma branch para sua feature: `git checkout -b minha-feature`.
3. Commit suas alterações: `git commit -m 'Adiciona minha feature'`.
4. Envie para o repositório remoto: `git push origin minha-feature`.
5. Abra um Pull Request.

## 📜 Licença

Este projeto está licenciado sob a [MIT License](LICENSE).

## 📞 Contato

Para dúvidas ou sugestões, entre em contato com [João Simino](https://github.com/JoaoSimino).