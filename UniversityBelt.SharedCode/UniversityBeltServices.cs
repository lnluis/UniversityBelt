using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json.Linq;

namespace UniversityBelt.SharedCode
{
    public static class UniversityBeltServices
    {
        public static MobileServiceClient MobileService = new MobileServiceClient(
            "https://universitybelt.azure-mobile.net/",
            "CGPrYVTTnIyiijYAlBQrIHAFAqrDhd40"
            );

        public static async Task<string> LoginWindowsPhoneAsync(string accessToken)
        {
            JObject token = JObject.FromObject(accessToken);
            MobileServiceUser userId =
                await MobileService.LoginAsync(MobileServiceAuthenticationProvider.Facebook, token);
            return userId.UserId;
        }

    }
}