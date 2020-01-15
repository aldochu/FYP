
public class Scores
{
    public string Result; //either pass or fail
    public string FoodToOrder1, FoodToOrder2, FoodOrdered1, FoodOrdered2;
    public int numOfCancelledOrder, numOfWrongOrdered, numOfWrongPayment, numOfFailedPick;
    public float payable, amtPaid;

    private Tray mytray;
    // Start is called before the first frame update
    public Scores(Tray mytray)
    {
        this.mytray = mytray;
    }

    public Scores()
    {
        mytray = new Tray();
        numOfCancelledOrder = numOfWrongOrdered = numOfWrongPayment = numOfFailedPick = 0;
        payable = amtPaid = 0;
        Result = "Fail";
    }
}
