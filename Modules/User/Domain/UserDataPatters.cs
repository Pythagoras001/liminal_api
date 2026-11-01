namespace liminal_api.Modules.User.Domain
{
    public record UserDataPatters
    {
        public const string USER_NAME_PATTERN = @"^[a-zA-Z0-9_]{3,20}$";
        public const string EMAIL_PATTERN = @"^[^\s@]+@[^\s@]+\.[^\s@]+$";
        public const int EMAIL_MAX_LENGTH = 255;
    }
}
