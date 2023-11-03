using System;
using System.Collections.Generic;

namespace MyCinema.Model;

public partial class INTERESTCINEMA
{
    public string Namecinema { get; set; }
    public string Times { get; set; }
    public long Idmovie { get; set; }
    public long Idcinema { get; set; }
    public long Idroom { get; set; }
    public int Idinterest { get; set; }
    
}

public partial class LISTCINEMA
{
    public string Namecinema { get; set; }
    
    public long Idcinema { get; set; }
    
    
}

public partial class CHAIR
{
    public int Idchair { get; set; }
    
    public int NumberChair { get; set; }

     public string RowChar { get; set; }

     public long? bill {get;set;} 
     public int Idcategorychair {get;set;}
   
    
}
public partial class CHAIRDETAILBILL
{
    public int Idchair { get; set; }
    
    public int NumberChair { get; set; }

     public string RowChar { get; set; }

    //  public long? bill {get;set;} 
     public int Idcategorychair {get;set;}
   
    
}

public partial class USERS
{
    public string Fullname { get; set; }
    
    public string Email { get; set; }

     public string phone { get; set; }

     public DateTime Birthday {get;set;} 

     public string avatar {get;set;} 
       public int gender {get;set;} 
      public int Idrole {get;set;} 
       public int Idusers {get;set;} 
        public int Idcinema {get;set;} 
         public int statuss {get;set;} 

         public string? address {get;set;}
    
}

public partial class CINEMA
{
    public int Idcinema {get;set;}
}

public partial class ReportTicket {
// public DateTime? DateHour {get;set;}

  public DateTime Datebill {get;set;}

  public int Total {get;set;}

  public int pricefood {get;set;} 
  public int statusbill {get;set;}
    
}


public partial class ReportFoodCombo {
// public DateTime? DateHour {get;set;}

  public DateTime datetimes {get;set;}

  public int totals {get;set;}

public int statusbillfoodcombo {get;set;}
    
}
//LAY HET ALL YEAR
public partial class ReportTicketALL {
// public DateTime? DateHour {get;set;}

  public int Datebill {get;set;}

  public int Total {get;set;}

  public int pricefood {get;set;} 
  public int statusbill {get;set;}
    
}


public partial class ReportFoodComboALL {
// public DateTime? DateHour {get;set;}

  public int datetimes {get;set;}

  public int totals {get;set;}

public int statusbillfoodcombo {get;set;}
    
}

public partial class ReportMovie {
public int stt {get;set;}

 public int totals {get;set;}

 public long? Idmovie {get;set;}

//  public string poster {get;set;}

//  public string Datebill {get;set;}

}
public partial class ReportFood {
  public int Idfood {get;set;}

  public int idcombo {get;set;}

  public int totals {get;set;}

  public string datetimes {get;set;}
  public int stt {get;set;}

}