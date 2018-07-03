using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CameraController : MonoBehaviour {

    public GameObject player;
    public Canvas menuCanvas;
	public Canvas winCanvas;
    public Button btSoundMute;
    public Button btMusicMute;
    public Sprite spMute;
    public Sprite spNotMute;

    private Vector3 offset;
    


    void Start () {
        offset = transform.position - player.transform.position;

        menuCanvas.enabled = false;
    }
	
	void Update () {
        transform.position = player.transform.position + offset;

		if (GlobalVariables.isPaused)
        {
            Time.timeScale = 0f;
            menuCanvas.enabled = true;
        }
        else
        {
            Time.timeScale = 1.0f;
            menuCanvas.enabled = false;
        }

		if (GlobalVariables.isWin)
		{
			Time.timeScale = 0f;
			winCanvas.enabled = true;
		}
		else
		{
			Time.timeScale = 1.0f;
			winCanvas.enabled = false;
		}
	}

    public void Pause()
    {
		GlobalVariables.isPaused = !GlobalVariables.isPaused;
        
    }

    public void MusicMute()
    {
        if (GlobalVariables.playMusic)
        {
            GlobalVariables.playMusic = false;
            btMusicMute.GetComponent<Image>().sprite = spMute;
            Camera.main.GetComponent<AudioSource>().mute = true;
        }
        else
        {
            GlobalVariables.playMusic = true;
            btMusicMute.GetComponent<Image>().sprite = spNotMute;
            Camera.main.GetComponent<AudioSource>().mute = false;
        }
    }

    public void SoundMute()
    {
        if (GlobalVariables.playSound)
        {
            GlobalVariables.playSound = false;
            btSoundMute.GetComponent<Image>().sprite = spMute;
        }
        else
        {
            GlobalVariables.playSound = true;
            btSoundMute.GetComponent<Image>().sprite = spNotMute;
        }
    }

	public void AppReplay()
	{
		GlobalVariables.isWin = false;
		SceneManager.LoadScene(1);

	}

    public void AppQuit()
    {
		GlobalVariables.isWin = false;
		SceneManager.LoadScene(0);
    }
}
