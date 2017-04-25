using UnityEngine;
using System.Collections;

public class VrPlayer : MonoBehaviour
{
    public static VrPlayer Instance;

    [SerializeField] private Rigidbody Rb;//probably head/torso, needed to apply velocity
    [SerializeField] private float FlySpeed = 15; //arbitrary starting numbers, playtest
    [SerializeField] private float MaxSpeed = 150; //arbitrary starting numbers, playtest
    [SerializeField] private float ProjectileSpeed = 15; //arbitrary starting numbers, playtest
    [SerializeField] private float AttackCoolDown = 0.5f; 
    private float AttackDelay = 0;

    private SteamVR_TrackedController Controller;//used for controller update
    //private SteamVR_Controller.Device Device;//used for touchpad update. Needed?

    //private GameObject pProjectile;//make a prefab

    [SerializeField] private int mHealth = 10;
    [SerializeField] private int mAmmo = 10; //arbitrary starting numbers, playtest
    [SerializeField] private int NumAmmo = 10; //arbitrary starting numbers, playtest


    public int GetHealth() { return mHealth; }

    // Use this for initialization
    private void Awake()
    {
  

        Controller = GetComponent<SteamVR_TrackedController>();
        Controller.PadClicked += Attack;
        Controller.MenuButtonClicked += Pause; //add Pause menu here
    }

    void Attack(object sender, ClickedEventArgs e)
    {

        if (mAmmo > 0 && AttackDelay > AttackCoolDown)
        {
            GameObject g = Manager.Instance.GetBullet();
            if(g != null)
            {
                AttackDelay = 0;
                --mAmmo;

                g.GetComponent<Bullet>().Reset();
                g.transform.position = this.transform.position; //this might wanna make an empty object infront of controller or with an offset
                g.transform.rotation = this.transform.rotation;
                g.GetComponent<Rigidbody>().AddForce(transform.forward * ProjectileSpeed, ForceMode.Impulse);//needs to be tested!!!
            }
            else
                Debug.Log("NOT ENOUGH BULLETS");
        }
    }

    void Pause(object sender, ClickedEventArgs e)
    {
        Debug.Log("FOUND");
        Manager.Instance.TogglePause();
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
                Manager.Instance.SetUpdatable(false);
            }
        }
        if(other.gameObject.tag == "Ammo")
        {
            Debug.Log("VR picked up ammo");
            mAmmo += NumAmmo;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Manager.Instance.IsUpdatable())
        {
            AttackDelay += Time.deltaTime;

            if(Controller.triggerPressed)
            {
                Fly();
            }
        }
    }
}
