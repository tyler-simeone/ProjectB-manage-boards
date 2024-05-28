using manage_boards.src.models;

namespace manage_boards.src.clients
{
    public interface IAuthClient
    { 
        public Task<TokenResponse> GetBearerToken();
    }
}