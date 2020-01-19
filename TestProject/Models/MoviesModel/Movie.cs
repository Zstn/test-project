namespace TestProject.Models.MoviesModel
{
    using System.ComponentModel.DataAnnotations;

    public class Movie : IMovie
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(100)]
        [Required]
        public string Name { get; set; }

        [Required]
        [Range(1, 9999, ErrorMessage = "Год должен быть в промежутке от 1 до 9999")]
        public int YearOfCreate { get; set; }

        [Required]
        [MaxLength(100)]
        public string NameOfDirector { get; set; }

        [Required]
        [MaxLength(50)]
        public string WhoAdded { get; set; }

        [Required]
        public string Description { get; set; }

        public string PathToPoster { get; set; }

        public Movie() { }

        public Movie(
            string name,
            int yearOfCreate,
            string nameOfDirector,
            string whoAdded,
            string description,
            string pathToPoster)
        {
            this.Name = name;
            this.YearOfCreate = yearOfCreate;
            this.NameOfDirector = nameOfDirector;
            this.WhoAdded = whoAdded;
            this.Description = description;
            this.PathToPoster = pathToPoster;
        }
    }
}