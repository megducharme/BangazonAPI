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

    public class ProductsController : Controller
    {
        private BangazonContext context;

        public ProductsController(BangazonContext ctx)
        {
            context = ctx;
        }

        [HttpGet]
       
        public IActionResult Get()
        {
            IQueryable<object> products = from product in context.Product select product; 

            if (products == null)
            {
                return NotFound();
            }

            return Ok(products);
        }

        [HttpGet("{id}", Name = "GetProduct")]
        public IActionResult Get([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                Product product = context.Product.Single(m => m.ProductId == id);
                //i only wnat one where the id equals the id from the route

                if (product == null)
                {
                    return NotFound();
                }
                
                return Ok(product);
            }
            catch (System.InvalidOperationException ex)
            {
                return NotFound();
            }
        }
        // POST api/values
        
        [HttpPost]
        public IActionResult Post([FromBody] Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
                //custom return values for each exception catch. Model state = model binding validation
            }

            context.Product.Add(product);
            //want to add it, but it is not posted yet. product = Product product which means a new instance of a product of type Product
            try
            {
                context.SaveChanges();
                //saves data to database
            }
            catch (DbUpdateException)
            {
                if (ProductExists(product.ProductId))
                {
                    //if the product exists then return status message
                    return new StatusCodeResult(StatusCodes.Status409Conflict);
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("GetProduct", new { id = product.ProductId }, product);
            //this is a lot of magic as well - the name of the route to produce the URL
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        //route template - may not be null
        public IActionResult Put(int id, [FromBody] Product product)
        {
            if (id != product.ProductId)
            {
                return BadRequest(ModelState);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                context.Product.Update(product);
                context.SaveChanges();
                
                return Ok(product);
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
           Product product = context.Product.Single(m => m.ProductId == id);

           if (product == null)
           {
              return NotFound();
           }

           try 
           {
               context.Product.Remove(product);
               context.SaveChanges(); 

               return Ok(product);   
           }
           
           catch (System.InvalidOperationException ex)
           {
                return NotFound();
           }
        }
         private bool ProductExists(int id)
        {
            return context.Product.Count(e => e.ProductId == id) > 0;
            //context is the database you're hitting, Product is the array that the products are in and if the product count is greater than 0 then there are products in there, therefore they exist
        }
    }
}