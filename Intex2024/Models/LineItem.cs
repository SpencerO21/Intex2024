using System;
using System.Collections.Generic;

namespace Intex2024.Models;

public partial class LineItem
{
    public int TransactionId { get; set; }

    public short ProductId { get; set; }

    public int Qty { get; set; }

    public int? Rating { get; set; }

    public int? CartId { get; set; }
}
