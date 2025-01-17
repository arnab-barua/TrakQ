using System.ComponentModel.DataAnnotations;

namespace TrakQ.Db.Data.Entities;

public class IncomeHead
{
    [Key]
    public int IncomeHeadId { get; set; }

    public string IncomeHeadName { get; set; } = string.Empty;
    public string? Note { get; set; }
}
