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
public class ReportController : ControllerBase
{
        private readonly CinemaContext _context;
        public ReportController(CinemaContext context)
        {
            _context = context;
            
        }

// API GET LIST VOUCHER
[HttpGet("ReportTicketALL")]
public IActionResult ReportTicketALL(int report_type)
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
            ReportAlltotal totalall = new ReportAlltotal();
                    //  List<reportTicketnew> reports = new List<reportTicketnew>();

    if (report_type == 1)
    {
        var date = "2023-10-06";
        var datenow = DateTime.Now.Date; // Gets the date of yesterday at 00:00:00
        var dateend = DateTime.Now.Date.AddHours(23).AddMinutes(59).AddSeconds(59); // Gets the end of today (23:59:59)

        var sqlfoodcombobill = "SELECT datetimes, IFNULL(total_price, 0) as totals,statusbillfoodcombo FROM cinema.FoodCombillPayment WHERE datetimes >= '"+datenow.ToString("yyyy-MM-dd HH:mm:ss")+"' AND datetimes <= '"+dateend.ToString("yyyy-MM-dd HH:mm:ss")+"'";
        var sqltest = "SELECT bil.Datebill, bil.Totalamount as Total, foodb.idBillfoodCombo,bil.statusbill, IFNULL(fcb.priceCombo, 0) as pricefood FROM cinema.Bill bil LEFT JOIN cinema.FoodComboWithBills foodb ON foodb.Idbill = bil.Idbill LEFT JOIN cinema.Foodcombo fcb ON fcb.Idcombo = foodb.idcombo WHERE bil.Datebill >= '" + datenow.ToString("yyyy-MM-dd HH:mm:ss") + "' AND bil.Datebill <= '" + dateend.ToString("yyyy-MM-dd HH:mm:ss") + "'";

        var dataget = _context.ReportTickets.FromSqlRaw(sqltest).AsEnumerable().ToList();
        var datafoodcombo = _context.ReportFoodCombos.FromSqlRaw(sqlfoodcombobill).AsEnumerable().ToList();
        List<DateTime> timeSlots = GetTimeSlotsInCurrentDay(1);
        
        

        foreach (DateTime timeSlot in timeSlots)
        {
            

            string only1 = timeSlot.ToString("HH");

            var reportitem = new reportTicketnew();
            reportitem.datebill = timeSlot.ToString("yyyy-MM-dd HH:mm:ss");

            foreach (var item in dataget)
            {
                if (item.statusbill == 1){
                    totalall.totalbill += item.Total;
                }else {
                      totalall.totalbillwait += item.Total;
                }
               totalall.totalfoodwithbill += item.pricefood;
               
                string only2 = item.Datebill.ToString("HH");

                if (only1 == only2 && item.statusbill == 1)
                {
                    reportitem.total += item.Total;

                    if (item.pricefood != null)
                    {
                        reportitem.priceCombo += item.pricefood;
                    }
                    else
                    {
                        reportitem.priceCombo = 0;
                    }
                }else if (only1 == only2 && item.statusbill == 0) {
                      reportitem.totalwait += item.Total;

                    if (item.pricefood != null)
                    {
                        reportitem.priceCombo += item.pricefood;
                    }
                    else
                    {
                        reportitem.priceCombo = 0;
                    }
                }
            }

            foreach (var item in datafoodcombo)
            {
                 if (item.statusbillfoodcombo == 1){
                      totalall.totalfood += item.totals;
                 }else {
                      totalall.totalfoodwait += item.totals;
                 }

                string only2 = item.datetimes.ToString("HH");

                if (only1 == only2 && item.statusbillfoodcombo == 1)
                {
                    reportitem.totalpricefoodcomboorder += item.totals;
                }else if (only1 == only2 && item.statusbillfoodcombo == 0) {
                    reportitem.totalpricefoodcomboorderwait += item.totals;
                }
            }

           totalall.reportTicketnews.Add(reportitem);
        }
    } else if (report_type == 9 ) {
         var date = "2023-10-06";
         var datenow = DateTime.Now.Date.AddDays(-1); // Gets the date of yesterday at 00:00:00
        var dateend = DateTime.Now.Date.AddDays(-1).AddHours(23).AddMinutes(59).AddSeconds(59); // Gets the end of today (23:59:59)

        // var datenow =  DateTime.Now.AddDays(-1) + " 00:00:00";
        // var dateend = DateTime.Now + " 23:59:59";

        var sqlfoodcombobill = "SELECT datetimes, IFNULL(total_price, 0) as totals,statusbillfoodcombo FROM cinema.FoodCombillPayment WHERE datetimes >= '"+datenow.ToString("yyyy-MM-dd HH:mm:ss")+"' AND datetimes <= '"+dateend.ToString("yyyy-MM-dd HH:mm:ss")+"'";
        var sqltest = "SELECT bil.Datebill, bil.Totalamount as Total, foodb.idBillfoodCombo,bil.statusbill, IFNULL(fcb.priceCombo, 0) as pricefood FROM cinema.Bill bil LEFT JOIN cinema.FoodComboWithBills foodb ON foodb.Idbill = bil.Idbill LEFT JOIN cinema.Foodcombo fcb ON fcb.Idcombo = foodb.idcombo WHERE bil.Datebill >= '" + datenow.ToString("yyyy-MM-dd HH:mm:ss") + "' AND bil.Datebill <= '" + dateend.ToString("yyyy-MM-dd HH:mm:ss") + "'";

        var dataget = _context.ReportTickets.FromSqlRaw(sqltest).AsEnumerable().ToList();
        var datafoodcombo = _context.ReportFoodCombos.FromSqlRaw(sqlfoodcombobill).AsEnumerable().ToList();
        List<DateTime> timeSlots = GetTimeSlotsInCurrentDay(9);
        
        

        foreach (DateTime timeSlot in timeSlots)
        {
            

            string only1 = timeSlot.ToString("HH");

            var reportitem = new reportTicketnew();
            reportitem.datebill = timeSlot.ToString("yyyy-MM-dd HH:mm:ss");

            foreach (var item in dataget)
            {
                if (item.statusbill == 1){
                    totalall.totalbill += item.Total;
                }else {
                      totalall.totalbillwait += item.Total;
                }
               totalall.totalfoodwithbill += item.pricefood;
               
                string only2 = item.Datebill.ToString("HH");

                if (only1 == only2 && item.statusbill == 1)
                {
                    reportitem.total += item.Total;

                    if (item.pricefood != null)
                    {
                        reportitem.priceCombo += item.pricefood;
                    }
                    else
                    {
                        reportitem.priceCombo = 0;
                    }
                }else if (only1 == only2 && item.statusbill == 0) {
                      reportitem.totalwait += item.Total;

                    if (item.pricefood != null)
                    {
                        reportitem.priceCombo += item.pricefood;
                    }
                    else
                    {
                        reportitem.priceCombo = 0;
                    }
                }
            }

            foreach (var item in datafoodcombo)
            {
                 if (item.statusbillfoodcombo == 1){
                      totalall.totalfood += item.totals;
                 }else {
                      totalall.totalfoodwait += item.totals;
                 }

                string only2 = item.datetimes.ToString("HH");

                if (only1 == only2 && item.statusbillfoodcombo == 1)
                {
                    reportitem.totalpricefoodcomboorder += item.totals;
                }else if (only1 == only2 && item.statusbillfoodcombo == 0) {
                    reportitem.totalpricefoodcomboorderwait += item.totals;
                }
            }

           totalall.reportTicketnews.Add(reportitem);
        }
    }else if(report_type == 2) // lay thu 2 toi thu 6
     {
         var date = "2023-10-06";
         var datenow = DateTime.Now.Date.AddDays(-1); // Gets the date of yesterday at 00:00:00
        var dateend = DateTime.Now.Date.AddDays(-1).AddHours(23).AddMinutes(59).AddSeconds(59); // Gets the end of today (23:59:59)

            DateTime currentDate = DateTime.Now.Date;

            DateTime firstDayOfWeek = currentDate;
            while (firstDayOfWeek.DayOfWeek != DayOfWeek.Monday)
            {
            firstDayOfWeek = firstDayOfWeek.AddDays(-1);
            }

            DateTime lastDayOfWeek = firstDayOfWeek.AddDays(6).AddHours(23).AddMinutes(59).AddSeconds(59);


        // var datenow =  DateTime.Now.AddDays(-1) + " 00:00:00";
        // var dateend = DateTime.Now + " 23:59:59";

        var sqlfoodcombobill = "SELECT datetimes, IFNULL(total_price, 0) as totals,statusbillfoodcombo FROM cinema.FoodCombillPayment WHERE datetimes >= '"+firstDayOfWeek.ToString("yyyy-MM-dd HH:mm:ss")+"' AND datetimes <= '"+lastDayOfWeek.ToString("yyyy-MM-dd HH:mm:ss")+"'";
        var sqltest = "SELECT bil.Datebill, bil.Totalamount as Total, foodb.idBillfoodCombo,bil.statusbill, IFNULL(fcb.priceCombo, 0) as pricefood FROM cinema.Bill bil LEFT JOIN cinema.FoodComboWithBills foodb ON foodb.Idbill = bil.Idbill LEFT JOIN cinema.Foodcombo fcb ON fcb.Idcombo = foodb.idcombo WHERE bil.Datebill >= '" + firstDayOfWeek.ToString("yyyy-MM-dd HH:mm:ss")+ "' AND bil.Datebill <= '" + lastDayOfWeek.ToString("yyyy-MM-dd HH:mm:ss")+ "'";

        var dataget = _context.ReportTickets.FromSqlRaw(sqltest).AsEnumerable().ToList();
        var datafoodcombo = _context.ReportFoodCombos.FromSqlRaw(sqlfoodcombobill).AsEnumerable().ToList();
        List<DateTime> timeSlots = GetDaysOfWeek();
        
        

        foreach (DateTime timeSlot in timeSlots)
        {
            

            string only1 = timeSlot.ToString("yyyy-MM-dd");

            var reportitem = new reportTicketnew();
            reportitem.datebill = timeSlot.ToString("yyyy-MM-dd HH:mm:ss");

            foreach (var item in dataget)
            {
                if (item.statusbill == 1){
                    totalall.totalbill += item.Total;
                }else {
                      totalall.totalbillwait += item.Total;
                }
               totalall.totalfoodwithbill += item.pricefood;
               
                string only2 = item.Datebill.ToString("yyyy-MM-dd");

                if (only1 == only2 && item.statusbill == 1)
                {
                    reportitem.total += item.Total;

                    if (item.pricefood != null)
                    {
                        reportitem.priceCombo += item.pricefood;
                    }
                    else
                    {
                        reportitem.priceCombo = 0;
                    }
                }else if (only1 == only2 && item.statusbill == 0) {
                      reportitem.totalwait += item.Total;

                    if (item.pricefood != null)
                    {
                        reportitem.priceCombo += item.pricefood;
                    }
                    else
                    {
                        reportitem.priceCombo = 0;
                    }
                }
            }

            foreach (var item in datafoodcombo)
            {
                 if (item.statusbillfoodcombo == 1){
                      totalall.totalfood += item.totals;
                 }else {
                      totalall.totalfoodwait += item.totals;
                 }

                string only2 = item.datetimes.ToString("yyyy-MM-dd");

                if (only1 == only2 && item.statusbillfoodcombo == 1)
                {
                    reportitem.totalpricefoodcomboorder += item.totals;
                }else if (only1 == only2 && item.statusbillfoodcombo == 0) {
                    reportitem.totalpricefoodcomboorderwait += item.totals;
                }
            }

           totalall.reportTicketnews.Add(reportitem);
        }
    }else if(report_type == 3) // lay theo thang
     {
          
       

         DateTime currentDate = DateTime.Now.Date;
         DateTime firstDayOfMonth = new DateTime(currentDate.Year, currentDate.Month, 1);
         DateTime lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1).AddHours(23).AddMinutes(59).AddSeconds(59);



