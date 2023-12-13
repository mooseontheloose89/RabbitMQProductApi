using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RabbitMQProductApi.Models;
using RabbitMQProductApi.RabbitMQ;
using RabbitMQProductApi.Services;

namespace RabbitMQProductApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IRabbitMQProducer _rabbitMQProducer;

        public ProductController(IProductService productService, IRabbitMQProducer rabbitMQProducer)
        {
            _productService = productService;
            _rabbitMQProducer = rabbitMQProducer;
        }

        [HttpGet("productlist")]
        public IEnumerable<Product> ProductList()
        {
            var productList = _productService.GetProductList();
            return productList;
        }

        [HttpGet("getproductbyid")]
        public Product GetProductById(int id)
        {
            return _productService.GetProductById(id);
        }

        [HttpPost("addproduct")]
        public Product AddProduct(Product product)
        {
            var productData = _productService.AddProduct(product);
            // Send inserted data to queue 
            _rabbitMQProducer.SendProductMessage(productData);
            return productData;
        }

        [HttpPut("updateproduct")]
        public Product UpdateProduct(Product product) 
        {
            return _productService.UpdateProduct(product);
        }

        [HttpDelete("deleteproduct")]
        public bool DeleteProduct(int id)
        {
            return _productService.DeleteProduct(id);
        } 
    }
}
