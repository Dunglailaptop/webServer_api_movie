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
public class AccountController : ControllerBase
{
        private readonly CinemaContext _context;
        public AccountController(CinemaContext context)
        {
            _context = context;
            
        }
// API DANG NHAP VAO TAI KHOAN **
[HttpGet("Logins")]
public IActionResult getdata(string username, string password)
{
    if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
    {
        return BadRequest("Account data is invalid.");
    }

    string sql = "CALL cinema.login(@p0, @p1)";
    var result = _context.Users.FromSqlRaw(sql, username, password).AsEnumerable().FirstOrDefault();

    var successApiResponse = new ApiResponse();
    
    string token = Request.Headers["token"];
    string filterHeaderValue2 = Request.Headers["ProjectId"];
    string filterHeaderValue3 = Request.Headers["Method"];
    string expectedToken = ValidHeader.Token;
    string method = Convert.ToString(ValidHeader.MethodGet);
    string ProjectId = Convert.ToString(ValidHeader.Project_id);

    if (result == null)
    {
        var apiResponse = new ApiResponse
        {
            Status = 404,
            Message = "Account not found.",
            Data = null
        };

        return NotFound(apiResponse);
    }
    else
    {
        if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(filterHeaderValue2) || string.IsNullOrEmpty(filterHeaderValue3))
        {
            return BadRequest("Authorize header not found in the request.");
        }
        else
        {
            if (token != expectedToken || filterHeaderValue2 != ProjectId || filterHeaderValue3 != method)
            {
                return Unauthorized("Invalid token.");
            }
            else
            {

                  var userDto = new UserDto();

        // Set properties based on the retrieved user entity
                  userDto.Idusers = result.Idusers;
                  userDto.Fullname = result.Fullname;
                  userDto.Email = result.Email;
                  userDto.Phone = result.Phone;
                  userDto.Birthday = result.Birthday.ToString();
                  userDto.Idrole = result.Idrole;
                  userDto.Avatar = result.Avatar;

                successApiResponse.Status = 200;
                successApiResponse.Message = "OK";
                successApiResponse.Data = userDto;

                return Ok(successApiResponse);
            }
        }
    }
}

[HttpGet("getrole")]
public IActionResult getdatarole(){
    var account = _context.Roles.ToList();
    var succapi = new ApiResponse();
    succapi.Status = 200;
    succapi.Message = "OK";
    succapi.Data = account;
    return Ok(succapi);
}

[HttpPost("CREATEACCOUNT")]
public IActionResult CreateAccount(Account account){
  // try {
  //     if(string.IsNullOrEmpty(account.Username)){
  //       throw new Exception("Username is empty");
  //     }
  //     if (string.IsNullOrEmpty(account.Password)){
  //       throw new Exception("password is empty");
  //     }
      
      
      

    
  // }catch(Exception ex) {
  //      throw ex;
  // }
   _context.Accounts.Add(account);
   _context.SaveChanges();
   
  return Ok("tao thanh cong");
}
// API CAP NHAT THONG TIN TAI KHOAN *
[HttpPost("UpdateAccount")]
public IActionResult UpdateAccount([FromBody] UserDto account)
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
            if (account.Fullname != null && account.Email != null && account.Idusers != null && account.Phone != null){
                   string sql = "CALL cinema.updateAccount(@p0,@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8)";
                   _context.Database.ExecuteSqlRaw(sql, account.Idusers,account.Fullname,account.Email,account.Phone,account.Birthday,account.Avatar,account.gender,account.address,account.Idrole);
                   _context.SaveChanges();
                    successApiResponse.Status = 200;
                     successApiResponse.Message = "OK";
                     successApiResponse.Data = "cậpp nhậtt tàii khoảnn thànhh công";
                      
            }else {
                return BadRequest("Vui long nhap day du thong tin tai khoan");
            }
                 

           }

        }
 return Ok(successApiResponse);
}
// API GET INFO ACCOUNT
[HttpGet("getInfoAccount")]
public IActionResult UpdateAccount(long id)
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
            if (id != null){
                   string sql = "CALL cinema.getInfoAccount(@p0)";
                   var dataget = _context.Users.FromSqlRaw(sql, id).AsEnumerable().FirstOrDefault();
                   UserDto us = new UserDto();
                   us.Idusers = dataget.Idusers;
                   us.Email = dataget.Email;
                   us.Birthday = dataget.Birthday.ToString();
                   us.Fullname = dataget.Fullname;
                   us.Phone = dataget.Phone;
                   us.Idrole = dataget.Idrole;
                   us.gender = dataget.gender;
                   us.address = dataget.address;
                   var role = _context.Roles.SingleOrDefault(x => x.Idrole == dataget.Idrole);
                   us.Avatar = dataget.Avatar;
                   us.idrolename = role.IdName;
                    successApiResponse.Status = 200;
                     successApiResponse.Message = "OK";
                     successApiResponse.Data = us;
                      
            }else {
                return BadRequest("khong tim thay thong tin tai khoan");
            }
                 

           }

        }
 return Ok(successApiResponse);
}



public class AccountDto
{
    // Define the properties you want to return in the response
    public long? EmployeeId { get; set; }
    public string Username { get; set; }
    // Other properties you want to expose
}
public class UserDto
{
    public long? Idusers { get; set; }
    public string Fullname { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string Birthday { get; set; }
    public int? Idrole { get; set; }
    public string Avatar { get; set; }
    public int? gender {get;set;}
    public string? address {get;set;}
    public string? idrolename {get;set;}
}


}
