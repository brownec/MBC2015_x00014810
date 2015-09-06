namespace MBC2015.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Budgets",
                c => new
                    {
                        BudgetId = c.Int(nullable: false, identity: true),
                        BudgetName = c.String(),
                        BudgetStartDate = c.DateTime(nullable: false),
                        BudgetEndDate = c.DateTime(nullable: false),
                        IncomePrimaryAmount = c.Double(nullable: false),
                        IncomeAdditionalAmount = c.Double(nullable: false),
                        TotalIncome = c.Double(nullable: false),
                        CarTaxAmount = c.Double(nullable: false),
                        CarInsuranceAmount = c.Double(nullable: false),
                        CarMaintenanceAmount = c.Double(nullable: false),
                        CarFuelAmount = c.Double(nullable: false),
                        CarNctAmount = c.Double(nullable: false),
                        CarTollChargesAmount = c.Double(nullable: false),
                        CarExpenseOtherAmount = c.Double(nullable: false),
                        TotalCarExpenses = c.Double(nullable: false),
                        HouseholdRentMortgageAmount = c.Double(nullable: false),
                        HouseholdGroceryAmount = c.Double(nullable: false),
                        HouseholdClothingAmount = c.Double(nullable: false),
                        HouseholdEducationFeesAmount = c.Double(nullable: false),
                        HouseholdSchoolSuppliesAmount = c.Double(nullable: false),
                        HouseholdMedicalExpensesAmount = c.Double(nullable: false),
                        HouseholdInsuranceAmount = c.Double(nullable: false),
                        HouseholdMaintenanceAmount = c.Double(nullable: false),
                        HouseholdExpenseOtherAmount = c.Double(nullable: false),
                        TotalHouseholdExpenses = c.Double(nullable: false),
                        PersonalSocialAmount = c.Double(nullable: false),
                        PersonalGymMembershipAmount = c.Double(nullable: false),
                        PersonalSportsExpenseAmount = c.Double(nullable: false),
                        PersonalHolidayExpenseAmount = c.Double(nullable: false),
                        PersonalSavingsAmount = c.Double(nullable: false),
                        PersonalLoanRepaymentAmount = c.Double(nullable: false),
                        PersonalHealthInsuranceAmount = c.Double(nullable: false),
                        PersonalExpenseOtherAmount = c.Double(nullable: false),
                        TotalPersonalExpenses = c.Double(nullable: false),
                        TravelBusAmount = c.Double(nullable: false),
                        TravelLuasAmount = c.Double(nullable: false),
                        TravelTaxiAmount = c.Double(nullable: false),
                        TravelTrainAmount = c.Double(nullable: false),
                        TravelPlaneAmount = c.Double(nullable: false),
                        TravelExpenseOtherAmount = c.Double(nullable: false),
                        TotalTravelExpenses = c.Double(nullable: false),
                        UtilityBillElectricityAmount = c.Double(nullable: false),
                        UtilityBillGasAmount = c.Double(nullable: false),
                        UtilityBillRefuseCollectionAmount = c.Double(nullable: false),
                        UtilityBillIrishWaterAmount = c.Double(nullable: false),
                        UtilityBillTVAmount = c.Double(nullable: false),
                        UtilityBillPhoneBillAmount = c.Double(nullable: false),
                        UtilityBillBroadbandAmount = c.Double(nullable: false),
                        UtilityBillOtherExpenseAmount = c.Double(nullable: false),
                        TotalUtilityBillExpenses = c.Double(nullable: false),
                        TotalExpenses = c.Double(nullable: false),
                        BudgetBalance = c.Double(nullable: false),
                        BudgetUserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.BudgetId)
                .ForeignKey("dbo.BudgetUsers", t => t.BudgetUserId, cascadeDelete: true)
                .Index(t => t.BudgetUserId);
            
            CreateTable(
                "dbo.BudgetUsers",
                c => new
                    {
                        BudgetUserId = c.Int(nullable: false, identity: true),
                        LastName = c.String(nullable: false, maxLength: 50),
                        FirstName = c.String(nullable: false, maxLength: 50),
                        DateOfBirth = c.DateTime(nullable: false),
                        AddressLine1 = c.String(nullable: false, maxLength: 50),
                        AddressLine2 = c.String(maxLength: 50),
                        Town = c.String(maxLength: 50),
                        Counties = c.Int(nullable: false),
                        Country = c.String(maxLength: 50),
                        PostCode = c.String(),
                        ContactNo = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.BudgetUserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Budgets", "BudgetUserId", "dbo.BudgetUsers");
            DropIndex("dbo.Budgets", new[] { "BudgetUserId" });
            DropTable("dbo.BudgetUsers");
            DropTable("dbo.Budgets");
        }
    }
}
