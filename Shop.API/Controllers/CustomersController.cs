using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    public class CustomersController : ControllerBase
    {
        //private readonly ShopContext _context;
        private readonly IUnitOfWork _uow;

        //public CustomersController(ShopContext context)
        public CustomersController(IUnitOfWork uow)
        {
            //_context = context;
            _uow = uow;
        }

        // GET: api/Customers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Customer>>> GetCustomers()
        {
            //return await _context.Customers.ToListAsync();
            var customers = await _uow.CustomerRepository.GetAsync(includes: c => c.Orders);
            return customers.ToList();
        }

        // GET: api/Customers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Customer>> GetCustomer(int id)
        {
            //var customer = await _context.Customers.FindAsync(id);
            var customer = await _uow.CustomerRepository.GetByIDAsync(id);

            if (customer == null)
            {
                return NotFound();
            }

            return customer;
        }

        // PUT: api/Customers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCustomer(int id, Customer customer)
        {
            if (id != customer.Id)
            {
                return BadRequest();
            }

            //_context.Entry(customer).State = EntityState.Modified;
            _uow.CustomerRepository.Update(customer);

            try
            {
                //await _context.SaveChangesAsync();
                await _uow.SaveAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerExists(id))
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

        // POST: api/Customers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Customer>> PostCustomer(Customer customer)
        {
            //_context.Customers.Add(customer);
            //await _context.SaveChangesAsync();
            _uow.CustomerRepository.Insert(customer);
            await _uow.SaveAsync();

            return CreatedAtAction("GetCustomer", new { id = customer.Id }, customer);
        }

        // DELETE: api/Customers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            //var customer = await _context.Customers.FindAsync(id);
            var customer = await _uow.CustomerRepository.GetByIDAsync(id);
            if (customer == null)
            {
                return NotFound();
            }

            //_context.Customers.Remove(customer);
            //await _context.SaveChangesAsync();
            _uow.CustomerRepository.Delete(id);
            await _uow.SaveAsync();

            return NoContent();
        }

        private bool CustomerExists(int id)
        {
            //return _context.Customers.Any(e => e.Id == id);
            return _uow.CustomerRepository.Get(e => e.Id == id).Any();
        }
    }
}
