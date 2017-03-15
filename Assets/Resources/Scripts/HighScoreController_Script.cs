using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HighScoreController_Script : MonoBehaviour {

    public Text HighScoresList;

	// Use this for initialization
	void Start ()
    {
        loadHighScoresList(ref HighScoresList);
	}

    void loadHighScoresList(ref Text highScoresList)
    {
        highScoresList.text = "";
       
        for (int i = 0; i < MainController_Script.userData.highscore.Count; i++) 
        {
            highScoresList.text += MainController_Script.userData.highscore[i].ToString() + "\n";
        }
        if (MainController_Script.userData.highscore.Count > 0)
            highScoresList.text.Remove(highScoresList.text.Length - 1);
    }
}
