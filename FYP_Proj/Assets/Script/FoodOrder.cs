[System.Serializable]
public class FoodOrder {
    // Start is called before the first frame update
    public string food;
    public int amt;

    public FoodOrder(string food)
    {
        this.food = food;
        amt = 0;
    }

    public FoodOrder()
    {
    }
}
