namespace LooveTaskManager.Domain.Common.Constants;

public static class LogMessages
{
    public static class Middleware
    {
        public const string DomainError = "Erro de domínio: {Message}, Código: {ErrorCode}, Status: {StatusCode}";
        public const string UnhandledError = "Erro não tratado: {Message}";
    }

    public static class Service
    {
        public const string CreatingTask = "Iniciando criação de tarefa";
        public const string TaskCreated = "Tarefa criada com sucesso. ID: {0}";
        public const string TaskCreationFailed = "Falha ao criar tarefa";
        public const string GettingTasks = "Obtendo lista de tarefas";
        public const string TasksRetrieved = "Tarefas obtidas com sucesso. Total: {0}";
        public const string GettingAllTasks = "Obtendo todas as tarefas";
        public const string GettingTaskById = "Obtendo tarefa por ID";
        public const string TaskRetrieved = "Tarefa obtida com sucesso";
        public const string TaskNotFound = "Tarefa não encontrada";
    }

    public static class Repository
    {
        public const string GettingAllEntities = "Obtendo todas as entidades";
        public const string GettingEntityById = "Obtendo entidade por ID";
        public const string AddingEntity = "Adicionando entidade";
        public const string EntityAdded = "Entidade adicionada com sucesso";
        public const string UpdatingEntity = "Atualizando entidade";
        public const string EntityUpdated = "Entidade atualizada com sucesso";
        public const string RemovingEntity = "Removendo entidade";
        public const string EntityRemoved = "Entidade removida com sucesso";
        public const string CheckingEntityExists = "Verificando existência da entidade";
        public const string EntityExists = "Entidade existe";
        public const string EntityDoesNotExist = "Entidade não existe";
        public const string ErrorAddingTask = "Erro ao adicionar tarefa";
        public const string ErrorUpdatingTask = "Erro ao atualizar tarefa";
        public const string ErrorDeletingTask = "Erro ao deletar tarefa";
        public const string ErrorGettingTasks = "Erro ao obter tarefas";
    }
} 