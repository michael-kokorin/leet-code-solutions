using NUnit.Framework;

namespace SudokuSolver;

[TestFixture]
public class Tests
{
    [Test]
    public void Pow_Test()
    {
        var res = MathHelper.MyPow(2, -2147483648);
    }
    
    [Test]
    public void Divide_Test()
    {
        var res = MathHelper.MyDivide(10, 3);
    }
}