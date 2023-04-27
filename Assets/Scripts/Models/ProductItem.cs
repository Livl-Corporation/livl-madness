namespace Models
{
    public class ProductItem
    {
        public string name;
        public ProductState status;
        public bool isOutOfStock;
        
        public ProductItem()
        {
            this.name = "";
            this.status = ProductState.UNCHECKED;
            this.isOutOfStock = false;
        }
        
        public ProductItem(string name, ProductState status, bool isOutOfStock)
        {
            this.name = name;
            this.status = status;
            this.isOutOfStock = isOutOfStock;
        }

        public ProductItem(string name)
        {
            this.name = name;
            this.status = ProductState.UNCHECKED;
            this.isOutOfStock = false;
        }
    }
}