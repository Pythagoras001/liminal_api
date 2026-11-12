using liminal_api.Modules.Image.Application.Port.Out;
using liminal_api.Modules.Image.Domain;
using liminal_api.Modules.User.Application.Command;
using liminal_api.Modules.User.Application.Port.In;
using liminal_api.Modules.User.Application.Port.Out;
using liminal_api.Modules.User.Domain;

namespace liminal_api.Modules.User.Application.Service
{
    public class UserService : IRegisterUserUseCase, IUpdateUserUseCase, ILoginUserUseCase
    {
        private readonly IUserRespositoryPort _userRepositoryPort;
        private readonly ICloudinaryImageRepositoryPort _cloudinaryImageRepositoryPort;
        private readonly IPasswordHasherPort _passwordHasherPort;
        private readonly ITokenServicePort _tokenServicePort;

        public UserService(IUserRespositoryPort userRepositoryPort, ICloudinaryImageRepositoryPort cloudinaryImageRepositoryPort, IPasswordHasherPort passwordHasherPort, ITokenServicePort tokenServicePort)
        {
            _userRepositoryPort = userRepositoryPort;
            _cloudinaryImageRepositoryPort = cloudinaryImageRepositoryPort;
            _passwordHasherPort = passwordHasherPort;
            _tokenServicePort = tokenServicePort;
        }

        public Task<UserRegister> registerUserUseCase(RegisterUserCommand command)
        {
            string hashedPassword = _passwordHasherPort.hashPassword(command.password);

            UserRegister userRegister = new UserRegister
            (
                userName: command.userName,
                email: command.email,
                password: hashedPassword
            );

            return _userRepositoryPort.save(userRegister);
        }

        public async Task<UserRegister> findUser(string id)
        {
            if (!long.TryParse(id, out long userId))
                throw new ArgumentException("Invalid user id.");

            UserRegister userRegister = await _userRepositoryPort.findById(userId)
                ?? throw new KeyNotFoundException($"User with id {userId} not found.");

            return userRegister;
        }

        public async Task<UserRegister> updateUserUseCase(UpdateUserCommand command)
        {
            UserRegister userRegister = await _userRepositoryPort.findById(command.userId)
                ?? throw new Exception($"User with id {command.userId} not found");

            if (!string.IsNullOrWhiteSpace(command.userName))
                userRegister.changeUserName(command.userName);

            if (!string.IsNullOrWhiteSpace(command.email))
                userRegister.changeEmail(command.email);

            if (command.imgProfile != null)
            {
                string? oldPublicId = userRegister.ImgProfile?.PublicId;
                ImageCloudinary imageCloudinary = await _cloudinaryImageRepositoryPort.UploadAsync(
                    command.imgProfile,
                    Guid.NewGuid().ToString());
                userRegister.changeImgProfile(imageCloudinary);

                UserRegister updatedUser = await _userRepositoryPort.update(userRegister);
                if (!string.IsNullOrEmpty(oldPublicId))
                    await _cloudinaryImageRepositoryPort.deleteImage(oldPublicId);

                return updatedUser;
            }

            return await _userRepositoryPort.update(userRegister);
        }

        public async Task<string> loginUserUseCase(LoginCommand command)
        {
            UserRegister userRegister = await _userRepositoryPort.findByEmail(command.email)
                ?? throw new UnauthorizedAccessException("Invalid email or password.");

            bool passwordValid = _passwordHasherPort.verifyPassword(userRegister.Password!, command.password);

            if (!passwordValid)
                throw new UnauthorizedAccessException("Invalid email or password.");

            string token = _tokenServicePort.generateToken(userRegister);

            return token;
        }
    }
}