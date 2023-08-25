namespace congestion.calculator.Vehicles;

public enum VehicleType
{
    Motorcycle = 0,
    Tractor = 1,
    Emergency = 2,
    Diplomat = 3,
    Foreign = 4,
    Military = 5,
    Bus = 6,
    Car = 7
}

/// <summary>
/// A vehicle that may need to pay toll tax
/// </summary>
public interface IVehicle
{
    /// <summary>
    /// Vehicle type for toll tax amount evaluation
    /// </summary>
    VehicleType VehicleType { get; }
}