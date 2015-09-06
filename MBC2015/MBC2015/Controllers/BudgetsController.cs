using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DotNet.Highcharts;
using DotNet.Highcharts.Enums;
using DotNet.Highcharts.Helpers;
using DotNet.Highcharts.Options;
using Point = DotNet.Highcharts.Options.Point;
using System.Drawing;
using MBC2015.Models;

namespace MBC2015.Controllers
{
    public class BudgetsController : Controller
    {
        private MyDbConnection db = new MyDbConnection();

        //// Global variables to be accessed throughout controller methods
        //double globalTotalIncome = 0;
        //double globalTotalCarExpenses = 0;
        //double globalTotalHouseholdExpenses = 0;
        //double globalTotalPersonalExpenses = 0;
        //double globalTotalTravelExpenses = 0;
        //double globalTotalUtilityBillExpenses = 0;
        //double globalTotalExpenses = 0;
        //double globalBudgetBalance = 0;

        // GET: Budgets
        public ActionResult Index()
        {
            var budgets = db.Budgets.Include(b => b.BudgetUser);
            return View(budgets.ToList());
        }

        // GET: Budgets/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Budget budget = db.Budgets.Find(id);
            if (budget == null)
            {
                return HttpNotFound();
            }
            return View(budget);
        }

        // GET: Budgets/Create
        public ActionResult Create(int id)
        {
            Budget b = new Budget();
            b.BudgetUserId = id;
            // ViewBag.BudgetUserId = new SelectList(db.BudgetUsers, "BudgetUserId", "LastName");
            return View(b);
        }

