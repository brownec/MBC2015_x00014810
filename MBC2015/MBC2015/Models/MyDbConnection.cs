/*
Student Name: 	Cliff Browne
Student ID:		X00014810
Module:			Project 4th Year
Course:			Computing
College:		I.T Tallaght, Dublin
*/

using System.Data.Entity;

namespace MBC2015.Models
{
    public class MyDbConnection : DbContext
    {
        public DbSet<Budget> Budgets { get; set; }
        public DbSet<BudgetUser> BudgetUsers { get; set; }
    }
}