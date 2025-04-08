namespace LooveTaskManager.Domain.Common.Constants;

public static class ErrorMessages
{
    public static class General
    {
        public const string InternalServerError = "Erro interno do servidor";
        public const string NotFound = "Recurso não encontrado";
        public const string BadRequest = "Invalid request";
        public const string Unauthorized = "Acesso não autorizado";
        public const string Forbidden = "Access forbidden";
        public const string ValidationError = "Erro de validação";
    }

    public static class Task
    {
        public const string EmptyTitle = "O título é obrigatório";
        public const string EmptyDescription = "A descrição é obrigatória";
        public const string PastDueDate = "A data de vencimento não pode ser no passado";
        public const string TitleTooLong = "O título não pode ter mais de 200 caracteres";
        public const string DescriptionTooLong = "A descrição não pode ter mais de 1000 caracteres";
        public const string DueDateRequired = "A data de vencimento é obrigatória";
        public const string StatusRequired = "O status é obrigatório";
        public const string InvalidStatus = "O status informado é inválido";
        public const string TitleAlreadyExists = "Já existe uma tarefa com este título";
        public const string TaskNotFound = "Tarefa não encontrada";
    }

    public static class Repository
    {
        public const string GetEntitiesError = "Error getting entities: {0}";
        public const string GetEntityError = "Error getting entity: {0}";
        public const string AddEntityError = "Error adding entity: {0}";
        public const string UnexpectedAddEntityError = "Unexpected error adding entity: {0}";
        public const string UpdateEntityConflict = "Conflict updating entity: {0}";
        public const string UpdateEntityError = "Error updating entity: {0}";
        public const string UnexpectedUpdateEntityError = "Unexpected error updating entity: {0}";
        public const string RemoveEntityError = "Error removing entity: {0}";
        public const string UnexpectedRemoveEntityError = "Unexpected error removing entity: {0}";
        public const string CheckEntityExistsError = "Error checking if entity exists: {0}";
        public const string EntityNotFound = "Entity not found";
    }

    public static class Http
    {
        public const string UnauthorizedAccess = "Unauthorized access";
        public const string OperationTimeout = "Operation exceeded the time limit";
        public const string ResourceNotFound = "Resource not found";
        public const string InvalidOperation = "Operação inválida";
        public const string InternalServerError = "An internal server error occurred";
    }
} 