        // POST: Budgets/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "BudgetId,BudgetUserId,BudgetName,BudgetStartDate,BudgetEndDate,IncomePrimaryAmount,IncomeAdditionalAmount,CarTaxAmount,CarInsuranceAmount,CarMaintenanceAmount,CarFuelAmount,CarNctAmount,CarTollChargesAmount,CarExpenseOtherAmount,HouseholdRentMortgageAmount,HouseholdGroceryAmount,HouseholdClothingAmount,HouseholdEducationFeesAmount,HouseholdSchoolSuppliesAmount,HouseholdMedicalExpensesAmount,HouseholdInsuranceAmount,HouseholdMaintenanceAmount,HouseholdExpenseOtherAmount,PersonalSocialAmount,PersonalGymMembershipAmount,PersonalSportsExpenseAmount,PersonalHolidayExpenseAmount,PersonalSavingsAmount,PersonalLoanRepaymentAmount,PersonalHealthInsuranceAmount,PersonalExpenseOtherAmount,TravelBusAmount,TravelLuasAmount,TravelTaxiAmount,TravelTrainAmount,TravelPlaneAmount,TravelExpenseOtherAmount,UtilityBillElectricityAmount,UtilityBillGasAmount,UtilityBillRefuseCollectionAmount,UtilityBillIrishWaterAmount,UtilityBillTVAmount,UtilityBillPhoneBillAmount,UtilityBillBroadbandAmount,UtilityBillOtherExpenseAmount")] Budget budget, int id)
        {
            budget.BudgetUserId = id;
            // Calculate TotalIncome
            budget.TotalIncome = (double)budget.IncomePrimaryAmount + (double)budget.IncomeAdditionalAmount;

            // Calculate TotalCarExpenses
            budget.TotalCarExpenses = (double)budget.CarTaxAmount + (double)budget.CarInsuranceAmount + (double)budget.CarMaintenanceAmount +
                (double)budget.CarFuelAmount + (double)budget.CarNctAmount + (double)budget.CarTollChargesAmount +
                (double)budget.CarExpenseOtherAmount;

            // Calculate TotalHouseholdExpenses
            budget.TotalHouseholdExpenses = (double)budget.HouseholdRentMortgageAmount + (double)budget.HouseholdGroceryAmount +
                (double)budget.HouseholdClothingAmount + (double)budget.HouseholdEducationFeesAmount +
                (double)budget.HouseholdSchoolSuppliesAmount + (double)budget.HouseholdMedicalExpensesAmount +
                (double)budget.HouseholdInsuranceAmount + (double)budget.HouseholdMaintenanceAmount +
                (double)budget.HouseholdExpenseOtherAmount;

            // Calculate TotalPersonalExpenses
            budget.TotalPersonalExpenses = (double)budget.PersonalSocialAmount + (double)budget.PersonalGymMembershipAmount +
                (double)budget.PersonalSportsExpenseAmount + (double)budget.PersonalHolidayExpenseAmount +
                (double)budget.PersonalSavingsAmount + (double)budget.PersonalLoanRepaymentAmount +
                (double)budget.PersonalHealthInsuranceAmount + (double)budget.PersonalExpenseOtherAmount;

            // Calculate TotalTravelExpenses
            budget.TotalTravelExpenses = (double)budget.TravelBusAmount + (double)budget.TravelLuasAmount +
                (double)budget.TravelTaxiAmount + (double)budget.PersonalHolidayExpenseAmount +
                (double)budget.TravelTrainAmount + (double)budget.TravelPlaneAmount +
                (double)budget.TravelExpenseOtherAmount;

            // Calculate TotalUtilityBillExpenses
            budget.TotalUtilityBillExpenses = (double)budget.UtilityBillElectricityAmount + (double)budget.UtilityBillGasAmount +
                (double)budget.UtilityBillRefuseCollectionAmount + (double)budget.UtilityBillIrishWaterAmount +
                (double)budget.UtilityBillTVAmount + (double)budget.UtilityBillPhoneBillAmount +
                (double)budget.UtilityBillBroadbandAmount + (double)budget.UtilityBillOtherExpenseAmount;

            // Calculate Subtotals
            budget.TotalExpenses = (double)budget.TotalCarExpenses + (double)budget.TotalHouseholdExpenses +
                (double)budget.TotalPersonalExpenses + (double)budget.TotalTravelExpenses +
                (double)budget.TotalUtilityBillExpenses;

            // Calculate Budget Balance
            budget.BudgetBalance = (double)budget.TotalIncome - (double)budget.TotalExpenses;

            if (ModelState.IsValid)
            {
                db.Budgets.Add(budget);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.BudgetUserId = new SelectList(db.BudgetUsers, "BudgetUserId", "LastName", budget.BudgetUserId);
            return View(budget);
        }

        // GET: Budgets/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Budget budget = db.Budgets.Find(id);
            if (budget == null)
            {
                return HttpNotFound();
            }
            ViewBag.BudgetUserId = new SelectList(db.BudgetUsers, "BudgetUserId", "LastName", budget.BudgetUserId);
            return View(budget);
        }

