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