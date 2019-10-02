using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace WebApi.Controllers
{
	[Route("api/[controller]")]
	public class ProductsController : ControllerBase
	{
		private readonly IProductRepository _productRepository;
		private readonly ILogger<ProductsController> _logger;

		private const string GetProductRouteName = "GetProductRoute";

		public ProductsController(IProductRepository productRepository,
			ILogger<ProductsController> logger)
		{
			_productRepository = productRepository;
			_logger = logger;
		}

		[HttpGet()]
		public IActionResult Get()
		{
			try
			{
				var products = _productRepository.GetAllProducts();
				return Ok(products);
			}
			catch (Exception e)
			{
				_logger.LogError(e, "Throw an exception during execution getting products");
			}

			return BadRequest("Could not get product list");
		}

		[HttpGet("{id}", Name = GetProductRouteName)]
		public IActionResult Get(int id)
		{
			// Use public IActionResult Get(int id, bool includeSomeProp = false)
			// For include additional data for entity in result
			// Parameter maps from queryString

			_logger.LogInformation("Get product item method init");

			try
			{
				var product = _productRepository.GetProductById(id);
				if (product == null)
					return NotFound($"Product {id} was not found");

				return Ok(product);
			}
			catch (Exception e)
			{
				_logger.LogError(e, "Throw an exception during execution getting product");
			}

			return BadRequest($"Could not get product {id}");
		}

		[HttpPost]
		public IActionResult Post([FromBody] Product model)
		{
			try
			{
				// Validation

				_productRepository.AddProduct(model);

				var newUri = Url.Link(GetProductRouteName, new { id = model.Id });
				return Created(newUri, model);
			}
			catch (Exception e)
			{
				_logger.LogError(e, "Throw an exception during execution creating product");
			}

			return BadRequest("Could not create product");
		}

		[HttpPut("{id}")]
		public IActionResult Put(int id, [FromBody] Product model)
		{
			try
			{
				var oldProduct = _productRepository.GetProductById(id);
				if (oldProduct == null)
					return NotFound($"Product with Id = {id} was not found");

				// Map model to old model
				oldProduct.Name = model.Name ?? oldProduct.Name;
				oldProduct.Count = model.Count > 0 ? model.Count : oldProduct.Count;

				return Ok(oldProduct);
			}
			catch (Exception e)
			{
				_logger.LogError(e, "Throw an exception during execution updating product");
			}

			return BadRequest($"Could not update product with Id = {id}");
		}

		[HttpDelete("{id}")]
		public IActionResult Delete(int id)
		{
			try
			{
				var product = _productRepository.GetProductById(id);
				if (product == null)
					return NotFound($"Product with id = {id} was not found");

				_productRepository.DeleteProduct(product);
				return Ok();
			}
			catch (Exception e)
			{
				_logger.LogError(e, "Throw an exception during execution deleting product");
			}

			return BadRequest("Could not delete product");
		}
	}
}