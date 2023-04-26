namespace Models
{
    public class ProductItem
    {
        public string name;
        public bool scanned = false;
        public bool isOutOfStock = false;

        public ProductItem(string name)
        {
            this.name = name;
        }
    }
}