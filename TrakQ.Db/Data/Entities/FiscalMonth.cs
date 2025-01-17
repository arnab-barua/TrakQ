using System.ComponentModel.DataAnnotations;

namespace TrakQ.Db.Data.Entities;

public class FiscalMonth
{
    [Key]
    public int Id { get; set; }

    [Range(2020, 2100)]
    public Int16 Year { get; set; }

    [Range(1,12)]
    public Byte Month { get; set; }
}
