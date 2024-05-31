namespace SudokuSolver;

public static class MathHelper
{
    public static double MyPow(double x, int n)
    {
        if (x == 0)
        {
            return 0;
        }

        if (n == 0)
        {
            return 1;
        }

        if (x == 1)
        {
            return 1;
        }

        if (x == -1)
        {
            return n % 2 == 0 ? 1 : -1;
        }

        long pow = n;
        if (n < 0)
        {
            x = 1 / x;
            pow = -pow;
        }

        var total = 1.0;
        while (0 < pow)
        {
            if (pow % 2 == 0)
            {
                x = x * x;
                pow /= 2;
            }
            else
            {
                total *= x;
                pow--;
            }
        }

        return total;
    }

    public static int MyDivide(int dividend, int divisor)
    {
        if (divisor == 1)
        {
            return dividend;
        }

        if (divisor == -1)
        {
            return -dividend;
        }

        var isNegative = dividend < 0 ^ divisor < 0;
        dividend = Math.Abs(dividend);
        divisor = Math.Abs(divisor);

        if (dividend < divisor)
        {
            return 0;
        }

        if (dividend == divisor)
        {
            return isNegative ? -1 : 1;
        }

        long division = 0;
        long val = 0;
        var newDivisor = (long)divisor;
        for (var index = 31; 0 <= index; index--)
        {
            long newVal = val + (newDivisor << index);
            if (newVal <= dividend)
            {
                val = newVal;
                division |= 1L << index;
            }
        }

        return isNegative ? -(int)division : (int)division;
    }
}