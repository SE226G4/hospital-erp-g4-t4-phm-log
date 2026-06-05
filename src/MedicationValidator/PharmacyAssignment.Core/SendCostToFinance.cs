namespace pharmAssignment.core;
public class DispensingLog
{
    public decimal Cost { get; set; }
    public bool IsSentToFinance { get; set; }

    public bool SendCostToFinance(bool insuranceProcessed,bool financeSystemAvailable,bool billingItemCreated)
    {
        if (!CanSend(insuranceProcessed,billingItemCreated))
            return false;

        return SubmitToFinance(financeSystemAvailable);
    }
    private bool CanSend(bool insuranceProcessed,bool billingItemCreated)
    {
        return Cost > 0
            && !IsSentToFinance
            && billingItemCreated
            && insuranceProcessed;
    }
    private bool SubmitToFinance(
        bool financeSystemAvailable)
    {
        if (!financeSystemAvailable)
            return false;
        IsSentToFinance = true;
        return true;
    }
}