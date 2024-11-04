using Xunit;
using System;
using ConsoleApp1;

namespace TestMultiplier;

public class MultiplierTest
{
    [Fact]
    public  void TestMultiply(){
        Multiplier multiplier = new Multiplier();
        
        int result = multiplier.Multiply(2, 3);
        Assert.Equal(6, result);
    }
}