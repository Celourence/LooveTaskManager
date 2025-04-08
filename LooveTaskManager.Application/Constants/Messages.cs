namespace LooveTaskManager.Application.Constants;

public static class Messages
{
    public static class Log
    {
        public static class Task
        {
            public const string CreatingTask = "Criando tarefa: {0}";
            public const string TaskCreated = "Tarefa criada com sucesso. ID: {0}";
            public const string ErrorCreatingTask = "Erro ao criar tarefa";
            public const string GettingTasks = "Obtendo todas as tarefas";
            public const string TasksRetrieved = "Tarefas obtidas com sucesso. Total: {0}";
            public const string ErrorGettingTasks = "Erro ao obter tarefas";
            public const string GettingTaskById = "Obtendo tarefa pelo ID: {0}";
            public const string TaskNotFound = "Tarefa não encontrada. ID: {0}";
            public const string ErrorGettingTaskById = "Erro ao obter tarefa pelo ID";
            public const string UpdatingTask = "Atualizando tarefa. ID: {0}";
            public const string TaskUpdated = "Tarefa atualizada com sucesso. ID: {0}";
            public const string ErrorUpdatingTask = "Erro ao atualizar tarefa";
            public const string DeletingTask = "Excluindo tarefa. ID: {0}";
            public const string TaskDeleted = "Tarefa excluída com sucesso. ID: {0}";
            public const string ErrorDeletingTask = "Erro ao excluir tarefa";
            public const string DuplicateTitle = "Já existe uma tarefa com o título: {0}";
            public const string ValidationFailed = "Falha na validação da tarefa";
        }

        public static class Repository
        {
            public const string AddingEntity = "Adicionando entidade ao repositório";
            public const string EntityAdded = "Entidade adicionada com sucesso";
            public const string ErrorAddingTask = "Erro ao adicionar tarefa ao repositório";
            public const string GettingAllEntities = "Obtendo todas as entidades";
            public const string ErrorGettingTasks = "Erro ao obter tarefas do repositório";
            public const string GettingEntityById = "Obtendo entidade pelo ID";
            public const string ErrorGettingTaskById = "Erro ao obter tarefa pelo ID do repositório";
            public const string UpdatingEntity = "Atualizando entidade no repositório";
            public const string EntityUpdated = "Entidade atualizada com sucesso";
            public const string ErrorUpdatingTask = "Erro ao atualizar tarefa no repositório";
            public const string RemovingEntity = "Removendo entidade do repositório";
            public const string EntityRemoved = "Entidade removida com sucesso";
            public const string ErrorDeletingTask = "Erro ao excluir tarefa do repositório";
            public const string CountingTasks = "Contando total de tarefas";
            public const string GettingTotalTasks = "Obtendo total de tarefas";
            public const string CheckingTitleExists = "Verificando existência de título";
            public const string CheckingEntityExists = "Verificando existência de entidade";
        }

        public static class Error
        {
            public const string UnhandledError = "Erro não tratado: {0}";
            public const string DomainError = "Erro de domínio: {0}, Status: {1}";
            public const string ValidationError = "Erro de validação: {0}";
            public const string DatabaseError = "Erro de banco de dados: {0}";
            public const string NotFoundError = "Recurso não encontrado: {0}";
            public const string ConflictError = "Conflito: {0}";
        }
    }

    public static class Error
    {
        public static class Task
        {
            public const string TitleRequired = "O título da tarefa é obrigatório";
            public const string TitleTooLong = "O título da tarefa não pode ter mais de 200 caracteres";
            public const string DescriptionTooLong = "A descrição da tarefa não pode ter mais de 4000 caracteres";
            public const string DueDateRequired = "A data de vencimento é obrigatória";
            public const string DueDateInPast = "A data de vencimento não pode ser no passado";
            public const string TitleAlreadyExists = "Já existe uma tarefa com este título";
            public const string TaskNotFound = "Tarefa não encontrada";
            public const string InvalidStatus = "Status inválido";
            public const string NotFound = "Tarefa não encontrada";
        }

        public static class Validation
        {
            public const string RequiredField = "O campo {0} é obrigatório";
            public const string InvalidLength = "O campo {0} deve ter entre {1} e {2} caracteres";
            public const string InvalidDate = "A data {0} é inválida";
            public const string InvalidEnum = "O valor {0} não é válido para o campo {1}";
        }

        public static class Database
        {
            public const string ConnectionError = "Erro ao conectar ao banco de dados";
            public const string QueryError = "Erro ao consultar o banco de dados";
            public const string SaveError = "Erro ao salvar no banco de dados";
            public const string DeleteError = "Erro ao excluir do banco de dados";
        }
    }
} 