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
            if (roleRepository.Get(userId) == Role.Admin) { return false; }

            var linksArray = restrictedResource["links"] as JArray;
            if (linksArray == null) { return false; }
            JToken[] restrictedLinks = linksArray.Where(
                item =>
                {
                    var linkItemObj = item as JObject;
                    var restrictedProp = linkItemObj?["restricted"];
                    if (restrictedProp?.Type != JTokenType.Boolean) { return false; }
                    return restrictedProp.Value<bool>();
                }).ToArray();
            if (restrictedLinks.Length == 0) { return false; }
            foreach (JToken restrictedLink in restrictedLinks)
            {
                linksArray.Remove(restrictedLink);
            }
            return true;
        }
    }
}