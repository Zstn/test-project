namespace TestProject.Models.MoviesModel
{
    using System.Collections.Generic;

    using Microsoft.Owin.Logging;

    public class MovieMainPageViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int YearOfCreate { get; set; }

        public string NameOfDirector { get; set; }

        public string WhoAdded { get; set; }

        public string Description { get; set; }

        public string PathToPoster { get; set; }

        public IList<IMovie> Movies { get; set; }

        public int PagesCount { get; set; }

        public int CurrentPage { get; set; }

        public MovieMainPageViewModel() { }

        public MovieMainPageViewModel(IMovie movie)
        {
            this.Id = movie.Id;
            this.Name = movie.Name;
            this.YearOfCreate = movie.YearOfCreate;
            this.NameOfDirector = movie.NameOfDirector;
            this.WhoAdded = movie.WhoAdded;
            this.Description = movie.Description;
            this.PathToPoster = movie.PathToPoster;
        }
    }
}