using NpuSearchService.Models;

namespace NpuSearchService.Services
{
    /// <summary>
    /// A client for interacting with the NpuCreationService.
    /// </summary>
    public class NpuCreationClient
    {
        private readonly HttpClient _httpClient;

        /// <summary>
        /// Initializes a new instance of the NpuCreationClient class.
        /// </summary>
        /// <param name="httpClient">The HttpClient to use for making requests.</param>
        public NpuCreationClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        /// <summary>
        /// Searches NpuCreations by the provided element name.
        /// </summary>
        /// <param name="elementName">The element name to search by.</param>
        /// <returns>A list of NpuCreations containing the element name.</returns>
        public async Task<IEnumerable<NpuCreation>> SearchByElementNameAsync(string elementName)
        {
            try
            {
                var response = await _httpClient.GetAsync($"/api/npucreation/search/{elementName}");

                if (!response.IsSuccessStatusCode)
                {
                    throw new InvalidOperationException($"Error searching by element name: {response.ReasonPhrase}");
                }

                var npuCreations = await response.Content.ReadFromJsonAsync<IEnumerable<NpuCreation>>();
                return npuCreations;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"An error occurred while searching for NPU creations: {ex.Message}");
            }
        }
    }
}