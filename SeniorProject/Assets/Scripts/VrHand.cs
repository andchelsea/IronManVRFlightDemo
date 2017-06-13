using UnityEngine;
using System.Collections;

public class VrHand : MonoBehaviour
{
    //Hand Variables
    private SteamVR_TrackedController Controller;//used for controller update
    private float AngleOffset;
    private Rigidbody Rb;

    //Flying Variables
    private float FlySpeed;
    private float MaxSpeed;
    ParticleSystem PS;

    //Shooting Variables
    AudioSource fireSound;
    private float ProjectileSpeed;
    private float AttackDelay;

    private void Start()
    {
        //Controller set up
        Controller = GetComponent<SteamVR_TrackedController>();
        Controller.PadClicked += Attack;//Call attack when clicked
        Controller.MenuButtonClicked += Pause; //Call pause menu when clicked

        //Find components
        ProjectileSpeed = VrPlayer.Instance.ProjectileSpeed;
        FlySpeed = VrPlayer.Instance.FlySpeed;
        MaxSpeed = VrPlayer.Instance.MaxSpeed;
        Rb = VrPlayer.Instance.Rb;
        fireSound = GetComponent<AudioSource>();
        AngleOffset = VrPlayer.Instance.AngleOffset;
        PS = GetComponent<ParticleSystem>();
    }
    
    void Attack(object sender, ClickedEventArgs e)
    {
        if (VrPlayer.Instance.Shootable(AttackDelay))//Can a bullet be fired?
        {
            AttackDelay = 0;
            GameObject g = Manager.Instance.GetBullet(); //small possiblility to get overrite a bullet currently in use
 
            g.GetComponent<Bullet>().Reset();
            g.transform.position = this.transform.position; // might replace this. with an empty object infront of controller or add an offset
            g.transform.rotation = this.transform.rotation;//   new Quaternion(this.transform.rotation.x, this.transform.rotation.y, this.transform.rotation.z, this.transform.rotation.w);

            //add offset from rotation to launch bullet out to the palm iron man style
            Vector3 dir = Quaternion.AngleAxis(AngleOffset, this.transform.forward) * transform.right;
            g.GetComponent<Rigidbody>().AddForce(dir * ProjectileSpeed, ForceMode.Impulse);

            fireSound.Play();
        }
    }

    void Pause(object sender, ClickedEventArgs e)
    {
        Manager.Instance.TogglePause();//Pauses for VR and PC Player
    }

    void Fly()
    {
        //Can add amount of flight equal to amount pressed on trigger, but was found to be too jittery
        //float triggerpress = SteamVR_Controller.Input((int)Controller.controllerIndex).hairTriggerDelta; 

        if (Rb.velocity.magnitude < MaxSpeed)//negleting to have a max speed can make the player fly though the bounding box
        {
            Vector3 dir = Quaternion.AngleAxis(AngleOffset, Controller.transform.forward) * transform.right;//Fly out of the palm
            //needs drag force
            Rb.AddForce(-dir * FlySpeed);// * triggerpress);
        }

        SteamVR_Controller.Input((int)Controller.controllerIndex).TriggerHapticPulse();
        PS.Emit(1);
    }

    void Update()
    {
        if(Manager.Instance.IsUpdatable())
        {
            AttackDelay += Time.deltaTime;//Time passed between shooting
            if (Controller.triggerPressed)
            {
                Fly();
            }
        }
    }
}
