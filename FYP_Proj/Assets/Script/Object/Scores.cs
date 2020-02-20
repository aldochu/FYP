
public class Scores
{
    public string Result; //either pass or fail
    public string FoodToOrder1, FoodToOrder2, FoodOrdered1, FoodOrdered2;
    public int numOfCancelledOrder, numOfWrongOrdered/*this will compare the food suppose to buy with the food brought*/,numOfWrongPayment, numOfFailedSeatPick;
    public float payable, amtPaid;

    //this section is for timing
    public float timeToGetTray, timeToGetUtenil1, timeToGetUtensil2;

    public int tray;
    public int utensil1;
    public int utensil2;

    // Start is called before the first frame update
    public Scores(Tray mytray)
    {
        this.tray = mytray.tray;
        this.utensil1 = mytray.utensil1;
        this.utensil2 = mytray.utensil2;
    }

    public void updateTrayValue(Tray myTray)
    {
        tray = myTray.tray;
        utensil1 = myTray.utensil1;
        utensil2 = myTray.utensil2;
    }

    public Scores()
    {
        tray = utensil1 = utensil2 = numOfCancelledOrder = numOfWrongOrdered = numOfWrongPayment = numOfFailedSeatPick = 0;
        payable = amtPaid = 0;
        Result = "Fail";
    }
}