        // POST: Budgets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "BudgetId,BudgetUserId,BudgetName,BudgetStartDate,BudgetEndDate,IncomePrimaryAmount,IncomeAdditionalAmount,CarTaxAmount,CarInsuranceAmount,CarMaintenanceAmount,CarFuelAmount,CarNctAmount,CarTollChargesAmount,CarExpenseOtherAmount,HouseholdRentMortgageAmount,HouseholdGroceryAmount,HouseholdClothingAmount,HouseholdEducationFeesAmount,HouseholdSchoolSuppliesAmount,HouseholdMedicalExpensesAmount,HouseholdInsuranceAmount,HouseholdMaintenanceAmount,HouseholdExpenseOtherAmount,PersonalSocialAmount,PersonalGymMembershipAmount,PersonalSportsExpenseAmount,PersonalHolidayExpenseAmount,PersonalSavingsAmount,PersonalLoanRepaymentAmount,PersonalHealthInsuranceAmount,PersonalExpenseOtherAmount,TravelBusAmount,TravelLuasAmount,TravelTaxiAmount,TravelTrainAmount,TravelPlaneAmount,TravelExpenseOtherAmount,UtilityBillElectricityAmount,UtilityBillGasAmount,UtilityBillRefuseCollectionAmount,UtilityBillIrishWaterAmount,UtilityBillTVAmount,UtilityBillPhoneBillAmount,UtilityBillBroadbandAmount,UtilityBillOtherExpenseAmount")] Budget budget)
        {
            // Calculate TotalIncome
            budget.TotalIncome = (double)budget.IncomePrimaryAmount + (double)budget.IncomeAdditionalAmount;

            // Calculate TotalCarExpenses
            budget.TotalCarExpenses = (double)budget.CarTaxAmount + (double)budget.CarInsuranceAmount + (double)budget.CarMaintenanceAmount +
                (double)budget.CarFuelAmount + (double)budget.CarNctAmount + (double)budget.CarTollChargesAmount +
                (double)budget.CarExpenseOtherAmount;

            // Calculate TotalHouseholdExpenses
            budget.TotalHouseholdExpenses = (double)budget.HouseholdRentMortgageAmount + (double)budget.HouseholdGroceryAmount +
                (double)budget.HouseholdClothingAmount + (double)budget.HouseholdEducationFeesAmount +
                (double)budget.HouseholdSchoolSuppliesAmount + (double)budget.HouseholdMedicalExpensesAmount +
                (double)budget.HouseholdInsuranceAmount + (double)budget.HouseholdMaintenanceAmount +
                (double)budget.HouseholdExpenseOtherAmount;

            // Calculate TotalPersonalExpenses
            budget.TotalPersonalExpenses = (double)budget.PersonalSocialAmount + (double)budget.PersonalGymMembershipAmount +
                (double)budget.PersonalSportsExpenseAmount + (double)budget.PersonalHolidayExpenseAmount +
                (double)budget.PersonalSavingsAmount + (double)budget.PersonalLoanRepaymentAmount +
                (double)budget.PersonalHealthInsuranceAmount + (double)budget.PersonalExpenseOtherAmount;

            // Calculate TotalTravelExpenses
            budget.TotalTravelExpenses = (double)budget.TravelBusAmount + (double)budget.TravelLuasAmount +
                (double)budget.TravelTaxiAmount + (double)budget.PersonalHolidayExpenseAmount +
                (double)budget.TravelTrainAmount + (double)budget.TravelPlaneAmount +
                (double)budget.TravelExpenseOtherAmount;

            // Calculate TotalUtilityBillExpenses
            budget.TotalUtilityBillExpenses = (double)budget.UtilityBillElectricityAmount + (double)budget.UtilityBillGasAmount +
                (double)budget.UtilityBillRefuseCollectionAmount + (double)budget.UtilityBillIrishWaterAmount +
                (double)budget.UtilityBillTVAmount + (double)budget.UtilityBillPhoneBillAmount +
                (double)budget.UtilityBillBroadbandAmount + (double)budget.UtilityBillOtherExpenseAmount;

            // Calculate Subtotals
            budget.TotalExpenses = (double)budget.TotalCarExpenses + (double)budget.TotalHouseholdExpenses +
                (double)budget.TotalPersonalExpenses + (double)budget.TotalTravelExpenses +
                (double)budget.TotalUtilityBillExpenses;

            // Calculate Budget Balance
            budget.BudgetBalance = (double)budget.TotalIncome - (double)budget.TotalExpenses;

            if (ModelState.IsValid)
            {
                db.Entry(budget).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(budget);
        }

        // GET: Budgets/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Budget budget = db.Budgets.Find(id);
            if (budget == null)
            {
                return HttpNotFound();
            }
            return View(budget);
        }

