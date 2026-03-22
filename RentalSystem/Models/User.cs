namespace RentalSystem.Models;

public abstract class User
{
    public string Id { get; } = Guid.NewGuid().ToString("N").Substring(0, 6);
    public string FirstName { get; set; }
    public string LastName { get; set; }
    
    public abstract int MaxActiveRentals { get; }

    protected User(string firstName, string lastName)
    {
        FirstName = firstName;
        LastName = lastName;
    }
}

public class Student : User
{
    public override int MaxActiveRentals => 2;

    public Student(string firstName, string lastName) : base(firstName, lastName) { }
}

public class Employee : User
{
    public override int MaxActiveRentals => 5;

    public Employee(string firstName, string lastName) : base(firstName, lastName) { }
}