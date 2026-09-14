
using System.Text.Json;

namespace ExpenseMauiApp
{
    public partial class MainPage : ContentPage
    {
        private readonly HttpClient _httpClient;

        public MainPage()
        {
            InitializeComponent();

            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://192.168.1.64:7205/api/")
            };
        }


        private async void OnLoadClicked(object sender, EventArgs e)
        {
            try
            {
                var response = await _httpClient.GetAsync("transactions");

                if (!response.IsSuccessStatusCode)
                {
                    await DisplayAlertAsync("Error", "API failed", "OK");
                    return;
                }

                var json = await response.Content.ReadAsStringAsync();

                var data = JsonSerializer.Deserialize<List<CategoryPercentage>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                collectionView.ItemsSource = data;
            }
            catch (Exception ex)
            {
                await DisplayAlertAsync("Error", ex.Message, "OK");
            }
        }
    }
}
