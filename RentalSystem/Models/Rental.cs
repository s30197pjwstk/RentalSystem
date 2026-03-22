namespace RentalSystem.Models;

public class Rental
{
    public string Id { get; } = Guid.NewGuid().ToString("N").Substring(0, 6);
    public User User { get; }
    public Equipment Equipment { get; }
    public DateTime RentDate { get; }
    public DateTime DueDate { get; }
    public DateTime? ReturnDate { get; private set; }
    public decimal PenaltyFee { get; private set; }

    public Rental(User user, Equipment equipment, DateTime rentDate, DateTime dueDate)
    {
        User = user;
        Equipment = equipment;
        RentDate = rentDate;
        DueDate = dueDate;
    }

    public void MarkAsReturned(DateTime returnDate, decimal penalty)
    {
        ReturnDate = returnDate;
        PenaltyFee = penalty;
        Equipment.Status = EquipmentStatus.Available;
    }

    public bool IsActive => ReturnDate == null;
    public bool IsOverdue(DateTime currentDate) => IsActive && currentDate > DueDate;
}