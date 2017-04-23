using UnityEngine;
using System.Collections;

public class FlareScript : MonoBehaviour {

    [SerializeField] private float FlareDeactivate = 1.5f;
    private float timer = 0.0f;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        timer += Time.deltaTime;
	    if(timer>FlareDeactivate)
        {
            this.GetComponent<GameObject>().SetActive(false);
        }
	}
}
