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
