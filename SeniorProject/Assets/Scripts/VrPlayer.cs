using UnityEngine;
using System.Collections;

public class VrPlayer : MonoBehaviour {

    public static VrPlayer Instance;

    [SerializeField] public Rigidbody Rb;//probably head/torso, needed to apply velocity

    [Space(10)]
    [SerializeField] public float FlySpeed = 15; //arbitrary starting numbers, playtest
    [SerializeField] public float MaxSpeed = 150; //arbitrary starting numbers, playtest
    [SerializeField] public float ProjectileSpeed = 15; //arbitrary starting numbers, playtest
    [SerializeField] public float AngleOffset = 90;

    [Space(10)]
    [SerializeField] public float lowGrav = 0.5f;
    [SerializeField] public float highGrav= 1.0f;
    private float gravMultiplier= 1.0f;

    [Space(10)]
    [SerializeField] public int Ammo = 10; //arbitrary starting numbers, playtest
    [SerializeField] private float AttackCoolDown = 0.5f; //arbitrary starting numbers, playtest
    [SerializeField] public int Health = 10; //arbitrary starting numbers, playtest

    private int startHealth;
    private int startAmmo;
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
        startHealth = Health;
        startAmmo = Ammo;
    }

   public void Reset()
    {
        this.transform.position.Set(0, 0.6f, -5);
        Health = startHealth;
        Ammo = startAmmo;
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
        if (other.collider.tag == "Land")
        {
            gravMultiplier = highGrav;
        }
        //may need to be on the hands (and body?) for ammo
    }
    void OnCollisionExit(Collision other)
    {
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
            Rb.AddForce(Physics.gravity*gravMultiplier, ForceMode.Force);
        }
    }
}
