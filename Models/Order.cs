using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Bangazon.Models
{
  public class Order
  {
    [Key]
    public int OrderId {get;set;}

    [Required]
    [DataType(DataType.Date)]
    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public DateTime DateCreated {get;set;}

    [DataType(DataType.Date)]
    public DateTime? DateCompleted {get;set;}

    public int CustomerId {get;set;}
    public Customer Customer {get;set;}

    public int? PaymentTypeId {get;set;}
    //create the order wtihout paying for it...yet. THis is the foreing key for the payment type table. 
    //PaymentTypeId is the primary key on the table PaymentType 
    public PaymentType PaymentType {get;set;} 

    public ICollection<LineItem> LineItems;
  }
}