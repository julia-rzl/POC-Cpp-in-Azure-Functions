using System.Runtime.InteropServices;

namespace POC_Cpp_Azure_Function.Calculators
{
    public class CalculateByDllImport
    {
        private const string DLL_NAME = "RZL.Lohn.Domain.Cpp64.dll";

        [DllImport(DLL_NAME)]
        public static extern IntPtr CreateCalculatorWithValues(double a, double b);

        [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern void CreateCalculator();

        [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern double Calculator_Addiere(IntPtr intPtr);

        [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern double Calculator_GetZahl1();

        [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern double Calculator_GetZahl2();

        [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern void Calculator_SetZahlen(double a, double b);
    }
}
