using CrossCutting.Common.Models;
using CrossCutting.Web.Models;

namespace CrossCutting.Web.Factories
{
    public interface IUserFactory
    {
        UserDto CreateUserDto(User user);
    }
}