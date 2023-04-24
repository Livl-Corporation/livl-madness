using System.Collections.Generic;
using Models;

namespace Interfaces
{
    public interface IProductListObserver
    {
        void UpdateProductList(List<ProductItem> productList);
    }
}