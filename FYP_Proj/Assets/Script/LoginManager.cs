using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoginManager : MonoBehaviour
{
    private bool LoadNow = false;
    // Start is called before the first frame update
    private bool displayCanvas = false;
    public Canvas[] SelectionCanvas;

    public void LanguageChineseSelection()
    {

        StaticVariable.ChineseLanguage = true;

        DisplayStageSelection();
    }


    public void LanguageEnglishSelection()
    {

        StaticVariable.ChineseLanguage = false;

        DisplayStageSelection();
    }

    private void DisplayStageSelection()
    {
        if (!displayCanvas)
        {
            SelectionCanvas[0].enabled = true;
            SelectionCanvas[1].enabled = true;
            displayCanvas = true;
        }     
    }


    public void loadScene(bool trainingScene)
    {
        if (trainingScene)
            StaticVariable.PracticeMode = true;
        else
            StaticVariable.PracticeMode = false;

        LoadNow = true;
        //SceneManager.LoadScene("SampleScene");
    }

    void Update()
    {
        // Press the space key to start coroutine
        if (LoadNow)
        {
            // Use a coroutine to load the Scene in the background
            StartCoroutine(LoadYourAsyncScene());
            LoadNow = false;
        }

        if (Input.GetKeyDown("w"))
        {
            LanguageEnglishSelection();
        }
    }

    IEnumerator LoadYourAsyncScene()
    {
        // The Application loads the Scene in the background as the current Scene runs.
        // This is particularly good for creating loading screens.
        // You could also load the Scene by using sceneBuildIndex. In this case Scene2 has
        // a sceneBuildIndex of 1 as shown in Build Settings.

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("SampleScene");

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}