        // var datenow =  DateTime.Now.AddDays(-1) + " 00:00:00";
        // var dateend = DateTime.Now + " 23:59:59";

        var sqlfoodcombobill = "SELECT datetimes, IFNULL(total_price, 0) as totals,statusbillfoodcombo FROM cinema.FoodCombillPayment WHERE datetimes >= '"+firstDayOfMonth.ToString("yyyy-MM-dd HH:mm:ss")+"' AND datetimes <= '"+lastDayOfMonth.ToString("yyyy-MM-dd HH:mm:ss")+"'";
        var sqltest = "SELECT bil.Datebill, bil.Totalamount as Total, foodb.idBillfoodCombo,bil.statusbill, IFNULL(fcb.priceCombo, 0) as pricefood FROM cinema.Bill bil LEFT JOIN cinema.FoodComboWithBills foodb ON foodb.Idbill = bil.Idbill LEFT JOIN cinema.Foodcombo fcb ON fcb.Idcombo = foodb.idcombo WHERE bil.Datebill >= '" + firstDayOfMonth.ToString("yyyy-MM-dd HH:mm:ss")+ "' AND bil.Datebill <= '" + lastDayOfMonth.ToString("yyyy-MM-dd HH:mm:ss")+ "'";

        var dataget = _context.ReportTickets.FromSqlRaw(sqltest).AsEnumerable().ToList();
        var datafoodcombo = _context.ReportFoodCombos.FromSqlRaw(sqlfoodcombobill).AsEnumerable().ToList();
        List<DateTime> timeSlots = GetDaysInCurrentMonth();
        
        

