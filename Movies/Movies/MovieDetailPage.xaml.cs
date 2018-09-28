
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Movies
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MovieDetailPage : ContentPage
    {
        private string _movieId;
        private HttpClient _client;
        private string _baseUrl = "https://www.omdbapi.com/?apikey=[YOUR_API_KEY]&plot=full&";

        public MovieDetailPage(string movieId)
        {
            InitializeComponent();
            _movieId = movieId;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            IsBusy = true;

            if (_client == null)
                _client = new HttpClient();

            try
            {
                HttpResponseMessage responseMessage = await _client.GetAsync(_baseUrl + "i=" + _movieId);
                string json = await responseMessage.Content.ReadAsStringAsync();
                MovieDetail movieDetail = JsonConvert.DeserializeObject<MovieDetail>(json);
                BindingContext = movieDetail;
            }
            catch (Exception ex)
            {
            }
            finally
            {
                IsBusy = false;
            }
        }
    }

    public class MovieDetail
    {
        public string Title { get; set; }
        public string Year { get; set; }
        public string Rated { get; set; }
        public string Released { get; set; }
        public string Runtime { get; set; }
        public string Genre { get; set; }
        public string Director { get; set; }
        public string Writer { get; set; }
        public string Actors { get; set; }
        public string Plot { get; set; }
        public string Language { get; set; }
        public string Country { get; set; }
        public string Awards { get; set; }
        public string Poster { get; set; }
        public List<Rating> Ratings { get; set; }
        public string Metascore { get; set; }
        public string imdbRating { get; set; }
        public string imdbVotes { get; set; }
        public string imdbID { get; set; }
        public string Type { get; set; }
        public string DVD { get; set; }
        public string BoxOffice { get; set; }
        public string Production { get; set; }
        public string Website { get; set; }
        public string Response { get; set; }
    }

    public class Rating
    {
        public string Source { get; set; }
        public string Value { get; set; }
    }
}