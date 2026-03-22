using RentalSystem.Models;
using RentalSystem.Exceptions;

namespace RentalSystem.Services;

public class RentalService
{
    private readonly List<Equipment> _equipments = new();
    private readonly List<User> _users = new();
    private readonly List<Rental> _rentals = new();
    private readonly IPenaltyCalculator _penaltyCalculator;

    public RentalService(IPenaltyCalculator penaltyCalculator)
    {
        _penaltyCalculator = penaltyCalculator;
    }

    public void AddEquipment(Equipment equipment) => _equipments.Add(equipment);
    public void AddUser(User user) => _users.Add(user);

    public IEnumerable<Equipment> GetAllEquipment() => _equipments;
    public IEnumerable<Equipment> GetAvailableEquipment() => _equipments.Where(e => e.Status == EquipmentStatus.Available);

    public void MarkEquipmentAsUnavailable(string equipmentId)
    {
        var eq = _equipments.FirstOrDefault(e => e.Id == equipmentId);
        if (eq == null) throw new DomainException("Equipment not found.");
        eq.Status = EquipmentStatus.Unavailable;
    }

    public void RentEquipment(User user, Equipment equipment, int days)
    {
        if (equipment.Status != EquipmentStatus.Available)
            throw new DomainException($"Equipment {equipment.Name} is currently {equipment.Status}.");

        int activeRentalsCount = _rentals.Count(r => r.User.Id == user.Id && r.IsActive);
        if (activeRentalsCount >= user.MaxActiveRentals)
            throw new DomainException($"User {user.FirstName} reached maximum active rentals limit ({user.MaxActiveRentals}).");

        var rentDate = DateTime.Now;
        var rental = new Rental(user, equipment, rentDate, rentDate.AddDays(days));
        
        equipment.Status = EquipmentStatus.Rented;
        _rentals.Add(rental);
    }

    public void ReturnEquipment(Rental rental, DateTime returnDate)
    {
        if (!rental.IsActive) throw new DomainException("This rental is already closed.");

        var penalty = _penaltyCalculator.CalculatePenalty(rental, returnDate);
        rental.MarkAsReturned(returnDate, penalty);
    }

    public IEnumerable<Rental> GetActiveRentalsForUser(User user) => 
        _rentals.Where(r => r.User.Id == user.Id && r.IsActive);

    public IEnumerable<Rental> GetOverdueRentals(DateTime currentDate) => 
        _rentals.Where(r => r.IsOverdue(currentDate));

    public void GenerateReport()
    {
        Console.WriteLine("\n--- SYSTEM REPORT ---");
        Console.WriteLine($"Total Equipment: {_equipments.Count}");
        Console.WriteLine($"Available Equipment: {_equipments.Count(e => e.Status == EquipmentStatus.Available)}");
        Console.WriteLine($"Active Rentals: {_rentals.Count(r => r.IsActive)}");
        Console.WriteLine($"Overdue Rentals: {GetOverdueRentals(DateTime.Now).Count()}");
        Console.WriteLine("---------------------\n");
    }
}