        foreach (DateTime timeSlot in timeSlots)
        {
            

            string only1 = timeSlot.ToString("yyyy-MM-dd");

            var reportitem = new reportTicketnew();
            reportitem.datebill = timeSlot.ToString("yyyy-MM-dd HH:mm:ss");

            foreach (var item in dataget)
            {
                if (item.statusbill == 1){
                    totalall.totalbill += item.Total;
                }else {
                      totalall.totalbillwait += item.Total;
                }
               totalall.totalfoodwithbill += item.pricefood;
               
                string only2 = item.Datebill.ToString("yyyy-MM-dd");

                if (only1 == only2 && item.statusbill == 1)
                {
                    reportitem.total += item.Total;

                    if (item.pricefood != null)
                    {
                        reportitem.priceCombo += item.pricefood;
                    }
                    else
                    {
                        reportitem.priceCombo = 0;
                    }
                }else if (only1 == only2 && item.statusbill == 0) {
                      reportitem.totalwait += item.Total;

                    if (item.pricefood != null)
                    {
                        reportitem.priceCombo += item.pricefood;
                    }
                    else
                    {
                        reportitem.priceCombo = 0;
                    }
                }
            }

            foreach (var item in datafoodcombo)
            {
                 if (item.statusbillfoodcombo == 1){
                      totalall.totalfood += item.totals;
                 }else {
                      totalall.totalfoodwait += item.totals;
                 }

                string only2 = item.datetimes.ToString("yyyy-MM-dd");

                if (only1 == only2 && item.statusbillfoodcombo == 1)
                {
                    reportitem.totalpricefoodcomboorder += item.totals;
                }else if (only1 == only2 && item.statusbillfoodcombo == 0) {
                    reportitem.totalpricefoodcomboorderwait += item.totals;
                }
            }

           totalall.reportTicketnews.Add(reportitem);
        }
    }else if(report_type == 10) {
         

       DateTime currentDate = DateTime.Now.Date;

// Lấy ngày đầu tiên của tháng hiện tại
DateTime firstDayOfCurrentMonth = new DateTime(currentDate.Year, currentDate.Month, 1);

// Lấy ngày cuối cùng của tháng trước
DateTime lastDayOfLastMonth = firstDayOfCurrentMonth.AddDays(-1).AddSeconds(-1);

// Lấy ngày đầu tiên của tháng trước
DateTime firstDayOfLastMonth = new DateTime(lastDayOfLastMonth.Year, lastDayOfLastMonth.Month, 1);



        // var datenow =  DateTime.Now.AddDays(-1) + " 00:00:00";
        // var dateend = DateTime.Now + " 23:59:59";

        var sqlfoodcombobill = "SELECT datetimes, IFNULL(total_price, 0) as totals,statusbillfoodcombo FROM cinema.FoodCombillPayment WHERE datetimes >= '"+firstDayOfLastMonth.ToString("yyyy-MM-dd HH:mm:ss")+"' AND datetimes <= '"+lastDayOfLastMonth.ToString("yyyy-MM-dd HH:mm:ss")+"'";
        var sqltest = "SELECT bil.Datebill, bil.Totalamount as Total, foodb.idBillfoodCombo,bil.statusbill, IFNULL(fcb.priceCombo, 0) as pricefood FROM cinema.Bill bil LEFT JOIN cinema.FoodComboWithBills foodb ON foodb.Idbill = bil.Idbill LEFT JOIN cinema.Foodcombo fcb ON fcb.Idcombo = foodb.idcombo WHERE bil.Datebill >= '" + firstDayOfLastMonth.ToString("yyyy-MM-dd HH:mm:ss")+ "' AND bil.Datebill <= '" + lastDayOfLastMonth.ToString("yyyy-MM-dd HH:mm:ss")+ "'";

        var dataget = _context.ReportTickets.FromSqlRaw(sqltest).AsEnumerable().ToList();
        var datafoodcombo = _context.ReportFoodCombos.FromSqlRaw(sqlfoodcombobill).AsEnumerable().ToList();
        List<DateTime> timeSlots = GetDaysInPreviousMonth();
        
        

        foreach (DateTime timeSlot in timeSlots)
        {
            

            string only1 = timeSlot.ToString("yyyy-MM-dd");

            var reportitem = new reportTicketnew();
            reportitem.datebill = timeSlot.ToString("yyyy-MM-dd HH:mm:ss");

            foreach (var item in dataget)
            {
                if (item.statusbill == 1){
                    totalall.totalbill += item.Total;
                }else {
                      totalall.totalbillwait += item.Total;
                }
               totalall.totalfoodwithbill += item.pricefood;
               
                string only2 = item.Datebill.ToString("yyyy-MM-dd");

                if (only1 == only2 && item.statusbill == 1)
                {
                    reportitem.total += item.Total;

                    if (item.pricefood != null)
                    {
                        reportitem.priceCombo += item.pricefood;
                    }
                    else
                    {
                        reportitem.priceCombo = 0;
                    }
                }else if (only1 == only2 && item.statusbill == 0) {
                      reportitem.totalwait += item.Total;

                    if (item.pricefood != null)
                    {
                        reportitem.priceCombo += item.pricefood;
                    }
                    else
                    {
                        reportitem.priceCombo = 0;
                    }
                }
            }

            foreach (var item in datafoodcombo)
            {
                 if (item.statusbillfoodcombo == 1){
                      totalall.totalfood += item.totals;
                 }else {
                      totalall.totalfoodwait += item.totals;
                 }

                string only2 = item.datetimes.ToString("yyyy-MM-dd");

                if (only1 == only2 && item.statusbillfoodcombo == 1)
                {
                    reportitem.totalpricefoodcomboorder += item.totals;
                }else if (only1 == only2 && item.statusbillfoodcombo == 0) {
                    reportitem.totalpricefoodcomboorderwait += item.totals;
                }
            }

           totalall.reportTicketnews.Add(reportitem);
        }
    }else if(report_type == 4) {
      
        var date1 = "";
         var date2 = "";
        List<(DateTime Start, DateTime End)> firstAndLastDays = GetFirstAndLastDaysOfLastThreeMonths();
        foreach (var dates in firstAndLastDays)
        {
     date1 = dates.Start.ToString("yyyy-MM-dd HH:mm:ss");
       date2 = dates.End.ToString("yyyy-MM-dd HH:mm:ss");
        }


        // var datenow =  DateTime.Now.AddDays(-1) + " 00:00:00";
        // var dateend = DateTime.Now + " 23:59:59";

        var sqlfoodcombobill = "SELECT datetimes, IFNULL(total_price, 0) as totals,statusbillfoodcombo FROM cinema.FoodCombillPayment WHERE datetimes >= '"+date1+"' AND datetimes <= '"+date2+"'";
        var sqltest = "SELECT bil.Datebill, bil.Totalamount as Total, foodb.idBillfoodCombo,bil.statusbill, IFNULL(fcb.priceCombo, 0) as pricefood FROM cinema.Bill bil LEFT JOIN cinema.FoodComboWithBills foodb ON foodb.Idbill = bil.Idbill LEFT JOIN cinema.Foodcombo fcb ON fcb.Idcombo = foodb.idcombo WHERE bil.Datebill >= '" + date1+ "' AND bil.Datebill <= '" + date2+ "'";

        var dataget = _context.ReportTickets.FromSqlRaw(sqltest).AsEnumerable().ToList();
        var datafoodcombo = _context.ReportFoodCombos.FromSqlRaw(sqlfoodcombobill).AsEnumerable().ToList();
        List<DateTime> timeSlots = GetDaysInLastThreeMonths();
        
        

        foreach (DateTime timeSlot in timeSlots)
        {
            

            string only1 = timeSlot.ToString("yyyy-MM-dd");

            var reportitem = new reportTicketnew();
            reportitem.datebill = timeSlot.ToString("yyyy-MM-dd HH:mm:ss");

            foreach (var item in dataget)
            {
                if (item.statusbill == 1){
                    totalall.totalbill += item.Total;
                }else {
                      totalall.totalbillwait += item.Total;
                }
               totalall.totalfoodwithbill += item.pricefood;
               
                string only2 = item.Datebill.ToString("yyyy-MM-dd");

                if (only1 == only2 && item.statusbill == 1)
                {
                    reportitem.total += item.Total;

                    if (item.pricefood != null)
                    {
                        reportitem.priceCombo += item.pricefood;
                    }
                    else
                    {
                        reportitem.priceCombo = 0;
                    }
                }else if (only1 == only2 && item.statusbill == 0) {
                      reportitem.totalwait += item.Total;

                    if (item.pricefood != null)
                    {
                        reportitem.priceCombo += item.pricefood;
                    }
                    else
                    {
                        reportitem.priceCombo = 0;
                    }
                }
            }

            foreach (var item in datafoodcombo)
            {
                 if (item.statusbillfoodcombo == 1){
                      totalall.totalfood += item.totals;
                 }else {
                      totalall.totalfoodwait += item.totals;
                 }

                string only2 = item.datetimes.ToString("yyyy-MM-dd");

                if (only1 == only2 && item.statusbillfoodcombo == 1)
                {
                    reportitem.totalpricefoodcomboorder += item.totals;
                }else if (only1 == only2 && item.statusbillfoodcombo == 0) {
                    reportitem.totalpricefoodcomboorderwait += item.totals;
                }
            }

           totalall.reportTicketnews.Add(reportitem);
        }

    } else if (report_type == 5) // lây bao cao theo nam
     {
             var date1 = "";
         var date2 = "";
        List<(DateTime Start, DateTime End)> firstAndLastDays = GetFirstAndLastDaysOfLastThreeMonths();
        foreach (var dates in firstAndLastDays)
        {
     date1 = dates.Start.ToString("yyyy-MM-dd HH:mm:ss");
       date2 = dates.End.ToString("yyyy-MM-dd HH:mm:ss");
        }


        // var datenow =  DateTime.Now.AddDays(-1) + " 00:00:00";
        // var dateend = DateTime.Now + " 23:59:59";

       

       
        List<DateTime> timeSlots = GetFirstDayOfMonth(DateTime.Now.Year);
        
        

        foreach (DateTime timeSlot in timeSlots)
        {
             string only1 = timeSlot.ToString("MM");
             string year = timeSlot.ToString("yyyy");
                var sqlfoodcombobill = "SELECT datetimes, IFNULL(total_price, 0) as totals, statusbillfoodcombo FROM cinema.FoodCombillPayment WHERE YEAR(datetimes) = '"+year+"' AND MONTH(datetimes) = '"+only1+"'";
                var sqltest = "SELECT bil.Datebill,bil.Totalamount as Total,foodb.idBillfoodCombo, IFNULL(fcb.priceCombo, 0) as pricefood, bil.statusbill FROM cinema.Bill bil LEFT JOIN cinema.FoodComboWithBills foodb ON foodb.Idbill = bil.Idbill LEFT JOIN cinema.Foodcombo fcb ON fcb.Idcombo = foodb.idcombo WHERE  MONTH(bil.Datebill) = '"+only1+"' AND YEAR(bil.Datebill) = '"+year+"'";
                var dataget = _context.ReportTickets.FromSqlRaw(sqltest).AsEnumerable().ToList();
                var datafoodcombo = _context.ReportFoodCombos.FromSqlRaw(sqlfoodcombobill).AsEnumerable().ToList();
           

            var reportitem = new reportTicketnew();
            reportitem.datebill = timeSlot.ToString("yyyy-MM-dd");

            foreach (var item in dataget)
            {
                if (item.statusbill == 1){
                    totalall.totalbill += item.Total;
                }else {
                      totalall.totalbillwait += item.Total;
                }
               totalall.totalfoodwithbill += item.pricefood;
               
                string only2 = item.Datebill.ToString("MM");

                if (only1 == only2 && item.statusbill == 1)
                {
                    reportitem.total += item.Total;

                    if (item.pricefood != null)
                    {
                        reportitem.priceCombo += item.pricefood;
                    }
                    else
                    {
                        reportitem.priceCombo = 0;
                    }
                }else if (only1 == only2 && item.statusbill == 0) {
                      reportitem.totalwait += item.Total;

                    if (item.pricefood != null)
                    {
                        reportitem.priceCombo += item.pricefood;
                    }
                    else
                    {
                        reportitem.priceCombo = 0;
                    }
                }
            }

            foreach (var item in datafoodcombo)
            {
                 if (item.statusbillfoodcombo == 1){
                      totalall.totalfood += item.totals;
                 }else {
                      totalall.totalfoodwait += item.totals;
                 }

                string only2 = item.datetimes.ToString("yyyy-MM-dd");

                if (only1 == only2 && item.statusbillfoodcombo == 1)
                {
                    reportitem.totalpricefoodcomboorder += item.totals;
                }else if (only1 == only2 && item.statusbillfoodcombo == 0) {
                    reportitem.totalpricefoodcomboorderwait += item.totals;
                }
            }

           totalall.reportTicketnews.Add(reportitem);
        }
    } else if (report_type == 11) {
                   var date1 = "";
         var date2 = "";
        List<(DateTime Start, DateTime End)> firstAndLastDays = GetFirstAndLastDaysOfLastThreeMonths();
        foreach (var dates in firstAndLastDays)
        {
     date1 = dates.Start.ToString("yyyy-MM-dd HH:mm:ss");
       date2 = dates.End.ToString("yyyy-MM-dd HH:mm:ss");
        }


        // var datenow =  DateTime.Now.AddDays(-1) + " 00:00:00";
        // var dateend = DateTime.Now + " 23:59:59";

       

       
       List<DateTime> timeSlots = GetFirstDayOfMonth(DateTime.Now.Year - 1);

        
        

        foreach (DateTime timeSlot in timeSlots)
        {
             string only1 = timeSlot.ToString("MM");
             string year = timeSlot.ToString("yyyy");
                var sqlfoodcombobill = "SELECT datetimes, IFNULL(total_price, 0) as totals, statusbillfoodcombo FROM cinema.FoodCombillPayment WHERE YEAR(datetimes) = '"+year+"' AND MONTH(datetimes) = '"+only1+"'";
                var sqltest = "SELECT bil.Datebill,bil.Totalamount as Total,foodb.idBillfoodCombo, IFNULL(fcb.priceCombo, 0) as pricefood, bil.statusbill FROM cinema.Bill bil LEFT JOIN cinema.FoodComboWithBills foodb ON foodb.Idbill = bil.Idbill LEFT JOIN cinema.Foodcombo fcb ON fcb.Idcombo = foodb.idcombo WHERE  MONTH(bil.Datebill) = '"+only1+"' AND YEAR(bil.Datebill) = '"+year+"'";
                var dataget = _context.ReportTickets.FromSqlRaw(sqltest).AsEnumerable().ToList();
                var datafoodcombo = _context.ReportFoodCombos.FromSqlRaw(sqlfoodcombobill).AsEnumerable().ToList();
           

            var reportitem = new reportTicketnew();
            reportitem.datebill = timeSlot.ToString("yyyy-MM-dd");

            foreach (var item in dataget)
            {
                if (item.statusbill == 1){
                    totalall.totalbill += item.Total;
                }else {
                      totalall.totalbillwait += item.Total;
                }
               totalall.totalfoodwithbill += item.pricefood;
               
                string only2 = item.Datebill.ToString("MM");

                if (only1 == only2 && item.statusbill == 1)
                {
                    reportitem.total += item.Total;

                    if (item.pricefood != null)
                    {
                        reportitem.priceCombo += item.pricefood;
                    }
                    else
                    {
                        reportitem.priceCombo = 0;
                    }
                }else if (only1 == only2 && item.statusbill == 0) {
                      reportitem.totalwait += item.Total;

                    if (item.pricefood != null)
                    {
                        reportitem.priceCombo += item.pricefood;
                    }
                    else
                    {
                        reportitem.priceCombo = 0;
                    }
                }
            }

            foreach (var item in datafoodcombo)
            {
                 if (item.statusbillfoodcombo == 1){
                      totalall.totalfood += item.totals;
                 }else {
                      totalall.totalfoodwait += item.totals;
                 }

                string only2 = item.datetimes.ToString("yyyy-MM-dd");

                if (only1 == only2 && item.statusbillfoodcombo == 1)
                {
                    reportitem.totalpricefoodcomboorder += item.totals;
                }else if (only1 == only2 && item.statusbillfoodcombo == 0) {
                    reportitem.totalpricefoodcomboorderwait += item.totals;
                }
            }

           totalall.reportTicketnews.Add(reportitem);
        }
    } else if (report_type == 6) {
                var date1 = "";
         var date2 = "";
        List<(DateTime Start, DateTime End)> firstAndLastDays = GetFirstAndLastDaysOfLastThreeMonths();
        foreach (var dates in firstAndLastDays)
        {
     date1 = dates.Start.ToString("yyyy-MM-dd HH:mm:ss");
       date2 = dates.End.ToString("yyyy-MM-dd HH:mm:ss");
        }


        // var datenow =  DateTime.Now.AddDays(-1) + " 00:00:00";
        // var dateend = DateTime.Now + " 23:59:59";

       

       
       List<DateTime> timeSlots = GetFirstDayOfEachMonth(3);

        
        

        foreach (DateTime timeSlot in timeSlots)
        {
             string only1 = timeSlot.ToString("MM");
             string year = timeSlot.ToString("yyyy");
                var sqlfoodcombobill = "SELECT datetimes, IFNULL(total_price, 0) as totals, statusbillfoodcombo FROM cinema.FoodCombillPayment WHERE YEAR(datetimes) = '"+year+"' AND MONTH(datetimes) = '"+only1+"'";
                var sqltest = "SELECT bil.Datebill,bil.Totalamount as Total,foodb.idBillfoodCombo, IFNULL(fcb.priceCombo, 0) as pricefood, bil.statusbill FROM cinema.Bill bil LEFT JOIN cinema.FoodComboWithBills foodb ON foodb.Idbill = bil.Idbill LEFT JOIN cinema.Foodcombo fcb ON fcb.Idcombo = foodb.idcombo WHERE  MONTH(bil.Datebill) = '"+only1+"' AND YEAR(bil.Datebill) = '"+year+"'";
                var dataget = _context.ReportTickets.FromSqlRaw(sqltest).AsEnumerable().ToList();
                var datafoodcombo = _context.ReportFoodCombos.FromSqlRaw(sqlfoodcombobill).AsEnumerable().ToList();
           

            var reportitem = new reportTicketnew();
            reportitem.datebill = timeSlot.ToString("yyyy-MM-dd");

            foreach (var item in dataget)
            {
                if (item.statusbill == 1){
                    totalall.totalbill += item.Total;
                }else {
                      totalall.totalbillwait += item.Total;
                }
               totalall.totalfoodwithbill += item.pricefood;
               
                string only2 = item.Datebill.ToString("MM");

                if (only1 == only2 && item.statusbill == 1)
                {
                    reportitem.total += item.Total;

                    if (item.pricefood != null)
                    {
                        reportitem.priceCombo += item.pricefood;
                    }
                    else
                    {
                        reportitem.priceCombo = 0;
                    }
                }else if (only1 == only2 && item.statusbill == 0) {
                      reportitem.totalwait += item.Total;

                    if (item.pricefood != null)
                    {
                        reportitem.priceCombo += item.pricefood;
                    }
                    else
                    {
                        reportitem.priceCombo = 0;
                    }
                }
            }

            foreach (var item in datafoodcombo)
            {
                 if (item.statusbillfoodcombo == 1){
                      totalall.totalfood += item.totals;
                 }else {
                      totalall.totalfoodwait += item.totals;
                 }

                string only2 = item.datetimes.ToString("yyyy-MM-dd");

                if (only1 == only2 && item.statusbillfoodcombo == 1)
                {
                    reportitem.totalpricefoodcomboorder += item.totals;
                }else if (only1 == only2 && item.statusbillfoodcombo == 0) {
                    reportitem.totalpricefoodcomboorderwait += item.totals;
                }
            }

           totalall.reportTicketnews.Add(reportitem);
        }
    }else if (report_type == 8) {
            var date1 = "";
            var date2 = "";
            List<(DateTime Start, DateTime End)> firstAndLastDays = GetFirstAndLastDaysOfLastThreeMonths();
            foreach (var dates in firstAndLastDays)
            {
            date1 = dates.Start.ToString("yyyy-MM-dd HH:mm:ss");
            date2 = dates.End.ToString("yyyy-MM-dd HH:mm:ss");
            }


        // var datenow =  DateTime.Now.AddDays(-1) + " 00:00:00";
        // var dateend = DateTime.Now + " 23:59:59";

       

   var sqlfoodcombobill = "SELECT YEAR(datetimes) as datetimes, IFNULL(total_price, 0) as totals, statusbillfoodcombo FROM cinema.FoodCombillPayment GROUP BY YEAR(datetimes), statusbillfoodcombo, IFNULL(total_price, 0) ";
var sqltest = "SELECT YEAR(bil.Datebill) as Datebill, bil.Totalamount as Total, IFNULL(fcb.priceCombo, 0) as pricefood, bil.statusbill FROM cinema.Bill bil LEFT JOIN cinema.FoodComboWithBills foodb ON foodb.Idbill = bil.Idbill LEFT JOIN cinema.Foodcombo fcb ON fcb.Idcombo = foodb.idcombo GROUP BY YEAR(bil.Datebill), bil.statusbill, bil.Totalamount, IFNULL(fcb.priceCombo, 0)";

var dataget = _context.ReportTicketALLs.FromSqlRaw(sqltest).AsEnumerable().ToList();
var datafoodcombo = _context.ReportFoodComboALLs.FromSqlRaw(sqlfoodcombobill).AsEnumerable().ToList();


   

        
        

      
            
              
           

            var reportitem = new reportTicketnew();
        

            foreach (var item in dataget)
            {
                int years = item.Datebill;
                    reportitem.datebill = GetFirstDayOfYear(years).ToString("yyyy-MM-dd");
                if (item.statusbill == 1){
                    totalall.totalbill += item.Total;
                }else {
                      totalall.totalbillwait += item.Total;
                }
               totalall.totalfoodwithbill += item.pricefood;
               
                string only2 = item.Datebill.ToString("MM");

                if (item.statusbill == 1)
                {
                    reportitem.total += item.Total;

                    if (item.pricefood != null)
                    {
                        reportitem.priceCombo += item.pricefood;
                    }
                    else
                    {
                        reportitem.priceCombo = 0;
                    }
                }else if (item.statusbill == 0) {
                      reportitem.totalwait += item.Total;

                    if (item.pricefood != null)
                    {
                        reportitem.priceCombo += item.pricefood;
                    }
                    else
                    {
                        reportitem.priceCombo = 0;
                    }
                }
            }

            foreach (var item in datafoodcombo)
            {
                 if (item.statusbillfoodcombo == 1){
                      totalall.totalfood += item.totals;
                 }else {
                      totalall.totalfoodwait += item.totals;
                 }

                string only2 = item.datetimes.ToString("yyyy-MM-dd");

                if ( item.statusbillfoodcombo == 1)
                {
                    reportitem.totalpricefoodcomboorder += item.totals;
                }else if (item.statusbillfoodcombo == 0) {
                    reportitem.totalpricefoodcomboorderwait += item.totals;
                }
            }

           totalall.reportTicketnews.Add(reportitem);
        
    }


                        successApiResponse.Status = 200;
                        successApiResponse.Message = "OK";
                        successApiResponse.Data = totalall;



  


   
          
       

      
    }
    catch (Exception ex)
    {    
    // Xử lý ngoại lệ
    }
  
            // }else {
            //     return BadRequest("khong tim thay thong tin");
            // }
                 

           }

        }
 return Ok(successApiResponse);
}

