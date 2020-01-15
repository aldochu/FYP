using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssistanceFunction : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject ButtonToMenu, FoodMenu;

    private string UIMenu1, UIMenu2;

    private int UIQUantity1, FoodAmt;

    public void setFood(string menu1, string menu2, int quantity1, int FoodAmt)
    {
        UIMenu1 = menu1;
        UIMenu2 = menu2;
        UIQUantity1 = quantity1;
        this.FoodAmt = FoodAmt;
    }

    public void OpenMenu()
    {
        FoodMenu.SetActive(true);
        ButtonToMenu.SetActive(false);
        FoodMenu.GetComponent<UpdateMenu>().UpdateFoodUI(UIMenu1, UIMenu2, UIQUantity1, FoodAmt);
    }

    public void CloseMenu()
    {
        ButtonToMenu.SetActive(true);
        FoodMenu.SetActive(false); 
    }


}
