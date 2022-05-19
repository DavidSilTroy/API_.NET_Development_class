using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.DAL;
using Shop.DAL.Data;
using Shop.DAL.Models;

namespace Shop.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        //private readonly ShopContext _context;
        private readonly IUnitOfWork _uow;

        //public ProductsController(ShopContext context)
        public ProductsController(IUnitOfWork uow)
        {
            //_context = context;
            _uow = uow;
        }

        // GET: api/Products
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            //return await _context.Products.ToListAsync();
            var products = await _uow.ProductRepository.GetAllAsync();
            return products.ToList();
        }

        // GET: api/Products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            //var product = await _context.Products.FindAsync(id);
            var product = await _uow.ProductRepository.GetByIDAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            return product;
        }

        // PUT: api/Products/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, Product product)
        {
            if (id != product.Id)
            {
                return BadRequest();
            }

            //_context.Entry(product).State = EntityState.Modified;
            _uow.ProductRepository.Update(product);

            try
            {
                //await _context.SaveChangesAsync();
                await _uow.SaveAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Products
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct(Product product)
        {
            //_context.Products.Add(product);
            //await _context.SaveChangesAsync();
            _uow.ProductRepository.Insert(product);
            await _uow.SaveAsync();

            return CreatedAtAction("GetProduct", new { id = product.Id }, product);
        }

        // DELETE: api/Products/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            //var product = await _context.Products.FindAsync(id);
            var product = await _uow.ProductRepository.GetByIDAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            //_context.Products.Remove(product);
            //await _context.SaveChangesAsync();
            _uow.ProductRepository.Delete(id);
            await _uow.SaveAsync();

            return NoContent();
        }

        private bool ProductExists(int id)
        {
            //return _context.Products.Any(e => e.Id == id);
            return _uow.ProductRepository.Get(e => e.Id == id).Any();
        }
    }
}
