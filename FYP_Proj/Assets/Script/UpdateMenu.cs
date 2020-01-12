using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateMenu : MonoBehaviour
{
    public Text Menu1, Menu2, Quantity1, Quantity2;

    private string UIMenu1, UIMenu2, UIQUantity1, UIQUantity2;

    private bool update = false;
    // Start is called before the first frame update


    public void UpdateFoodUI(string menu1, string menu2, int quantity1, int FoodAmt)
    {
        if (FoodAmt < 2)
        {
            UIMenu1 = menu1;
            UIQUantity1 = "X 1";
        }
        else
        {
            UIMenu1 = menu1;
            if (quantity1 == 2)
            {        
                UIQUantity1 = "X 2";
            }
            else
            {
                UIMenu2 = menu2;
                UIQUantity1 = "X 1";
                UIQUantity2 = "X 1";
            }
        }


        update = true;

    }

    // Update is called once per frame
    void Update()
    {
        if (update)
        {
            Menu1.text = UIMenu1;
            Menu2.text = UIMenu2;
            Quantity1.text = UIQUantity1;
            Quantity2.text = UIQUantity2;
            update = false;
        }
        

    }
}
