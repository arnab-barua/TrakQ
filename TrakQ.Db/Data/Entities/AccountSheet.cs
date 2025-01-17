using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrakQ.Db.Data.Entities;

public class AccountSheet
{
    [Key]
    public int Id { get; set; } 

    [Precision(10,2)]
    public decimal OpeningBalance { get; set; }

    [Precision(10, 2)]
    public decimal? ClosingBalance { get; set; }



    public int AccountId { get; set; }

    [ForeignKey(nameof(AccountId))]
    public Account Account { get; set; } = null!;


    public int FiscalMonthId { get; set; }

    [ForeignKey(nameof(FiscalMonthId))]
    public FiscalMonth FiscalMonth { get; set; } = null!;
}
