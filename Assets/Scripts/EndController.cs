using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndController : MonoBehaviour {

    public AudioClip soundWin;

    void Update () {
        transform.Rotate(new Vector3(0, -45, 0) * Time.deltaTime);
    }

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
