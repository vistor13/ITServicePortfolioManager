namespace ITServicePortfolioManager.BLL.Messages;

public static class Messages
{
    public static class Error
    {
        public const string UserAlreadyExist =
            "Користувач з таким іменем вже існує.";

        public const string IncorrectPassword =
            "Неправильний пароль.";

        public const string UserNotFound =
            "Користувача з таким іменем не знайдено.";
        
        public const string TasksNotFoundByFilter = 
            "Завдання за заданим фільтром не знайдено.";
        
        public const string TasksNotFoundForUser = 
            "У вас ще немає жодного сформованого пакету сервісів";

    }
}