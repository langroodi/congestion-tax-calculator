using congestion.calculator;
using congestion.calculator.Vehicles;

namespace congestion_tax_calculator_tests;

public class CongestionTaxCalculatorTest
{
    [TestCase(VehicleType.Car, "2013-01-13 8:00:00", 0)]
    [TestCase(VehicleType.Motorcycle, "2013-01-14 8:00:00", 0)]
    [TestCase(VehicleType.Car, "2013-01-14 4:00:00", 0)]
    [TestCase(VehicleType.Car, "2013-01-14 21:00:00", 0)]
    [TestCase(VehicleType.Car, "2013-01-15 8:00:00", 13)]
    [TestCase(VehicleType.Car, "2013-02-07 06:23:27;2013-02-07 15:27:00", 21)]
    [TestCase(VehicleType.Car, "2013-02-08 06:20:27;2013-02-08 06:27:00;2013-02-08 14:35:00;2013-02-08 15:29:00", 21)]
    [TestCase(VehicleType.Car, "2013-02-08 17:49:00;2013-02-08 18:29:00;2013-02-08 18:35:00", 13)]
    [TestCase(VehicleType.Car, "2013-03-28 14:07:27", 0)]
    public void GetTaxTest(VehicleType vehicleType, string datesStr, int expectedTax)
    {
        IVehicle vehicle = VehicleFactory.Create(vehicleType);

        var dateTimes = new List<DateTime>();
        string[] dates = datesStr.Split(";");
        foreach (string date in dates)
        {
            dateTimes.Add(DateTime.Parse(date));
        }

        int actualTax = CongestionTaxCalculator.GetTax(vehicle, dateTimes.ToArray());

        Assert.AreEqual(expectedTax, actualTax);
    }
}