using UnityEngine;
using System.Collections;

public class VrPlayer : MonoBehaviour {

    public static VrPlayer Instance;

    //Variables
    [SerializeField] public Rigidbody Rb;
    [SerializeField] public float AngleOffset = 90;

    [Space(10)]
    //Flying Variables
    [SerializeField] public float FlySpeed = 15; 
    [SerializeField] public float MaxSpeed = 150; 

    [Space(10)]
    //Gravity Manipulation
    [SerializeField] public float lowGrav = 0.5f;
    [SerializeField] public float highGrav= 1.0f;
    private float gravMultiplier= 1.0f;

    [Space(10)]
    //Shooting Variables
    [SerializeField] public float ProjectileSpeed = 15; 
    [SerializeField] public int Ammo = 10; 
    [SerializeField] private float AttackCoolDown = 0.5f;
    private int startAmmo;

    //Health Variables
    [SerializeField] public int Health = 10;
    private int startHealth;

    //Basic Singleton
    void Awake ()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;

        gravMultiplier = highGrav;
        startHealth = Health;
        startAmmo = Ammo;
    }

   //Reset Game 
   public void Reset()
    {
        this.transform.position.Set(0, 0.6f, -5);
        Health = startHealth;
        Ammo = startAmmo;
    }

    public int GetHealth() { return Health; }

    //Check if VR Player can shoot
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
        if (other.collider.tag == "Land")//Make it harder to take off
        {
            gravMultiplier = highGrav;
        }
    }
    void OnCollisionExit(Collision other)
    {
        if (other.collider.tag == "Land")//Make it easier to fly
        {
            gravMultiplier = lowGrav;
        }
    }

	// Update is called once per frame
	void Update ()
    {
        if (Manager.Instance.IsUpdatable())
        {
            Rb.AddForce(Physics.gravity*gravMultiplier, ForceMode.Force);//Apply gravity
        }
    }
}
