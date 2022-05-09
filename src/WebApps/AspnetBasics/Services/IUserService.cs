using AspnetBasics.Models;

namespace AspnetBasics.Services
{
    public interface IUserService
    {
        Task<UserInfoViewModel> GetUserInfo();
    }
}
