using shop.ModelDTO;

namespace shop.Services.AuthService
{
    public interface IAuthService
    {
        Task Register(RegisterRequestDTO registerRequestDTO);
    }
}
