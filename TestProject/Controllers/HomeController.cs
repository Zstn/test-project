using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TestProject.Controllers
{
    using System.Data.Entity;
    using System.Threading.Tasks;

    using Microsoft.Owin.Logging;

    using TestProject.Models;
    using TestProject.Models.MoviesModel;

    public class HomeController : Controller
    {
        private IReadOnlyList<IMovie> _allMovies;

        private const int MoviesToDisplay = 12;

        public HomeController()
        {
        }

        [HttpGet]
        public async Task<ActionResult> MainPage(int page=1)
        {
            await this.UpdateNews();
            var viewModel = new MovieMainPageViewModel();
            viewModel.Movies = this.GetMoviesAsync(page).Result.ToList();
            viewModel.PagesCount = this.GetPagesCount().Result;
            viewModel.CurrentPage = page;
            return View(viewModel);
        }

        [HttpGet]
        public ActionResult MovieViewPage(int id=0)
        {
            if (id == 0)
            {
                return this.HttpNotFound("Страница не найдена");
            }
            var movie = this.GetMovieById(id);
            if (movie.Result != null)
            {
                var model = new MovieMainPageViewModel(movie.Result);
                return this.View(model);
            }

            return this.HttpNotFound("Страница не найдена");
        }

        private async Task UpdateNews()
        {
            using (var dbContext = new ApplicationDbContext())
            {
                try
                {
                    var tempMovies = await dbContext.Movies.AsNoTracking().Cast<Movie>().ToListAsync();
                    if (tempMovies.Count != 0)
                    {
                        this._allMovies = tempMovies;
                        this._allMovies = this._allMovies.OrderByDescending(m => m.Id).ToList();
                    }
                }
                catch (Exception e)
                {
                    ViewBag.Message = "ERROR:" + e.Message.ToString();
                }
            }
        }

        public async Task<int> GetPagesCount()
        {
            return this._allMovies != null && this._allMovies.Count > 0 ? (int)Math.Ceiling((double)this._allMovies.Count / MoviesToDisplay) : 1;
        }

        public async Task<ICollection<IMovie>> GetMoviesAsync(int page)
        {
            if (this._allMovies != null && this._allMovies.Count > 0)
            {
                return this._allMovies.Skip((page - 1) * MoviesToDisplay).Take(MoviesToDisplay).ToList();
            }

            return new List<IMovie>();
        }

        //Returns Movie object by selected Id
        public async Task<IMovie> GetMovieById(int id)
        {
            var movie = new Movie();
            using (var dbContext = new ApplicationDbContext())
            {
                var tempMovie = dbContext.Movies.Find(id);
                if (tempMovie != null)
                {
                    movie.Name = tempMovie.Name;
                    movie.Description = tempMovie.Description;
                    movie.YearOfCreate = tempMovie.YearOfCreate;
                    movie.NameOfDirector = tempMovie.NameOfDirector;
                    movie.PathToPoster = tempMovie.PathToPoster;
                    movie.Id = tempMovie.Id;
                    movie.WhoAdded = tempMovie.WhoAdded;
                }
            }
            return movie;
        }
    }
}