using System.Runtime.InteropServices;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using MyCinema.Model;
using Newtonsoft.Json;
using System.Security.Cryptography.X509Certificates;

namespace webapiserver.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AutointerestMovieController : ControllerBase
    {
        private readonly CinemaContext _context;

        public AutointerestMovieController(CinemaContext context)
        {
            _context = context;
        }

 
public class MovieSchedule
{
    public string MovieName { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
}

public class RoomAuto
{
    public string Name { get; set; }
    public List<MovieSchedule> Schedule { get; set; } = new List<MovieSchedule>();
}

// public class CinemaScheduler
// {
//     public static List<RoomAuto> GenerateSchedule(List<string> movieList, List<string> roomList, int durationInMinutes)
//     {
//         List<RoomAuto> rooms = new List<RoomAuto>();

//         foreach (string roomName in roomList)
//         {
//             RoomAuto room = new RoomAuto { Name = roomName };

//             DateTime currentDate = DateTime.Now.Date;
//             DateTime endDate = currentDate.AddDays(7);

//             TimeSpan startHour = TimeSpan.FromHours(8); // Thời gian bắt đầu (8 giờ sáng)
//             TimeSpan endHour = TimeSpan.FromHours(22);  // Thời gian kết thúc (10 giờ tối)

//             DateTime currentTime = currentDate + startHour;

//             while (currentTime.TimeOfDay <= endHour && currentDate <= endDate)
//             {
//                 foreach (string movieName in movieList)
//                 {
//                     DateTime endTime = currentTime.AddMinutes(durationInMinutes);

//                     if (endTime.TimeOfDay <= endHour)
//                     {
//                         MovieSchedule newMovie = new MovieSchedule
//                         {
//                             MovieName = movieName,
//                             StartTime = currentTime,
//                             EndTime = endTime
//                         };

//                         room.Schedule.Add(newMovie);
//                     }
//                 }

//                 currentTime = currentDate + startHour; // Reset lại thời gian bắt đầu cho ngày tiếp theo
//                 currentDate = currentDate.AddDays(1); // Chuyển qua ngày tiếp theo
//             }

//             rooms.Add(room);
//         }

//         return rooms;
//     }
// }
//    public static List<RoomAuto> GenerateSchedule(List<string> movieList, List<string> roomList, int durationInMinutes)
//     {
//         List<RoomAuto> rooms = new List<RoomAuto>();

//         foreach (string roomName in roomList)
//         {
//             RoomAuto room = new RoomAuto { Name = roomName };

//             DateTime currentDate = DateTime.Now.Date;
//             DateTime endDate = currentDate.AddDays(7);

//             TimeSpan startHour = TimeSpan.FromHours(8); // Thời gian bắt đầu (8 giờ sáng)
//             TimeSpan endHour = TimeSpan.FromHours(22);  // Thời gian kết thúc (10 giờ tối)

//             DateTime currentTime = currentDate + startHour;

//             while (currentTime.TimeOfDay <= endHour && currentDate <= endDate)
//             {
//                 foreach (string movieName in movieList)
//                 {
//                     DateTime endTime = currentTime.AddMinutes(durationInMinutes);

//                     if (endTime.TimeOfDay <= endHour)
//                     {
//                         MovieSchedule newMovie = new MovieSchedule
//                         {
//                             MovieName = movieName,
//                             StartTime = currentTime,
//                             EndTime = endTime
//                         };

//                         room.Schedule.Add(newMovie);
//                     }
//                 }

//                 currentTime = currentDate + startHour; // Reset lại thời gian bắt đầu cho ngày tiếp theo
//                 currentDate = currentDate.AddDays(1); // Chuyển qua ngày tiếp theo
//             }

//             rooms.Add(room);
//         }

//         return rooms;
//     }

// public static List<RoomAuto> GenerateSchedule(List<string> movieList, List<string> roomList, int movieDurationMinutes)
// {
//     List<RoomAuto> rooms = new List<RoomAuto>();

//     foreach (string roomName in roomList)
//     {
//         RoomAuto room = new RoomAuto { Name = roomName };

//         DateTime currentDate = DateTime.Now.Date;
//         DateTime endDate = currentDate.AddDays(7);

//         while (currentDate <= endDate)
//         {
//             DateTime startTime = currentDate.Date + TimeSpan.FromHours(8); // 8:00 AM
//             DateTime endTime = currentDate.Date + TimeSpan.FromHours(22).Add(TimeSpan.FromMinutes(30)); // 10:30 PM

//             foreach (string movieName in movieList)
//             {
//                 DateTime movieStartTime = startTime;
//                 DateTime movieEndTime = movieStartTime.AddMinutes(movieDurationMinutes);

//                 while (movieEndTime <= endTime)
//                 {
//                     MovieSchedule newMovie = new MovieSchedule
//                     {
//                         MovieName = movieName,
//                         StartTime = movieStartTime,
//                         EndTime = movieEndTime
//                     };

//                     room.Schedule.Add(newMovie);

//                     movieStartTime = movieEndTime; // Next movie starts after current movie
//                     movieEndTime = movieStartTime.AddMinutes(movieDurationMinutes);
//                 }
//             }

          

//             // Move to the next day
//             currentDate = currentDate.AddDays(1);
//         }
//           rooms.Add(room);
//     }

//     return rooms;
// }

public static List<RoomAuto> GenerateSchedule(DateTime dateStart,DateTime dateEnd,List<string> movieList, List<string> roomList, int movieDurationMinutes)
{
    Random random = new Random();
    List<RoomAuto> rooms = new List<RoomAuto>();

    foreach (string roomName in roomList)
    {
        RoomAuto room = new RoomAuto { Name = roomName };

        DateTime currentDate = dateStart;
        DateTime endDate = dateEnd;

        while (currentDate <= endDate)
        {
            DateTime startTime = currentDate.Date + TimeSpan.FromHours(8); // 8:00 AM
            DateTime endTime = currentDate.Date + TimeSpan.FromHours(22).Add(TimeSpan.FromMinutes(30)); // 10:30 PM
          
            
            while (startTime.AddMinutes(movieDurationMinutes) <= endTime)
            {
                  string randomMovie = movieList[random.Next(movieList.Count)]; // Chọn ngẫu nhiên một bộ phim
                 string[] parts = randomMovie.Split('/'); // Tách chuỗi theo ký tự '/'
                int movieDurationMinutess = Convert.ToInt32(parts[1]); // Chuyển đổi phần tử thứ hai thành số nguyên
                DateTime movieEndTime = startTime.AddMinutes(movieDurationMinutess);

                MovieSchedule newMovie = new MovieSchedule
                {
                    MovieName = randomMovie,
                    StartTime = startTime,
                    EndTime = movieEndTime
                };

                room.Schedule.Add(newMovie);

                startTime = movieEndTime; // Next movie starts after current movie
            }

           
            // Move to the next day
            currentDate = currentDate.AddDays(1);
        }
         rooms.Add(room);

    }

    return rooms;
}

// public static List<RoomAuto> GenerateSchedule( DateTime startDate, DateTime endDate,List<string> movieList, List<string> roomList)
// {
//     List<RoomAuto> rooms = new List<RoomAuto>();
//     Random random = new Random();

//     int roomIndex = 0;

//     for (DateTime currentDate = startDate; currentDate <= endDate; currentDate = currentDate.AddDays(1))
//     {
//         RoomAuto room = new RoomAuto { Name = roomList[roomIndex % roomList.Count] }; // Lặp lại các rạp
//         roomIndex++;

//         DateTime currentTime = currentDate + TimeSpan.FromHours(8); // Bắt đầu từ 8h sáng

//         while (currentTime.TimeOfDay <= TimeSpan.FromHours(22.5)) // 22h30
//         {
//             string randomMovie = movieList[random.Next(movieList.Count)];
//             string[] parts = randomMovie.Split('/');
//             int movieDurationMinutes = Convert.ToInt32(parts[1]);

//             DateTime movieEndTime = currentTime.AddMinutes(movieDurationMinutes);
//             if (movieEndTime.TimeOfDay <= TimeSpan.FromHours(22.5))
//             {
//                 MovieSchedule newMovie = new MovieSchedule
//                 {
//                     MovieName = randomMovie,
//                     StartTime = currentTime,
//                     EndTime = movieEndTime
//                 };

//                 room.Schedule.Add(newMovie);
//                 currentTime = movieEndTime;
//             }
//             else
//             {
//                 break;
//             }
//         }

//         rooms.Add(room);
//     }

//     return rooms;
// }








[HttpPost("getlistvoucher")]
public IActionResult GetListVoucher([FromBody] MovieRoomLists datas)
{
    var successApiResponse = new ApiResponse();

    string token = Request.Headers["token"];
    string filterHeaderValue2 = Request.Headers["ProjectId"];
    string filterHeaderValue3 = Request.Headers["Method"];
    string expectedToken = ValidHeader.Token;
    string method = Convert.ToString(ValidHeader.MethodPost);
    string Pojectid = Convert.ToString(ValidHeader.Project_id);

    if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(filterHeaderValue2) || string.IsNullOrEmpty(filterHeaderValue3))
    {
        return BadRequest("Authorize header not found in the request.");
    }
    else
    {
        if (token != expectedToken || filterHeaderValue2 != Pojectid || filterHeaderValue3 != method)
        {
            return Unauthorized("Invalid token.");
        }
        else
        {
            try
            {
    //   List<string> movieList = new List<string>
    //     {
    //         "Movie 1",
    //         "Movie 2",
    //         "Movie 3"
    //     };
        

    //     List<string> roomList = new List<string>
    //     {
    //         "Room A",
    //         "Room B",
    //         "Room C"
    //     };

        int durationInMinutes = 120; // Độ dài mỗi buổi phim (2 giờ)

         List<string> movieList = datas.MovieList;
        List<string> roomList = datas.RoomList;
        DateTime dayStart = datas.dayStart;
DateTime dayEnd = datas.dayEnd;
       var schedule = GenerateSchedule(dayStart,dayEnd,movieList, roomList,durationInMinutes);

///////
// List<MovieSchedule> movieList = new List<MovieSchedule>
// {
//     new MovieSchedule { MovieName = "Movie 1", StartTime = DateTime.Now.AddHours(1), EndTime = DateTime.Now.AddHours(3) },
//     new MovieSchedule { MovieName = "Movie 2", StartTime = DateTime.Now.AddHours(4), EndTime = DateTime.Now.AddHours(6) }
// };

// List<string> roomList = new List<string>
// {
//     "Room A",
//     "Room B"
// };

// List<RoomAuto> schedule = GenerateSchedule(movieList, roomList,120);


                successApiResponse.Status = 200;
                successApiResponse.Message = "OK";
                successApiResponse.Data = schedule;
            }
            catch (Exception ex)
            {
                successApiResponse.Status = 500;
                successApiResponse.Message = "Internal Server Error";
                successApiResponse.Data = ex.Message;
            }
        }
    }
    return Ok(successApiResponse);
}

    }

  public class MovieRoomLists
{
    public DateTime dayStart {get;set;}
     public DateTime dayEnd {get;set;}
    public List<string> MovieList { get; set; }
    public List<string> RoomList { get; set; }
}
}
