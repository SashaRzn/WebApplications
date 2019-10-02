using System.Collections.Generic;

namespace WebApi
{
	public interface IProductRepository
	{
		IEnumerable<Product> GetAllProducts();
		Product GetProductById(int id);
		void AddProduct(Product model);
		void DeleteProduct(Product product);
	}
}
