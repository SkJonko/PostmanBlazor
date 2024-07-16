using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Text.Json;

namespace PostmanBlazor.Components.ViewModels
{
    public class ViewModelBase : ComponentBase
    {

        #region PROPERTIES

        private bool _isLoading = false;

        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                if (_isLoading != value)
                {
                    _isLoading = value;

                    StateHasChanged();
                }
            }
        }

        #endregion


        #region INJECT

        [Inject]
        public IHttpClientFactory HttpClientFactory { get; set; }

        [Inject]
        public IJSRuntime JS { get; set; }

        #endregion


        /// <summary>
        /// Beutify the Response of JSON.
        /// </summary>
        /// <param name="json">The JSON response.</param>
        /// <returns></returns>
        protected string BeautifyJson(string json)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(json))
                    return string.Empty;

                var jsonObject = JsonSerializer.Deserialize<object>(json);
                return JsonSerializer.Serialize(jsonObject, new JsonSerializerOptions { WriteIndented = true });
            }
            catch (JsonException)
            {
                return "Invalid JSON format";
            }
        }
    }
}