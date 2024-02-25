namespace BraunauMobil.VeloBasar.IntegrationTests.Steps.SecondBasar.Sell;

public class SellProducts(TestContext context)
{
    public async Task Run()
    {
        await new Sale1(context).Run();
        await new LockProductUnlockAndSellIt(context).Run();
        await new LooseProductUnlockAndSellIt(context).Run();
        await new SellTwoProductsCancelOneSellItAgainCancelItAndFinallySellIt(context).Run();
    }
}
