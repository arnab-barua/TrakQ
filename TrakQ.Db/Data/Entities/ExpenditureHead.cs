using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace TrakQ.Db.Data.Entities;

public class ExpenditureHead
{
    [Key]
    public int ExpenditureHeadId { get; set; }

    public string HeadName { get; set; } = string.Empty;
    public int ParentHeadId { get; set; } = 0;
    public string? Note { get; set; }

    [Precision(10, 2)]
    public decimal? FixedAmount { get; set; }

    [Precision(10, 2)]
    public decimal? Budget { get; set; }

}
