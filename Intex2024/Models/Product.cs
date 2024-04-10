using System;
using System.Collections.Generic;

namespace Intex2024.Models;

public partial class Product
{
    public short ProductId { get; set; }

    public string Name { get; set; } = null!;

    public short Year { get; set; }

    public short NumParts { get; set; }

    public short Price { get; set; }

    public string ImgLink { get; set; } = null!;

    public string PrimaryColor { get; set; } = null!;

    public string SecondaryColor { get; set; } = null!;

    public string Description { get; set; } = null!;

    public short? RelatedItem1 { get; set; }

    public short? RelatedItem2 { get; set; }

    public short? RelatedItem3 { get; set; }

    public string Category1 { get; set; } = null!;

    public string? Category2 { get; set; }

    public string? Category3 { get; set; }
}
