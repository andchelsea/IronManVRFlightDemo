using UnityEngine;
using System.Collections;

public class VrPlayer : MonoBehaviour {

    public static VrPlayer Instance;

    [SerializeField] public Rigidbody Rb;//probably head/torso, needed to apply velocity
    [SerializeField] public float FlySpeed = 15; //arbitrary starting numbers, playtest
    [SerializeField] public float MaxSpeed = 150; //arbitrary starting numbers, playtest
    [SerializeField] public float ProjectileSpeed = 15; //arbitrary starting numbers, playtest
    [SerializeField] public float lowGrav = 0.5f;
    [SerializeField] public float highGrav= 1.0f;
    [SerializeField] public int Ammo = 10; //arbitrary starting numbers, playtest
    public float gravMultiplier= 1.0f;
    //privates
    //private float AttackDelay = 0;
    [SerializeField] private float AttackCoolDown = 0.5f; //arbitrary starting numbers, playtest
    [SerializeField] public int Health = 10; //arbitrary starting numbers, playtest
   // [SerializeField] private int NumAmmo = 10; //arbitrary starting numbers, playtest
    void Awake ()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
        gravMultiplier = highGrav;
    }


    public int GetHealth() { return Health; }
    public bool Shootable(float AttackDelay)
    {
        if (Ammo > 0 && AttackDelay > AttackCoolDown && Manager.Instance.IsUpdatable())
        {
            --Ammo;
            AmmoManager.Instance.AmmoUpdate();
            return true;
        }
        else
            return false;
    }

    void OnCollisionEnter(Collision other)
    {

        //if (other.collider.tag == "Land")
        //{
        //    Rb.mass = 2;
        //}
        if (other.collider.tag == "Land")
        {
            gravMultiplier = highGrav;
        }
        //may need to be on the hands (and body?) for ammo
    }
    void OnCollisionExit(Collision other)
    {

       // if (other.collider.tag == "Land")
       // {
       //     Rb.mass = 0.5f;
       // }
        if (other.collider.tag == "Land")
        {
            gravMultiplier = lowGrav;
        }
    }

    // Use this for initialization

	// Update is called once per frame
	void Update ()
    {
        if (Manager.Instance.IsUpdatable())
        {
            //AttackDelay += Time.deltaTime;
            Rb.AddForce(Physics.gravity*gravMultiplier,ForceMode.Force);
        }
    }
}
