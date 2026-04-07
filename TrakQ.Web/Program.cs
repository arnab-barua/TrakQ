using Microsoft.EntityFrameworkCore;
using TrakQ.Db;
using TrakQ.Web.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddDbContextFactory<AppDbContext>();
builder.Services.AddScoped<IExpenseReportService, ExpenseReportService>();
builder.Services.AddScoped<IIncomeReportService, IncomeReportService>();
builder.Services.AddScoped<ISummaryReportService, SummaryReportService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}

app.UseStaticFiles();
app.UseRouting();
app.MapRazorPages();

app.Run();
