namespace Intex2024.Models.ViewModels
{
    public class ProductDetailsViewModel
    {
        public Product Product { get; set; }
        public Product RelatedProduct1 { get; set; }
        public Product RelatedProduct2 { get; set; }
        public Product RelatedProduct3 { get; set; }
        public LineItem LineItem {  get; set; }
    }
}