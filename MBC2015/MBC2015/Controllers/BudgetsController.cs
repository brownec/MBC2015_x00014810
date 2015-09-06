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

            return View(b);
        }

        // ******************** BUDGET ANALYSIS FORECAST ********************
        public ActionResult Forecast(int? id)
        {
            Budget b = new Budget();
            b = db.Budgets.Where(p => p.BudgetId == id).SingleOrDefault();

            var buds = from e in db.Budgets where e.BudgetUserId == id select e;
            return View(b);
        }

        // ******************** INDIVIDUAL BUDGET ANALYSIS CHARTS ********************
        public ActionResult Charts(int? id)
        {
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
            }); 
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
                   Y = 10,
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
                });
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