using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static Intex2024.Models.Cart;

namespace Intex2024.Models;

public partial class Transaction
{
    public int TransactionId { get; set; }
    public ICollection<CartLine> Lines { get; set; }
            = new List<CartLine>();
    public long CustomerId { get; set; }

    [Required(ErrorMessage = "Please enter a Date")]
    public DateOnly Date { get; set; }

    [Required(ErrorMessage = "Please enter a Day of the week")]
    public string DayOfWeek { get; set; } = null!;

    [Required(ErrorMessage = "Please enter a time")]
    public short Time { get; set; }

    [Required(ErrorMessage = "Please an entry mode")]
    public string EntryMode { get; set; } = null!;

    [Required(ErrorMessage = "Please enter an amount")]
    public short Amount { get; set; }

    [Required(ErrorMessage = "Please enter a type of transaction")]
    public string TypeOfTransaction { get; set; } = null!;

    [Required(ErrorMessage = "Please enter a Country")]
    public string CountryOfTransaction { get; set; } = null!;

    [Required(ErrorMessage = "Please enter a Shipping Address")]
    public string ShippingAddress { get; set; } = null!;

    [Required(ErrorMessage = "Please enter a Bank")]
    public string Bank { get; set; } = null!;

    [Required(ErrorMessage = "Please enter a card")]
    public string TypeOfCard { get; set; } = null!;

    public bool Fraud { get; set; }
}
