using SampleWebApi.DomainModel;

namespace SampleWebApi.Repositories
{
    public class RoleRepository
    {
        public Role Get(long userId)
        {
            return userId % 100 == 0 ? Role.Admin : Role.Normal;
        }
    }
}