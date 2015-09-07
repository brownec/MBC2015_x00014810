/*
Student Name: 	Cliff Browne
Student ID:		X00014810
Module:			Project 4th Year
Course:			Computing
College:		I.T Tallaght, Dublin
*/

using System;
using System.ComponentModel.DataAnnotations;

namespace MBC2015.Models
{
    public class Budget
    {
        [Display(Name = "Budget ID")]
        public int BudgetId { get; set; }

        [Display(Name = "Budget Name")]
        public String BudgetName { get; set; }

        // Budget Start Date - Nullable
        [Required]
        [DataType(DataType.Date, ErrorMessage = "Please enter a valid date in the format dd/mm/yyyy")]
        [Display(Name = "Budget Start Date")]
        public DateTime? BudgetStartDate { get; set; }

        // Budget End Date - Nullable
        [Required]
        [DataType(DataType.Date, ErrorMessage = "Please enter a valid date in the format dd/mm/yyyy")]
        [Display(Name = "Budget End Date")]
        public DateTime? BudgetEndDate { get; set; }

        // ---------- INCOME ----------
        [Required(ErrorMessage = "Must include Primary Income value!")]
        [Range(0, double.MaxValue, ErrorMessage = "The value must be greater than 0")]
        [DisplayFormat(DataFormatString = "{0:F2}", ApplyFormatInEditMode = true)]
        [Display(Name = "Income Amount")]
        public double IncomePrimaryAmount { get; set; }

        [DisplayFormat(DataFormatString = "{0:F2}", ApplyFormatInEditMode = true)]
        [Range(0, double.MaxValue, ErrorMessage = "The value must be greater than 0")]
        [Display(Name = "Additional Income")]
        public double IncomeAdditionalAmount { get; set; }

        [DisplayFormat(DataFormatString = "{0:F2}", ApplyFormatInEditMode = true)]
        [Range(0, double.MaxValue, ErrorMessage = "The value must be greater than 0")]
        [Display(Name = "Total Income")]
        public double TotalIncome { get; set; }

        // ---------- CAR EXPENDITURE ----------
        [DisplayFormat(DataFormatString = "{0:F2}", ApplyFormatInEditMode = true)]
        [Range(0, double.MaxValue, ErrorMessage = "The value must be greater than 0")]
        [Display(Name = "Car Tax")]
        public double CarTaxAmount { get; set; }

        [DisplayFormat(DataFormatString = "{0:F2}", ApplyFormatInEditMode = true)]
        [Range(0, double.MaxValue, ErrorMessage = "The value must be greater than 0")]
        [Display(Name = "Car Insurance")]
        public double CarInsuranceAmount { get; set; }

        [DisplayFormat(DataFormatString = "{0:F2}", ApplyFormatInEditMode = true)]
        [Range(0, double.MaxValue, ErrorMessage = "The value must be greater than 0")]
        [Display(Name = "Car Maintenance")]
        public double CarMaintenanceAmount { get; set; }

        [DisplayFormat(DataFormatString = "{0:F2}", ApplyFormatInEditMode = true)]
        [Range(0, double.MaxValue, ErrorMessage = "The value must be greater than 0")]
        [Display(Name = "Fuel Amount")]
        public double CarFuelAmount { get; set; }

        [DisplayFormat(DataFormatString = "{0:F2}", ApplyFormatInEditMode = true)]
        [Range(0, double.MaxValue, ErrorMessage = "The value must be greater than 0")]
        [Display(Name = "NCT Amount")]
        public double CarNctAmount { get; set; }

        [DisplayFormat(DataFormatString = "{0:F2}", ApplyFormatInEditMode = true)]
        [Range(0, double.MaxValue, ErrorMessage = "The value must be greater than 0")]
        [Display(Name = "Toll Charges")]
        public double CarTollChargesAmount { get; set; }

