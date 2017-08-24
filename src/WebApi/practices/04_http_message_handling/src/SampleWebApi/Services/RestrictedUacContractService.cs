using System;
using Newtonsoft.Json.Linq;
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
            /*
             * The sensitive information should be removed if current user is not an
             * Admin. 
             * 
             * The sensitive information is placed in a property called 'links',
             * which is an array of objects. Each object should have an optional 
             * boolean property called 'restricted'. And its value should be regarded
             * as `false` if the property is not defined.
             * 
             * If the user is an Admin, then nothing should be changed, while if the
             * user is a normal user. Then all restricted inforamtion should be removed.
             * 
             * The function will return true if the JSON content has been updated. Or
             * else it will return false.
             */
      }
    }
}