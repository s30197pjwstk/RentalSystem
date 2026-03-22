using RentalSystem.Models;

namespace RentalSystem.Services;

public interface IPenaltyCalculator
{
    decimal CalculatePenalty(Rental rental, DateTime returnDate);
}

public class StandardPenaltyCalculator : IPenaltyCalculator
{
    private const decimal DailyPenaltyRate = 5.0m; 

    public decimal CalculatePenalty(Rental rental, DateTime returnDate)
    {
        if (returnDate <= rental.DueDate) return 0;

        int daysLate = (returnDate - rental.DueDate).Days;
        return daysLate * DailyPenaltyRate;
    }
}