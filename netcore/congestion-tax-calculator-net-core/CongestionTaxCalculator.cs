using System;
using congestion.calculator.Vehicles;

namespace congestion.calculator;

public static class CongestionTaxCalculator
{
    private const int cMaximumTollTaxPerDay = 60;
    private const int cSinglChargeRuleIntervalInMinutes = 60;

    /// <summary>
    /// Calculate the total toll tax for one day
    /// </summary>
    /// <param name="vehicle">The vehicle that need to pay toll tax</param>
    /// <param name="dates">Date and time of all passes on one day</param>
    /// <returns>The total congestion tax for that day</returns>
    /// <exception cref="ArgumentOutOfRangeException">Throws if the
    /// <paramref name="vehicle"/> type is not supported.</exception>
    public static int GetTax(IVehicle vehicle, DateTime[] dates)
    {
        if (IsTollFreeVehicle(vehicle))
            return 0;

        DateTime intervalStart = dates[0];
        int tempTax = GetTax(intervalStart);
        int totalTax = tempTax;

        for (int i = 1; i < dates.Length; ++i)
        {
            DateTime date = dates[i];
            int nextTax = GetTax(date);

            var timeDiff = date - intervalStart;

            // Applying single charge rule:
            // https://github.com/volvo-cars/congestion-tax-calculator/blob/main/ASSIGNMENT.md#the-single-charge-rule
            if (timeDiff.TotalMinutes <= cSinglChargeRuleIntervalInMinutes)
            {
                if (nextTax >= tempTax)
                {
                    // Cancel the last added tax
                    totalTax -= tempTax;
                    // Update the last added tax
                    tempTax = nextTax;
                    // Add the higher tax
                    totalTax += tempTax;
                }
            }
            else
            {
                // Reset the interval
                intervalStart = date;
                // Update the last added tax
                tempTax = nextTax;
                // Add the tax to the total tax
                totalTax += tempTax;
            }
        }

        if (totalTax > cMaximumTollTaxPerDay)
            totalTax = cMaximumTollTaxPerDay;

        return totalTax;
    }

    private static bool IsTollFreeVehicle(IVehicle vehicle)
    {
        // https://github.com/volvo-cars/congestion-tax-calculator/blob/main/ASSIGNMENT.md#tax-exempt-vehicles
        switch (vehicle.VehicleType)
        {
            case VehicleType.Motorcycle:
            case VehicleType.Tractor:
            case VehicleType.Emergency:
            case VehicleType.Diplomat:
            case VehicleType.Foreign:
            case VehicleType.Military:
            case VehicleType.Bus:
                return true;
            case VehicleType.Car:
                return false;
            default:
                throw new ArgumentOutOfRangeException(nameof(vehicle.VehicleType));
        }
    }

    private static int GetTax(DateTime date)
    {
        if (IsTollFreeDate(date))
            return 0;

        int hour = date.Hour;
        int minute = date.Minute;

        // The statement order matters
        // https://github.com/volvo-cars/congestion-tax-calculator/blob/main/ASSIGNMENT.md#congestion-tax-rules-in-gothenburg

        if (hour == 6 && minute <= 29)
            return 8;

        if (hour == 6)
            return 13;

        if (hour == 7)
            return 18;

        if (hour == 8 && minute <= 29)
            return 13;

        if (hour >= 8 && hour <= 14)
            return 8;

        if (hour == 15 && minute <= 29)
            return 13;

        if (hour >= 15 && hour <= 16)
            return 18;

        if (hour == 17)
            return 13;

        if (hour == 18 && minute <= 29)
            return 8;

        return 0;
    }

    private static bool IsTollFreeDate(DateTime date)
    {
        int year = date.Year;
        int month = date.Month;
        int day = date.Day;

        if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday)
            return true;

        // note The scope is limited to the 2013 calender
        if (year == 2013)
        {
            if ((month == 1 && day == 1) ||
                (month == 3 && (day == 28 || day == 29)) ||
                (month == 4 && (day == 1 || day == 30)) ||
                (month == 5 && (day == 1 || day == 8 || day == 9)) ||
                (month == 6 && (day == 5 || day == 6 || day == 21)) ||
                (month == 7) ||
                (month == 11 && day == 1) ||
                (month == 12 && (day == 24 || day == 25 || day == 26 || day == 31)))
            {
                return true;
            }
        }

        return false;
    }
}