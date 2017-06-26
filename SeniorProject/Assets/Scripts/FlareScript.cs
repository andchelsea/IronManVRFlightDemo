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
        //Only update if VR player is alive or game is not paused
        if (Manager.Instance.IsUpdatable())
        {
            timer += Time.deltaTime;

            //if flare is old, kill it
            if (timer > FlareDeactivate)
            {
                this.gameObject.SetActive(false);

                //if .Stop() is not called, will cause issues with .Play()
                this.GetComponent<ParticleSystem>().Stop();
            }
        }
	}
}
