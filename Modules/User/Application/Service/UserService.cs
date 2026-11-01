using liminal_api.Modules.User.Application.Command;
using liminal_api.Modules.User.Application.Port.In;
using liminal_api.Modules.User.Application.Port.Out;
using liminal_api.Modules.User.Domain;

namespace liminal_api.Modules.User.Application.Service
{
    public class UserService : IRegisterUserUseCase, IUpdateUserUseCase
    {

        private readonly IUserRespositoryPort _userRepositoryPort;
        private readonly IPasswordHasherPort _passwordHasherPort;

        public UserService(IUserRespositoryPort userRepositoryPort, IPasswordHasherPort passwordHasherPort)
        {
            _userRepositoryPort = userRepositoryPort;
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


            throw new NotImplementedException();
        }
    }
}
