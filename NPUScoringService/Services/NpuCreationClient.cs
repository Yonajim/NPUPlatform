using NpuScoreService.Models;

namespace NpuScoreService.Services
{
    /// <summary>
    /// A client to interact with the NpuCreationService.
    /// </summary>
    public class NpuCreationClient
    {
        private readonly HttpClient _httpClient;

        /// <summary>
        /// Initializes a new instance of the NpuCreationClient class.
        /// </summary>
        /// <param name="httpClient">The HttpClient instance to be used for making requests.</param>
        public NpuCreationClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        /// <summary>
        /// Retrieves an NpuCreation by its ID asynchronously.
        /// </summary>
        /// <param name="id">The ID of the NpuCreation to retrieve.</param>
        /// <returns>A Task containing the NpuCreation if found.</returns>
        public async Task<NpuCreation?> GetNpuCreationByIdAsync(long? id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/NpuCreation/{id}");

                if (!response.IsSuccessStatusCode)
                {
                    throw new InvalidOperationException($"Error getting NpuCreation by Id: {response.ReasonPhrase}");
                }

                var npuCreation = await response.Content.ReadFromJsonAsync<NpuCreation>();
                return npuCreation;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error getting NpuCreation: {ex.Message}");
            }
        }
    }
}
