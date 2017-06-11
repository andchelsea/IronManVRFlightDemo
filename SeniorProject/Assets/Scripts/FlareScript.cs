using UnityEngine;
using System.Collections;

public class FlareScript : MonoBehaviour
{
    //Kill flare after lifespan
    [SerializeField] private float FlareDeactivate = 1.5f;
    private float timer = 0.0f;

    //Called when a flare is spawned
    public void Reset()
    {
        timer = 0.0f;
        this.gameObject.SetActive(true);
        this.GetComponent<ParticleSystem>().Play();
    }
	
	void Update () {
        if (Manager.Instance.IsUpdatable())//Only update if VR player is alive or game is not paused
        {
            timer += Time.deltaTime;
            if (timer > FlareDeactivate)//if flare is old, kill it
            {
                this.gameObject.SetActive(false);
                this.GetComponent<ParticleSystem>().Stop();//if .Stop() is not called, will cause issues with .Play()
            }
        }
	}
}
