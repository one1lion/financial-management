Console.WriteLine("Quarterly Taxes Calculator");

// Get user input
Console.Write("Projected Gross Annual Income: ");
decimal projectedGrossAnnualIncome = decimal.Parse(Console.ReadLine());

Console.Write("Projected Business Expenses: ");
decimal projectedBusinessExpenses = decimal.Parse(Console.ReadLine());

Console.Write("Taxes Paid Year-to-Date: ");
decimal taxesPaidYearToDate = decimal.Parse(Console.ReadLine());

Console.Write("Additional W2 Salary: ");
decimal additionalW2Salary = decimal.Parse(Console.ReadLine());

Console.Write("State/Zip Code: ");
string stateZipCode = Console.ReadLine();

// Calculate taxable income
decimal taxableIncome = projectedGrossAnnualIncome - projectedBusinessExpenses - taxesPaidYearToDate - additionalW2Salary;

// Calculate federal estimated taxes
decimal federalTaxableIncome = taxableIncome - 25500m; // Standard deduction for married filing jointly
decimal federalTax = CalculateFederalTax(federalTaxableIncome);
decimal federalEstimatedTax = federalTax / 4;

// Calculate state estimated taxes
decimal stateEstimatedTax = CalculateStateTax(stateZipCode, taxableIncome) / 4;

// Display results
Console.WriteLine();
Console.WriteLine("Quarterly Estimated Taxes:");
Console.WriteLine($"Federal: {federalEstimatedTax:C}");
Console.WriteLine($"State: {stateEstimatedTax:C}");

Console.ReadLine();

static decimal CalculateFederalTax(decimal taxableIncome)
{
    decimal[] brackets = { 0m, 19750m, 80250m, 171050m, 326600m, 414700m, 622050m, decimal.MaxValue };
    decimal[] rates = { 0.10m, 0.12m, 0.22m, 0.24m, 0.32m, 0.35m, 0.37m };
    decimal tax = 0m;

    for (int i = 0; i < brackets.Length - 1; i++)
    {
        if (taxableIncome > brackets[i])
        {
            decimal taxableAmount = Math.Min(brackets[i + 1] - brackets[i], taxableIncome - brackets[i]);
            tax += taxableAmount * rates[i];
        }
        else
        {
            break;
        }
    }

    return tax;
}

static decimal CalculateStateTax(string stateZipCode, decimal taxableIncome)
{
    // TODO: Implement state tax calculation based on zip code
    return 0m;
}