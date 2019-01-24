using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace TMDbLibTests.TestFramework
{
    public class FailingContractResolver : DefaultContractResolver
    {
        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            JsonProperty res = base.CreateProperty(member, memberSerialization);

            if (!res.Ignored)
                // If we haven't explicitly stated that a field is not needed, we require it for compliance
                res.Required = Required.AllowNull;

            return res;
        }
    }
}