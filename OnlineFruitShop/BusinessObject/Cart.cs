using System.ComponentModel.DataAnnotations.Schema;


namespace BusinessObject
{
    public partial class Cart
    {
        public int CartId { get; set; }

        public int UserId { get; set; }

        public int ProductId { get; set; }

        public int Quantity { get; set; }

        public virtual Product Product { get; set; }

        public virtual User User { get; set; }

        [NotMapped]
        public string ProductName => Product?.ProductName ?? "";

        [NotMapped]
        public decimal UnitPrice => Product?.Price ?? 0;

        [NotMapped]
        public decimal Total => Quantity * UnitPrice;
        [NotMapped]
        public bool IsSelected { get; set; }
    }
}

