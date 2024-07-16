using PostmanBlazor.Models.Types;
using System.Diagnostics;

namespace PostmanBlazor.Components.ViewModels.Postman
{
    public class PostmanBase : ViewModelBase
    {
        #region PROPERTIES

        /// <summary>
        /// The selected Method
        /// </summary>
        protected string SelectedMethod { get; set; } = "GET";

        /// <summary>
        /// The Request URL
        /// </summary>
        protected string RequestUrl { get; set; } = string.Empty;

        /// <summary>
        /// The Data
        /// </summary>
        protected string RequestData { get; set; } = string.Empty;

        /// <summary>
        /// The Response status
        /// </summary>
        protected string ResponseStatus { get; set; } = string.Empty;

        /// <summary>
        /// The Response JSON
        /// </summary>
        protected string ResponseJson { get; set; } = string.Empty;

        /// <summary>
        /// The Headers that we want to add in the Request
        /// </summary>
        protected List<KeyValuePair<string, string>> Headers = new();

        /// <summary>
        /// The key of Header
        /// </summary>
        protected string HeaderKey = string.Empty;

        /// <summary>
        /// The value of Header.
        /// </summary>
        protected string HeaderValue = string.Empty;

        /// <summary>
        /// Enumerable of the Postman Methods
        /// </summary>
        protected static IEnumerable<HttpMethodsPostman> HttpMethodsPostman { get; set; } =
        [
            new()
            {
                Color = "#147020",
                Type = "GET"
            },
            new()
            {
                Color = "#b58a12",
                Type = "POST"
            },
            new()
            {
                Color = "#2156a6",
                Type = "PUT"
            },
            new()
            {
                Color = "#b01a33",
                Type = "DELETE"
            },
        ];

        /// <summary>
        /// How many Time elapsed to answer the call.
        /// </summary>
        protected TimeSpan TimeElapsed { get; set; }

        #endregion

        /// <summary>
        /// On Initialized Async
        /// </summary>
        /// <returns></returns>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
        }

        /// <summary>
        /// Send the Request.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">Throws exception if the Operation doesn't exists.</exception>
        protected async Task SendRequest()
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            try
            {
                IsLoading = true;

                using var httpClient = HttpClientFactory.CreateClient("PostmanClient");

                HttpRequestMessage request = new(new HttpMethod(SelectedMethod), RequestUrl);

                foreach (var header in Headers)
                {
                    request.Headers.Add(header.Key, header.Value);
                }

                if (SelectedMethod == "POST" || SelectedMethod == "PUT")
                    request.Content = new StringContent(RequestData);

                HttpResponseMessage httpResponse = await httpClient.SendAsync(request);


                if (!httpResponse.IsSuccessStatusCode)
                    ResponseStatus = $"Error: {httpResponse.StatusCode}";

                ResponseJson = await httpResponse.Content.ReadAsStringAsync();
                ResponseJson = BeautifyJson(ResponseJson);

                ResponseStatus = $"{(int)httpResponse.StatusCode} {httpResponse.ReasonPhrase}";
            }
            catch (Exception ex)
            {
                ResponseStatus = "Error";
                ResponseJson = $"Error: {ex.Message}";
            }
            finally
            {
                stopwatch.Stop();
                TimeElapsed = stopwatch.Elapsed;

                IsLoading = false;
            }
        }

        /// <summary>
        /// Add Headers in HTTPClient.
        /// </summary>
        protected void AddHeader()
        {
            if (!string.IsNullOrWhiteSpace(HeaderKey) && !string.IsNullOrWhiteSpace(HeaderValue))
            {
                Headers.Add(new KeyValuePair<string, string>(HeaderKey, HeaderValue));
                HeaderKey = string.Empty;
                HeaderValue = string.Empty;
            }
        }

        /// <summary>
        /// Remove the Header.
        /// </summary>
        /// <param name="header">The header that you want to Remove.</param>
        protected void RemoveHeader(KeyValuePair<string, string> header) =>
            Headers.Remove(header);

    }
}