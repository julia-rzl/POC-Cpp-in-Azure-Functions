using System.Runtime.InteropServices;

namespace POC_Cpp_Azure_Function.Calculators
{
    public class CalculateByDllImport
    {
        // This dll need to be replaced y the actual C++ DLL from the nuget package
        [DllImport("PocDll.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int Add(int a, int b);
    }
}
