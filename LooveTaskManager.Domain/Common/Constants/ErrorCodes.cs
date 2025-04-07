namespace LooveTaskManager.Domain.Common.Constants;

public static class ErrorCodes
{
    public static class General
    {
        public const string InternalServerError = "INTERNAL_SERVER_ERROR";
        public const string NotFound = "NOT_FOUND";
        public const string BadRequest = "BAD_REQUEST";
        public const string Unauthorized = "UNAUTHORIZED";
        public const string Forbidden = "FORBIDDEN";
        public const string ValidationError = "VALIDATION_ERROR";
    }

    public static class Task
    {
        public const string EmptyTitle = "TASK_EMPTY_TITLE";
        public const string EmptyDescription = "TASK_EMPTY_DESCRIPTION";
        public const string PastDueDate = "TASK_PAST_DUE_DATE";
        public const string TitleTooLong = "TASK_TITLE_TOO_LONG";
        public const string TitleAlreadyExists = "TASK_TITLE_ALREADY_EXISTS";
        public const string TaskNotFound = "TASK_NOT_FOUND";
    }

    public static class Repository
    {
        public const string GetEntitiesError = "REPOSITORY_GET_ENTITIES_ERROR";
        public const string GetEntityError = "REPOSITORY_GET_ENTITY_ERROR";
        public const string AddEntityError = "REPOSITORY_ADD_ENTITY_ERROR";
        public const string UpdateEntityError = "REPOSITORY_UPDATE_ENTITY_ERROR";
        public const string RemoveEntityError = "REPOSITORY_REMOVE_ENTITY_ERROR";
        public const string EntityNotFound = "REPOSITORY_ENTITY_NOT_FOUND";
    }
} 