using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Bangazon.Data;
using Bangazon.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
 

namespace BangazonAPI.Controllers
{
    [ProducesAttribute("application/json")]
    [Route("[controller]")]

    public class OrdersController : Controller
    {
        private BangazonContext context;

        public OrdersController(BangazonContext ctx)
        {
            //a controller will have the context which is the database you're hitting
            context = ctx;
        }

        [HttpGet]
       
        public IActionResult Get()
        {
            IQueryable<object> orders = from order in context.Order select order; 

            if (orders == null)
            {
                return NotFound();
            }

            return Ok(orders);
            //put all the customers to the body of the response and send it back to the client

        }

        [HttpGet("{id}", Name = "GetOrder")]
        public IActionResult Get([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                Order order = context.Order.Single(m => m.OrderId == id);
                //i only wnat one where the id equals the id from the route

                if (order == null)
                {
                    return NotFound();
                }
                
                return Ok(order);
            }
            catch (System.InvalidOperationException ex)
            {
                return NotFound();
            }
        }
        // POST api/values
        
        [HttpPost]
        public IActionResult Post([FromBody] Order order)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            context.Order.Add(order);
            //want to add it, but it is not posted yet
            try
            {
                context.SaveChanges();
                //saves data to database
            }
            catch (DbUpdateException)
            {
                if (OrderExists(order.OrderId))
                {
                    return new StatusCodeResult(StatusCodes.Status409Conflict);
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("GetOrder", new { id = order.OrderId }, order);
            //this is a lot of magic as well - 
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Order order)
        {
            if (id != order.OrderId)
            {
                return BadRequest(ModelState);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                context.Order.Update(order);
                context.SaveChanges();
                
                return Ok(order);
            }
            catch (System.InvalidOperationException ex)
            {
                return NotFound();
            }
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
           Order order = context.Order.Single(m => m.OrderId == id);

           if (order == null)
           {
              return NotFound();
           }

           try 
           {
               context.Order.Remove(order);
               context.SaveChanges(); 

               return Ok(order);   
           }
           
           catch (System.InvalidOperationException ex)
           {
                return NotFound();
           }
        }
         private bool OrderExists(int id)
        {
            return context.Order.Count(e => e.OrderId == id) > 0;
        }
    }
}