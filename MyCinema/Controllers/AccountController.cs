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

        // // GET: api/Products
        // [HttpGet]
        // // [Authorize]
        // public  IEnumerable<Account> GetAccount()
        // {
        //     return  _context.Accounts.ToList();
        // }


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


// [HttpGet("Logins")]
// public IActionResult getdata(string username, string password){

//        if (username == null && password == null)
//         {
//         return BadRequest("Account data is invalid.");
//         }

//     string sql = "CALL  cinema.login(@p0, @p1)";
//   var valuelogin =  _context.Database.ExecuteSqlRaw(sql, username, password);
//     _context.SaveChanges();

//       //  var account = _context.Accounts.FirstOrDefault(x => x.Username == username && x.Password == password);
//       //   var accountDto = new AccountDto();
//            var successApiResponse = new ApiResponse();

//          // Retrieve a specific request header
//        string token = Request.Headers["token"];
//        string filterHeaderValue2 = Request.Headers["ProjectId"];
//        string filterHeaderValue3 = Request.Headers["Method"];
//        string expectedToken = ValidHeader.Token;
//        string method =Convert.ToString(ValidHeader.MethodGet);
//        string Pojectid = Convert.ToString(ValidHeader.Project_id);
   

//    if (valuelogin == null)
//     {
//         var apiResponse = new ApiResponse
//         {
//             Status = 404,
//             Message = "Account not found.",
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
              
//                      successApiResponse.Status = 200;
//                      successApiResponse.Message = "OK";
//                      successApiResponse.Data = valuelogin;
//                       Console.WriteLine(valuelogin.ToString());

//            }

//         }

  
     
//     // var hashedPassword = HashPassword("123");
//     }


//     return Ok(successApiResponse);
// }

[HttpGet("getrole")]
public IActionResult getdatarole(){
    var account = _context.Roles.ToList();
    var succapi = new ApiResponse();
    succapi.Message = "200";
    succapi.Data = account;
    return Ok(succapi);
}

       
[HttpGet("login")]
public IActionResult Login(string username, string password)
{
     var account = _context.Accounts.FirstOrDefault(x => x.Username == username && x.Password == password);
        var accountDto = new AccountDto();
           var successApiResponse = new ApiResponse();

         // Retrieve a specific request header
       string token = Request.Headers["token"];
       string filterHeaderValue2 = Request.Headers["ProjectId"];
       string filterHeaderValue3 = Request.Headers["Method"];
       string expectedToken = ValidHeader.Token;
       string method =Convert.ToString(ValidHeader.MethodGet);
       string Pojectid = Convert.ToString(ValidHeader.Project_id);
   

   if (account == null)
    {
        var apiResponse = new ApiResponse
        {
            Status = 404,
            Message = "Account not found.",
            Data = null
        };

        return NotFound(apiResponse);
    }else {
    
        if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(filterHeaderValue2) || string.IsNullOrEmpty(filterHeaderValue3))
        {
        // The "Authorize" header was not found in the request
         
           return BadRequest("Authorize header not found in the request.");
        }else {

            if (token != expectedToken || filterHeaderValue2 != Pojectid || filterHeaderValue3 != method)
          {
            return Unauthorized("Invalid token."); // Return an error response if the tokens don't match
          }else{
              
             
                 
                    accountDto.EmployeeId = account.Idusers;
                    accountDto.Username = account.Username;
               
               var employeeInfo = _context.Users.SingleOrDefault(x=>x.Idusers == accountDto.EmployeeId);

                    successApiResponse.Status = 200;
                     successApiResponse.Message = "OK";
                      successApiResponse.Data = employeeInfo;
                     

           }

        }

  
     
    // var hashedPassword = HashPassword("123");
    }


    return Ok(successApiResponse);
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

  [HttpPost]
public IActionResult InsertAccount([FromBody] Account account)
{
    if (account == null)
    {
        return BadRequest("Account data is invalid.");
    }

    string sql = "CALL  Cinema.InsertAccount(@p0, @p1, @p2)";
    _context.Database.ExecuteSqlRaw(sql, account.Username, account.Password, account.Idusers);
    _context.SaveChanges();

    return Ok("Account inserted successfully.");
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
}


}