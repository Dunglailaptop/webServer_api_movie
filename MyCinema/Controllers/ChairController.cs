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
public class ChairController : ControllerBase
{
        private readonly CinemaContext _context;
        public ChairController(CinemaContext context)
        {
            _context = context;
            
        }

// API GET LIST CHAIR
[HttpGet("getlistchair")]
public IActionResult getListVoucher(long Idroom,long idcinema,int idinterest)
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
            if (idcinema != null && Idroom != null && idinterest != null){
                
                  
               try
                 {
                      string sql = "call cinema.getListChairRoomCinema(@p0,@p1,@p2)";
                   var dataget = _context.CHAIRS.FromSqlRaw(sql,Idroom,idcinema,idinterest).AsEnumerable().ToList();
                      successApiResponse.Status = 200;
                     successApiResponse.Message = "OK";
                     successApiResponse.Data = dataget;
                 }
                 catch (IndexOutOfRangeException ex)
                  {
    
                  }     
            }else {
                return BadRequest("khong tim thay thong tin");
            }
                 

           }

        }
 return Ok(successApiResponse);
}

// API GET LIST CHAIR in room
[HttpGet("getlistchairRoom")]
public IActionResult getlistchairRoom(long Idroom)
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
            if ( Idroom != null ){
                
                  
               try
                 {
                     
                   var dataget = _context.Chairs.Where(x=> x.Idroom == Idroom).ToList();
                      successApiResponse.Status = 200;
                     successApiResponse.Message = "OK";
                     successApiResponse.Data = dataget;
                 }
                 catch (IndexOutOfRangeException ex)
                  {
    
                  }     
            }else {
                return BadRequest("khong tim thay thong tin");
            }
                 

           }

        }
 return Ok(successApiResponse);
}

// API CREATE ROOM AND CHAIR
[HttpPost("CreateChairInRoom")]
public IActionResult CreateChairInRoom([FromBody] RoomREsponse DATA)
{
    // khoi tao api response
    var successApiResponse = new ApiResponse();
    //header
       string token = Request.Headers["token"];
       string filterHeaderValue2 = Request.Headers["ProjectId"];
       string filterHeaderValue3 = Request.Headers["Method"];
       string expectedToken = ValidHeader.Token;
       string method =Convert.ToString(ValidHeader.MethodPost);
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
            if ( DATA != null ){
                
                  
               try
                 {
                  string text = "";
                     Room room = new Room {
                        Allchair = DATA.allschair,
                        Nameroom = DATA.nameroom,
                        Idcinema = DATA.Idcinema,
                        Chairinrow = DATA.numberChair,
                        Statusroom = 1
                     };
                   _context.Rooms.Add(room);
                   _context.SaveChanges();
                   long newRoom = room.Idroom;
                   if (newRoom > 0){ 
                        string sql = "CALL cinema.insert_values(@p0,@p1,@p2)";
                        _context.Database.ExecuteSqlRaw(sql,DATA.numberChair,newRoom,DATA.allschair);
                        _context.SaveChanges();
                        text = "Thêm phòng thành công";
                   }else 
                   {
                     text = "Thêm phòng thất bại";
                   }
                   
                      successApiResponse.Status = 200;
                     successApiResponse.Message = "OK";
                     successApiResponse.Data = text;
                 }
                 catch (IndexOutOfRangeException ex)
                  {
    
                  }     
            }else {
                return BadRequest("khong tim thay thong tin");
            }
                 

           }

        }
 return Ok(successApiResponse);
}

public class RoomREsponse {
   public long Idcinema {get;set;}
   public int numberChair   {get;set;}
      public int allschair   {get;set;}
         public string nameroom   {get;set;}
}

}
