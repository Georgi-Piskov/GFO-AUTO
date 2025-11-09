using NoActivityFiler.Validators;
using Xunit;

namespace NoActivityFiler.Tests;

public class EikAttributeTests
{
    private static string BuildValid9Digit()
    {
        // Choose 8 digits and compute check digit per spec
        var digits = new[] {1,2,3,4,5,6,7,8};
        int[] w1 = {1,2,3,4,5,6,7,8};
        int sum = 0; for (int i = 0; i < 8; i++) sum += digits[i]*w1[i];
        int r = sum % 11; int check;
        if (r < 10) check = r; else { int[] w2 = {3,4,5,6,7,8,9,10}; sum = 0; for (int i=0;i<8;i++) sum += digits[i]*w2[i]; r = sum % 11; check = r < 10 ? r : 0; }
        return string.Concat(digits.Select(d => d.ToString())) + check.ToString();
    }

    [Fact]
    public void Valid_9_Digit_Eik_Passes()
    {
        var attr = new EikAttribute();
        var eik = BuildValid9Digit();
        Assert.True(attr.IsValid(eik));
    }

    [Theory]
    [InlineData("12345678")] // too short
    [InlineData("123456789012")] // wrong length
    [InlineData("12345678901234")] // too long
    [InlineData("ABCDEFGHI")] // non-digits
    public void Invalid_Eik_Fails(string eik)
    {
        var attr = new EikAttribute();
        Assert.False(attr.IsValid(eik));
    }
}