        // POST: Budgets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Budget budget = db.Budgets.Find(id);
            db.Budgets.Remove(budget);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // ******************** BUDGET ANALYSIS SUMMARY ********************
        public ActionResult Summary(int? id)
        {
            Budget b = new Budget();
            b = db.Budgets.Where(p => p.BudgetId == id).SingleOrDefault();

            //// -------------------- INCOME --------------------
            //double totalIncome = 0;
            //ViewBag.IncomePrimaryAmount = b.IncomePrimaryAmount;
            //ViewBag.IncomeAdditionalAmount = b.IncomeAdditionalAmount;
            //// Calculate TotalIncome and set result as globalTotalIncome
            //// sets totalIncome initially equal to primaryIncome
            //totalIncome = (double)b.IncomePrimaryAmount;
            //// if there is Additional Income execute the following
            //if(b.IncomeAdditionalAmount != null)
            //{
            //    totalIncome = (double)b.IncomePrimaryAmount + (double)b.IncomeAdditionalAmount;
            //}
            //// pass TotalIncome to Summary.cshtml
            //ViewBag.TotalIncome = totalIncome;
            //// totalIncome = globalTotalIncome;
            //// -------------------- END OF INCOME --------------------

            //// -------------------- CAR EXPENDITURE --------------------
            //double totalCarExpenses = 0;
            //ViewBag.CarTaxAmount = b.CarTaxAmount;
            //ViewBag.CarInsuranceAmount = b.CarInsuranceAmount;
            //ViewBag.CarMaintenanceAmount = b.CarMaintenanceAmount;
            //ViewBag.CarFuelAmount = b.CarFuelAmount;
            //ViewBag.CarNctAmount = b.CarNctAmount;
            //ViewBag.CarTollChargesAmount = b.CarTollChargesAmount;
            //ViewBag.CarExpenseOtherAmount = b.CarExpenseOtherAmount;
            //// Calculate TotalCarExpenses and set result as globalTotalCarExpense
            //totalCarExpenses = (double)b.CarTaxAmount + (double)b.CarInsuranceAmount + (double)b.CarMaintenanceAmount +
            //    (double)b.CarFuelAmount + (double)b.CarNctAmount + (double)b.CarTollChargesAmount +
            //    (double)b.CarExpenseOtherAmount;
            //// pass TotalCarExpenses to Summary.cshtml
            //ViewBag.TotalCarExpenses = totalCarExpenses;
            //// totalCarExpenses = b.TotalCarExpenses;
            //// -------------------- END OF CAR EXPENDITURE --------------------

            //// -------------------- HOUSEHOLD EXPENDITURE --------------------
            //double totalHouseholdExpenses = 0;
            //ViewBag.HouseholdRentMortgageAmount = b.HouseholdRentMortgageAmount;
            //ViewBag.HouseholdGroceryAmount = b.HouseholdGroceryAmount;
            //ViewBag.HouseholdClothingAmount = b.HouseholdClothingAmount;
            //ViewBag.HouseholdEducationFeesAmount = b.HouseholdEducationFeesAmount;
            //ViewBag.HouseholdSchoolSuppliesAmount = b.HouseholdSchoolSuppliesAmount;
            //ViewBag.HouseholdMedicalExpensesAmount = b.HouseholdMedicalExpensesAmount;
            //ViewBag.HouseholdInsuranceAmount = b.HouseholdInsuranceAmount;
            //ViewBag.HouseholdMaintenanceAmount = b.HouseholdMaintenanceAmount;
            //ViewBag.HouseholdExpenseOtherAmount = b.HouseholdExpenseOtherAmount;
            //// Calculate TotalHouseholdExpenses
            //totalHouseholdExpenses = (double)b.HouseholdRentMortgageAmount + (double)b.HouseholdGroceryAmount +
            //    (double)b.HouseholdClothingAmount + (double)b.HouseholdEducationFeesAmount +
            //    (double)b.HouseholdSchoolSuppliesAmount + (double)b.HouseholdMedicalExpensesAmount +
            //    (double)b.HouseholdInsuranceAmount + (double)b.HouseholdMaintenanceAmount +
            //    (double)b.HouseholdExpenseOtherAmount;
            //// pass TotalHouseholdExpenses to Summary.cshtml
            //ViewBag.TotalHouseholdExpenses = totalHouseholdExpenses;
            //// totalHouseholdExpenses = globalTotalHouseholdExpenses;
            //// -------------------- END OF HOUSEHOLD EXPENDITURE --------------------

            //// -------------------- PERSONAL EXPENDITURE --------------------
            //double totalPersonalExpenses = 0;
            //ViewBag.PersonalSocialAmount = b.PersonalSocialAmount;
            //ViewBag.PersonalGymMembershipAmount = b.PersonalGymMembershipAmount;
            //ViewBag.PersonalSportsExpenseAmount = b.PersonalSportsExpenseAmount;
            //ViewBag.PersonalHolidayExpenseAmount = b.PersonalHolidayExpenseAmount;
            //ViewBag.PersonalSavingsAmount = b.PersonalSavingsAmount;
            //ViewBag.PersonalLoanRepaymentAmount = b.PersonalLoanRepaymentAmount;
            //ViewBag.PersonalHealthInsuranceAmount = b.PersonalHealthInsuranceAmount;
            //ViewBag.PersonalExpenseOtherAmount = b.PersonalExpenseOtherAmount;
            //// Calculate TotalPersonalExpenses and set result as globalTotalPersonalExpenses
            //totalPersonalExpenses = (double)b.PersonalSocialAmount + (double)b.PersonalGymMembershipAmount +
            //    (double)b.PersonalSportsExpenseAmount + (double)b.PersonalHolidayExpenseAmount +
            //    (double)b.PersonalSavingsAmount + (double)b.PersonalLoanRepaymentAmount +
            //    (double)b.PersonalHealthInsuranceAmount + (double)b.PersonalExpenseOtherAmount;
            //// pass TotalPersonalExpenses to Summary.cshtml
            //ViewBag.TotalPersonalExpenses = totalPersonalExpenses;
            //// totalPersonalExpenses = globalTotalPersonalExpenses;
            //// -------------------- END OF PERSONAL EXPENDITURE --------------------

            //// -------------------- TRAVEL EXPENDITURE --------------------
            //double totalTravelExpenses = 0;
            //ViewBag.TravelBusAmount = b.TravelBusAmount;
            //ViewBag.TravelLuasAmount = b.TravelLuasAmount;
            //ViewBag.TravelTaxiAmount = b.TravelTaxiAmount;
            //ViewBag.TravelTrainAmount = b.TravelTrainAmount;
            //ViewBag.TravelPlaneAmount = b.TravelPlaneAmount;
            //ViewBag.TravelExpenseOtherAmount = b.TravelExpenseOtherAmount;
            //// Calculate TotalTravelExpenses and set result as globalTotalTravelExpenses
            //totalTravelExpenses = (double)b.TravelBusAmount + (double)b.TravelLuasAmount +
            //    (double)b.TravelTaxiAmount + (double)b.PersonalHolidayExpenseAmount +
            //    (double)b.TravelTrainAmount + (double)b.TravelPlaneAmount +
            //    (double)b.TravelExpenseOtherAmount;
            //// pass TotalTravelExpenses to Summary.cshtml
            //ViewBag.TotalTravelExpenses = totalTravelExpenses;
            //// totalTravelExpenses = globalTotalTravelExpenses;
            //// -------------------- END OF TRAVEL EXPENDITURE --------------------

            //// -------------------- UTILITY BILL EXPENDITURE --------------------
            //double totalUtilityBillExpenses = 0;
            //ViewBag.UtilityBillElectricityAmount = b.UtilityBillElectricityAmount;
            //ViewBag.UtilityBillGasAmount = b.UtilityBillGasAmount;
            //ViewBag.UtilityBillRefuseCollectionAmount = b.UtilityBillRefuseCollectionAmount;
            //ViewBag.UtilityBillIrishWaterAmount = b.UtilityBillIrishWaterAmount;
            //ViewBag.UtilityBillTVAmount = b.UtilityBillTVAmount;
            //ViewBag.UtilityBillPhoneBillAmount = b.UtilityBillPhoneBillAmount;
            //ViewBag.UtilityBillBroadbandAmount = b.UtilityBillBroadbandAmount;
            //ViewBag.UtilityBillOtherExpenseAmount = b.UtilityBillOtherExpenseAmount;
            //// Calculate TotalUtilityBillExpenses and set result as globalTotalPersonalExpenses
            //totalUtilityBillExpenses = (double)b.UtilityBillElectricityAmount + (double)b.UtilityBillGasAmount +
            //    (double)b.UtilityBillRefuseCollectionAmount + (double)b.UtilityBillIrishWaterAmount +
            //    (double)b.UtilityBillTVAmount + (double)b.UtilityBillPhoneBillAmount +
            //    (double)b.UtilityBillBroadbandAmount + (double)b.UtilityBillOtherExpenseAmount;
            //// pass TotalUtilityBillExpenses to Summary.cshtml
            //ViewBag.TotalUtilityBillExpenses = totalUtilityBillExpenses;
            //// totalUtilityBillExpenses = globalTotalUtilityBillExpenses;
            //// -------------------- END OF UTILITY BILL EXPENDITURE --------------------

            //// -------------------- SUBTOTAL CALCULATION -------------------- 
            //// INCOME - same as TotalIncome calculated above
            //// -------------------- TOTAL EXPENSES CALCULATION --------------------
            //double totalExpenses = 0;
            //totalExpenses = (double)totalCarExpenses + (double)totalHouseholdExpenses +
            //    (double)totalPersonalExpenses + (double)totalTravelExpenses +
            //    (double)totalUtilityBillExpenses;
            //ViewBag.TotalExpenses = totalExpenses;
            //// totalExpenses = globalTotalExpenses;
            //// -------------------- BUDGET BALANCE CALCULATION --------------------
            //double budgetBalance = 0;
            //budgetBalance = (double)totalIncome - (double)totalExpenses;
            //ViewBag.BudgetBalance = budgetBalance;
            //// budgetBalance = globalBudgetBalance;
            return View(b);
        }

