namespace Models
{
    public class ProductItem
    {
        public string Name;
        public bool Scanned;
        public bool OutOfStock;
        public string ScannedBy;
        
        public ProductItem()
        {
            Name = "";
            Scanned = false;
            OutOfStock = false;
            ScannedBy = null;
        }
        
        public ProductItem(ProductItem product, string scannedBy)
        {
            Name = product.Name;
            Scanned = true;
            OutOfStock = product.OutOfStock;
            ScannedBy = scannedBy;
        }

        public ProductItem(string name)
        {
            Name = name;
            Scanned = false;
            OutOfStock = false;
            ScannedBy = null;
        }
        
        public ProductItem(ProductItem item, bool outOfStock)
        {
            Name = item.Name;
            Scanned = item.Scanned;
            OutOfStock = outOfStock;
            ScannedBy = item.ScannedBy;
        }
        
    }
}