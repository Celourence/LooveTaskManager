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

## 🛠️ Desafios e Soluções

1. **Mapeamento de Objetos**:
   - Desafio: Configuração do Mapster para evitar múltiplas inicializações (numca havia trabalhado com essa biblioteca, o automapper mudou seu modelo e agora é pago)
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
- SQL Server (ou Docker para SQL Server)
- Visual Studio 2022 ou VS Code

## 🚀 Como Executar

1. **Clone o repositório**:
   ```bash
   git clone https://github.com/seu-usuario/LooveTaskManager.git
   cd LooveTaskManager
   ```

2. **Configure o banco de dados**:
   - Atualize a connection string em `appsettings.json`
   - Execute as migrações:
   ```bash
   dotnet ef database update --project LooveTaskManager.Infrastructure --startup-project LooveTaskManager.API
   ```

3. **Execute a aplicação**:
   ```bash
   cd LooveTaskManager.API
   dotnet run
   ```

4. **Acesse a documentação**:
   - Swagger UI: https://localhost:5001/swagger
   - Swagger JSON: https://localhost:5001/swagger/v1/swagger.json

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

