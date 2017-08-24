using System.Linq;
using Newtonsoft.Json.Linq;
using SampleWebApi.DomainModel;
using SampleWebApi.Repositories;

namespace SampleWebApi.Services
{
    public class RestrictedUacContractService
    {
        readonly RoleRepository roleRepository;

        public RestrictedUacContractService(RoleRepository roleRepository)
        {
            this.roleRepository = roleRepository;
        }

        public bool RemoveRestrictedInfo(long userId, JObject restrictedResource)
        {
            if (roleRepository.Get(userId) == Role.Admin) return false;

            var links = restrictedResource["links"] as JArray;
            if (links == null) return false;

            var resitrictedLinks = links.Where(link =>
            {
                var linkObj = link as JObject;
                var isRestricted = linkObj?["restricted"];
                return isRestricted?.Type == JTokenType.Boolean &&
                    isRestricted.Value<bool>();
            }).ToList();

            if (resitrictedLinks.Count == 0) return false;

            foreach (var resitrictedLink in resitrictedLinks)
            {
                links.Remove(resitrictedLink);
            }

            return true;
        }
    }
}