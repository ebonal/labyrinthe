using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapHoleController : MonoBehaviour {

    public AudioClip soundFall;

    private float sec = 0.15f;

    void OnTriggerEnter (Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            StartCoroutine(FallInHole(other));

        }
    }

    IEnumerator FallInHole (Collider player)
    {
        // Son AHHHHHH 
        if (GlobalVariables.playSound)
            GetComponent<AudioSource>().PlayOneShot(soundFall);

        yield return new WaitForSeconds (sec);
        
        // Reset Forces
        Rigidbody rb = player.GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        player.gameObject.transform.position = GlobalVariables.startPosition;
    }
}
