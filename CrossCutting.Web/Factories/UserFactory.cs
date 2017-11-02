using CrossCutting.Common.Models;
using CrossCutting.Web.Models;

namespace CrossCutting.Web.Factories
{
    public class UserFactory : IUserFactory
    {
        public UserDto CreateUserDto(User user)
        {
            return new UserDto
            {
                Id = user.Id,
                Name = $"{user.FirstName} {user.LastName}",
                BirthDate = user.BirthDate,
                Employer = user.Employer
            };
        }
    }
}