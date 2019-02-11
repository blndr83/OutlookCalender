using Microsoft.Graph;
using System.Threading.Tasks;

namespace CoreServices
{
    public interface IClientService
    {
        Task<GraphServiceClient> GraphServiceClient(string loginHint);
    }
}
