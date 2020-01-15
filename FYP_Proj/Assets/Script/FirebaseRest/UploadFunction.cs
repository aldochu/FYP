using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Proyecto26;

public class UploadFunction : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //CreateUser();
    }

    // Update is called once per frame
    public void CreateUser()
    {
        Tray myTray = new Tray();
        myTray.tray = 1;
        myTray.utensil1 = 1;
        myTray.utensil2 = 2;


        string CurrentDateTime = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm");

        RestClient.Put("https://fyphealtyliving.firebaseio.com/"+"test/"+ CurrentDateTime + ".json", myTray);
    }

    public void GetUser()
    {
        RestClient.Get<Tray>("https://fyphealtyliving.firebaseio.com/" + "test" + "/" + "2020-01-15 01:38" + ".json").Then(response =>
        {
            //do what ever function you want after retrive the information, it's a callback function
            Tray myTray = new Tray();
            myTray = response;

            Debug.Log("myTray" + myTray.utensil2);

        });
    }

    public void UploadScores(Scores sc)
    {
        string CurrentDateTime = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm");

        RestClient.Put("https://fyphealtyliving.firebaseio.com/" + "scores/" + "username" + CurrentDateTime + ".json", sc);
    }
}
