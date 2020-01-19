namespace TestProject.Models.MoviesModel
{
    public interface IMovie
    {
        int Id { get; set; }

        string Name { get; set; }

        int YearOfCreate { get; set; }

        string NameOfDirector { get; set; }

        string WhoAdded { get; set; }

        string Description { get; set; }

        string PathToPoster { get; set; }
    }
}