namespace TrakQ.Web.Models;

public class ExpenseHeadTotalNode
{
    public int HeadId { get; init; }
    public string HeadName { get; init; } = "";
    public int ParentHeadId { get; init; }
    public decimal Total { get; set; }
    public decimal? Budget { get; init; }
    public List<ExpenseHeadTotalNode> Children { get; init; } = [];
}
