using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour
{
    //How long should a bullet be alive for before it despawns?
    [SerializeField] private float Life = 0;
    //Keeps track of how long the bullet has been alive for?
    private float Lifetime = 3;

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
        if(Manager.Instance.IsUpdatable()) //Only update if VR player is alive or game is not paused
        {
             Lifetime -= Time.deltaTime;
             if (Lifetime < 0.0f) //check if the bullet is too old
            {
                this.gameObject.SetActive(false);
            }
        }
    }
}
