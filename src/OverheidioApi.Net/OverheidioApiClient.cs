using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using OverheidioApi.Net.Helpers;
using OverheidioApi.Net.Models;
using OverheidioApi.Net.Wrappers;

namespace OverheidioApi.Net
{
    public class OverheidioApiClient
    {
        private const string ApiBaseUrl = "https://overheid.io/";
        private readonly HttpClient _httpClient;

        /// <summary>
        /// Instantiates a new OverheidioClient
        /// </summary>
        /// <param name="apiKey">API key which can be generated on overheid.io</param>
        public OverheidioApiClient(string apiKey)
        {
            if (string.IsNullOrWhiteSpace(apiKey))
                throw new ArgumentException("Parameter apiKey needs a value");

            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(ApiBaseUrl)
            };

            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _httpClient.DefaultRequestHeaders.Add("ovio-api-key", apiKey);
        }

        /// <summary>
        /// Find corporations by specifying searchcriteria. Results will contain Dutch descriptions.
        /// Returned corporations may not include all property values depending on the <paramref name="fields"/> parameter.
        /// </summary>
        /// <param name="size">How many results should be returned, default 100</param>
        /// <param name="page">Which page (if more results are available than specified <paramref name="size"/>) should be retrieved</param>
        /// <param name="sort">On which field should be sorted</param>
        /// <param name="order">How should the results be ordered, "asc" or "desc"</param>
        /// <param name="fields">Fields to be included in the result. By default tradename, dossier- and subdossiernumber are included. See following link for all possible fields <see href="https://overheid.io/documentatie/kvk#show"/></param>
        /// <param name="filters">Used for specifying filters. Key is the field, value is the value to filter on. I.e. key: postcode, value: 3083cz. See following link for all possible fields <see href="https://overheid.io/documentatie/kvk#show"/></param>
        /// <param name="query">String to search for in <paramref name="queryfields"/>. * wildcard is available.</param>
        /// <param name="queryfields">Fields in which should be searched for <paramref name="query"/></param>
        /// <returns>List of results found including JSON+HAL metadata</returns>
        public async Task<ApiHalResultWrapper> ZoekBedrijf(int size = 100, int page = 1, string sort = "", string order = "desc", string[] fields = null, KeyValuePair<string, string>[] filters = null, string query = "", string[] queryfields = null)
        {
            if (size <= 0)
                throw new ArgumentOutOfRangeException("Parameter size cannot 0 or less");

            if (page <= 0)
                throw new ArgumentOutOfRangeException("Parameter page cannot 0 or less");

            // TODO maybe use an enum?
            if (order.ToLowerInvariant() != "desc" && order.ToLowerInvariant() != "asc")
                throw new ArgumentOutOfRangeException("Parameter order can only be asc or desc");

            var urlBuilder = new StringBuilder($"api/kvk?size={size}&page={page}");

            if (!string.IsNullOrWhiteSpace(sort))
                urlBuilder.Append($"&sort={sort}");

            if (!string.IsNullOrWhiteSpace(order))
                urlBuilder.Append($"&order={order}");

            if (fields != null && fields.Length > 0)
                foreach (var field in fields)
                    urlBuilder.Append($"&fields[]={field}");

            if (filters != null && filters.Length > 0)
                foreach (var filter in filters)
                    urlBuilder.Append($"&filters[{filter.Key}]={filter.Value}");

            if (!string.IsNullOrWhiteSpace(query))
                urlBuilder.Append($"&query={query}");

            if (queryfields != null && queryfields.Length > 0)
                foreach (var field in queryfields)
                    urlBuilder.Append($"&queryfields[]={field}");

            var jsonResult = await _httpClient.GetStringAsync(urlBuilder.ToString());
            return JsonHelper.Deserialize<ApiHalResultWrapper>(jsonResult);
        }

