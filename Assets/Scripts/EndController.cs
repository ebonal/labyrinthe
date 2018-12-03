using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndController : MonoBehaviour {

    public AudioClip soundWin;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (GlobalVariables.playSound)
                GetComponent<AudioSource>().PlayOneShot(soundWin);

			GlobalVariables.isWin = true;

        }
    }
}
