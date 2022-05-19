using Shop.DAL.Data;
using Shop.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.DAL
{
    public class UnitOfWork : IUnitOfWork
    {
        private ShopContext _context;
        private GenericRepository<Product> productRepository;
        private GenericRepository<Customer> customerRepository;
        private GenericRepository<Order> orderRepository;
        private GenericRepository<ProductOrder> productOrderRepository;

        public UnitOfWork(ShopContext context)
        {
            _context = context;
        }

        public GenericRepository<Product> ProductRepository
        {
            get
            {
                if(productRepository == null)
                {
                    productRepository = new GenericRepository<Product>(_context);
                }
                return productRepository;
            }
        }

        public GenericRepository<Customer> CustomerRepository
        {
            get
            {
                if (customerRepository == null)
                {
                    customerRepository = new GenericRepository<Customer>(_context);
                }
                return customerRepository;
            }
        }

        public GenericRepository<Order> OrderRepository
        {
            get
            {
                if (orderRepository == null)
                {
                    orderRepository = new GenericRepository<Order>(_context);
                }
                return orderRepository;
            }
        }

        public GenericRepository<ProductOrder> ProductOrderRepository
        {
            get
            {
                if (productOrderRepository == null)
                {
                    productOrderRepository = new GenericRepository<ProductOrder>(_context);
                }
                return productOrderRepository;
            }
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
