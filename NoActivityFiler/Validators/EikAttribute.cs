using System.ComponentModel.DataAnnotations;

namespace NoActivityFiler.Validators;

// Validates Bulgarian EIK/UIC: 9 or 13 digits with checksum
public class EikAttribute : ValidationAttribute
{
    public EikAttribute()
    {
        ErrorMessage = "Невалиден ЕИК.";
    }

    public override bool IsValid(object? value)
    {
        if (value is null) return true; // Required handled elsewhere
        var s = value.ToString()!.Trim();
        if (string.IsNullOrEmpty(s)) return true;
        if (!(s.Length == 9 || s.Length == 13)) return false;
        if (!s.All(char.IsDigit)) return false;

        if (s.Length == 9) return Validate9(s);
        return Validate13(s);
    }

    private static bool Validate9(string s)
    {
        int[] w1 = { 1,2,3,4,5,6,7,8 };
        int sum = 0;
        for (int i = 0; i < 8; i++) sum += (s[i]-'0') * w1[i];
        int r = sum % 11;
        int check;
        if (r < 10)
            check = r;
        else
        {
            int[] w2 = { 3,4,5,6,7,8,9,10 };
            sum = 0;
            for (int i = 0; i < 8; i++) sum += (s[i]-'0') * w2[i];
            r = sum % 11;
            check = r < 10 ? r : 0;
        }
        return check == (s[8]-'0');
    }

    // 13-digit UIC for branches: first 9 is valid 9-digit; 13th is checksum over first 12
    private static bool Validate13(string s)
    {
        if (!Validate9(s.Substring(0,9))) return false;
        int[] w1 = { 2,7,3,5,5,7,9,4,6,8,2,7 };
        int sum = 0;
        for (int i = 0; i < 12; i++) sum += (s[i]-'0') * w1[i];
        int r = sum % 11;
        int check;
        if (r < 10)
            check = r;
        else
        {
            int[] w2 = { 4,9,5,7,7,9,11,6,8,10,4,9 };
            sum = 0;
            for (int i = 0; i < 12; i++) sum += (s[i]-'0') * w2[i];
            r = sum % 11;
            check = r < 10 ? r : 0;
        }
        return check == (s[12]-'0');
    }
}

