using System.Text.RegularExpressions;

namespace ProgramacionV.Api.Validators;

public static class TelefonoValidator
{
    private static readonly Regex Formato = new(@"^3\d{9}$");

    public static bool EsValido(string telefono)
    {
        return !string.IsNullOrWhiteSpace(telefono) && Formato.IsMatch(telefono);
    }
}
