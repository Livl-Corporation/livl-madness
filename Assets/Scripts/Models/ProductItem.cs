namespace Models
{
    public class ProductItem
    {
        public string name;
        public bool scanned;
        public bool isOutOfStock;
        
        public ProductItem()
        {
            this.name = "";
            this.scanned = false;
            this.isOutOfStock = false;
        }
        
        public ProductItem(string name, bool scanned, bool isOutOfStock)
        {
            this.name = name;
            this.scanned = scanned;
            this.isOutOfStock = isOutOfStock;
        }

        public ProductItem(string name)
        {
            this.name = name;
            this.scanned = false;
            this.isOutOfStock = false;
        }
    }
}