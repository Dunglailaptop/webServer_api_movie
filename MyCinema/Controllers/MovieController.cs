using System.Security.Cryptography.X509Certificates;
using System.Runtime.InteropServices;
using System.IO.MemoryMappedFiles;
using System.Net.Cache;
using System;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using MyCinema.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Linq;


namespace webapiserver.Controllers;

[ApiController]
[Route("[controller]")]
public class MovieController : ControllerBase
{
        private readonly CinemaContext _context;
          private readonly IWebHostEnvironment _environment;

        public MovieController(CinemaContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

      

       



[HttpGet("ListMovie")]
public IActionResult getListMovies(int offset_value, int page_size,int Status)
{
    string sql = "CALL cinema.getListMovieNowShow(@p0, @p1, @p2)";
    var result = _context.Movies.FromSqlRaw(sql, offset_value, page_size, Status).ToList();
    var successApiResponse = new ApiResponse();

    // Retrieve specific request headers
    string token = Request.Headers["token"];
    string filterHeaderValue2 = Request.Headers["ProjectId"];
    string filterHeaderValue3 = Request.Headers["Method"];
    string expectedToken = ValidHeader.Token;
    string method = Convert.ToString(ValidHeader.MethodGet);
    string Pojectid = Convert.ToString(ValidHeader.Project_id);

    if (result == null || result.Count == 0) // Check if the result list is empty
    {
        var apiResponse = new ApiResponse
        {
            Status = 404,
            Message = "Movies not found.",
            Data = null
        };

        return NotFound(apiResponse);
    }
    else
    {
        if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(filterHeaderValue2) || string.IsNullOrEmpty(filterHeaderValue3))
        {
            // The "Authorize" header was not found in the request
            return BadRequest("Authorize header not found in the request.");
        }
        else
        {
            if (token != expectedToken || filterHeaderValue2 != Pojectid || filterHeaderValue3 != method)
            {
                return Unauthorized("Invalid token."); // Return an error response if the tokens don't match
            }
            else
            {
                var moviesList = new List<MovieItem>();

                foreach (var item in result)
                {
                    var movie = new MovieItem
                    {
                        MovieID = item.Idmovie,
                        Namemovie = item.Namemovie,
                        Poster = item.Poster,
                        Timeall = item.Timeall
                    };

                    moviesList.Add(movie);
                }

                successApiResponse.Status = 200;
                successApiResponse.Message = "OK";
                successApiResponse.Data = moviesList;
            }
        }
    }

    return Ok(successApiResponse);
}


[HttpGet("DetailMovie")]
public IActionResult getDetailMovies(long Idmovie)
{
    string sql = "CALL cinema.getDetailMovie(@p0)";
    var result = _context.Movies.FromSqlRaw(sql, Idmovie).AsEnumerable().FirstOrDefault();
    var successApiResponse = new ApiResponse();

    // Retrieve specific request headers
    string token = Request.Headers["token"];
    string filterHeaderValue2 = Request.Headers["ProjectId"];
    string filterHeaderValue3 = Request.Headers["Method"];
    string expectedToken = ValidHeader.Token;
    string method = Convert.ToString(ValidHeader.MethodGet);
    string Pojectid = Convert.ToString(ValidHeader.Project_id);

    if (result == null) // Check if the result list is empty
    {
        var apiResponse = new ApiResponse
        {
            Status = 404,
            Message = "Movies not found.",
            Data = null
        };

        return NotFound(apiResponse);
    }
    else
    {
        if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(filterHeaderValue2) || string.IsNullOrEmpty(filterHeaderValue3))
        {
            // The "Authorize" header was not found in the request
            return BadRequest("Authorize header not found in the request.");
        }
        else
        {
            if (token != expectedToken || filterHeaderValue2 != Pojectid || filterHeaderValue3 != method)
            {
                return Unauthorized("Invalid token."); // Return an error response if the tokens don't match
            }
            else
            {
                var moviedetail = new MovieItem();
                moviedetail.MovieID = result.Idmovie;
                moviedetail.Namemovie = result.Namemovie;
                moviedetail.Author = result.Author;
                moviedetail.Describes = result.Describes;
                moviedetail.Poster = result.Poster;
                moviedetail.Timeall = result.Timeall;
                moviedetail.Yearbirthday = result.Yearbirthday;
                var getvideofile = _context.Videousers.Where(x => x.Idvideo == result.Idvideo).FirstOrDefault();
                moviedetail.Videofile = getvideofile.Videofile;
        
                successApiResponse.Status = 200;
                successApiResponse.Message = "OK";
                successApiResponse.Data = moviedetail;
            }
        }
    }

