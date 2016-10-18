using System;
using System.Collections.Generic;
//these all implement an ICollection saying that they must implement these things. Must have a count property, add, clear, contains and so on. 
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Bangazon.Models
{
  public class Product
  {
    [Key]
    public int ProductId {get;set;}

    [Required]
    //required means that it will not go to the database without these parameters passed in. If this is not passed in, it will fail and not any part of it will get to the database
    [DataType(DataType.Date)]
    //the date must be of date type 
    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    //if there is no date that is passed in (which must be built in the way that the databse will accept it) then the database will automatically assign a date to it

    public DateTime DateCreated {get;set;}

    [Required]
    [StringLength(255)]
    //maximum length on that string
    public string Description { get; set; }

    [Required]
    public double Price { get; set; }
    public ICollection<LineItem> LineItems;
    //product.LineItem = ICollection<LineItem> Product.LineItem
  }
}