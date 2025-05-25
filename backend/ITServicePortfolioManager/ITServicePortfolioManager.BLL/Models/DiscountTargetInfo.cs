namespace ITServicePortfolioManager.BLL.Models;

public class DiscountTargetInfo
{
    public int? ProviderIndex { get; set; }
    public List<int>? GroupIndexes { get; set; }

    public DiscountTargetInfo(int providerIndex)
    {
        ProviderIndex = providerIndex;
    }

    public DiscountTargetInfo(List<int> groupIndexes)
    {
        GroupIndexes = groupIndexes;
    }
}
