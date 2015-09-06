using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using MBC2015.Models;

namespace MBC2015.Models
{
    public enum Counties
    {
        Antrim, Armagh, Carlow, Cavan, Clare, Cork, Derry, Donegal, Down, Dublin,
        Dublin1, Dublin2, Dublin3, Dublin4, Dublin5, Dublin6,
        Dublin6W, Dublin7, Dublin8, Dublin9, Dublin10, Dublin11,
        Dublin12, Dublin13, Dublin14, Dublin15, Dublin16, Dublin17,
        Dublin18, Dublin20, Dublin22, Dublin24, Fermanagh, Galway,
        Kerry, Kildare, Kilkenny, Laois, Leitrim, Limerick, Longford, Louth, Mayo, Meath, Monaghan,
        Offaly, Roscommon, Sligo, Tipperary, Tyrone, Waterford, Westmeath, Wexford, Wicklow, Other
    };
    public class BudgetUser
    {
        public int BudgetUserId { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Last name cannot be longer than 50 characters.")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "First name cannot be longer than 50 characters.")]
        [Column("FirstName")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        // DOB is nullable (not 01/01/0001 at launch)
        [Required]
        [DataType(DataType.Date, ErrorMessage = "Please enter a valid date in the format dd/mm/yyyy")]
        [Display(Name = "D.O.B")]
        public DateTime? DateOfBirth { get; set; }

        [Required(ErrorMessage = "Address Line 1 is Mandatory")]
        [StringLength(50, ErrorMessage = "Address Line 1 cannot be more than 50 characters.")]
        [Display(Name = "Address Line 1")]
        public String AddressLine1 { get; set; }

        [StringLength(50, ErrorMessage = "Address Line 2 cannot be more than 50 characters.")]
        [Display(Name = "Address Line 2")]
        public String AddressLine2 { get; set; }

        [StringLength(50, ErrorMessage = "Town cannot be more than 50 characters.")]
        [Display(Name = "Town")]
        public String Town { get; set; }

        // [StringLength(50, ErrorMessage = "County cannot be more than 50 characters.")]
        [Display(Name = "County")]
        public Counties Counties { get; set; }

        [StringLength(50, ErrorMessage = "Country cannot be more than 50 characters.")]
        [Display(Name = "Country")]
        public String Country { get; set; }

        [Display(Name = "Post Code")]
        public String PostCode { get; set; }

        [Required(ErrorMessage = "Contact Number is Mandatory")]
        [Display(Name = "Contact No.")]
        public String ContactNo { get; set; }

        //public Budget budget {get;set}
        public virtual ICollection<Budget> Budgets { get; set; }
    }
}