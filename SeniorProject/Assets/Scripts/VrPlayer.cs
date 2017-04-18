using UnityEngine;
using System.Collections;

public class VrPlayer : MonoBehaviour
{
    [SerializeField] private Rigidbody Rb;//probably head/torso, needed to apply velocity
    [SerializeField] private float FlySpeed = 15; //arbitrary starting numbers, playtest
    [SerializeField] private float MaxSpeed = 150; //arbitrary starting numbers, playtest
    [SerializeField] private float ProjectileSpeed = 15; //arbitrary starting numbers, playtest
    [SerializeField] private float AttackCoolDown = 0.5f; 
    private float AttackDelay = 0;

    private SteamVR_TrackedController Controller;//used for controller update
    //private SteamVR_Controller.Device Device;//used for touchpad update. Needed?

    [SerializeField] private GameObject pProjectile;//make a prefab

    private int mHealth = 10, mAmmo = 10; //arbitrary starting numbers, playtest


    public int GetHealth() { return mHealth; }

    // Use this for initialization
    void Start ()
    {
	}
	
    void Awake()
    {
        Controller = GetComponent<SteamVR_TrackedController>();
        Controller.PadClicked += Attack;
        Controller.MenuButtonClicked += Pause; //add Pause menu here
    }

    void Attack(object sender, ClickedEventArgs e)
    {
        if (mAmmo > 0 && AttackDelay > AttackCoolDown)
        {
            AttackDelay = 0;
            --mAmmo;

            GameObject p = Instantiate(pProjectile, this.transform.position, this.transform.rotation) as GameObject; //this might wanna make an empty object infront of controller or with an offset
            Rigidbody PRB = p.GetComponent<Rigidbody>();
            PRB.AddForce(PRB.transform.forward * ProjectileSpeed, ForceMode.Impulse); //needs to be tested!!!
        }
    }

    void Pause(object sender, ClickedEventArgs e)
    {
        Debug.Log("FOUND");
        UiManager.Instance.TogglePause();//<--VR player pause seperate??
    }

    void Fly()
    {
        if(Rb.velocity.magnitude < MaxSpeed)
            Rb.AddForce(-Controller.transform.forward*FlySpeed);//needs drag force
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            Debug.Log("Enemy hit you!");
            --mHealth;

            if (mHealth <= 0)
            {
                UiManager.Instance.SetUpdatable(false);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(UiManager.Instance.IsUpdatable())
        {
            AttackDelay += Time.deltaTime;

            if(Controller.triggerPressed)
            {
                Fly();
            }
        }
    }
}
