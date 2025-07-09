using RZL.Lohn.DomainCLI;

namespace POC_Cpp_Azure_Function.Calculators
{
    public class CalculateByCLI
    {
        public double Add(double a, double b)
        {
            var calculator = new Calculator(a, b);
            return calculator.Addiere();
        }
    }
}
