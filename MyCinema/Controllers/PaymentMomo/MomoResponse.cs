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
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Text;
using System.Security.Cryptography;
using Microsoft.Extensions.Options;
using RestSharp;
// using MySql.Data.EntityFrameworkCore;

namespace webapiserver.Controllers;

[ApiController]
[Route("[controller]")]
public class PaymentMomoController : ControllerBase
{
        private readonly CinemaContext _context;
        private readonly VnpayConfig vnpayConfig;
       
         private readonly IHttpContextAccessor httpContextAccessor;
        public PaymentMomoController(CinemaContext context,IOptions<VnpayConfig> VnpayConfigs,IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
          
            this.vnpayConfig  = VnpayConfigs.Value;
            this.httpContextAccessor = httpContextAccessor;
        }
             

      


   [HttpGet("MakePayment")]
    public  IActionResult MakePayment()
    {
        // string apiUrl = "https://test-payment.momo.vn/gw_payment/transactionProcessor";
        // string partnerCode = "MOMOBKUN20180529";
        // string accessKey = "klm05TvNBzhg7h7j";
        // string secretKey = "at67qH6mk8w5Y1nAyMoYKMWACiEi2bsa";
        // string returnUrl = "http://your_return_url.com";
        // string merchantRefId = Guid.NewGuid().ToString(); // Unique identifier for the order
        // long amount = 10000; // Replace with the actual amount

        // string signature = GenerateSignature(partnerCode, merchantRefId, amount, returnUrl, secretKey);

        // using (HttpClient client = new HttpClient())
        // {
        //     var requestContent = new PaymentMomoRequest
        //     {
        //         PartnerCode = partnerCode,
        //         requestType = "captureWallet",
        //         orderId = merchantRefId,
        //         amount  = amount,
        //         orderInfo = "Order description here",
        //         signature = signature
        //     };

        //     string jsonContent = Newtonsoft.Json.JsonConvert.SerializeObject(requestContent);
        //     var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        //     HttpResponseMessage response = await client.PostAsync(apiUrl, content);

        //     if (response.IsSuccessStatusCode)
        //     {
        //         string responseContent = await response.Content.ReadAsStringAsync();

        //         // Process the response content as needed.

        //         return Content(responseContent);
        //     }
        //     else
        //     {
        //         return Content("Error: Unable to process payment.");
        //     }


        // }
         
        var successApiResponse = new ApiResponse();
        successApiResponse.Status = 200;
        successApiResponse.Message = "OK";

        successApiResponse.Data = "none";

        return Ok(successApiResponse);
    }

    private string GenerateSignature(string partnerCode, string merchantRefId, long amount, string returnUrl, string secretKey)
    {
        string rawSignature = $"{partnerCode}{merchantRefId}{amount}{returnUrl}{secretKey}";
        byte[] bytes = System.Text.Encoding.UTF8.GetBytes(rawSignature);
        byte[] hashBytes;

        using (var sha256 = new System.Security.Cryptography.SHA256Managed())
        {
            hashBytes = sha256.ComputeHash(bytes);
        }

        string signature = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
        return signature;
    }

public class responsePayment {
    public int idorder {get;set;}

    public decimal amount {get;set;}

    public string urlpayment {get;set;}
}

    // API GET LIST VOUCHER
[HttpPost("CreateLinkVNPAY")]
public IActionResult CreateLinkVNPAY([FromBody] responsePayment responsePayments)
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
                    var reponsepaymentvnpay = new responsePayment();
                    var orderid = DateTime.Now.Ticks;
                    var note =   "Thanh toan don hang:" +orderid.ToString();
                        var vnpayRequest = new VnpayPayRequest(vnpayConfig.Version,
                       vnpayConfig.TmnCode,DateTime.Now, 
                        GetIpAddress(),responsePayments.amount,
                      "VND","other",
                     note,vnpayConfig.ReturnUrl,responsePayments.idorder.ToString());
                      var paymentUrlvn = string.Empty;
                      paymentUrlvn = vnpayRequest.GetLink(vnpayConfig.PaymentUrl,vnpayConfig.HashSecret);
                      reponsepaymentvnpay.amount = responsePayments.amount;
                      reponsepaymentvnpay.idorder = responsePayments.idorder;
                      reponsepaymentvnpay.urlpayment = paymentUrlvn; 
                   var baseurl = vnpayConfig.PaymentUrl+"?vnp_Amount=1806000&vnp_Command=pay&vnp_CreateDate="+DateTime.Now+"&vnp_CurrCode=VND&vnp_IpAddr="+IpAddress+"&vnp_Locale=vn&vnp_OrderInfo=Thanh+toan+don+hang+%3A5&vnp_OrderType=other&vnp_ReturnUrl=https%3A%2F%2Fdomainmerchant.vn%2FReturnUrl&vnp_TmnCode="+vnpayConfig.TmnCode+"&vnp_TxnRef=5&vnp_Version="+vnpayConfig.Version;
                      successApiResponse.Status = 200;
                     successApiResponse.Message = "OK";

                     successApiResponse.Data = reponsepaymentvnpay;
                
                
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



        public  string? IpAddress => httpContextAccessor?.HttpContext?.Connection?.LocalIpAddress?.ToString();

            public string GetIpAddress()
            {
            string ipAddress;
            try
            {
            ipAddress = httpContextAccessor.HttpContext.Request.Headers["X-Forwarded-For"];

            if (string.IsNullOrEmpty(ipAddress) || (ipAddress.ToLower() == "unknown") || ipAddress.Length > 45)
            ipAddress = httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
            }
            catch (Exception ex)
            {
            ipAddress = "Invalid IP:" + ex.Message;
            }

            return ipAddress;
            }
public static string GenerateVnpTxnRef()
{
    // Sử dụng ticks của thời gian hiện tại để tạo giá trị duy nhất
    long ticks = DateTime.Now.Ticks;
    return "VNP" + ticks.ToString();
}


// // API GET LIST VOUCHER
// [HttpPost("createLinkMomo")]
// public IActionResult createLinkMomo([FromBody] )
// {
//     // khoi tao api response
//     var successApiResponse = new ApiResponse();
//     //header
//        string token = Request.Headers["token"];
//        string filterHeaderValue2 = Request.Headers["ProjectId"];
//        string filterHeaderValue3 = Request.Headers["Method"];
//        string expectedToken = ValidHeader.Token;
//        string method =Convert.ToString(ValidHeader.MethodGet);
//        string Pojectid = Convert.ToString(ValidHeader.Project_id);
//     //check header
//         if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(filterHeaderValue2) || string.IsNullOrEmpty(filterHeaderValue3))
//         {
//         // The "Authorize" header was not found in the request
//            return BadRequest("Authorize header not found in the request.");
//         }else {

//             if (token != expectedToken || filterHeaderValue2 != Pojectid || filterHeaderValue3 != method)
//           {
//             return Unauthorized("Invalid token."); // Return an error response if the tokens don't match
//           }else{
//             // if (date != null && Idmovie != null){
                
                  
//                try
//                  {
//                      var momoOneTimePayRequest = new PaymentMomoRequest(momoConfig.PartnerCode,)
                   
//                       successApiResponse.Status = 200;
//                      successApiResponse.Message = "OK";
//                      successApiResponse.Data = dataget;
//                  }
//                  catch (IndexOutOfRangeException ex)
//                   {
    
//                   }     
//             // }else {
//             //     return BadRequest("khong tim thay thong tin");
//             // }
                 

//            }

//         }
//  return Ok(successApiResponse);
// }







}
