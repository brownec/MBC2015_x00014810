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
    public class BudgetUsersController : Controller
    {
        private MyDbConnection db = new MyDbConnection();

        // GET: BudgetUsers
        public ActionResult Index()
        {
            return View(db.BudgetUsers.ToList());
        }

        // GET: BudgetUsers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BudgetUser budgetUser = db.BudgetUsers.Find(id);
            if (budgetUser == null)
            {
                return HttpNotFound();
            }
            return View(budgetUser);
        }

        // GET: BudgetUsers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: BudgetUsers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "BudgetUserId,LastName,FirstName,DateOfBirth,AddressLine1,AddressLine2,Town,Counties,Country,PostCode,ContactNo")] BudgetUser budgetUser)
        {
            if (ModelState.IsValid)
            {
                db.BudgetUsers.Add(budgetUser);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(budgetUser);
        }

        // GET: BudgetUsers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BudgetUser budgetUser = db.BudgetUsers.Find(id);
            if (budgetUser == null)
            {
                return HttpNotFound();
            }
            return View(budgetUser);
        }

        // POST: BudgetUsers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "BudgetUserId,LastName,FirstName,DateOfBirth,AddressLine1,AddressLine2,Town,Counties,Country,PostCode,ContactNo")] BudgetUser budgetUser)
        {
            if (ModelState.IsValid)
            {
                db.Entry(budgetUser).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(budgetUser);
        }

        // GET: BudgetUsers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BudgetUser budgetUser = db.BudgetUsers.Find(id);
            if (budgetUser == null)
            {
                return HttpNotFound();
            }
            return View(budgetUser);
        }

        // POST: BudgetUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            BudgetUser budgetUser = db.BudgetUsers.Find(id);
            db.BudgetUsers.Remove(budgetUser);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // ******************** BUDGET ANALYSIS CHARTS ********************
        public ActionResult Charts(int? id)
        {
            BudgetUser u = new BudgetUser();
            // return list of budgets specific to one user
            u = db.BudgetUsers.Where(user => user.BudgetUserId == id).SingleOrDefault();

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
            // Budget Names
            //String[] budgetNames = new string[size];
            //int c10 = 0;
            //foreach (var item in total)
            //{
            //    budgetNames[c10] = item.BudgetName;
            //    c10++;
            //}
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
            ViewBag.carAverage = carExpenseTotal / size;
            ViewBag.size = size;
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
                    // new Series { Name = "Monthly Income", Data = new Data(income)},
                    // new Series { Name = "Monthly Expenditure", Data = new Data(expenditure1)}
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










        public ActionResult Forecast(int? id)
        {
            BudgetUser u = new BudgetUser();
            // return list of budgets specific to one user
            u = db.BudgetUsers.Where(user => user.BudgetUserId == id).SingleOrDefault();

            var total = from e in db.Budgets where e.BudgetUserId == id select e;
            int size = total.Count();

            // ******************* COLUMN BAR CHART - TOTAL EXPENDITURE (Month on Month) **********
            // Budget Names
            String[] budgetNames = new string[size];
            int c1 = 0;
            foreach (var item in total)
            {
                budgetNames[c1] = item.BudgetName;
                c1++;
            }
            // Income
            object[] income = new object[size];
            int c2 = 0;
            foreach (var item in total)
            {
                income[c2] = item.TotalIncome;
                c2++;
            }

            object[] expenditure1 = new object[size];
            int c3 = 0;
            foreach (var item in total)
            {
                expenditure1[c3] = item.TotalExpenses;
                c3++;
            }
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
            ViewBag.carAverage = carExpenseTotal / size;
            ViewBag.size = size;
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
                    new Series { Name = "Monthly Income", Data = new Data(income)},
                    new Series { Name = "Monthly Expenditure", Data = new Data(expenditure1)}
                    //new Series { Name = "Car Expenses", Data = new Data(car)},
                    //new Series { Name = "Household Expenses", Data = new Data(household)},
                    //new Series { Name = "Personal Expenses", Data = new Data(personal)},
                    //new Series { Name = "Travel Expenses", Data = new Data(travel)},
                    //new Series { Name = "Utility Bill Expenses", Data = new Data(utilityBill)}
                })
                .SetCredits(new Credits
                {
                    Enabled = false
                }); // remove hyperlink for highchart
            // ---------------------------- END OF COLUMN CHART ----------------------------
            // -----------------------------------------------------------------------------

            // DRILLDOWN
            string[] categories = { "Income", "Expenditure" };
            const string NAME = "Forecast";
            Data data = new Data(new[]
            {
                new Point
                {
                    Y = 55.11,
                    Color = Color.FromName("colors[0]"),
                    Drilldown = new Drilldown
                    {
                        Name = "Monthly Income",
                        Categories = new[] { "Car", "Household", "Personal", "Travel", "Utility Bill" },
                        Data = new Data(new object[] { 10.85, 7.35, 33.06, 2.81 }),
                        Color = Color.FromName("colors[0]")
                    }
                },
                new Point
                {
                    Y = 21.63,
                    Color = Color.FromName("colors[1]"),
                    Drilldown = new Drilldown
                    {
                        Name = "Monthly Expenditure",
                        Categories = new[] { "Car", "Household", "Personal", "Travel", "Utility Bill" },
                        Data = new Data(new object[] { car, household, personal, travel, utilityBill }),
                        Color = Color.FromName("colors[1]")
                    }
                },
            });

            Highcharts chart = new Highcharts("chart")
                .InitChart(new Chart { DefaultSeriesType = ChartTypes.Column })
                .SetTitle(new Title { Text = "Browser market share, April, 2011" })
                .SetSubtitle(new Subtitle { Text = "Click the columns to view versions. Click again to view brands." })
                .SetXAxis(new XAxis { Categories = categories })
                .SetYAxis(new YAxis { Title = new YAxisTitle { Text = "Total percent market share" } })
                .SetLegend(new Legend { Enabled = false })
                .SetTooltip(new Tooltip { Formatter = "TooltipFormatter" })
                .SetPlotOptions(new PlotOptions
                {
                    Column = new PlotOptionsColumn
                    {
                        Cursor = Cursors.Pointer,
                        Point = new PlotOptionsColumnPoint { Events = new PlotOptionsColumnPointEvents { Click = "ColumnPointClick" } },
                        DataLabels = new PlotOptionsColumnDataLabels
                        {
                            Enabled = true,
                            Color = Color.FromName("colors[0]"),
                            Formatter = "function() { return this.y +'%'; }",
                            Style = "fontWeight: 'bold'"
                        }
                    }
                })
                .SetSeries(new Series
                {
                    Name = "Browser brands",
                    Data = data,
                    Color = Color.White
                })
                .SetExporting(new Exporting { Enabled = false })
                .AddJavascripFunction(
                    "TooltipFormatter",
                    @"var point = this.point, s = this.x +':<b>'+ this.y +'% market share</b><br/>';
                      if (point.drilldown) {
                        s += 'Click to view '+ point.category +' versions';
                      } else {
                        s += 'Click to return to browser brands';
                      }
                      return s;"
                )
                .AddJavascripFunction(
                    "ColumnPointClick",
                    @"var drilldown = this.drilldown;
                      if (drilldown) { // drill down
                        setChart(drilldown.name, drilldown.categories, drilldown.data.data, drilldown.color);
                      } else { // restore
                        setChart(name, categories, data.data);
                      }"
                )
                .AddJavascripFunction(
                    "setChart",
                    @"chart.xAxis[0].setCategories(categories);
                      chart.series[0].remove();
                      chart.addSeries({
                         name: name,
                         data: data,
                         color: color || 'white'
                      });",
                    "name", "categories", "data", "color"
                )
                .AddJavascripVariable("colors", "Highcharts.getOptions().colors")
                .AddJavascripVariable("name", "'{0}'".FormatWith(NAME))
                .AddJavascripVariable("categories", JsonSerializer.Serialize(categories))
                .AddJavascripVariable("data", JsonSerializer.Serialize(data));

            return View(chart3);
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
