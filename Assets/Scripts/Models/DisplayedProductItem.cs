namespace Models
{
    public class DisplayedProductItem : ProductItem
    {

        public ProductState Status;

        public DisplayedProductItem(ProductItem item)
        {
            Name = item.Name;
            Scanned = item.Scanned;
            OutOfStock = item.OutOfStock;
            ScannedBy = item.ScannedBy;
            Status = ProductState.UNCHECKED;
        }
        
        public DisplayedProductItem(ProductItem item, bool success)
        {
            Name = item.Name;
            Scanned = item.Scanned;
            OutOfStock = item.OutOfStock;
            ScannedBy = item.ScannedBy;

            Status = success ? ProductState.CHECKED : ProductState.FAILED;
           
        }
 
    }
}