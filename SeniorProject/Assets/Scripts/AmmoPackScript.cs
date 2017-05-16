using UnityEngine;
using System.Collections;

public class AmmoPackScript : MonoBehaviour {

    [SerializeField] private Shader unlit;//make a shader
    [SerializeField] private Shader lit;//make a shader
    [SerializeField] private int numAmmo;
    AudioSource PickupSound;
    // Use this for initialization
    void Start ()
    {
        PickupSound = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    void OnMouseEnter()
    {
        //GetComponent<Material>().shader = lit;//testing required
    }

    void OnMouseExit()
    {
        //GetComponent<Material>().shader = unlit;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            //Debug.Log("Player picked you up");
            VrPlayer.Instance.Ammo += numAmmo;
            AmmoManager.Instance.AmmoUpdate();
            PickupSound.Play();
            this.gameObject.SetActive(false);
        }
    }
}
