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
    public float gravMultiplier= 1.0f;
    //privates
    private float AttackDelay = 0;
    [SerializeField] private float AttackCoolDown = 0.5f; //arbitrary starting numbers, playtest
    [SerializeField] private int Health = 10; //arbitrary starting numbers, playtest
    [SerializeField] private int Ammo = 10; //arbitrary starting numbers, playtest
    [SerializeField] private int NumAmmo = 10; //arbitrary starting numbers, playtest
    void Awake ()
    {
        if (Instance != null && Instance != this)
            Destroy(this.gameObject);
        Instance = this;
        gravMultiplier = highGrav;
    }

    void OnCollisionExit(Collision other)
    {

        if (other.collider.tag == "Land")
        {
            Rb.mass = 0.5f;
        }
    }
    public int GetHealth() { return Health; }
    public bool Shootable()
    {
        if (Ammo > 0 && AttackDelay > AttackCoolDown && Manager.Instance.IsUpdatable())
        {
            --Ammo;
            AttackDelay = 0;
            return true;
        }
        else
            return false;
    }

    void OnCollisionEnter(Collision other)
    {

        if (other.collider.tag == "Land")
        {
            Rb.mass = 2;
        }
        //may need to be on the hands (and body?) for ammo
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            Debug.Log("Enemy hit you!");
            --Health;

            if (Health <= 0)
            {
                Manager.Instance.SetUpdatable(false);
            }
        }

        if (other.tag == "Ammo")
        {
            Debug.Log("VR picked up ammo");
            Ammo += NumAmmo;
        }
        if(other.tag=="Land")
        {
            gravMultiplier = highGrav;
        }
    }
    // Use this for initialization
	void OnTriggerExit(Collider other)
    {
        if (other.tag == "Land")
        {
            gravMultiplier = lowGrav;
        }
    }
	// Update is called once per frame
	void Update ()
    {
        if (Manager.Instance.IsUpdatable())
        {
            AttackDelay += Time.deltaTime;
        }
        Rb.AddForce(Physics.gravity*gravMultiplier,ForceMode.Force);
    }
}
