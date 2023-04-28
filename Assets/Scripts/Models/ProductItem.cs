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
        
        public ProductItem(string name, string scannedBy)
        {
            Name = name;
            Scanned = true;
            OutOfStock = false;
            ScannedBy = scannedBy;
        }

        public ProductItem(string name)
        {
            Name = name;
            Scanned = false;
            OutOfStock = false;
            ScannedBy = null;
        }
    }
}