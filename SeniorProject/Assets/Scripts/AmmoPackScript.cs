using UnityEngine;
using System.Collections;

public class AmmoPackScript : MonoBehaviour {

    //Unused, trying to get the ammo to highlight when the PC player mouses over it
    [SerializeField] private Shader unlit;//make a shader
    [SerializeField] private Shader lit;//make a shader

    //Variables
    [SerializeField] private int numAmmo;
    private AudioSource PickupSound;

    //Find components
    void Start ()
    {
        PickupSound = GetComponent<AudioSource>();
	}

    //Used, trying to get the ammo to shine when PC player mouses over
    void OnMouseEnter()
    {
        //GetComponent<Material>().shader = lit;//testing required
    }

    //Unused, trying to get the ammo to stop shinning when the PC player moves away from it
    void OnMouseExit()
    {
        //GetComponent<Material>().shader = unlit;
    }

    //If the VR player collides with a ammo pack, increase their ammo
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            //Debug.Log("Player picked you up");
            VrPlayer.Instance.Ammo += numAmmo;
            AmmoManager.Instance.AmmoUpdate();
            PickupSound.Play();
            this.gameObject.SetActive(false);
            VrPlayer.Instance.AddScore(5);
        }
    }
}