// API GET LIST VOUCHER
[HttpGet("ReportProduct")]
public IActionResult ReportProduct(int report_type)
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
                    var date = "2023-10-06";
                    var datenow = DateTime.Now.Date; // Gets the date of yesterday at 00:00:00
                    var dateend = DateTime.Now.Date.AddHours(23).AddMinutes(59).AddSeconds(59); 

                  
                    var sqltest = "SELECT DATE_FORMAT(bill.Datebill, '%Y-%m-%d %H') as Datebill,bill.Idmovie,m.poster,IFNULL(bill.Totalamount,0) as totals FROM cinema.Bill  as bill INNER JOIN cinema.movie as m on m.Idmovie = bill.Idmovie WHERE bill.Datebill >= '2023-10-06 00:00:00' AND bill.Datebill <= '2023-10-06 23:59:59' GROUP BY  DATE_FORMAT(bill.Datebill, '%Y-%m-%d %H'),bill.Idmovie,m.poster,IFNULL(bill.Totalamount,0) ORDER BY  DATE_FORMAT(bill.Datebill, '%Y-%m-%d %H')";

                    var dataget = _context.ReportMovies.FromSqlRaw(sqltest).AsEnumerable().ToList();
                    
                //   foreach (var iitem in dataget ){

                //   }
            
                      successApiResponse.Status = 200;
                     successApiResponse.Message = "OK";
                     successApiResponse.Data = dataget;
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


