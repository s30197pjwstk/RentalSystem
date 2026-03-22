using RentalSystem.Models;
using RentalSystem.Services;
using RentalSystem.Exceptions;

var penaltyCalculator = new StandardPenaltyCalculator();
var rentalService = new RentalService(penaltyCalculator);

Console.WriteLine("=== INITIALIZING RENTAL SYSTEM ===");

var laptop1 = new Laptop("Dell XPS", 16, "i7");
var projector1 = new Projector("Epson 4K", "3840x2160", 3000);
var camera1 = new Camera("Sony A7III", 24.2, true);
rentalService.AddEquipment(laptop1);
rentalService.AddEquipment(projector1);
rentalService.AddEquipment(camera1);

var student = new Student("Jan", "Kowalski");
var employee = new Employee("Anna", "Nowak");
rentalService.AddUser(student);
rentalService.AddUser(employee);

Console.WriteLine("\n[1] Renting Equipment...");
rentalService.RentEquipment(student, laptop1, 7); 
Console.WriteLine($"Success: {student.FirstName} rented {laptop1.Name}. Status: {laptop1.Status}");

Console.WriteLine("\n[2] Testing Business Rules (Limits & Availability)...");
rentalService.RentEquipment(student, projector1, 2); 
try
{
    rentalService.RentEquipment(student, camera1, 1); 
}
catch (DomainException ex)
{
    Console.WriteLine($"Expected Error (Limit): {ex.Message}");
}

try
{
    rentalService.RentEquipment(employee, laptop1, 3);
}
catch (DomainException ex)
{
    Console.WriteLine($"Expected Error (Availability): {ex.Message}");
}

Console.WriteLine("\n[3] Returning in time...");
var studentRentals = rentalService.GetActiveRentalsForUser(student).ToList();
rentalService.ReturnEquipment(studentRentals[0], DateTime.Now.AddDays(5)); 
Console.WriteLine($"Returned {studentRentals[0].Equipment.Name} in time. Penalty: {studentRentals[0].PenaltyFee} PLN.");

Console.WriteLine("\n[4] Overdue Return...");
rentalService.ReturnEquipment(studentRentals[1], DateTime.Now.AddDays(5)); 
Console.WriteLine($"Returned {studentRentals[1].Equipment.Name} late. Penalty: {studentRentals[1].PenaltyFee} PLN.");

rentalService.MarkEquipmentAsUnavailable(camera1.Id);

rentalService.GenerateReport();