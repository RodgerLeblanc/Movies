using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Movies
{
    public partial class MainPage : ContentPage
    {
        private HttpClient _client;
        private string _baseUrl = "https://www.omdbapi.com/?apikey=[YOUR_API_KEY]&";

        public MainPage()
        {
            InitializeComponent();

            //No navigation bar on first screen
            NavigationPage.SetHasNavigationBar(this, false);

            SetNoResultsLabelIsVisible();
        }

        private void SetNoResultsLabelIsVisible()
        {
            noResultsLabel.IsVisible = listView.ItemsSource == null;
        }

        private async void searchBar_SearchButtonPressed(object sender, EventArgs e)
        {
            listView.ItemsSource = String.IsNullOrWhiteSpace(searchBar.Text) ?
                null :
                await GetMovies(searchBar.Text);

            SetNoResultsLabelIsVisible();
        }

        private async Task<IList<Movie>> GetMovies(string searchText)
        {
            if (_client == null)
                _client = new HttpClient();

            try
            {
                HttpResponseMessage responseMessage = await _client.GetAsync(_baseUrl + "s=" + searchText);
                string json = await responseMessage.Content.ReadAsStringAsync();
                OmdbResult result = JsonConvert.DeserializeObject<OmdbResult>(json);
                return result.Search;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (String.IsNullOrWhiteSpace(e.NewTextValue))
            {
                listView.ItemsSource = null;
                SetNoResultsLabelIsVisible();
            }
        }

        private async void listView_Refreshing(object sender, EventArgs e)
        {
            listView.ItemsSource = String.IsNullOrWhiteSpace(searchBar.Text) ?
                null :
                await GetMovies(searchBar.Text);

            SetNoResultsLabelIsVisible();

            listView.IsRefreshing = false;
        }

        private async void listView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (!(listView.SelectedItem is Movie movie))
                return;

            await Navigation.PushAsync(new MovieDetailPage(movie.imdbID));

            listView.SelectedItem = null;
        }
    }

    public class OmdbResult
    {
        public List<Movie> Search { get; set; }
        public string totalResults { get; set; }
        public string Response { get; set; }
    }

    public class Movie
    {
        public string Title { get; set; }
        public string Year { get; set; }
        public string imdbID { get; set; }
        public string Type { get; set; }
        public string Poster { get; set; }
    }
}
