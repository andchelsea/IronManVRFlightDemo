using UnityEngine;
using System.Collections;

public class FlareScript : MonoBehaviour {

    [SerializeField] private float FlareDeactivate = 1.5f;
    private float timer = 0.0f;

    // Use this for initialization
    void Start ()
    {
	
	}

    public void Reset()
    {
        timer = 0.0f;
        this.gameObject.SetActive(true);
        this.GetComponent<ParticleSystem>().Play();
    }
	
	// Update is called once per frame
	void Update () {
        if (Manager.Instance.IsUpdatable())
        {
            timer += Time.deltaTime;
            if (timer > FlareDeactivate)
            {
                this.gameObject.SetActive(false);
                this.GetComponent<ParticleSystem>().Stop();
            }
        }
	}
}
