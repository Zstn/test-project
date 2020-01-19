using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TestProject.Controllers
{
    using System.Data.Entity;
    using System.IO;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;

    using Microsoft.Owin.Logging;

    using TestProject.Models;
    using TestProject.Models.MoviesModel;

    [Authorize]
    public class MoviesController : Controller
    {
        private const string PathToPosters = "\\images\\posters\\";

        private const string DefaultPathToPoster = "\\images\\standart_poster\\images.png";

        private static readonly Regex CheckImageRegex = new Regex(@"((\.jpg)|(\.jpeg)|(\.png)|(\.gif))$");

        public MoviesController() { }

        public async Task<ActionResult> MovieConfigPage(int id=0)
        {
            var model = GetMovieById(id);
            return this.View(model.Result);
        }


        [HttpPost]
        public async Task<ActionResult> AddAsync(MovieViewModel viewModel)
        {
            if (!string.IsNullOrEmpty(viewModel.Name) && !string.IsNullOrEmpty(viewModel.NameOfDirector)
                                                      && !string.IsNullOrEmpty(viewModel.Description)
                                                      && !string.IsNullOrEmpty(viewModel.WhoAdded)
                                                      && viewModel.YearOfCreate > 0 || viewModel.YearOfCreate <= 9999 
                                                      && viewModel.Poster != null)
            {
                await this.AddMovie(viewModel);
            }

            return this.RedirectToAction("MovieConfigPage");
        }

        [HttpPost]
        public async Task<ActionResult> EditAsync(MovieViewModel viewModel)
        {
            if (!string.IsNullOrEmpty(viewModel.Name) && !string.IsNullOrEmpty(viewModel.NameOfDirector)
                                                      && !string.IsNullOrEmpty(viewModel.Description)
                                                      && !string.IsNullOrEmpty(viewModel.WhoAdded)
                                                      && viewModel.YearOfCreate > 0 || viewModel.YearOfCreate <= 9999)
            {

                await this.ChangeMovie(viewModel);
            }

            return this.RedirectToAction("MovieConfigPage");

        }

        public async Task AddMovie(MovieViewModel model)
        {

            if (string.IsNullOrEmpty(model.Name))
            {
                return;
            }

            if (string.IsNullOrEmpty(model.WhoAdded))
            {
                return;
            }

            if (string.IsNullOrEmpty(model.NameOfDirector))
            {
                return;
            }

            if (string.IsNullOrEmpty(model.Description))
            {
                return;
            }

            if (model.YearOfCreate <= 0 || model.YearOfCreate > 9999)
            {
                return;
            }

            if (!CheckImageRegex.IsMatch(model.Poster.FileName))
            {
                return;
            }

            var pathToMovie = DefaultPathToPoster;

            if (model.Poster != null && model.Poster.FileName != String.Empty)
            {
                pathToMovie = Path.Combine(PathToPosters + model.Name + "\\" + model.Poster.FileName);
            }

            using (var dbContext = new ApplicationDbContext())
            {
                try
                {
                    dbContext.Movies.Add(new Movie(model.Name, model.YearOfCreate, model.NameOfDirector, model.WhoAdded, model.Description, pathToMovie));
                    AddFileToServer(model.Poster, model.Name);
                    await dbContext.SaveChangesAsync();
                }
                catch (Exception e)
                {
                    ViewBag.Message = "ERROR:" + e.Message.ToString();
                }
            }

        }

        public async Task ChangeMovie(MovieViewModel movie)
        {
            if (movie == null)
            {
                return;
            }

            if (string.IsNullOrEmpty(movie.Name))
            {
                return;
            }

            if (string.IsNullOrEmpty(movie.Description))
            {
                return;
            }

            if (string.IsNullOrEmpty(movie.NameOfDirector))
            {
                return;
            }

            if (string.IsNullOrEmpty(movie.WhoAdded))
            {
                return;
            }

            if (movie.YearOfCreate <= 0 || movie.YearOfCreate > 9999)
            {
                return;
            }

            using (var dbContext = new ApplicationDbContext())
            {
                try
                {
                    var oldMovie = await dbContext.Movies.SingleOrDefaultAsync(m => m.Id == movie.Id);
                    if (oldMovie != null)
                    {
                        oldMovie.Name = movie.Name;
                        oldMovie.YearOfCreate = movie.YearOfCreate;
                        oldMovie.Description = movie.Description;
                        oldMovie.NameOfDirector = movie.NameOfDirector;

                        var currentPath = Server.MapPath(oldMovie.PathToPoster);

                        if (movie.PathToPoster != null && System.IO.File.Exists(currentPath) 
                                                       && Server.MapPath(oldMovie.PathToPoster) != Server.MapPath(DefaultPathToPoster) 
                                                       && Server.MapPath(oldMovie.PathToPoster) != Server.MapPath(movie.PathToPoster))
                        {
                            try
                            {
                                System.IO.File.Delete(currentPath);
                                this.AddFileToServer(movie.Poster, movie.Name);
                                oldMovie.PathToPoster = movie.PathToPoster;
                            }
                            catch (Exception e)
                            {
                                ViewBag.Message = "ERROR:" + e.Message.ToString();
                            }
                        }

                        await dbContext.SaveChangesAsync();
                    }
                }
                catch (Exception e)
                {
                    ViewBag.Message = "ERROR:" + e.Message.ToString();
                }
            }

        }

        private void AddFileToServer(HttpPostedFileBase poster, string fileName)
        {
            if (poster != null && poster.ContentLength > 0)
            {
                try
                {
                    var path = Path.Combine(Server.MapPath(PathToPosters), fileName + "\\",
                                               poster.FileName);
                    var pathToFolder = Path.Combine(Server.MapPath(PathToPosters) + fileName);
                    if (!Directory.Exists(pathToFolder))
                    {
                        Directory.CreateDirectory(pathToFolder);
                    }
                    using (var filestream = new FileStream(path, FileMode.Create))
                    {
                        poster.InputStream.CopyTo(filestream);
                    }
                }
                catch (Exception e)
                {
                    ViewBag.Message = "ERROR:" + e.Message.ToString();
                }
            }
        }

        public async Task<MovieViewModel> GetMovieById(int id)
        {
            var movie = new MovieViewModel();
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