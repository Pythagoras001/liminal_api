using liminal_api.Modules.Image.Domain;

namespace liminal_api.Modules.User.Domain
{
    public class UserRegister
    {
        private string _id;
        private string? _userName;
        private string? _email;
        private string? _password;
        private ImageCloudinary _imgProfile;
        private DateTime _createdAt;


        public UserRegister(string? id, string? userName, string? email, string? password, ImageCloudinary imgProfile, DateTime createdAt)
        {
            _id = id;
            _userName = userName;
            _email = email;
            _password = password;
            _imgProfile = imgProfile;
            _createdAt = createdAt;
        }
        public UserRegister(string? userName, string? email, string? password)
        {
            _userName = userName;
            _email = email;
            _password = password;
        }


        public string Id { get => _id; set => _id = value; }
        public string? UserName { get => _userName; set => _userName = value; }
        public string? Email { get => _email; set => _email = value; }
        public string? Password { get => _password; set => _password = value; }
        public ImageCloudinary ImgProfile { get => _imgProfile; set => _imgProfile = value; }
        public DateTime CreatedAt { get => _createdAt; set => _createdAt = value; }

       
        public string? Token { get; set; }

        public void changeImgProfile(ImageCloudinary imgProfile)
        {
            ImgProfile = imgProfile;
        }

        public void changeUserName(string userName)
        {
            if (!System.Text.RegularExpressions.Regex.IsMatch(userName, UserDataPatters.USER_NAME_PATTERN))
            {
                throw new ArgumentException("Invalid user name format.");
            }
            UserName = userName;
        }

        public void changeEmail(string email)
        {
            if (!System.Text.RegularExpressions.Regex.IsMatch(email, UserDataPatters.EMAIL_PATTERN) || email.Length > UserDataPatters.EMAIL_MAX_LENGTH)
            {
                throw new ArgumentException("Invalid email format.");
            }
            Email = email;
        }

        public UserRegister initDefaults()
        {
            CreatedAt = DateTime.UtcNow;
            return this;
        }
    }
}
