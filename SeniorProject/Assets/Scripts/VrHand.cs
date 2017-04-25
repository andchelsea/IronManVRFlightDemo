using UnityEngine;
using System.Collections;

public class VrHand : MonoBehaviour
{
    private SteamVR_TrackedController Controller;//used for controller update
    private float ProjectileSpeed;
    private float FlySpeed;
    private float MaxSpeed;
    private Rigidbody Rb;

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

    }

    void Attack(object sender, ClickedEventArgs e)
    {

        if (VrPlayer.Instance.Shootable())
        {
            GameObject g = Manager.Instance.GetBullet();
            if(g != null)
            {
                g.GetComponent<Bullet>().Reset();
                g.transform.position = this.transform.position; //this might wanna make an empty object infront of controller or with an offset

                //Gives bullets funky rotations, FIX???
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



    // Update is called once per frame
    void Update()
    {
        if(Manager.Instance.IsUpdatable())
        {
            if(Controller.triggerPressed)
            {
                Fly();
            }
        }
    }
}
