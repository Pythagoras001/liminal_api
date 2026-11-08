using liminal_api.Modules.Image.Application.Port.Out;
using liminal_api.Modules.Image.Domain;
using liminal_api.Modules.User.Application.Command;
using liminal_api.Modules.User.Application.Port.In;
using liminal_api.Modules.User.Application.Port.Out;
using liminal_api.Modules.User.Domain;

namespace liminal_api.Modules.User.Application.Service
{
    public class UserService : IRegisterUserUseCase, IUpdateUserUseCase
    {
        private readonly IUserRespositoryPort _userRepositoryPort;
        private readonly ICloudinaryImageRepositoryPort _cloudinaryImageRepositoryPort;
        private readonly IPasswordHasherPort _passwordHasherPort;

        public UserService(IUserRespositoryPort userRepositoryPort, ICloudinaryImageRepositoryPort cloudinaryImageRepositoryPort, IPasswordHasherPort passwordHasherPort)
        {
            _userRepositoryPort = userRepositoryPort;
            _cloudinaryImageRepositoryPort = cloudinaryImageRepositoryPort;
            _passwordHasherPort = passwordHasherPort;
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

        public async Task<UserRegister> updateUserUseCase(UpdateUserCommand command)
        {
            UserRegister userRegister = await _userRepositoryPort.findById(command.userId)
                ?? throw new Exception($"User with id {command.userId} not found");

            userRegister.changeUserName(command.userName);
            userRegister.changeEmail(command.email);

            if(command.imgProfile != null)
            {
                ImageCloudinary imageCloudinary = await _cloudinaryImageRepositoryPort.changeImageAsync(command.imgProfile, Guid.NewGuid().ToString(), userRegister.ImgProfile.PublicId);
                userRegister.changeImgProfile(imageCloudinary);
            }

            return await _userRepositoryPort.update(userRegister);
        }
    }
}
