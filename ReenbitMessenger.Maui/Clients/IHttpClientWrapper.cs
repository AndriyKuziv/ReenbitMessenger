namespace ReenbitMessenger.Maui.Clients
{
    public interface IHttpClientWrapper
    {
        void Initialize();
        Task<TResponse?> GetAsync<TResponse>(string url);
        Task<TResponse?> PostAsync<TResponse, TRequest>(string url, TRequest request);
        Task<bool> PostAsync<TRequest>(string url, TRequest request);
        Task<TResponse?> PutAsync<TResponse, TRequest>(string url, TRequest request);
        Task<TResponse?> DeleteAsync<TResponse>(string url);
        Task<bool> DeleteAsync(string url);
        Task<bool> SetToken();
        Task DeleteToken();
    }
}
