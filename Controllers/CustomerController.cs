using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Bangazon.Data;
using Bangazon.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
//bangazon.data now it knows where to go to get the informatoin 

namespace BangazonAPI.Controllers
{
    //entity framework - interacting with your database without hitting your database at all - the middle man between the models (classes) - entity framework handles the translation between all of the different databases with different standards. All of this is under a library in .Net. ORM object relational mapping.
    //DB context
    [ProducesAttribute("application/json")]
    //all will product json 
    [Route("[controller]")]
    //localhost5000/customers is the route
    public class CustomersController : Controller
    {
        private BangazonContext context;

        public CustomersController(BangazonContext ctx)
        {
            //a controller will have the context which is the database you're hitting
            context = ctx;
        }
        // GET api/values
        [HttpGet]
       
        public IActionResult Get()
        {
            IQueryable<object> customers = from customer in context.Customer select customer;
            //select customer.FirstName - only bring back the first name for each of the customers. In sql this is backwards - the select comes first 
            //out interface to the database is in context right now. context.Customer is the customer in the DB
            //from the customer table select everything, then hold it inside the customers variable. At the end it will hold a collection of customers that are in the database. If there are none, it will be null. Then the code below runs. 

            if (customers == null)
            {
                return NotFound();
                //NotFound() is a helper function. It is a valid 404 response back to the client. 
            }

            return Ok(customers);
            //put all the customers to the body of the response and send it back to the client

        }


        // GET api/values/5
      [HttpGet("{id}", Name = "GetCustomer")]
      //naming the HTTP response based off the request and reference the entire process elsewhere in the code. That is what happens when you post. Its a request and a response. Save the customer, create it in the database and defer to the get handler, pass in the ID that got created and construct the appropritae response back to the client then it gets to the OK statemetn. Instead of duplicating the code in the post method - instead of creating the customer, then turing around and getting the customer back out with teh id on it. 
        public IActionResult Get([FromRoute] int id)
        {
            try
            {
                Customer customer = context.Customer.Single(m => m.CustomerId == id);
                //i only wnat one where the id equals the id from the route

                if (customer == null)
                {
                    return NotFound();
                }
                
                return Ok(customer);
            }
            catch (System.InvalidOperationException ex)
            {
                return NotFound();
            }
        }
        // POST api/values
        
        [HttpPost]
        public IActionResult Post([FromBody] Customer customer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            context.Customer.Add(customer);
            //want to add it, but it is not posted yet
            try
            {
                context.SaveChanges();
                //saves data to database
            }
            catch (DbUpdateException)
            {
                if (CustomerExists(customer.CustomerId))
                {
                    return new StatusCodeResult(StatusCodes.Status409Conflict);
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("GetCustomer", new { id = customer.CustomerId }, customer);
            //this is a lot of magic as well - 
        }

        // PUT api/values/5
        [HttpPut("{id}", Name = "UpdateCustomer")]
        public IActionResult Put(int id, [FromBody] Customer customer)
        {
            if (id != customer.CustomerId)
            {
                return BadRequest(ModelState);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                context.Customer.Update(customer);
                context.SaveChanges();
                
                return Ok(customer);
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
           Customer customer = context.Customer.Single(m => m.CustomerId == id);

           if (customer == null)
           {
              return NotFound();
           }

           try 
           {
               context.Customer.Remove(customer);
               context.SaveChanges(); 

               return Ok(customer);   
           }
           
           catch (System.InvalidOperationException ex)
           {
                return NotFound();
           }
        }
         private bool CustomerExists(int id)
        {
            return context.Customer.Count(currentCustomer => currentCustomer.CustomerId == id) > 0;
        }
    }
}


//  public IEnumerable<string> Get()
//         {
//             return new string[] { "value1", "value2" };
//         }

//         // GET api/values/5
//         [HttpGet("{id}")]
//         //IActionResult I want to perform an action and get a result. Read from the DB and return something. Perform an actual action. I want to query things from this object - from customer