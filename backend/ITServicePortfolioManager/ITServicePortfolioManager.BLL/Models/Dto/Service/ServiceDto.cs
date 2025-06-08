namespace ITServicePortfolioManager.BLL.Models.Dto.Service;

public class ServiceDto(int price, int labourIntensity, double incomeForProvider, double discount)
{
    public int Price { get; private set; } = price;
    public int LabourIntensity { get; private set; } = labourIntensity;
    public double IncomeForProvider { get; private set; } = incomeForProvider;
    public double Discount { get; set; } = discount;
}