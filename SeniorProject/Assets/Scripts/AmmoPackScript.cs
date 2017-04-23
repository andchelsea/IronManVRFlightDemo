using UnityEngine;
using System.Collections;

public class AmmoPackScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "MainCamera")
        {
            Debug.Log("Player picked you up");
            //ADD in making this obj inactive????
        }
    }
}
