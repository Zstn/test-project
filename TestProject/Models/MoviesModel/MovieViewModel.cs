namespace TestProject.Models.MoviesModel
{
    using System.ComponentModel;
    using System.Web;

    public class MovieViewModel
    {
        public int Id { get; set; }

        [DisplayName("Название")]
        public string Name { get; set; }

        [DisplayName("Год выпуска")]
        public int YearOfCreate { get; set; }

        [DisplayName("Режиссёр")]
        public string NameOfDirector { get; set; }

        [DisplayName("Кто добавил")]
        public string WhoAdded { get; set; }

        [DisplayName("Описание")]
        public string Description { get; set; }

        public HttpPostedFileBase Poster { get; set; }

        [DisplayName("Имя файла")]
        public string PathToPoster { get; set; }
    }
}