    return Ok(successApiResponse);
}


// [HttpGet("ListMovie")]
// public IActionResult getListMovies(int offset_value,int page_size )
// {
//        string sql = "CALL cinema.getListMovieNowShow(@p0,@p1)";
//        var result[] = _context.Movies.FromSqlRaw(sql, offset_value, page_size).ToList();
//      var successApiResponse = new ApiResponse();
//          // Retrieve a specific request header
//        string token = Request.Headers["token"];
//        string filterHeaderValue2 = Request.Headers["ProjectId"];
//        string filterHeaderValue3 = Request.Headers["Method"];
//        string expectedToken = ValidHeader.Token;
//        string method =Convert.ToString(ValidHeader.MethodGet);
//        string Pojectid = Convert.ToString(ValidHeader.Project_id);
   

//    if (result == null)
//     {
//         var apiResponse = new ApiResponse
//         {
//             Status = 404,
//             Message = "Movie not found.",
//             Data = null
//         };

//         return NotFound(apiResponse);
//     }else {
    
//         if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(filterHeaderValue2) || string.IsNullOrEmpty(filterHeaderValue3))
//         {
//         // The "Authorize" header was not found in the request
         
//            return BadRequest("Authorize header not found in the request.");
//         }else {

//             if (token != expectedToken || filterHeaderValue2 != Pojectid || filterHeaderValue3 != method)
//           {
//             return Unauthorized("Invalid token."); // Return an error response if the tokens don't match
//           }else{
//                     var movies = new MovieItem();
//                     movies.MovieID = result.Idmovie;
//                     movies.Namemovie = result.Namemovie;
//                      movies.Poster = result.Poster;
//                      movies.Timeall = result.Timeall;
//                      successApiResponse.Status = 200;
//                      successApiResponse.Message = "OK";
//                      successApiResponse.Data = movies;
//            }

//         }

  
     
//     // var hashedPassword = HashPassword("123");
//     }


//     return Ok(successApiResponse);
// }

// // upload file Image
// [HttpPost("UploadImage")]
// public async Task<ActionResult> UploadImage()
// {
//     bool Results = false;
//     try
//     {
//         var _uploadedfiles = Request.Form.Files;
//         foreach (IFormFile source in _uploadedfiles)
//         {
//             string Filename = source.FileName;
//             string Filepath = GetFilePath(Filename);

//             if (!System.IO.Directory.Exists(Filepath))
//             {
//                 System.IO.Directory.CreateDirectory(Filepath);
//             }

//             string imagepath = Path.Combine(Filepath, Filename);
            
//             if (System.IO.File.Exists(imagepath))
//             {
//                 System.IO.File.Delete(imagepath);
//             }
//             using (FileStream stream = System.IO.File.Create(imagepath))
//             {
//                 await source.CopyToAsync(stream);
//                 Results = true;
//             }
//         }
//     }
//     catch (Exception ex)
//     {
//         // Handle or log the exception
//          Console.WriteLine($"Exception: {ex.Message}");
//     }
//     return Ok(Results);
// }

//   [NonAction]
//     private string GetFilePath(string ProductCode)
//     {
//         // Use Path.Combine to ensure platform-independent path construction
//    return this._environment.WebRootPath + "/Uploads/Movie/" + ProductCode;
//     }
//  [NonAction]
//     private string GetImagebyProduct(string productcode)
//     {
//         string ImageUrl = string.Empty;
//         string HostUrl = "https://localhost:7118/";
//         string Filepath = GetFilePath(productcode);
//         string Imagepath = Filepath;
//         if (!System.IO.File.Exists(Imagepath))
//         {
//             ImageUrl = HostUrl + "/Uploads/common/noimage.png";
//         }
//         else
//         {
//             ImageUrl = HostUrl + "/Uploads/Movie/" + productcode;
//         }
//         return ImageUrl;

//     }
//     //get file image
//     [HttpGet("getImage")]
//     public string getImage(){
//         return GetImagebyProduct("age.png");
//     }

     
public class MovieItem
{
    public long? MovieID { get; set; }
    public string Namemovie { get; set; }
    public string Author { get; set; }
    public DateTime? Yearbirthday { get; set; }
    public int? Timeall { get; set; }
    public string Describes { get; set; }
    public string Poster { get; set; }
    public int Statusshow { get; set; }
    public string Videofile {get; set;}

    public long? Idvideo {get; set;}

    public int? Idcategory {get; set;}

    public int? Idnation {get;set;}
}

    
}