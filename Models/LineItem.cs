using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Bangazon.Models
{
  public class LineItem
  {
    [Key]
    public int LineItemId {get;set;}

    public int OrderId { get; set; }
    public Order Order { get; set; }
    public int ProductId { get; set; }
    public Product Product { get; set; }
  }
}
//common name join table - join two thigns together in a many to many relationship. This is used to define the relationship.