using UnityEngine;
using System.Collections;

public class VrPlayer : MonoBehaviour {

    public static VrPlayer Instance;

    //Variables
    [SerializeField] public Rigidbody Rb;
    [SerializeField] public float AngleOffset = 90; //used for flying and shooting
    private int Score = 0;

    //Flying Variables
    [Space(10)]
    [SerializeField] public float FlySpeed = 15; 
    [SerializeField] public float MaxSpeed = 150; 

    //Gravity Manipulation
    [Space(10)]
    [SerializeField] public float lowGrav = 0.5f;
    [SerializeField] public float highGrav= 1.0f;
    private float gravMultiplier= 1.0f;

    //Shooting Variables
    [Space(10)]
    [SerializeField] public float ProjectileSpeed = 15; 
    [SerializeField] public int Ammo = 10; 
    [SerializeField] private float AttackCoolDown = 0.5f;
    private int startAmmo;

    //Health Variables
    [SerializeField] public int Health = 10;
    private int startHealth;

    void Awake ()
    {
        //Basic Singleton
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;

        //Set Variables
        gravMultiplier = highGrav;
        startHealth = Health;
        startAmmo = Ammo;
    }

    //Update score
    public void AddScore(int points)
    {
        Score += points;
        ScoreManager.Instance.UpdateScore();
    }
    public int GetScore() { return Score; }

   //Reset Game 
   public void Reset()
    {
        this.transform.position.Set(0, 0.6f, -5);
        Health = startHealth;
        Ammo = startAmmo;
        Score = 0;
        ScoreManager.Instance.UpdateScore();
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
        //Make it harder to take off, reduces motion sickness
        if (other.collider.tag == "Land")
        {
            gravMultiplier = highGrav;
        }
    }
    void OnCollisionExit(Collision other)
    {
        //Make it easier to fly, reduces motion sickness
        if (other.collider.tag == "Land")
        {
            gravMultiplier = lowGrav;
        }
    }

	// Update is called once per frame
	void Update ()
    {
        if (Manager.Instance.IsUpdatable())
        {
            //Apply gravity
            Rb.AddForce(Physics.gravity*gravMultiplier, ForceMode.Force);
        }
    }
}
