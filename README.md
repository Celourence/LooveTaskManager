# LooveTaskManager

Uma API RESTful para gerenciamento de tarefas, desenvolvida com .NET 8 e arquitetura limpa.

## 🚀 Tecnologias

- **.NET 8**: Framework principal
- **Entity Framework Core**: ORM para acesso a dados
- **SQL Server**: Banco de dados relacional
- **Mapster**: Mapeamento de objetos
- **FluentValidation**: Validação de dados
- **Serilog**: Logging estruturado
- **Swagger**: Documentação da API
- **xUnit**: Framework de testes
- **Moq**: Framework de mocking para testes

## 🏗️ Arquitetura

A aplicação segue os princípios da Arquitetura Limpa (Clean Architecture) e DDD (Domain-Driven Design):

- **LooveTaskManager.Domain**: Camada de domínio com entidades, interfaces e regras de negócio
- **LooveTaskManager.Application**: Camada de aplicação com serviços, DTOs e casos de uso
- **LooveTaskManager.Infrastructure**: Camada de infraestrutura com implementações concretas
- **LooveTaskManager.API**: Camada de apresentação com controllers e configurações
- **LooveTaskManager.Tests**: Camada de testes com testes unitários e de integração

## 🎯 Funcionalidades

- Criação de tarefas
- Atualização de tarefas
- Exclusão de tarefas
- Listagem de tarefas
- Busca de tarefa por ID
- Validação de dados
- Tratamento de erros
- Logging estruturado

## 🛠️ Desafios e Soluções

1. **Mapeamento de Objetos**:
   - Desafio: Configuração do Mapster para evitar múltiplas inicializações
   - Solução: Implementação de flag estática para controle de configuração

2. **Testes de Integração**:
   - Desafio: Isolamento de testes e gerenciamento de banco de dados em memória
   - Solução: Implementação de `TestFixture` e `TestBase` para gerenciar o ciclo de vida dos testes

3. **Validação de Dados**:
   - Desafio: Validações complexas de regras de negócio
   - Solução: Uso do FluentValidation com validações customizadas

4. **Tratamento de Erros**:
   - Desafio: Padronização de respostas de erro
   - Solução: Middleware de tratamento de exceções com ProblemDetails

## 📋 Pré-requisitos

- .NET 8 SDK
- Docker Desktop
- Visual Studio 2022 ou VS Code

## 🚀 Como Executar

### 1. Configurando o Banco de Dados

#### Usando Docker (Recomendado)

1. **Inicie o Docker Desktop**

2. **Execute o container do SQL Server**:
   ```bash
   docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=L00v&Passw0rd" -p 1433:1433 --name sql-server -d mcr.microsoft.com/mssql/server:2022-latest
   ```

3. **Verifique se o container está rodando**:
   ```bash
   docker ps
   ```

4. **Para parar o container**:
   ```bash
   docker stop sql-server
   ```

5. **Para remover o container**:
   ```bash
   docker rm sql-server
   ```
6. **Execute as migrations**


### 2. Executando a Aplicação

#### Opção 1: Usando Visual Studio (Recomendado)

1. **Abra a solução no Visual Studio 2022**:
   - Abra o arquivo `LooveTaskManager.sln`
   - Aguarde a restauração dos pacotes NuGet

2. **Configure o banco de dados**:
   - Verifique se a connection string em `appsettings.json` está correta:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=localhost,1433;Database=LooveTaskManager;User Id=sa;Password=L00v&Passw0rd;TrustServerCertificate=True"
     }
   }
   ```
   - No Visual Studio, abra o Console do Gerenciador de Pacotes (Tools > NuGet Package Manager > Package Manager Console)
   - Execute as migrações:
   ```
   Update-Database -Project LooveTaskManager.Infrastructure -StartupProject LooveTaskManager.API
   ```
   ou
   Utilize o comando BASH no seu terminal no diretório da aplicação:
      ```bash
   dotnet ef database update --project LooveTaskManager.Infrastructure --startup-project LooveTaskManager.API
   ```   



3. **Execute a aplicação**:
   - No Visual Studio, certifique-se de que o projeto `LooveTaskManager.API` está definido como projeto de inicialização
   - Selecione o profile "IIS Express" na barra de ferramentas
   - Pressione F5 ou clique no botão "Play" para iniciar a aplicação
   - O navegador será aberto automaticamente com a documentação Swagger

   #### Exemplos de Uso no Swagger UI

   1. **Criar uma Tarefa (POST /api/v1/task)**
      ```json
      {
        "title": "Implementar autenticação",
        "description": "Adicionar autenticação JWT na API",
        "dueDate": "2024-03-31T23:59:59",
        "status": 0
      }
      ```

   2. **Atualizar uma Tarefa (PUT /api/v1/task/{id})**
      ```json
      {
        "title": "Implementar autenticação JWT",
        "description": "Adicionar autenticação JWT na API com refresh token",
        "dueDate": "2024-04-15T23:59:59",
        "status": 1
      }
      ```

   3. **Listar Tarefas (GET /api/v1/task)**
      - Suporta paginação com parâmetros `skip` e `take`
      - Exemplo: `/api/v1/task?skip=0&take=10`

## 🧪 Executando os Testes

```bash
dotnet test
```

## 📝 Endpoints da API

- `GET /api/v1/task`: Lista todas as tarefas
- `GET /api/v1/task/{id}`: Obtém uma tarefa específica
- `POST /api/v1/task`: Cria uma nova tarefa
- `PUT /api/v1/task/{id}`: Atualiza uma tarefa existente
- `DELETE /api/v1/task/{id}`: Remove uma tarefa

## 🔒 Status Codes

- 200: Sucesso
- 201: Criado
- 204: Sem conteúdo
- 400: Requisição inválida
- 404: Não encontrado
- 409: Conflito
- 500: Erro interno do servidor

