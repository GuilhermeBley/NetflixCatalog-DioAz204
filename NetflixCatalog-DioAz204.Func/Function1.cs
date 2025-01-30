using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NetflixCatalog_DioAz204.Func.Model;
using NetflixCatalog_DioAz204.Func.Services;
using System.Text.Json;

namespace NetflixCatalog_DioAz204.Func
{
    public class Function1
    {
        private readonly ILogger<Function1> _logger;
        private readonly ContainerStorageRepository _storageRepository;
        private readonly MovieRepository _movieRepository;

        public Function1(
            ILogger<Function1> logger, ContainerStorageRepository storageRepository, MovieRepository movieRepository)
        {
            _logger = logger;
            _storageRepository = storageRepository;
            _movieRepository = movieRepository;
        }

        [Function("Movie")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequest req)
        {

            try
            {
                var form = await req.ReadFormAsync();
                var file = form.Files["file"];
                var movieData = form["movie"];

                if (file == null || string.IsNullOrWhiteSpace(movieData))
                {
                    return new BadRequestObjectResult("Invalid request. Missing file or movie data.");
                }

                var movie = JsonSerializer.Deserialize<MovieModel>(movieData.ToString());

                if (movie == null)
                {
                    return new BadRequestObjectResult("Invalid movie data.");
                }

                using var stream = file.OpenReadStream();
                await _storageRepository.UploadOrReplaceFileAsync(file.FileName, stream, file.ContentType);

                movie.Id = Guid.NewGuid();
                movie.CreatedAt = DateTime.UtcNow;
                var createdMovie = await _movieRepository.CreateAsync(movie);

                return new OkObjectResult(new { Id = createdMovie.Id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing movie upload.");
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
