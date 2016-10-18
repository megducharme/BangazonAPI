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
    [ProducesAttribute("application/json")]
    [RouteAttribute("[controller]")]
    public class LineItemController: Controller
    {
        private BangazonContext context;
        public LineItemController(BangazonContext ctx)
        {
            context = ctx;
        }
        [HttpGet]
        /////// update
        /////// update
        /////// update
        /////// update
        /////// update
        public IActionResult Get(int orderId)
        {
            IQueryable<object> lineitems = from lineitem in context.LineItem select lineitem;
            if (lineitems == null)
            {
                return NotFound();
            }

            return Ok(lineitems);
        }

        [HttpPost]
        public IActionResult Post([FromBody] LineItem lineitem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            context.LineItem.Add(lineitem);
            //want to add it, but it is not posted yet
            try
            {
                context.SaveChanges();
                //saves data to database
            }
            catch (DbUpdateException)
            {
                if (LineItemExists(lineitem.LineItemId))
                {
                    return new StatusCodeResult(StatusCodes.Status409Conflict);
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("GetLineItem", new { id = lineitem.LineItemId }, lineitem);
            //this is a lot of magic as well  
        }  
        public IActionResult Delete(int id)
        {
           LineItem lineitem = context.LineItem.Single(m => m.LineItemId == id);

           if (lineitem == null)
           {
              return NotFound();
           }

           try 
           {
               context.LineItem.Remove(lineitem);
               context.SaveChanges(); 

               return Ok(lineitem);   
           }
           
           catch (System.InvalidOperationException ex)
           {
                return NotFound();
           }
        }
        private bool LineItemExists(int id)
        {
            return context.LineItem.Count(currentLineItem => currentLineItem.LineItemId == id) > 0;
        }
    }
}