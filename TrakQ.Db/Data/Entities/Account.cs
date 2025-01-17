using System.ComponentModel.DataAnnotations;

namespace TrakQ.Db.Data.Entities;

public class Account
{
    [Key]
    public int AccountId { get; set; }

    [MaxLength(100)]
    public string AccountName { get; set; } = string.Empty;
}
