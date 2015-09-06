using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using MBC2015.Models;

namespace MBC2015.Models
{
    public class MyDbConnection : DbContext
    {
        public DbSet<Budget> Budgets { get; set; }
        public DbSet<BudgetUser> BudgetUsers { get; set; }
    }
}