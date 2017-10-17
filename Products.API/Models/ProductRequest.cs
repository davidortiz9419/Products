namespace Products.API.Models
{
    using Domain;

    public class ProductRequest : Product
    {
        public byte[] ImageArray { get; set; }
    }
}