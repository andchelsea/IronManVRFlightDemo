using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour
{
    //Variables
    [SerializeField] private float Life = 0;//How long should a bullet be alive for before it despawns?
    private float Lifetime = 3;//Keeps track of how long the bullet has been alive for?

    void Start ()
    {
        Lifetime = Life;
    }
	
    //Function called everytime a bullet is spawned
    public void  Reset()
    {
        Lifetime = Life;
        this.gameObject.SetActive(true);
        this.GetComponent<Rigidbody>().velocity = Vector3.zero; 
    }

	void Update ()
    {
        //Only update if VR player is alive or game is not paused
        if(Manager.Instance.IsUpdatable()) 
        {
             Lifetime -= Time.deltaTime;

             //check if the bullet is too old
             if (Lifetime < 0.0f) 
            {
                this.gameObject.SetActive(false);
            }
        }
    }
}
