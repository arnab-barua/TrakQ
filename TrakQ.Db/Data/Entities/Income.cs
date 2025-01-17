using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrakQ.Db.Data.Entities;

public class Income
{
    [Key]
    public int IncomeId { get; set; }
    public int IncomeHeadId { get; set; }
    public DateTime IncomeDate { get; set; } = DateTime.Now;

    [Precision(10, 2)]
    public decimal Amount { get; set; } = 0;
    public string? Remark { get; set; }
    public bool IsDeleted { get; set; } = false;
    public Guid UserId { get; set; }


    [ForeignKey(nameof(IncomeHeadId))]
    public IncomeHead IncomeHead { get; set; } = null!;
}
