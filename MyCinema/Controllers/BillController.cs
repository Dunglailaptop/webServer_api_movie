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
            if ( bills != null ){
                
                  
               try
                 {
                    Bills bl = new Bills();
                    var dataAccount = _context.Accounts.Where(x=>x.Idusers == bills.Idaccount).SingleOrDefault();
                    if (dataAccount.points > bills.bills.Totalamount) {
                       Bill billspay = new Bill {
                         Idcinema = bills.bills.Idcinema,
                         Idinterest = bills.bills.Idinterest,
                         Iduser = bills.bills.Iduser,
                         Idmovie = bills.bills.Idmovie,
                         Vat = bills.bills.Vat,
                         Quantityticket = bills.bills.Quantityticket,
                         Totalamount = bills.bills.Totalamount,
                         Datebill = DateTime.Now,
                         Note = bills.bills.Note,
                         Statusbill = bills.bills.Statusbill,
                          Idvoucher = 10
                       };
                       _context.Bills.Add(billspay);
                       _context.SaveChanges();
                       bl.bills.Idbill = billspay.Idbill;
                       if (billspay.Idbill != 0) {
                            foreach (var item in bills.bills.Tickets) {
                                Ticket TC = new Ticket {
                                Idbill =  billspay.Idbill,
                                Idchair = item.Idchair,
                                Idinterest = bills.bills.Idinterest,
                                Pricechair = item.Pricechair
                                };
                            }
                       }
                      if (bills.combobill.Count != 0){
                         foreach(var item in bills.combobill) {
                              FoodComboWithBills foodcombowithbill = new FoodComboWithBills {
                                idcombo = item.idcombo,
                                Idbill = item.Idbill,
                              };
                              _context.FoodComboWithBills.Add(foodcombowithbill);
                              _context.SaveChanges();
                         };
                      };

                    }   else {

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

    public long? Idaccount {get;set;}
    public Bill bills {get;set;}

    public List<Ticket> ticket {get;set;}

    public List<FoodComboWithBills> combobill {get;set;} 
}

}
