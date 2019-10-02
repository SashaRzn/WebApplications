using System.Collections.Generic;
using System.Linq;

namespace WebApi
{
	public class ProductRepository : IProductRepository
	{
		private static List<Product> products;

		public ProductRepository()
		{
			InitProducts();
		}

		public void AddProduct(Product model)
		{
			model.Id = products.Count + 1;
			products.Add(model);
		}

		public void DeleteProduct(Product product)
		{
			products.Remove(product);
		}

		public IEnumerable<Product> GetAllProducts()
		{
			return products.AsEnumerable();
		}

		public Product GetProductById(int id)
		{
			var product = products.FirstOrDefault(p => p.Id == id);
			return product;
		}

		private void InitProducts()
		{
			products = new List<Product>()
			{
				new Product
				{
					Id = 1,
					Name = "Potato",
					Count = 2
				},
				new Product
				{
					Id = 2,
					Name = "Limon",
					Count = 4
				}
			};
		}
	}
}
