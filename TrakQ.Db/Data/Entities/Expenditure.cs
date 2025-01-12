using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrakQ.Db.Data.Entities;
public class Expenditure
{
    [Key]
    public int ExpenditureId { get; set; }
    public int ExpenditureHeadId { get; set; }
    public int ParentHeadId { get; set; } = 0;
    public DateTime ExpenditureDate { get; set; } = DateTime.Now;

    [Precision(10, 2)]
    public decimal Amount { get; set; } = 0;
    public string? Remark { get; set; }
    public bool IsDeleted { get; set; } = false;


    [ForeignKey(nameof(ExpenditureHeadId))]
    public ExpenditureHead ExpenditureHead { get; set; } = null!;
}