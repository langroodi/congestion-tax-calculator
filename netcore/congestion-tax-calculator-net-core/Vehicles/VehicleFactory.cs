using System;

namespace congestion.calculator.Vehicles
{
    /// <summary>
    /// Factory pattern to create a <see cref="IVehicle"/>
    /// </summary>
    public static class VehicleFactory
    {
        public static IVehicle Create(VehicleType vehicleType)
        {
            switch (vehicleType)
            {
                case VehicleType.Motorcycle:
                    return new Motorbike();
                case VehicleType.Car:
                    return new Car();
                default:
                    throw new ArgumentOutOfRangeException(nameof(vehicleType));
            }
        }
    }
}
