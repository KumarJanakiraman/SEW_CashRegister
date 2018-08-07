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
    public class ItemMasterController : ControllerBase
    {
        private readonly IItemRepository _repository;
        private readonly ILogger<ItemMasterController> _logger;
        public ItemMasterController(IItemRepository repository, ILoggerFactory loggerFactory)
        {
            _repository = repository;
            _logger = loggerFactory.CreateLogger<ItemMasterController>();
        }


        [Route("items")]
        [HttpGet()]
        [ProducesResponseType(typeof(List<Item>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetItems()
        {
            var itemMaster = await _repository.GetItemsAsync();
            if (itemMaster == null)
            {
                return Ok(new List<Item>());
            }

            return Ok(itemMaster);
        }

        [Route("{itemId:int}")]
        [HttpGet]
        [ProducesResponseType(typeof(Item), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetItem(long itemId)
        {
            try
            {
                var item = await _repository.GetItemMasterAsync(itemId);
                return Ok(item);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }


        [Route("add")]
        [HttpPost]
        [ProducesResponseType(typeof(Item), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> AddItem([FromBody]Item value)
        {
            var validation = value.Validate(); //validation for item master
            if (validation.Count() > 0)
                return Ok(validation);


            var exists = await _repository.GetItemByNameAsync(value.ItemName.ToString());
            if (exists != null)
            {
                return Ok(new { Message = $"Item name {value.ItemName.ToString()} already exists" });
            }
            var _itemMaster = await _repository.AddItemMasterAsync(value);
            return Ok(_itemMaster);
        }

        [Route("update")]
        [HttpPost]
        [ProducesResponseType(typeof(Item), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> UpdateItem([FromBody]Item value)
        {
            var validation = value.Validate(); //validation for item master
            if (validation.Count() > 0)
                return Ok(validation);

            var itemmaster = await _repository.GetItemMasterAsync(value.ItemID);
            if (itemmaster == null)
            {
                return NotFound(new { Message = $"Item {value.ItemName.ToString()} does'not exist" });
            }
            itemmaster = value;
            var _itemMaster = await _repository.UpdateItemMasterAsync(itemmaster);
            return Ok(_itemMaster);
        }

    }
}