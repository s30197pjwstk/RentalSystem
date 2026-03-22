namespace RentalSystem.Models;

public enum EquipmentStatus { Available, Rented, Unavailable }

public abstract class Equipment
{
    public string Id { get; } = Guid.NewGuid().ToString("N").Substring(0, 6); 
    public string Name { get; set; }
    public EquipmentStatus Status { get; set; } = EquipmentStatus.Available;

    protected Equipment(string name)
    {
        Name = name;
    }

    public abstract string GetDetails();
}

public class Laptop : Equipment
{
    public int RamGb { get; set; }
    public string Processor { get; set; }

    public Laptop(string name, int ramGb, string processor) : base(name)
    {
        RamGb = ramGb;
        Processor = processor;
    }

    public override string GetDetails() => $"Laptop [RAM: {RamGb}GB, CPU: {Processor}]";
}

public class Projector : Equipment
{
    public string Resolution { get; set; }
    public int Lumens { get; set; }

    public Projector(string name, string resolution, int lumens) : base(name)
    {
        Resolution = resolution;
        Lumens = lumens;
    }

    public override string GetDetails() => $"Projector [Res: {Resolution}, Brightness: {Lumens}lm]";
}

public class Camera : Equipment
{
    public double Megapixels { get; set; }
    public bool HasLensIncluded { get; set; }

    public Camera(string name, double megapixels, bool hasLensIncluded) : base(name)
    {
        Megapixels = megapixels;
        HasLensIncluded = hasLensIncluded;
    }

    public override string GetDetails() => $"Camera [{Megapixels}MP, Lens: {(HasLensIncluded ? "Yes" : "No")}]";
}