// public partial class ReportMovieshow {
//  public int totals {get;set;}

//  public long Idmovie {get;set;}

//  public string poster {get;set;}

//  public DateTime Datebill {get;set;}

// }


 public static DateTime GetFirstDayOfYear(int year)
    {
        return new DateTime(year, 1, 1);
    }

public static List<DateTime> GetTimeSlotsInCurrentDay(int statuses)
{
    List<DateTime> timeSlots = new List<DateTime>();
    DateTime currentDate = new DateTime();
            if (statuses == 1 ) {
            currentDate = DateTime.Now.Date;
            } else if (statuses == 9) {
            currentDate = DateTime.Now.Date.AddDays(-1);
            }
    // Lấy ngày hiện tại
   

    // Đặt giờ bắt đầu và kết thúc trong ngày
    DateTime startTime = currentDate.AddHours(1); // Bắt đầu từ 01:00:00
    DateTime endTime = currentDate.AddHours(23); // Kết thúc vào 23:00:00

    // Thêm các thời điểm vào danh sách
    while (startTime <= endTime)
    {
        timeSlots.Add(startTime);
        startTime = startTime.AddHours(1); // Thêm 1 giờ
    }

    return timeSlots;
}

public static List<DateTime> GetDaysOfWeek()
{
    List<DateTime> daysOfWeek = new List<DateTime>();

    DateTime currentDate = DateTime.Now.Date;

    // Tìm ngày của thứ Hai trong tuần
    DateTime monday = currentDate;
    while (monday.DayOfWeek != DayOfWeek.Monday)
    {
        monday = monday.AddDays(-1);
    }

    // Thêm các ngày từ thứ Hai đến Chủ Nhật vào danh sách
    for (int i = 0; i < 7; i++)
    {
        daysOfWeek.Add(monday.AddDays(i));
    }

    return daysOfWeek;
}

