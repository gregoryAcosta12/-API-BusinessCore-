using System;
using System.Threading.Tasks;

namespace BusinessCore.Application.Interfaces
{
    /// <summary>
    /// Interfaz del servicio de caché
    /// Define las operaciones para la gestión de caché
    /// </summary>
    public interface ICacheService
    {
        // Operaciones básicas
        Task<T> GetAsync<T>(string key);
        Task SetAsync<T>(string key, T value);
        Task SetAsync<T>(string key, T value, TimeSpan expiration);
        Task<bool> RemoveAsync(string key);
        Task<bool> ExistsAsync(string key);

        // Operaciones adicionales
        Task<T> GetOrSetAsync<T>(string key, Func<Task<T>> factory, TimeSpan? expiration = null);
        Task<T> GetOrSetAsync<T>(string key, Func<T> factory, TimeSpan? expiration = null);
        Task<bool> RefreshAsync(string key);
        Task ClearAsync(string prefix = null);

        // Operaciones con colecciones
        Task<T> GetOrCreateCollectionAsync<T>(string key, Func<Task<T>> factory);
        Task<bool> AddToSetAsync<T>(string key, T value);
        Task<bool> RemoveFromSetAsync<T>(string key, T value);
        Task<long> GetSetCountAsync(string key);
    }
}