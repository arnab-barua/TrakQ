namespace TrakQ.Dto;

public record ParentTypeExpenseHeadDto
{
    public int ExpenditureHeadId { get; init; }
    public string HeadName { get; init; } = string.Empty;
    public List<KeyValuePair<int, string>> ChildHeads { get; init; }


    public ParentTypeExpenseHeadDto()
    {
        ChildHeads = [];
    }
}