        //// ******************** COMPLEXITY CALCULATIONS ********************
        //// find the lowest Budget Balance
        ///* idea to return budget balances with lowest balance
        //   and display
        // * i)assign 1st element in array as minimum value
        // * ii)loop through array and compare each element in the array
        // * iii)if accessed variable value less than current minimum value, replace
        //*/
        //int index;
        //public int findMinimumBudgetBalance(int[] budgetBal)
        //{
        //    int minimumBudgetBalance = budgetBal[0]; // assign first value in array as minimumBudgetBalance
        //    for(int i = 0; i < budgetBal.Length-1; i++) // begin loop
        //        if (budgetBal[index] < minimumBudgetBalance) // compare each element accessed in array
        //            minimumBudgetBalance = budgetBal[index]; // replace minimumBudgetBalance with lower value if found
        //    return minimumBudgetBalance; 
        //}

        //// find the highest Budget Balance
        ///* idea to return budget balances with highest balance
        //   and display
        // * i)assign 1st array element to variable that stores maximum value
        // * ii)loop through array comparing each element with value stored in variable
        // * iii)replace current value if accessed value is greater
        //*/
        //public int findMaximumBudgetBalance(int[] budgetBal)
        //{
        //    int maximumBudgetBalance = budgetBal[0]; // assign first value in array as maximumBudgetBalance
        //    for(int i = 0; i < budgetBal.Length - 1; i++) // begin loop
        //        if (budgetBal[index] > maximumBudgetBalance) // compare each element in array
        //            maximumBudgetBalance = budgetBal[index]; // replace maximumBudgetBalance with higher value if found
        //    return maximumBudgetBalance;
        //}

