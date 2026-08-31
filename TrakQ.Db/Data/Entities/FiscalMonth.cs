using System.ComponentModel.DataAnnotations;

using Microsoft.EntityFrameworkCore;

namespace TrakQ.Db.Data.Entities;

[Index(nameof(Year), nameof(Month), IsUnique = true)]
public class FiscalMonth
{
    [Key]
    public int Id { get; set; }

    [Range(2020, 2100)]
    public Int16 Year { get; set; }

    [Range(1,12)]
    public Byte Month { get; set; }
}
