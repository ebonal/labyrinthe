using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour {

    public Button btPlay, btExit, btSmall, btMedium, btLarge, btExtraLarge;
    public Text txtTitle, txtTitleShadow;


    public void PlayClick () {

        txtTitle.text = "Taille du labyrinthe";
        txtTitle.fontSize = 55;
        txtTitleShadow.text = "Taille du labyrinthe";
        txtTitleShadow.fontSize = 55;

        btPlay.gameObject.SetActive(false);
        btExit.gameObject.SetActive(false);

        btSmall.gameObject.SetActive(true);
        btMedium.gameObject.SetActive(true);
        btLarge.gameObject.SetActive(true);
        btExtraLarge.gameObject.SetActive(true);
        
    }
	
    public void SizeClick(int choice) {

        switch (choice)
        {
            case 1:
                GlobalVariables.mazeSize = 6;
                break;
            case 2:
                GlobalVariables.mazeSize = 8;
                break;
            case 3:
                GlobalVariables.mazeSize = 10;
                break;
            case 4:
                GlobalVariables.mazeSize = 12;
                break;
            default:
                GlobalVariables.mazeSize = 8;
                break;
        }
        GlobalVariables.isPaused = false;

        SceneManager.LoadScene(1);
    }

    public void Exit () {
        Application.Quit();
    }
}
