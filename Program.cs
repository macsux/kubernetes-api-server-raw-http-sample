using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using k8s;
using Newtonsoft.Json.Linq;

namespace KubernetesApiServerRawHttpSample
{
    class Program
    {
        static async Task Main()
        {
            var config = KubernetesClientConfiguration.BuildDefaultConfig();
            var client = new Kubernetes(config) {HttpClient = {BaseAddress = new Uri(config.Host)}};
            var request = new HttpRequestMessage(HttpMethod.Get, "api/v1/namespaces/default/pods");
            await client.Credentials.ProcessHttpRequestAsync(request, CancellationToken.None);
            var response = await client.HttpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var json = JObject.Parse(await response.Content.ReadAsStringAsync()).ToString(Newtonsoft.Json.Formatting.Indented);
            Console.WriteLine(json);
        }
    }
}