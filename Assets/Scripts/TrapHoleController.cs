using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapHoleController : MonoBehaviour {

    public AudioClip soundFall;

    private float sec = 0.15f;

    void Update () {
        transform.Rotate(new Vector3 (0, 25, 0) * Time.deltaTime);
	}

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

        
        Vector3 v = GameObject.FindGameObjectWithTag("Start").transform.position;
        v.y = v.y + 1.2f;

        // Reset Forces
        Rigidbody rb = player.GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        player.gameObject.transform.position = v;
        Debug.Log("retour start point");
    }
}
