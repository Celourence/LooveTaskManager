# LooveTaskManager

![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?style=for-the-badge&logo=.net&logoColor=white)
![C#](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=c-sharp&logoColor=white)
![SQL Server](https://img.shields.io/badge/SQL_Server-CC2927?style=for-the-badge&logo=microsoft-sql-server&logoColor=white)
![Docker](https://img.shields.io/badge/Docker-2496ED?style=for-the-badge&logo=docker&logoColor=white)
![Swagger](https://img.shields.io/badge/Swagger-85EA2D?style=for-the-badge&logo=swagger&logoColor=white)

LooveTaskManager é uma aplicação de gerenciamento de tarefas desenvolvida com .NET 8, seguindo os princípios da Arquitetura Limpa (Clean Architecture) e os padrões de projeto SOLID.

## 📋 Índice

- [Arquitetura](#-arquitetura)
- [Tecnologias e Bibliotecas](#-tecnologias-e-bibliotecas)
- [Padrões e Boas Práticas](#-padrões-e-boas-práticas)
- [API Endpoints](#-api-endpoints)
- [Como Executar](#-como-executar)
- [Estrutura do Projeto](#-estrutura-do-projeto)

## 🏗️ Arquitetura

O projeto segue a Arquitetura Limpa (Clean Architecture), que separa a aplicação em camadas concêntricas, cada uma com responsabilidades específicas:

```
LooveTaskManager/
├── LooveTaskManager.Domain/         # Camada de domínio (entidades, regras de negócio)
├── LooveTaskManager.Application/    # Camada de aplicação (casos de uso, interfaces)
├── LooveTaskManager.Infrastructure/ # Camada de infraestrutura (implementações concretas)
├── LooveTaskManager.API/            # Camada de apresentação (API REST)
└── LooveTaskManager.Tests/          # Testes unitários e de integração
```

### Fluxo de Dados

```
API → Application → Domain ← Infrastructure
```

- **Domain**: Contém as entidades e regras de negócio, sem dependências externas
- **Application**: Define os casos de uso e interfaces, dependendo apenas do domínio
- **Infrastructure**: Implementa as interfaces definidas na camada de aplicação
- **API**: Ponto de entrada da aplicação, orquestra as chamadas entre as camadas

## 🛠️ Tecnologias e Bibliotecas

### Core
- **.NET 8**: Framework de desenvolvimento
- **C#**: Linguagem de programação

### Persistência
- **Entity Framework Core 8.0.3**: ORM para acesso a dados
- **SQL Server**: Banco de dados relacional

### API
- **ASP.NET Core 8**: Framework para construção de APIs web
- **Swagger/OpenAPI**: Documentação e teste da API

### Mapeamento
- **Mapster 7.4.0**: Biblioteca de mapeamento entre objetos (escolhida como alternativa ao AutoMapper, que agora é pago)

### Validação
- **FluentValidation 11.11.0**: Validação de objetos com sintaxe fluente

### Logging
- **Serilog 4.2.0**: Framework de logging estruturado
- **Serilog.AspNetCore 8.0.3**: Integração com ASP.NET Core
- **Serilog.Sinks.Console 5.0.1**: Sink para console
- **Serilog.Sinks.File 5.0.0**: Sink para arquivo
- **Serilog.Enrichers.Environment 3.0.1**: Enriquecimento com informações do ambiente
- **Serilog.Enrichers.Thread 4.0.0**: Enriquecimento com informações de thread

## 🧩 Padrões e Boas Práticas

### Padrões de Projeto
- **Repository Pattern**: Abstração do acesso a dados
- **Unit of Work**: Gerenciamento de transações
- **CQRS (Command Query Responsibility Segregation)**: Separação de operações de leitura e escrita
- **Mediator Pattern**: Desacoplamento entre componentes
- **Dependency Injection**: Injeção de dependências

### Princípios SOLID
- **S - Single Responsibility Principle**: Cada classe tem uma única responsabilidade
- **O - Open/Closed Principle**: Entidades abertas para extensão, fechadas para modificação
- **L - Liskov Substitution Principle**: Subtipos devem ser substituíveis por seus tipos base
- **I - Interface Segregation Principle**: Interfaces específicas para cada cliente
- **D - Dependency Inversion Principle**: Dependa de abstrações, não de implementações concretas

### Outras Boas Práticas
- **Tratamento de Exceções Centralizado**: Middleware para tratamento de erros
- **Logging Estruturado**: Uso de Serilog para logs detalhados
- **Validação de Entrada**: Uso de FluentValidation para validação de dados
- **Documentação de API**: Swagger para documentação e teste da API
- **Mensagens Centralizadas**: Constantes para mensagens de log e erro
- **Versionamento de API**: Suporte a diferentes versões da API

## 🔌 API Endpoints

### Tarefas

| Método | Endpoint | Descrição | Códigos de Resposta |
|--------|----------|-----------|---------------------|
| POST | `/api/v1/tasks` | Cria uma nova tarefa | 201: Criado<br>400: Dados inválidos<br>409: Título duplicado<br>500: Erro interno |
| GET | `/api/v1/tasks` | Obtém todas as tarefas (com paginação) | 200: Sucesso<br>500: Erro interno |
| GET | `/api/v1/tasks/{id}` | Obtém uma tarefa pelo ID | 200: Sucesso<br>404: Não encontrado<br>500: Erro interno |

### Parâmetros de Paginação
- `skip`: Número de registros a pular (padrão: 0)
- `take`: Número de registros a retornar (padrão: 10)

### Códigos de Erro

| Código | Descrição | ErrorCode |
|--------|-----------|-----------|
| 400 | Dados inválidos | VALIDATION_ERROR |
| 404 | Recurso não encontrado | NOT_FOUND |
| 409 | Conflito (título duplicado) | TASK_TITLE_ALREADY_EXISTS |
| 500 | Erro interno do servidor | INTERNAL_SERVER_ERROR |

### Exemplo de Requisição para Criação de Tarefa
```json
{
  "title": "Título da Tarefa",
  "description": "Descrição da tarefa",
  "dueDate": "2023-12-31T23:59:59",
  "status": 0
}
```

### Exemplo de Resposta de Erro
```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.6.1",
  "title": "Erro de validação",
  "status": 400,
  "detail": "A data de vencimento não pode ser no passado",
  "instance": "/api/v1/tasks",
  "errorCode": "VALIDATION_ERROR"
}
```

## 🚀 Como Executar

### Pré-requisitos
- .NET 8 SDK
- SQL Server (ou Docker com SQL Server)
- Docker (opcional)

### Configuração do Banco de Dados

#### Usando SQL Server Local
1. Certifique-se de que o SQL Server está em execução
2. Verifique as configurações de conexão em `appsettings.json`

#### Usando Docker
```bash
docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=L00v&Passw0rd" -p 1433:1433 --name sql-server -d mcr.microsoft.com/mssql/server:2022-latest
```

### Executando a Aplicação

1. Clone o repositório
```bash
git clone https://github.com/seu-usuario/LooveTaskManager.git
cd LooveTaskManager
```

2. Restaure os pacotes
```bash
dotnet restore
```

3. Execute as migrações do banco de dados
```bash
cd LooveTaskManager.Infrastructure
dotnet ef database update --startup-project ../LooveTaskManager.API
```

4. Execute a aplicação
```bash
cd ../LooveTaskManager.API
dotnet run --urls "http://localhost:8103"
```

5. Acesse a documentação da API
```
http://localhost:8103/swagger
```

## 📁 Estrutura do Projeto

### LooveTaskManager.Domain
- **Entities**: Entidades do domínio
- **Interfaces**: Interfaces dos repositórios
- **Common**: Classes comuns (exceções, constantes)
- **Enums**: Enumerações

### LooveTaskManager.Application
- **DTOs**: Objetos de transferência de dados
- **Interfaces**: Interfaces dos serviços
- **Services**: Implementação dos casos de uso
- **Constants**: Constantes de mensagens

### LooveTaskManager.Infrastructure
- **Data**: Contexto do Entity Framework
- **Repositories**: Implementação dos repositórios
- **IoC**: Configuração de injeção de dependência

### LooveTaskManager.API
- **Controllers**: Controladores da API
- **Middlewares**: Middlewares personalizados
- **Models**: Modelos específicos da API

## 📝 Licença

Este projeto está licenciado sob a licença MIT - veja o arquivo LICENSE para detalhes.
