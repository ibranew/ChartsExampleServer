using System;
using System.Collections.Generic;

namespace ChartsExampleServer.Models;

public partial class Sale
{
    public int Id { get; set; }

    public int? PersonelId { get; set; }

    public int? Price { get; set; }
}