        [DisplayFormat(DataFormatString = "{0:F2}", ApplyFormatInEditMode = true)]
        [Range(0, double.MaxValue, ErrorMessage = "The value must be greater than 0")]
        [Display(Name = "Car Other")]
        public double CarExpenseOtherAmount { get; set; }

        [DisplayFormat(DataFormatString = "{0:F2}", ApplyFormatInEditMode = true)]
        [Range(0, double.MaxValue, ErrorMessage = "The value must be greater than 0")]
        [Display(Name = "Total Car Expenses")]
        public double TotalCarExpenses { get; set; }

        // ---------- HOUSEHOLD EXPENDITURE ----------
        [DisplayFormat(DataFormatString = "{0:F2}", ApplyFormatInEditMode = true)]
        [Range(0, double.MaxValue, ErrorMessage = "The value must be greater than 0")]
        [Display(Name = "Rent/Mortgage")]
        public double HouseholdRentMortgageAmount { get; set; }

        [DisplayFormat(DataFormatString = "{0:F2}", ApplyFormatInEditMode = true)]
        [Range(0, double.MaxValue, ErrorMessage = "The value must be greater than 0")]
        [Display(Name = "Groceries")]
        public double HouseholdGroceryAmount { get; set; }

        [DisplayFormat(DataFormatString = "{0:F2}", ApplyFormatInEditMode = true)]
        [Range(0, double.MaxValue, ErrorMessage = "The value must be greater than 0")]
        [Display(Name = "Clothing")]
        public double HouseholdClothingAmount { get; set; }

        [DisplayFormat(DataFormatString = "{0:F2}", ApplyFormatInEditMode = true)]
        [Range(0, double.MaxValue, ErrorMessage = "The value must be greater than 0")]
        [Display(Name = "Education Fees")]
        public double HouseholdEducationFeesAmount { get; set; }

        [DisplayFormat(DataFormatString = "{0:F2}", ApplyFormatInEditMode = true)]
        [Range(0, double.MaxValue, ErrorMessage = "The value must be greater than 0")]
        [Display(Name = "School Supplies")]
        public double HouseholdSchoolSuppliesAmount { get; set; }

        [DisplayFormat(DataFormatString = "{0:F2}", ApplyFormatInEditMode = true)]
        [Range(0, double.MaxValue, ErrorMessage = "The value must be greater than 0")]
        [Display(Name = "Medical Expenses")]
        public double HouseholdMedicalExpensesAmount { get; set; }

        [DisplayFormat(DataFormatString = "{0:F2}", ApplyFormatInEditMode = true)]
        [Range(0, double.MaxValue, ErrorMessage = "The value must be greater than 0")]
        [Display(Name = "Household Insurance")]
        public double HouseholdInsuranceAmount { get; set; }

        [DisplayFormat(DataFormatString = "{0:F2}", ApplyFormatInEditMode = true)]
        [Range(0, double.MaxValue, ErrorMessage = "The value must be greater than 0")]
        [Display(Name = "Household Maintenance")]
        public double HouseholdMaintenanceAmount { get; set; }

        [DisplayFormat(DataFormatString = "{0:F2}", ApplyFormatInEditMode = true)]
        [Range(0, double.MaxValue, ErrorMessage = "The value must be greater than 0")]
        [Display(Name = "Household Other")]
        public double HouseholdExpenseOtherAmount { get; set; }

        [DisplayFormat(DataFormatString = "{0:F2}", ApplyFormatInEditMode = true)]
        [Range(0, double.MaxValue, ErrorMessage = "The value must be greater than 0")]
        [Display(Name = "Total Household Expenses")]
        public double TotalHouseholdExpenses { get; set; }

        // ---------- PERSONAL EXPENDITURE ----------
        [DisplayFormat(DataFormatString = "{0:F2}", ApplyFormatInEditMode = true)]
        [Range(0, double.MaxValue, ErrorMessage = "The value must be greater than 0")]
        [Display(Name = "Social")]
        public double PersonalSocialAmount { get; set; }