        /// <summary>
        /// Get a data about a specific corporation by dossier- and subdossiernumber
        /// </summary>
        /// <param name="dossiernummer">The chamber of commerce dossiernumber</param>
        /// <param name="subdossiernummer">The chamber of commerce subdossiernumber</param>
        /// <returns>Object with all information about the corporation, or null when a corporation isn't found or any other error occurs</returns>
        public async Task<Bedrijf> GetBedrijf(string dossiernummer, string subdossiernummer)
        {
            var resultJson = await _httpClient.GetStringAsync($"api/kvk/{dossiernummer}/{subdossiernummer}");
            return JsonHelper.Deserialize<Bedrijf>(resultJson);
        }

        /// <summary>
        /// Retrieves suggestions, useable in a autocomplete searchbox for example.
        /// </summary>
        /// <param name="query">String to search for, wildcards not supported searches as: searchte*, spaces are stripped</param>
        /// <param name="size">How many results are returned. Default 10, maximum 25</param>
        /// <param name="fields">Which fields to include in the suggestion. Can only be handelsnaam (tradename), straat (street) and dossiernummer (dossiernumber)</param>
        /// <returns>List of suggestions found with the given <paramref name="query"/> or null if an error occurs</returns>
        public async Task<BedrijfSuggestieResultaten> GetBedrijfSuggesties(string query, int size = 10, string[] fields = null)
        {
            if (size <= 0)
                throw new ArgumentOutOfRangeException("Parameter size cannot 0 or less");

            if (size > 25)
                throw new ArgumentOutOfRangeException("Parameter size cannot be more than 25");

            if (fields != null && fields.Except(new[] { "handelsnaam", "straat", "dossiernummer" }).Any())
                throw new ArgumentOutOfRangeException("Parameter fields can only contain values: handelsnaam, straat and dossiernummer");

            var urlBuilder = new StringBuilder($"suggest/kvk/{query}?size={size}");

            if (fields != null && fields.Length > 0)
                foreach (var field in fields)
                    urlBuilder.Append($"&fields[]={field}");

            var resultJson = await _httpClient.GetStringAsync(urlBuilder.ToString());

            try
            {
                var resultSuggestions = JsonHelper.Deserialize<BedrijfSuggestieResultaten>(resultJson);

                return resultSuggestions;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Find vehicles by specifying searchcriteria. Results will contains Dutch descriptions.
        /// Returned vehicles may not include all property values depending on the <paramref name="fields"/> parameter.
        /// </summary>
        /// <param name="size">How many results should be returned, default 100</param>
        /// <param name="page">Which page (if more results are available than specified <paramref name="size"/>) should be retrieved</param>
        /// <param name="sort">On which field should be sorted</param>
        /// <param name="order">How should the results be ordered, "asc" or "desc"</param>
        /// <param name="fields">Fields to be included in the result. By default date of first allowance, licenseplate, commercial designation, brand, and vehicle type are included. See following link for all possible fields <see href="https://overheid.io/documentatie/voertuiggegevens#show"/></param>
        /// <param name="filters">Used for specifying filters. Key is the field, value is the value to filter on. I.e. key: kenteken, value: 02-JZX-1. See following link for all possible fields <see href="https://overheid.io/documentatie/voertuiggegevens#show"/></param>
        /// <param name="query">String to search for in <paramref name="queryfields"/>. * wildcard is available.</param>
        /// <param name="queryfields">Fields in which should be searched for <paramref name="query"/></param>
        /// <returns>List of results found including JSON+HAL metadata</returns>
        public async Task<ApiHalResultWrapper> ZoekVoertuig(int size = 100, int page = 1, string sort = "", string order = "desc", string[] fields = null, KeyValuePair<string, string>[] filters = null, string query = "", string[] queryfields = null)
        {
            if (size <= 0)
                throw new ArgumentOutOfRangeException("Parameter size cannot 0 or less");

            if (page <= 0)
                throw new ArgumentOutOfRangeException("Parameter page cannot 0 or less");

            // TODO maybe use an enum?
            if (order.ToLowerInvariant() != "desc" && order.ToLowerInvariant() != "asc")
                throw new ArgumentOutOfRangeException("Parameter order can only be asc or desc");

            var urlBuilder = new StringBuilder($"/api/voertuiggegevens?size={size}&page={page}");

            if (!string.IsNullOrWhiteSpace(sort))
                urlBuilder.Append($"&sort={sort}");

            if (!string.IsNullOrWhiteSpace(order))
                urlBuilder.Append($"&order={order}");

            if (fields != null && fields.Length > 0)
                foreach (var field in fields)
                    urlBuilder.Append($"&fields[]={field}");

            if (filters != null && filters.Length > 0)
                foreach (var filter in filters)
                    urlBuilder.Append($"&filters[{filter.Key}]={filter.Value}");

            if (!string.IsNullOrWhiteSpace(query))
                urlBuilder.Append($"&query={query}");

            if (queryfields != null && queryfields.Length > 0)
                foreach (var field in queryfields)
                    urlBuilder.Append($"&queryfields[]={field}");

            var jsonResult = await _httpClient.GetStringAsync(urlBuilder.ToString());
            return JsonHelper.Deserialize<ApiHalResultWrapper>(jsonResult);
        }

        /// <summary>
        /// Retrieves details about the vehicle corresponding to the given <paramref name="kenteken"/>
        /// </summary>
        /// <param name="kenteken">The licenseplate in Dutch formatting i.e. 4-TFL-24</param>
        /// <returns>Object with all information about the vehicle, or null when a vehicle isn't found or any other error occurs</returns>
        public async Task<Voertuig> GetVoertuig(string kenteken)
        {
            var resultJson = await _httpClient.GetStringAsync($"api/voertuiggegevens/{kenteken}");
            return JsonHelper.Deserialize<Voertuig>(resultJson);
        }

        /// <summary>
        /// Retrieves suggestions, useable in a autocomplete searchbox for example.
        /// </summary>
        /// <param name="query">String to search for, wildcards not supported searches as: searchte*, spaces are stripped</param>
        /// <param name="size">How many results are returned. Default 10, maximum 25</param>
        /// <param name="fields">Which fields to include in the suggestion. Can only be </param>
        /// <returns>List of suggestions found with the given <paramref name="query"/> or null if an error occurs</returns>
        public async Task<VoertuigSuggestieResultaten> GetVoertuigSuggesties(string query, int size = 10, string[] fields = null)
        {
            if (size <= 0)
                throw new ArgumentOutOfRangeException("Parameter size cannot 0 or less");

            if (size > 25)
                throw new ArgumentOutOfRangeException("Parameter size cannot be more than 25");

            if (fields != null && fields.Except(new[] { "" }).Any())
                throw new ArgumentOutOfRangeException("Parameter fields can only contain values: handelsnaam, straat and dossiernummer");

            var urlBuilder = new StringBuilder($"suggest/voertuiggegevens/{query}?size={size}");

            if (fields != null && fields.Length > 0)
                foreach (var field in fields)
                    urlBuilder.Append($"&fields[]={field}");

            var resultJson = await _httpClient.GetStringAsync(urlBuilder.ToString());

            try
            {
                var resultSuggestions = JsonHelper.Deserialize<VoertuigSuggestieResultaten>(resultJson);

                return resultSuggestions;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Find addresses by specifying searchcriteria. Results will contains Dutch descriptions.
        /// Returned addresses may not include all property values depending on the <paramref name="fields"/> parameter.
        /// </summary>
        /// <param name="size">How many results should be returned, default 100</param>
        /// <param name="page">Which page (if more results are available than specified <paramref name="size"/>) should be retrieved</param>
        /// <param name="sort">On which field should be sorted</param>
        /// <param name="order">How should the results be ordered, "asc" or "desc"</param>
        /// <param name="fields">Fields to be included in the result. By default date of first allowance, licenseplate, commercial designation, brand, and vehicle type are included. See following link for all possible fields <see href="https://overheid.io/documentatie/voertuiggegevens#show"/></param>
        /// <param name="filters">Used for specifying filters. Key is the field, value is the value to filter on. I.e. key: kenteken, value: 02-JZX-1. See following link for all possible fields <see href="https://overheid.io/documentatie/voertuiggegevens#show"/></param>
        /// <param name="query">String to search for in <paramref name="queryfields"/>. * wildcard is available.</param>
        /// <param name="queryfields">Fields in which should be searched for <paramref name="query"/></param>
        /// <returns>List of results found including JSON+HAL metadata</returns>
        public async Task<ApiHalResultWrapper> ZoekAdres(int size = 100, int page = 1, string sort = "", string order = "desc", string[] fields = null, KeyValuePair<string, string>[] filters = null, string query = "", string[] queryfields = null)
        {
            if (size <= 0)
                throw new ArgumentOutOfRangeException("Parameter size cannot 0 or less");

            if (page <= 0)
                throw new ArgumentOutOfRangeException("Parameter page cannot 0 or less");

            // TODO maybe use an enum?
            if (order.ToLowerInvariant() != "desc" && order.ToLowerInvariant() != "asc")
                throw new ArgumentOutOfRangeException("Parameter order can only be asc or desc");

            var urlBuilder = new StringBuilder($"/api/bag?size={size}&page={page}");

            if (!string.IsNullOrWhiteSpace(sort))
                urlBuilder.Append($"&sort={sort}");

            if (!string.IsNullOrWhiteSpace(order))
                urlBuilder.Append($"&order={order}");

            if (fields != null && fields.Length > 0)
                foreach (var field in fields)
                    urlBuilder.Append($"&fields[]={field}");

            if (filters != null && filters.Length > 0)
                foreach (var filter in filters)
                    urlBuilder.Append($"&filters[{filter.Key}]={filter.Value}");

            if (!string.IsNullOrWhiteSpace(query))
                urlBuilder.Append($"&query={query}");

            if (queryfields != null && queryfields.Length > 0)
                foreach (var field in queryfields)
                    urlBuilder.Append($"&queryfields[]={field}");

            var jsonResult = await _httpClient.GetStringAsync(urlBuilder.ToString());
            return JsonHelper.Deserialize<ApiHalResultWrapper>(jsonResult);
        }

        /// <summary>
        /// Retrieves details about the address corresponding to the given <paramref name="id"/>
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Object with all information about the address, or null when a address isn't found or any other error occurs</returns>
        public async Task<Adres> GetAdres(string id)
        {
            var resultJson = await _httpClient.GetStringAsync($"api/bag/{id}");
            return JsonHelper.Deserialize<Adres>(resultJson);
        }

        /// <summary>
        /// Retrieves suggestions, useable in a autocomplete searchbox for example.
        /// </summary>
        /// <param name="query">String to search for, wildcards not supported searches as: searchte*, spaces are stripped</param>
        /// <param name="size">How many results are returned. Default 10, maximum 25</param>
        /// <param name="fields">Which fields to include in the suggestion. Can only be postcode (zip code), provincienaam (province), gemeentenaam (municipality), woonplaatsnaam (city) and openbareruimtenaam (public domain)</param>
        /// <returns>List of suggestions found with the given <paramref name="query"/> or null if an error occurs</returns>
        public async Task<AdresSuggestieResultaten> GetAdresSuggesties(string query, int size = 10, string[] fields = null)
        {
            if (size <= 0)
                throw new ArgumentOutOfRangeException("Parameter size cannot 0 or less");

            if (size > 25)
                throw new ArgumentOutOfRangeException("Parameter size cannot be more than 25");

            if (fields != null && fields.Except(new[] { "postcode", "provincienaam", "gemeentenaam", "woonplaatsnaam", "openbareruimtenaam" }).Any())
                throw new ArgumentOutOfRangeException("Parameter fields can only contain values: handelsnaam, straat and dossiernummer");

            var urlBuilder = new StringBuilder($"suggest/bag/{query}?size={size}");

            if (fields != null && fields.Length > 0)
                foreach (var field in fields)
                    urlBuilder.Append($"&fields[]={field}");

            var resultJson = await _httpClient.GetStringAsync(urlBuilder.ToString());

            try
            {
                var resultSuggestions = JsonHelper.Deserialize<AdresSuggestieResultaten>(resultJson);

                return resultSuggestions;
            }
            catch
            {
                return null;
            }
        }
    }
}
