using UnityEngine;
using System.Collections;

public class VrHand : MonoBehaviour
{
    private SteamVR_TrackedController Controller;//used for controller update
    private float ProjectileSpeed;
    private float FlySpeed;
    private float MaxSpeed;
    private Rigidbody Rb;
    private float AttackDelay;
     AudioSource fireSound;
    //could add low hum for flight sound?

    ParticleSystem PS;

    // Use this for initialization
    private void Start()
    {
        Controller = GetComponent<SteamVR_TrackedController>();
        Controller.PadClicked += Attack;
        Controller.MenuButtonClicked += Pause; //add Pause menu here
        ProjectileSpeed = VrPlayer.Instance.ProjectileSpeed;
        FlySpeed = VrPlayer.Instance.FlySpeed;
        MaxSpeed = VrPlayer.Instance.MaxSpeed;
        Rb = VrPlayer.Instance.Rb;
        fireSound = GetComponent<AudioSource>();
        PS = GetComponent<ParticleSystem>();
    }
    
    void Attack(object sender, ClickedEventArgs e)
    {
        if (VrPlayer.Instance.Shootable(AttackDelay))
        {
            AttackDelay = 0;
            GameObject g = Manager.Instance.GetBullet();//small possiblility to get rid of ammo without spawning
            if(g != null)
            {
                g.GetComponent<Bullet>().Reset();
                g.transform.position = this.transform.position; //this might wanna make an empty object infront of controller or with an offset

                //Gives bullets funky rotations, FIX???
                //g.transform.rotation = new Quaternion(this.transform.rotation.x, this.transform.rotation.y, this.transform.rotation.z, this.transform.rotation.w);
                //Vector3 v = 
                g.transform.rotation = this.transform.rotation;//   new Quaternion(this.transform.rotation.x, this.transform.rotation.y, this.transform.rotation.z, this.transform.rotation.w);

                g.GetComponent<Rigidbody>().AddForce(new Vector3(this.transform.forward.x, this.transform.forward.y, this.transform.forward.z) * ProjectileSpeed, ForceMode.Impulse);//needs to be tested!!!
                fireSound.Play();
            }
            else
                Debug.Log("NOT ENOUGH BULLETS");
        }
    }

    void Pause(object sender, ClickedEventArgs e)
    {
        Manager.Instance.TogglePause();
    }

    void Fly()
    {
        //float triggerpress = SteamVR_Controller.Input((int)Controller.controllerIndex).hairTriggerDelta;
        if (Rb.velocity.magnitude < MaxSpeed)
            Rb.AddForce(-Controller.transform.forward * FlySpeed);// * triggerpress);//needs drag force
        SteamVR_Controller.Input((int)Controller.controllerIndex).TriggerHapticPulse();
        PS.Play();
    }



    // Update is called once per frame
    void Update()
    {
        if(Manager.Instance.IsUpdatable())
        {
            AttackDelay += Time.deltaTime;
            if (Controller.triggerPressed)
            {
                Fly();
            }
        }
    }
}
