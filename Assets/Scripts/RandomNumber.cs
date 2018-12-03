using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomNumber : MonoBehaviour {


	public static int getDirection () {

		return Random.Range (1, 5);
	}

    public static int getRandomInRange (int max) {

        return Random.Range(0, max);
    }
}
