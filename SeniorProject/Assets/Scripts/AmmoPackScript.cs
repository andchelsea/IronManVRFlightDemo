using UnityEngine;
using System.Collections;

public class AmmoPackScript : MonoBehaviour {

    //Unused, TODO get the ammo to highlight when the PC player mouses over it
    //[SerializeField] private Shader unlit;//make a shader
    //[SerializeField] private Shader lit;//make a shader

    //Variables
    [SerializeField] private int numAmmo=5;
    private AudioSource PickupSound;

    //Find components
    void Start ()
    {
        PickupSound = GetComponent<AudioSource>();
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