public static List<DateTime> GetDaysInCurrentMonth()
{
    List<DateTime> daysInMonth = new List<DateTime>();

    DateTime firstDayOfMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

    int daysInThisMonth = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);

    for (int i = 0; i < daysInThisMonth; i++)
    {
        daysInMonth.Add(firstDayOfMonth.AddDays(i));
    }

    return daysInMonth;
}

public static List<DateTime> GetDaysInPreviousMonth()
{
    List<DateTime> daysInMonth = new List<DateTime>();

    DateTime firstDayOfPreviousMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(-1);

    int daysInPreviousMonth = DateTime.DaysInMonth(firstDayOfPreviousMonth.Year, firstDayOfPreviousMonth.Month);

    for (int i = 0; i < daysInPreviousMonth; i++)
    {
        daysInMonth.Add(firstDayOfPreviousMonth.AddDays(i));
    }

    return daysInMonth;
}

public static List<DateTime> GetDaysInLastThreeMonths()
{
    List<DateTime> daysInLastThreeMonths = new List<DateTime>();

    DateTime currentDate = DateTime.Now;

    for (int i = 0; i < 3; i++)
    {
        DateTime firstDayOfMonth = new DateTime(currentDate.Year, currentDate.Month, 1);
        DateTime lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1).AddHours(23).AddMinutes(59).AddSeconds(59);

        for (DateTime date = firstDayOfMonth; date <= lastDayOfMonth; date = date.AddDays(1))
        {
            daysInLastThreeMonths.Add(date);
        }

        currentDate = currentDate.AddMonths(-1);
    }

    return daysInLastThreeMonths;
}
public static List<(DateTime Start, DateTime End)> GetFirstAndLastDaysOfLastThreeMonths()
{
    List<(DateTime Start, DateTime End)> firstAndLastDays = new List<(DateTime Start, DateTime End)>();

    DateTime currentDate = DateTime.Now.Date;

    for (int i = 0; i < 3; i++)
    {
        DateTime firstDayOfLastMonth = currentDate.AddMonths(-1);
        DateTime lastDayOfLastMonth = new DateTime(firstDayOfLastMonth.Year, firstDayOfLastMonth.Month, DateTime.DaysInMonth(firstDayOfLastMonth.Year, firstDayOfLastMonth.Month))
            .AddHours(23).AddMinutes(59).AddSeconds(59);

        firstAndLastDays.Add((firstDayOfLastMonth, lastDayOfLastMonth));

        currentDate = firstDayOfLastMonth;
    }

    return firstAndLastDays;
}

