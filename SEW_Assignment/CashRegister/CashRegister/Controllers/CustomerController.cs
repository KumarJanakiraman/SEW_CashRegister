using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using CashRegister.Model;
using CashRegister.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CashRegister.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerRepository _repository;
        private readonly ILogger<CustomerController> _logger;
        public CustomerController(ICustomerRepository repository, ILoggerFactory loggerFactory)
        {
            _repository = repository;
            _logger = loggerFactory.CreateLogger<CustomerController>();
        }


        [Route("Customers")]
        [HttpGet()]
        [ProducesResponseType(typeof(List<Item>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetItemMasters()
        {
            var itemMaster = await _repository.GetCustomersAsync();
            if (itemMaster == null)
            {
                return Ok(new List<Customer>());
            }

            return Ok(itemMaster);
        }

        [Route("{CustomerID:int}")]
        [HttpGet]
        [ProducesResponseType(typeof(Customer), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetCustomer(long CustomerID)
        {
            try
            {
                var Customer = await _repository.GetCustomerAsync(CustomerID);
                return Ok(Customer);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }


        [Route("add")]
        [HttpPost]
        [ProducesResponseType(typeof(Customer), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> AddCustomer([FromBody]Customer value)
        {
          
            var _Customer = await _repository.AddCustomerAsync(value);
            return Ok(_Customer);
        }

        [Route("update")]
        [HttpPost]
        [ProducesResponseType(typeof(Customer), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> UpdateItem([FromBody]Customer value)
        {
            var _Customer = await _repository.GetCustomerAsync(value.CustID);
            if (_Customer == null)
            {
                return NotFound(new { Message = $"Customer  {value.CustID.ToString()}  does'not exist" });
            }
           
            var Customer = await _repository.UpdateCustomerAsync(_Customer);
            return Ok(Customer);
        }

    }
}