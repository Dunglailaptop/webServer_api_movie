using System.Net.NetworkInformation;
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
public class BillController : ControllerBase
{
        private readonly CinemaContext _context;
        public BillController(CinemaContext context)
        {
            _context = context;
            
        }



// API GET LIST CHAIR in room
[HttpPost("PaymentBill")]
public IActionResult PaymentBill([FromBody] Bills bills)
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
            if ( bills != null ){
                
                  
               try
                 {
                    Bills bl = new Bills();
                    var dataAccount = _context.Accounts.Where(x=>x.Idusers == bills.Iduser).SingleOrDefault();
                    if (dataAccount.points > bills.Totalamount) {
                       Bill billspay = new Bill {
                         Idcinema = bills.Idcinema,
                         Idinterest = bills.Idinterest,
                         Iduser = bills.Iduser,
                         Idmovie = bills.Idmovie,
                         Vat = bills.Vat,
                         Quantityticket = bills.Quantityticket,
                         Totalamount = bills.Totalamount,
                         Datebill = DateTime.Now,
                         Note = bills.Note,
                         Statusbill = bills.Statusbill,
                          Idvoucher = 10
                       };
                       _context.Bills.Add(billspay);
                       _context.SaveChanges();
                       bl.Idbill = billspay.Idbill;
                       if (billspay.Idbill != 0) {
                            foreach (var item in bills.ticket) {
                                Ticket TC = new Ticket {
                                Idbill =  billspay.Idbill,
                                Idchair = item.Idchair,
                                Idinterest = bills.Idinterest,
                                Pricechair = item.Pricechair
                                };
                                _context.Tickets.Add(TC);
                                _context.SaveChanges();
                            }
                       }
                      if (bills.combobill.Count != 0){
                         foreach(var item in bills.combobill) {
                              FoodComboWithBills foodcombowithbill = new FoodComboWithBills {
                                idcombo = item.idcombo,
                                Idbill = billspay.Idbill,
                              };
                              _context.FoodComboWithBills.Add(foodcombowithbill);
                              _context.SaveChanges();
                         };
                      };

                    }   else {
                      return BadRequest("So tien trong tai khoan cua ban khong du xin vui long kiem tra lai");
                    }  
                
                      successApiResponse.Status = 200;
                     successApiResponse.Message = "OK";
                     successApiResponse.Data = bl;
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

// API BILL PAYMENT FOODCOMBO
[HttpPost("postPaymentFoodComboBill")]
public IActionResult postPaymentFoodComboBill(paymentBill foodcombobill)
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
                  FoodCombillPayment foodcombo = new FoodCombillPayment();
                  
                  foodcombo.idFoodlistcombo = 21;
                  foodcombo.IdFoodcombo = 1;
                  foodcombo.datetimes = DateTime.Now;
                  foodcombo.numbers = foodcombobill.numbers;
                  foodcombo.total_price = foodcombobill.total_price;
                  foodcombo.iduser = foodcombobill.iduser;
                  foodcombo.idcinemas = foodcombobill.idcinemas;
                  foodcombo.statusbillfoodcombo = 0;
                     _context.FoodCombillPayment.Add(foodcombo);
                     _context.SaveChanges();
                     if (foodcombo.id != null) {
                       foreach (var item in  foodcombobill.foodComboBills)
                        {
                        ListFoodCombo list = new ListFoodCombo();
                        list.idfoodcombobill = foodcombo.id;
                        list.Idfoodcombo = item.Idfoodcombo;
                        _context.ListFoodCombo.Add(list);
                        _context.SaveChanges();       
                        }
                     } 
                      successApiResponse.Status = 200;
                     successApiResponse.Message = "OK";
                     successApiResponse.Data = foodcombo;
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


[HttpGet("getListBillinAccount")]
public IActionResult getListBillinAccount(long? iduser)
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
                        List<InfoBill> billarray = new List<InfoBill>();

                        var dataBill = _context.Bills
                            .Where(x => x.Iduser == iduser)
                            .ToList();

                        foreach (var item in dataBill)
                        {
                        InfoBill infoBills = new InfoBill();

                        var dataticket = _context.Tickets
                                    .Where(x => x.Idbill == item.Idbill)
                                    .ToList();

                        foreach (var itemchair in dataticket)
                        {
                        var datachair = _context.Chairs
                                        .Where(x => x.Idchair == itemchair.Idchair)
                                        .SingleOrDefault();

                        if (datachair != null)
                        {
                        var chairs = datachair.RowChar + datachair.NumberChair.ToString() + ",";
                         infoBills.numberchair += chairs;
                        }
                        }

                        var datainterest = _context.Cinemainterests
                                        .Where(x => x.Idinterest == item.Idinterest)
                                        .SingleOrDefault();
                        var dataMovie = _context.Movies.Where(x=>x.Idmovie == datainterest.Idmovie).SingleOrDefault();
                        if (datainterest != null)
                        {
                        infoBills.Datebill = item.Datebill ?? default(DateTime);
                        infoBills.Quantityticket = item.Quantityticket ?? 0;
                        infoBills.Totalamount = item.Totalamount ?? 0;
                        infoBills.Namemovie = dataMovie.Namemovie;
                        infoBills.poster = dataMovie.Poster;
                        infoBills.starttime = datainterest.Times;
                        infoBills.endtime = datainterest.TimeEnd;

                        billarray.Add(infoBills);
                        }
                        }

                        successApiResponse.Status = 200;
                        successApiResponse.Message = "OK";
                        successApiResponse.Data = billarray;

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

[HttpGet("getListAllBillTicket")]
public IActionResult getListAllBillTicket(long? idcinema,int status)
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
                        List<InfoBill> billarray = new List<InfoBill>();

                        var dataBill = _context.Bills.Where(x=>x.Idcinema == idcinema && x.Statusbill == status).ToList();

                        foreach (var item in dataBill)
                        {
                            InfoBill infoBills = new InfoBill();

                            var dataticket = _context.Tickets
                                        .Where(x => x.Idbill == item.Idbill)
                                        .ToList();

                        foreach (var itemchair in dataticket)
                        {
                                var datachair = _context.Chairs
                                                .Where(x => x.Idchair == itemchair.Idchair)
                                                .SingleOrDefault();

                                if (datachair != null)
                                {
                                      var chairs = datachair.RowChar + datachair.NumberChair.ToString() + ",";
                                      infoBills.numberchair += chairs;
                                }
                        }

                        var datainterest = _context.Cinemainterests
                                        .Where(x => x.Idinterest == item.Idinterest && x.Idcinema == idcinema)
                                        .SingleOrDefault();
                        var dataMovie = _context.Movies.Where(x=>x.Idmovie == datainterest.Idmovie).SingleOrDefault();
                        if (datainterest != null)
                        {
                              infoBills.Datebill = item.Datebill ?? default(DateTime);
                              infoBills.Quantityticket = item.Quantityticket ?? 0;
                              infoBills.Totalamount = item.Totalamount ?? 0;
                              infoBills.Namemovie = dataMovie.Namemovie;
                              infoBills.poster = dataMovie.Poster;
                              infoBills.starttime = datainterest.Times;
                              infoBills.endtime = datainterest.TimeEnd;
                               infoBills.statusbill = item.Statusbill;
                               infoBills.idbill = item.Idbill;
                              billarray.Add(infoBills);
                        }
                  }

                        successApiResponse.Status = 200;
                        successApiResponse.Message = "OK";
                        successApiResponse.Data = billarray;

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

[HttpGet("getListAllBillFoodCombo")]
public IActionResult getListAllBillFoodCombo(int idcinema,int status)
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
                    
                      List<InfoBillFoodCombo> combobill = new List<InfoBillFoodCombo>();
                      var dataBillFoodcombo = _context.FoodCombillPayment.Where(x => x.idcinemas == idcinema && x.statusbillfoodcombo == status).ToList();

                      foreach (var item in dataBillFoodcombo) {
                      InfoBillFoodCombo infobillfoodcombo = new InfoBillFoodCombo();
                      infobillfoodcombo.total_prices = item.total_price;
                      infobillfoodcombo.quantity = item.numbers;
                      infobillfoodcombo.time = item.datetimes;
                      infobillfoodcombo.id = item.id;
                      infobillfoodcombo.status = item.statusbillfoodcombo;
                      var dataGetInfoListFoodCombo = _context.ListFoodCombo.Where(x => x.idfoodcombobill == item.id).ToList();

                      foreach (var item2 in dataGetInfoListFoodCombo) {
                         var datagetFoodcomboonly = _context.Foodcombo.Where(x => x.idcombo == item2.Idfoodcombo).SingleOrDefault();
                         infobillfoodcombo.listfoodcombo.Add(datagetFoodcomboonly);
                        }
                         combobill.Add(infobillfoodcombo);
                      }
                      

                        successApiResponse.Status = 200;
                        successApiResponse.Message = "OK";
                        successApiResponse.Data = combobill;

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

[HttpGet("getListBillFoodinAccount")]
public IActionResult getListBillFoodinAccount(long? iduser)
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
                      InfoBillFoodCombo infobillfoodcombo = new InfoBillFoodCombo();
                      List<InfoBillFoodCombo> combobill = new List<InfoBillFoodCombo>();
                      var dataBillFoodcombo = _context.FoodCombillPayment.Where(x => x.iduser == iduser).ToList();

                      foreach (var item in dataBillFoodcombo) {
                      infobillfoodcombo.total_prices = item.total_price;
                      infobillfoodcombo.quantity = item.numbers;
                      infobillfoodcombo.time = item.datetimes;
                      infobillfoodcombo.id = item.id;
                      var dataGetInfoListFoodCombo = _context.ListFoodCombo.Where(x => x.idfoodcombobill == item.id).ToList();

                      foreach (var item2 in dataGetInfoListFoodCombo) {
                         var datagetFoodcomboonly = _context.Foodcombo.Where(x => x.idcombo == item2.Idfoodcombo).SingleOrDefault();
                         infobillfoodcombo.listfoodcombo.Add(datagetFoodcomboonly);
                        }
                      }
                       combobill.Add(infobillfoodcombo);

                        successApiResponse.Status = 200;
                        successApiResponse.Message = "OK";
                        successApiResponse.Data = combobill;

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

public class InfoBillFoodCombo {

  public int id {get;set;}
  
  public int total_prices {get;set;}

public int quantity {get;set;}
  
public DateTime time {get;set;}

public int status {get;set;}
public List<FoodCombo> listfoodcombo {get;set;} = new List<FoodCombo>();

}

public class InfoBill {

  public long? idbill {get;set;}
  public int? Totalamount {get;set;}
  public DateTime? Datebill {get;set;}

  public int? Quantityticket {get;set;}
  public int? statusbill  {get;set;}

  public string Namemovie {get;set;}

  public string poster {get;set;}

  public DateTime? starttime {get;set;}
  
  public DateTime? endtime {get;set;}

  public string numberchair {get;set;}

   

}

public class paymentBill {
   public int id { get; set; }
    public int  IdFoodcombo { get; set; }
 
    public int idFoodlistcombo { get; set; }
    
    public int numbers {get;set;}

    public DateTime datetimes {get;set;}

    public int total_price {get;set;}

    public long? iduser {get;set;}

    public long? idcinemas {get;set;}
    public int status {get;set;}
  public List<ListFoodCombo> foodComboBills {get;set;}
}

public class Bills {

    
     public long Idbill { get; set; }

    public long? Idmovie { get; set; }

    public int? Idvoucher { get; set; }

    public long? Iduser { get; set; }

    public int? Idinterest { get; set; }

    public long? Idcinema { get; set; }

    public int? Quantityticket { get; set; }

    public int? Vat { get; set; }

    public int? Totalamount { get; set; }

    public DateTime? Datebill { get; set; }

    public string? Note { get; set; }

    public int? Statusbill { get; set; }

    public List<ticketes> ticket {get;set;}

    public List<combobills> combobill {get;set;} 
}

public class ticketes {
    public long Idticket { get; set; }

    public int? Idchair { get; set; }

    public int? Idinterest { get; set; }

    public int? Pricechair { get; set; }

    public long? Idbill { get; set; }
}

public class combobills {
  public int IdBillfoodCombo {get;set;}

      public int idcombo {get;set;}

      public long? Idbill {get;set;}
}

}
