using System;
using System.Collections.Generic;

namespace Intex2024.Models;

public partial class Transaction
{
    public int TransactionId { get; set; }

    public long CustomerId { get; set; }

    public DateOnly Date { get; set; }

    public string DayOfWeek { get; set; } = null!;

    public short Time { get; set; }

    public string EntryMode { get; set; } = null!;

    public short Amount { get; set; }

    public string TypeOfTransaction { get; set; } = null!;

    public string CountryOfTransaction { get; set; } = null!;

    public string ShippingAddress { get; set; } = null!;

    public string Bank { get; set; } = null!;

    public string TypeOfCard { get; set; } = null!;

    public bool Fraud { get; set; }
}
