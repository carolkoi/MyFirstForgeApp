using Autodesk.Forge;
using Autodesk.Forge.Client;
using Autodesk.Forge.Model;

namespace MyForgeApp.Models
{
    /// <summary>
    /// This is where will implement all the OSS (Object Storage Service) logic of our server application.
    /// </summary>
    public partial class ForgeService
    {
        private async Task EnsureBucketExists(string bucketKey)
        {
            var token = await GetInternalToken();
            var api = new BucketsApi();
            api.Configuration.AccessToken = token.AccessToken;
            try
            {
                await api.GetBucketDetailsAsync(bucketKey);
            }
            catch (ApiException e)
            {
                if (e.ErrorCode == 404)
                {
                    await api.CreateBucketAsync(new PostBucketsPayload(bucketKey, null, PostBucketsPayload.PolicyKeyEnum.Temporary));
                }
                else
                {
                    throw e;
                }
            }
        }

        public async Task<ObjectDetails> UploadModel(string objectName, Stream content, long contentLength)
        {
            await EnsureBucketExists(_bucket);
            var token = await GetInternalToken();
            var api = new ObjectsApi();
            api.Configuration.AccessToken = token.AccessToken;
            var obj = (await api.UploadObjectAsync(_bucket, objectName, (int)contentLength, content)).ToObject<ObjectDetails>();
            return obj;
        }

        public async Task<IEnumerable<ObjectDetails>> GetObjects()
        {
            const int PageSize = 64;
            await EnsureBucketExists(_bucket);
            var token = await GetInternalToken();
            var api = new ObjectsApi();
            api.Configuration.AccessToken = token.AccessToken;
            var results = new List<ObjectDetails>();
            var response = (await api.GetObjectsAsync(_bucket, PageSize)).ToObject<BucketObjects>();
            results.AddRange(response.Items);
            while (!string.IsNullOrEmpty(response.Next))
            {
                var queryParams = Microsoft.AspNetCore.WebUtilities.QueryHelpers.ParseQuery(new Uri(response.Next).Query);
                response = (await api.GetObjectsAsync(_bucket, PageSize, null, queryParams["startAt"])).ToObject<BucketObjects>();
                results.AddRange(response.Items);
            }
            return results;
        }
    }
}
