// using System;
// using System.Collections.Generic;



// public class Program
// {
//     public static List<MovieSchedule> ScheduleMovies(string[] movieList, TimeSpan maxDuration)
//     {
//         List<MovieSchedule> schedule = new List<MovieSchedule>();
//         DateTime currentTime = DateTime.Parse("2023-09-14 10:00"); // Thời gian bắt đầu chiếu

//         foreach (string movie in movieList)
//         {
//             DateTime endTime = currentTime.Add(maxDuration);
//             schedule.Add(new MovieSchedule(movie, currentTime, endTime));

//             currentTime = endTime.AddMinutes(15); // Cách nhau 15 phút cho chuẩn bị
//         }

//         return schedule;
//     }

//     public static void Main()
//     {
//         string[] movies = { "Movie A", "Movie B", "Movie C", "Movie D", "Movie E" };
//         TimeSpan maxDuration = TimeSpan.FromHours(2); // Thời gian tối đa mỗi bộ phim

//         List<MovieSchedule> schedule = ScheduleMovies(movies, maxDuration);

//         // In lịch chiếu đã được xếp
//         foreach (var item in schedule)
//         {
//             Console.WriteLine($"{item.MovieName} - {item.StartTime} - {item.EndTime}");
//         }
//     }
// }
using System.Resources;
using System;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using MyCinema.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;
// using MySql.Data.EntityFrameworkCore;

namespace webapiserver.Controllers;

[ApiController]
[Route("[controller]")]
public class AutointerestMovieController : ControllerBase
{
        private readonly CinemaContext _context;
        public AutointerestMovieController(CinemaContext context)
        {
            _context = context;
            
        }

 public static List<MovieSchedule> GenerateMovieSchedule(string[] movieList, TimeSpan duration, int numberOfShowtimes, List<DateTime> weekDays)
    {
        List<MovieSchedule> schedule = new List<MovieSchedule>();
        DateTime currentTime;

        foreach (DateTime day in weekDays)
        {
            currentTime = day;

            foreach (string movie in movieList)
            {
                for (int i = 0; i < numberOfShowtimes; i++)
                {
                    DateTime endTime = currentTime.Add(duration);
                    schedule.Add(new MovieSchedule(movie, currentTime, endTime));
                    currentTime = endTime.AddMinutes(15); // Cách nhau 15 phút cho chuẩn bị
                }

                currentTime = day; // Reset lại thời gian cho bộ phim tiếp theo
            }
        }

        return schedule;
    }

//  public static List<MovieSchedule> ScheduleMovies(string[] movieList, TimeSpan maxDuration)
//     {
//         List<MovieSchedule> schedule = new List<MovieSchedule>();
//         DateTime currentTime = DateTime.Parse("2023-09-14 10:00"); // Thời gian bắt đầu chiếu

//         foreach (string movie in movieList)
//         {
//             DateTime endTime = currentTime.Add(maxDuration);
//             schedule.Add(new MovieSchedule(movie, currentTime, endTime));

//             currentTime = endTime.AddMinutes(15); // Cách nhau 15 phút cho chuẩn bị
//         }

//         return schedule;
//     }

    public static void Main()
    {
       
    }

// API GET LIST VOUCHER
[HttpGet("getlistvoucher")]
public IActionResult getListVoucher()
{
    // khoi tao api response
    var successApiResponse = new ApiResponse();
    //header
       string token = Request.Headers["token"];
       string filterHeaderValue2 = Request.Headers["ProjectId"];
       string filterHeaderValue3 = Request.Headers["Method"];
       string expectedToken = ValidHeader.Token;
       string method =Convert.ToString(ValidHeader.MethodGet);
       string Pojectid = Convert.ToString(ValidHeader.Project_id);
    //check header
        if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(filterHeaderValue2) || string.IsNullOrEmpty(filterHeaderValue3))
        {
        // The "Authorize" header was not found in the request
           return BadRequest("Authorize header not found in the request.");
        }else {

            if (token != expectedToken || filterHeaderValue2 != Pojectid || filterHeaderValue3 != method)
          {
            return Unauthorized("Invalid token."); // Return an error response if the tokens don't match
          }else{
            // if (date != null && Idmovie != null){
                
                  
               try
                 {
                  string[] movies = { "Movie A", "Movie B", "Movie C", "Movie D", "Movie E" };
        TimeSpan duration = TimeSpan.FromHours(2); // Thời gian mỗi bộ phim
        int numberOfShowtimes = 5; // Số suất chiếu cho mỗi bộ phim
        List<DateTime> weekDays = new List<DateTime>();

        // Tạo danh sách các ngày trong tuần
        DateTime startDate = DateTime.Parse("2023-09-18"); // Thời gian bắt đầu tuần

        for (int i = 0; i < 7; i++)
        {
            weekDays.Add(startDate.AddDays(i));
        }

        List<MovieSchedule> schedule = GenerateMovieSchedule(movies, duration, numberOfShowtimes, weekDays);
      
       
                //       string sql = "call cinema.getListVoucher()";
                //    var dataget = _context.Vouchers.FromSqlRaw(sql).AsEnumerable().ToList();
                      successApiResponse.Status = 200;
                     successApiResponse.Message = "OK";
                     successApiResponse.Data = schedule;
                 }
                 catch (IndexOutOfRangeException ex)
                  {
    
                  }     
            // }else {
            //     return BadRequest("khong tim thay thong tin");
            // }
                 

           }

        }
 return Ok(successApiResponse);
}



}

public class MovieSchedule
{
    public string MovieName { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }

    public MovieSchedule(string name, DateTime start, DateTime end)
    {
        MovieName = name;
        StartTime = start;
        EndTime = end;
    }
}
