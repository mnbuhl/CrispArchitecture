using Microsoft.AspNetCore.Http;

namespace CrispArchitecture.Api.Helpers
{
    public class LocationUri
    {
        public string GetLocationUri(HttpRequest request, string id)
        {
            string baseUrl = $"{request.Scheme}://{request.Host.ToUriComponent()}";
            string locationUri = baseUrl + request.Path.Value + "/" + id;

            return locationUri;
        }
    }
}