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
public class FoodComboController : ControllerBase
{
        private readonly CinemaContext _context;
        public FoodComboController(CinemaContext context)
        {
            _context = context;
            
        }

// API GET LIST VOUCHER
[HttpPost("CreateFoodCombo")]
public IActionResult CreateFoodCombo([FromBody] FoodComboNew combofood )
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
            // if (date != null && Idmovie != null){
                
                  
               try
                 {
                    FoodCombo combo = new FoodCombo {
                        nametittle = combofood.nametittle,
                        descriptions = combofood.discription,
                        priceCombo = combofood.priceCombo,
                        picture = combofood.picture
                    };
                    _context.Foodcombo.Add(combo);
                    _context.SaveChanges();
                      successApiResponse.Status = 200;
                     successApiResponse.Message = "OK";
                     successApiResponse.Data = combo;
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

// API GET LIST VOUCHER
[HttpPost("AddFoodComboFood")]
public IActionResult AddFoodComboFood([FromBody] FoodComboNew combofood )
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
            // if (date != null && Idmovie != null){
                
                  
               try
                 {
                List<FoodComboBill> data  = new List<FoodComboBill>();
                FoodCombo foodcomboo = new FoodCombo {
                  nametittle = combofood.nametittle,
                  descriptions = combofood.discription,
                  priceCombo = combofood.priceCombo,
                  picture = combofood.picture
                };
                   _context.Foodcombo.Add(foodcomboo);
                    _context.SaveChanges();
                    if (foodcomboo.idcombo != 0) {
                          foreach (var foods in combofood.foodCreates) {
                          FoodComboBill combo = new FoodComboBill {
                          idcombo = foodcomboo.idcombo,
                          Idfood = foods.Idfood
                          };
                          data.Add(combo);
                          _context.FoodComboBill.Add(combo);
                          _context.SaveChanges();
                          }
                    }
                 
                 
                      successApiResponse.Status = 200;
                     successApiResponse.Message = "OK";
                     successApiResponse.Data = data;
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

// API GET LIST VOUCHER
[HttpPost("CreateFoodNew")]
public IActionResult CreateFoodNew([FromBody] Foodnew combofood )
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
            // if (date != null && Idmovie != null){
                
                  
               try
                 {
              
                   Food fd = new Food {
                    Namefood = combofood.Namefood,
                    Quantityfood = combofood.Quantityfood,
                    Pricefood = combofood.Pricefood,
                    Picture = combofood.Picture,
                    Idcategoryfood = combofood.Idcategoryfood,
                    datecreate =  combofood.datecreate
                   };


                 
                    _context.Foods.Add(fd);
                    _context.SaveChanges();
                  
                 
                      successApiResponse.Status = 200;
                     successApiResponse.Message = "OK";
                     successApiResponse.Data = fd.Idfood;
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


[HttpGet("getListFood")]
public IActionResult getListFood(int Idcategoryfood)
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
              
                 

                    List<Food> data = new List<Food>();
                 if (Idcategoryfood == 0) {
                      data  = _context.Foods.ToList();
                 }else {
                   data  = _context.Foods.Where(x=>x.Idcategoryfood == Idcategoryfood).ToList();
                 }
                   
                 
                      successApiResponse.Status = 200;
                     successApiResponse.Message = "OK";
                     successApiResponse.Data = data;
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

[HttpGet("getListComboFood")]
public IActionResult getListComboFood()
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
              
                 


                 
                     var data  = _context.Foodcombo.ToList();
                 
                      successApiResponse.Status = 200;
                     successApiResponse.Message = "OK";
                     successApiResponse.Data = data;
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



public class FoodComboNew {

  public int idcombo {get;set;}
    public string nametittle {get;set;}
    public string discription {get;set;}

    public int priceCombo {get;set;}

    public string picture {get;set;} 

   public List<FoodCreate> foodCreates {get;set;} = new List<FoodCreate>();

}

public class FoodCreate {
    public int Idfood {get;set;}
}
public partial class Foodnew
{
    public int Idfood { get; set; }

    public string? Namefood { get; set; }

    public int? Quantityfood { get; set; }

    public string? Picture { get; set; }

    public int? Pricefood { get; set; }

    public int? Idcategoryfood { get; set; }

    public DateTime datecreate {get;set;}

  
}


}