        // ******************** BUDGET ANALYSIS FORECAST ********************
        public ActionResult Forecast(int? id)
        {
            Budget b = new Budget();
            b = db.Budgets.Where(p => p.BudgetId == id).SingleOrDefault();

            var buds = from e in db.Budgets where e.BudgetUserId == id select e;

            //Budget b = new Budget();
            //// return list of budgets specific to one user
            //b = db.Budgets.Where(user => user.BudgetUserId == id).SingleOrDefault();

            return View(b);
        }

        // ******************** INDIVIDUAL BUDGET ANALYSIS CHARTS ********************
        public ActionResult Charts(int? id)
        {
            //BudgetUser u = new BudgetUser();
            //// return list of budgets specific to one user
            //u = db.BudgetUsers.Where(user => user.BudgetUserId == id).SingleOrDefault();

            //var total = from e in db.Budgets where e.BudgetUserId == id select e;
            Budget bud = new Budget();
            bud = db.Budgets.Where(b => b.BudgetId == id).SingleOrDefault();
            var total = from e in db.Budgets where e.BudgetUserId == id select e;
            int size = total.Count();
            // System.Diagnostics.Debug.WriteLine("size: " + size);

            // ******************** TOTAL INCOME/EXPENDITURE CHART ********************
            object[] income = new object[size];
            int c1 = 0;
            foreach (var item in total)
            {
                income[c1] = item.TotalIncome;
                c1++;
            }

            String[] budgetNames = new string[size];
            int c2 = 0;
            foreach (var item in total)
            {
                budgetNames[c2] = item.BudgetName;
                c2++;
            }

            object[] expenditure1 = new object[size];
            int c3 = 0;
            foreach (var item in total)
            {
                expenditure1[c3] = item.TotalExpenses;
                c3++;
            }

            Highcharts chart1 = new Highcharts("chart1")
            .InitChart(new Chart
            {
                DefaultSeriesType = ChartTypes.Line,
                MarginRight = 130,
                MarginBottom = 50,
                ClassName = "chart1"
            })
            .SetTitle(new Title
            {
                Text = " Total Income by Budget ",
                X = -20
            })
            .SetSubtitle(new Subtitle
            {
                Text = " Monthly Budget Analysis Chart ",
                X = -20
            })
            .SetXAxis(new XAxis
            {
                Categories = budgetNames
            })
            .SetYAxis(new YAxis
            {
                Title = new YAxisTitle
                {
                    Text = "Income/Expenditure in €"
                },
                PlotLines = new[]
                {
                    new YAxisPlotLines
                    {
                        Value = 0,
                        Width = 1,
                        Color = ColorTranslator.FromHtml("#808080")
                    }
                }
            })
            .SetTooltip(new Tooltip
            {
                Crosshairs = new Crosshairs(true, true)
            })
            .SetLegend(new Legend
            {
                Layout = Layouts.Vertical,
                Align = HorizontalAligns.Center,
                VerticalAlign = VerticalAligns.Top,
                X = -10,
                Y = 70,
                BorderWidth = 0
            })
                //.SetSeries based on totalIncome(income) and totalExpenditure(expenditure1) objects
            .SetSeries(new[]
            {
                new Series {Name = "Total Income", Data = new Data(income)},
                new Series {Name = "Total Expenditure", Data = new Data(expenditure1)}
            })
            .SetCredits(new Credits
            {
                Enabled = false // remove hyperlink for highchart
            });
            // -------------------- END TOTAL INCOME/EXPENDITURE CHART --------------------
            // ----------------------------------------------------------------------------

            // ******************** BUDGET BALANCE CHART ********************
            object[] budBal = new object[size];
            int c4 = 0;
            foreach (var item in total)
            {
                budBal[c4] = item.BudgetBalance;
                c4++;
            }

            Highcharts chart2 = new Highcharts("chart2")
            .InitChart(new Chart
            {
                DefaultSeriesType = ChartTypes.Line,
                MarginRight = 130,
                MarginBottom = 50,
                ClassName = "chart2"
            })
            .SetTitle(new Title
            {
                Text = " Budget Balances by Budget "
            })
            .SetSubtitle(new Subtitle
            {
                Text = " Monthly Budget Analysis Chart "
            })
            .SetXAxis(new XAxis
            {
                Categories = budgetNames
            })
            .SetYAxis(new YAxis
            {
                Title = new YAxisTitle
                {
                    Text = "Budget Balance in €"
                },
                PlotLines = new[]
                 { 
                     new YAxisPlotLines
                     {
                         Value = 0,
                         Width = 1,
                         Color = ColorTranslator.FromHtml("#808080")
                     }
                 }
            })
            .SetTooltip(new Tooltip
            {
                Crosshairs = new Crosshairs(true, true)
            })
            .SetLegend(new Legend
            {
                Layout = Layouts.Vertical,
                Align = HorizontalAligns.Center,
                VerticalAlign = VerticalAligns.Top,
                X = -10,
                Y = 70,
                BorderWidth = 0
            })
                //.SetSeries based on Budget Balance objects
             .SetSeries(new[]
            {
                new Series{Name = "Balance", Data = new Data(budBal)}
            })
            .SetCredits(new Credits
            {
                Enabled = false
            }); // remove hyperlink for highchart
            // -------------------------- END BUDGET BALANCE CHART ------------------------
            // ----------------------------------------------------------------------------

            // ******************* COLUMN BAR CHART - TOTAL EXPENDITURE (Month on Month) **********
            // CAR
            var carExpenseTotal = 0.0;
            object[] car = new object[size];
            int c5 = 0;
            foreach (var item in total)
            {
                car[c5] = item.TotalCarExpenses;
                carExpenseTotal += (double)item.TotalCarExpenses;
                c5++;
            }
            // calculate average of Car Expense
            var carAverage = carExpenseTotal / size;

            // HOUSEHOLD
            object[] household = new object[size];
            int c6 = 0;
            foreach (var item in total)
            {
                household[c6] = item.TotalHouseholdExpenses;
                c6++;
            }
            // PERSONAL
            object[] personal = new object[size];
            int c7 = 0;
            foreach (var item in total)
            {
                personal[c7] = item.TotalPersonalExpenses;
                c7++;
            }
            // TRAVEL
            object[] travel = new object[size];
            int c8 = 0;
            foreach (var item in total)
            {
                travel[c8] = item.TotalTravelExpenses;
                c8++;
            }
            // UTILITY BILLS
            object[] utilityBill = new object[size];
            int c9 = 0;
            foreach (var item in total)
            {
                utilityBill[c9] = item.TotalUtilityBillExpenses;
                c9++;
            }

            Highcharts chart3 = new Highcharts("chart3")
                .InitChart(new Chart
                {
                    DefaultSeriesType = ChartTypes.Column,
                    MarginRight = 130,
                    MarginBottom = 50,
                    ClassName = "chart3"
                })
                .SetTitle(new Title
                {
                    Text = " Expenditure Totals (Month-On-Month) "
                })
                .SetSubtitle(new Subtitle
                {
                    Text = " Monthly Budget Analysis Chart "
                })
                .SetXAxis(new XAxis
                {
                    Categories = budgetNames
                })
                .SetYAxis(new YAxis
                {
                    Min = 0,
                    Title = new YAxisTitle { Text = " Expenditure Total in € " }
                })
                .SetLegend(new Legend
                {
                    //Layout = Layouts.Vertical,
                    //Align = HorizontalAligns.Right,
                    //VerticalAlign = VerticalAligns.Top,
                    //X = 100,
                    Y = 10,
                    //Floating = true,
                    //BackgroundColor = new BackColorOrGradient(ColorTranslator.FromHtml("#FFFFFF")),
                    //Shadow = true
                })
                .SetPlotOptions(new PlotOptions
                {
                    Column = new PlotOptionsColumn
                    {
                        PointPadding = 0.2,
                        BorderWidth = 0
                    }
                })
                .SetSeries(new[]
                {
                    new Series { Name = "Car Expenses", Data = new Data(car)},
                    new Series { Name = "Household Expenses", Data = new Data(household)},
                    new Series { Name = "Personal Expenses", Data = new Data(personal)},
                    new Series { Name = "Travel Expenses", Data = new Data(travel)},
                    new Series { Name = "Utility Bill Expenses", Data = new Data(utilityBill)}
                })
                .SetCredits(new Credits
                {
                    Enabled = false
                }); // remove hyperlink for highchart
            // ---------------------------- END OF COLUMN CHART ----------------------------
            // -----------------------------------------------------------------------------
            return View(new Container(new[] { chart1, chart2, chart3 }));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}