        [DisplayFormat(DataFormatString = "{0:F2}", ApplyFormatInEditMode = true)]
        [Range(0, double.MaxValue, ErrorMessage = "The value must be greater than 0")]
        [Display(Name = "Gym Membership")]
        public double PersonalGymMembershipAmount { get; set; }

        [DisplayFormat(DataFormatString = "{0:F2}", ApplyFormatInEditMode = true)]
        [Range(0, double.MaxValue, ErrorMessage = "The value must be greater than 0")]
        [Display(Name = "Sports Membership")]
        public double PersonalSportsExpenseAmount { get; set; }

        [DisplayFormat(DataFormatString = "{0:F2}", ApplyFormatInEditMode = true)]
        [Range(0, double.MaxValue, ErrorMessage = "The value must be greater than 0")]
        [Display(Name = "Holiday Expenses")]
        public double PersonalHolidayExpenseAmount { get; set; }

        [DisplayFormat(DataFormatString = "{0:F2}", ApplyFormatInEditMode = true)]
        [Range(0, double.MaxValue, ErrorMessage = "The value must be greater than 0")]
        [Display(Name = "Savings")]
        public double PersonalSavingsAmount { get; set; }

        [DisplayFormat(DataFormatString = "{0:F2}", ApplyFormatInEditMode = true)]
        [Range(0, double.MaxValue, ErrorMessage = "The value must be greater than 0")]
        [Display(Name = "Loan Repayment")]
        public double PersonalLoanRepaymentAmount { get; set; }

        [DisplayFormat(DataFormatString = "{0:F2}", ApplyFormatInEditMode = true)]
        [Range(0, double.MaxValue, ErrorMessage = "The value must be greater than 0")]
        [Display(Name = "Health Insurance")]
        public double PersonalHealthInsuranceAmount { get; set; }

        [DisplayFormat(DataFormatString = "{0:F2}", ApplyFormatInEditMode = true)]
        [Range(0, double.MaxValue, ErrorMessage = "The value must be greater than 0")]
        [Display(Name = "Personal Other")]
        public double PersonalExpenseOtherAmount { get; set; }

        [DisplayFormat(DataFormatString = "{0:F2}", ApplyFormatInEditMode = true)]
        [Range(0, double.MaxValue, ErrorMessage = "The value must be greater than 0")]
        [Display(Name = "Total Personal Expenses")]
        public double TotalPersonalExpenses { get; set; }

        // ---------- TRAVEL EXPENDITURE ----------
        [DisplayFormat(DataFormatString = "{0:F2}", ApplyFormatInEditMode = true)]
        [Range(0, double.MaxValue, ErrorMessage = "The value must be greater than 0")]
        [Display(Name = "Bus")]
        public double TravelBusAmount { get; set; }

        [DisplayFormat(DataFormatString = "{0:F2}", ApplyFormatInEditMode = true)]
        [Range(0, double.MaxValue, ErrorMessage = "The value must be greater than 0")]
        [Display(Name = "Luas")]
        public double TravelLuasAmount { get; set; }

        [DisplayFormat(DataFormatString = "{0:F2}", ApplyFormatInEditMode = true)]
        [Range(0, double.MaxValue, ErrorMessage = "The value must be greater than 0")]
        [Display(Name = "Taxi")]
        public double TravelTaxiAmount { get; set; }

        [DisplayFormat(DataFormatString = "{0:F2}", ApplyFormatInEditMode = true)]
        [Range(0, double.MaxValue, ErrorMessage = "The value must be greater than 0")]
        [Display(Name = "Train")]
        public double TravelTrainAmount { get; set; }

        [DisplayFormat(DataFormatString = "{0:F2}", ApplyFormatInEditMode = true)]
        [Range(0, double.MaxValue, ErrorMessage = "The value must be greater than 0")]
        [Display(Name = "Plane")]
        public double TravelPlaneAmount { get; set; }

