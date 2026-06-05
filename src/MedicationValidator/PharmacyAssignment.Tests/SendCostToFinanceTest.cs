using Xunit;
using pharmAssignment.core;
namespace pharmAssignment.Tests

{
    public class DispensingLogTests
{
    [Fact]
    public void ReturnFalse_WhenCostIsZero()
    {
        var log = new DispensingLog
        {
            Cost = 0
        };

        Assert.False(
            log.SendCostToFinance(true,true,true));
    }

    [Fact]
    public void ReturnFalse_WhenAlreadySent()
    {
        var log = new DispensingLog
        {
            Cost = 100,
            IsSentToFinance = true
        };

        Assert.False(
            log.SendCostToFinance(true,true,true));
    }

    [Fact]
    public void ReturnFalse_WhenBillingItemNotCreated()
    {
        var log = new DispensingLog
        {
            Cost = 100
        };

        Assert.False(
            log.SendCostToFinance(true,true,false));
    }

    [Fact]
    public void ReturnFalse_WhenInsuranceNotProcessed()
    {
        var log = new DispensingLog
        {
            Cost = 100
        };

        Assert.False(
            log.SendCostToFinance(false,true,true));
    }

    [Fact]
    public void ReturnFalse_WhenFinanceSystemUnavailable()
    {
        var log = new DispensingLog
        {
            Cost = 100
        };

        Assert.False(
            log.SendCostToFinance(true,false,true));
    }

    [Fact]
    public void ReturnTrue_WhenAllConditionsValid()
    {
        var log = new DispensingLog
        {
            Cost = 100
        };

        Assert.True(
            log.SendCostToFinance(true,true,true));
    }
}
}