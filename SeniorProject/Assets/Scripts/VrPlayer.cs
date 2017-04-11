using UnityEngine;
using System.Collections;

public class VrPlayer : MonoBehaviour
{
    [SerializeField] private Rigidbody mRB;//probably head/torso, needed to apply velocity
    [SerializeField] private float mFlySpeed = 15; //arbitrary starting numbers, playtest
    [SerializeField] private float mMaxSpeed = 150; //arbitrary starting numbers, playtest
    [SerializeField] private float mProjectileSpeed = 15; //arbitrary starting numbers, playtest
    [SerializeField] private float mAttackCoolDown = 0.5f; 
    [SerializeField] private float mAttackDelay = 0;

    private SteamVR_TrackedController mController;//used for controller update
    private SteamVR_Controller.Device mDevice;//used for touchpad update. Needed?

    [SerializeField] private GameObject pProjectile;//make a prefab

    private int mHealth = 10, mAmmo = 10; //arbitrary starting numbers, playtest

    // Use this for initialization
    void Start ()
    {
	}
	
    void Awake()
    {
        mController = GetComponent<SteamVR_TrackedController>();
        mController.PadClicked += Attack;
        //mController.MenuButtonClicked //add Pause menu here
    }

    void Attack(object sender, ClickedEventArgs e)
    {
        if (mAmmo > 0 && mAttackDelay > mAttackCoolDown)
        {
            GameObject p = Instantiate(pProjectile, this.transform.position, this.transform.rotation) as GameObject; //this might wanna make an empty object infront of controller or with an offset
            Rigidbody PRB = p.GetComponent<Rigidbody>();
            PRB.AddForce(PRB.transform.forward * mProjectileSpeed, ForceMode.Impulse); //needs to be tested!!!
            mAttackDelay = 0;
            --mAmmo;
        }
    }

    void Fly()
    {
        if(mRB.velocity.magnitude < mMaxSpeed)
            mRB.AddForce(-mController.transform.forward*mFlySpeed);//needs drag force
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            Debug.Log("Enemy hit you!");
            --mHealth;
        }
    }

    // Update is called once per frame
    void Update()
    {
        mAttackDelay += Time.deltaTime;
        if(mController.triggerPressed)
        {
            Fly();
        }
    }
}