        [DisplayFormat(DataFormatString = "{0:F2}", ApplyFormatInEditMode = true)]
        [Range(0, double.MaxValue, ErrorMessage = "The value must be greater than 0")]
        [Display(Name = "Travel Other")]
        public double TravelExpenseOtherAmount { get; set; }

        [DisplayFormat(DataFormatString = "{0:F2}", ApplyFormatInEditMode = true)]
        [Range(0, double.MaxValue, ErrorMessage = "The value must be greater than 0")]
        [Display(Name = "Total Travel Expenses")]
        public double TotalTravelExpenses { get; set; }

        // ---------- UTILITY BILLS EXPENDITURE ----------
        [DisplayFormat(DataFormatString = "{0:F2}", ApplyFormatInEditMode = true)]
        [Range(0, double.MaxValue, ErrorMessage = "The value must be greater than 0")]
        [Display(Name = "Electricity")]
        public double UtilityBillElectricityAmount { get; set; }

        [DisplayFormat(DataFormatString = "{0:F2}", ApplyFormatInEditMode = true)]
        [Range(0, double.MaxValue, ErrorMessage = "The value must be greater than 0")]
        [Display(Name = "Gas")]
        public double UtilityBillGasAmount { get; set; }

        [DisplayFormat(DataFormatString = "{0:F2}", ApplyFormatInEditMode = true)]
        [Range(0, double.MaxValue, ErrorMessage = "The value must be greater than 0")]
        [Display(Name = "Refuse/Waste")]
        public double UtilityBillRefuseCollectionAmount { get; set; }

        [DisplayFormat(DataFormatString = "{0:F2}", ApplyFormatInEditMode = true)]
        [Range(0, double.MaxValue, ErrorMessage = "The value must be greater than 0")]
        [Display(Name = "Irish Water")]
        public double UtilityBillIrishWaterAmount { get; set; }

        [DisplayFormat(DataFormatString = "{0:F2}", ApplyFormatInEditMode = true)]
        [Range(0, double.MaxValue, ErrorMessage = "The value must be greater than 0")]
        [Display(Name = "TV Subscription")]
        public double UtilityBillTVAmount { get; set; }

        [DisplayFormat(DataFormatString = "{0:F2}", ApplyFormatInEditMode = true)]
        [Range(0, double.MaxValue, ErrorMessage = "The value must be greater than 0")]
        [Display(Name = "Phone/Mobile")]
        public double UtilityBillPhoneBillAmount { get; set; }

        [DisplayFormat(DataFormatString = "{0:F2}", ApplyFormatInEditMode = true)]
        [Range(0, double.MaxValue, ErrorMessage = "The value must be greater than 0")]
        [Display(Name = "Broadband/Internet")]
        public double UtilityBillBroadbandAmount { get; set; }

        [DisplayFormat(DataFormatString = "{0:F2}", ApplyFormatInEditMode = true)]
        [Range(0, double.MaxValue, ErrorMessage = "The value must be greater than 0")]
        [Display(Name = "Utility Bill Other")]
        public double UtilityBillOtherExpenseAmount { get; set; }

        [DisplayFormat(DataFormatString = "{0:F2}", ApplyFormatInEditMode = true)]
        [Range(0, double.MaxValue, ErrorMessage = "The value must be greater than 0")]
        [Display(Name = "Total Utility Bill Expenses")]
        public double TotalUtilityBillExpenses { get; set; }

        [DisplayFormat(DataFormatString = "{0:F2}", ApplyFormatInEditMode = true)]
        [Display(Name = "Total Expenditure")]
        public double TotalExpenses { get; set; }

        [DisplayFormat(DataFormatString = "{0:F2}", ApplyFormatInEditMode = true)]
        [Display(Name = "Budget Balance")]
        public double BudgetBalance { get; set; }

        // navigation properties
        [Display(Name = "User ID")]
        public int BudgetUserId { get; set; }
        public virtual BudgetUser BudgetUser { get; set; }
    }
}