public static List<DateTime> GetFirstDayOfMonth(int year)
{
    List<DateTime> dates = new List<DateTime>();

    for (int month = 1; month <= 12; month++)
    {
        DateTime firstDayOfMonth = new DateTime(year, month, 1);
        dates.Add(firstDayOfMonth);
    }

    return dates;
}

public static List<DateTime> GetFirstDayOfEachMonth(int numYears)
{
    List<DateTime> dates = new List<DateTime>();

    int currentYear = DateTime.Now.Year;

    for (int i = 0; i < numYears; i++)
    {
        for (int month = 1; month <= 12; month++)
        {
            int year = currentYear - i;
            DateTime firstDayOfMonth = new DateTime(year, month, 1);

            if (year == 2023 && month == 1)
            {
                dates.Add(firstDayOfMonth);
            }
            else if (year != 2023)
            {
                dates.Add(firstDayOfMonth);
            }
        }
    }

    return dates;
}



public class ReportAlltotal {
    public int totalbill {get;set;}

    public int totalbillwait {get;set;}

    public int totalfoodwithbill {get;set;}

    public int totalfood {get;set;}

    public int totalfoodwait {get;set;}

    public List<reportTicketnew> reportTicketnews {get;set;} = new List<reportTicketnew>();
}

public class reportTicketnew {
    public string datebill {get;set;}

    public int total {get;set;} // tong tien trang thai hoan tat

    public int totalwait {get;set;} // tong tien trang thai cho nhan

    public int priceCombo {get;set;} // tong tine mon ban kem

    public int totalpricefoodcomboorder {get;set;} // tong tien mon combo da hoan tat

    public int  totalpricefoodcomboorderwait {get;set;} // tong tine mon combo cho nhan 
}



}
