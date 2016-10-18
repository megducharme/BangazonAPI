using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
//Everythign inside the [] is an annotation. We need data for a database.
using System.ComponentModel.DataAnnotations.Schema;
// ^^^ Steve does not know what the difference between the DataAnnotations and DataAnnotations.Schema is so I am not going to worry about it, either
//for database generated 


namespace Bangazon.Models
{
  public class Customer
  //the name of the class is the name of the table 
  {
    [Key]
    //new instance of the key attribute class
    //the primary key - it's name will be CustomerId - this is the column in the database of type integer 
    //there is an assumption that this is required 
    public int CustomerId {get;set;}

    [Required]
    [DataType(DataType.Date)]
    //the type has to be a Date
    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    //I want the database to generate a date
    public DateTime DateCreated {get;set;}
    //another annotation - this is required, an empty value cannot be put in, and it better be of type date. I want the database to generate the value of this. 

    [Required]
    public string FirstName { get; set; }

    [Required]
    public string LastName { get; set; }

    public ICollection<PaymentType> PaymentTypes;
    //this is the "many" side of the equation. This is establishing the relationship as opposed to actually housing the collection of payment types the customer can use
    //this combined with the customer on the payment will set up the foreign key relationship
  }
}

//this is how you define the database with properties