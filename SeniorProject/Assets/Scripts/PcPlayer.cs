using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PcPlayer : MonoBehaviour
{
    public static PcPlayer Instance;
    private Rigidbody Rb;

    //Movement Variables
    private float Pitch = 0.0f;
    private float Yaw = 0.0f;
    [SerializeField] private float PitchSpeed = 2.0f;
    [SerializeField] private float YawSpeed = 2.0f;
    [SerializeField] private float Speed = 10.0f;
    [SerializeField] private float MaxPitchAngle = 120.0f;
    [SerializeField] private float MinPitchAngle = -120.0f;

    //Flare Variables
    [SerializeField] private float MaxRayDist = 120.0f;//How far the ray should check (for flare creation)
    [SerializeField] private GameObject pFlare1;//make a prefab
    [SerializeField] private GameObject pFlare2;//make a prefab
    [SerializeField] private float FlareCoolDown = 1.0f;
    private float Flare1Delay = 0.0f;
    private float Flare2Delay = 0.0f;

    //Basic Singleton
    private void Start ()
    {
        if (Instance != null && Instance != this)
            Destroy(this.gameObject);
        Instance = this;

        Rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked; //aiming crosshairs
        Cursor.visible = false;

        //Create Flare 1 & 2, Design Idea: have flare 3 & 4 as well
        pFlare1 = Instantiate(pFlare1, this.transform.position, this.transform.rotation) as GameObject;
        pFlare1.SetActive(false);

        pFlare2 = Instantiate(pFlare2);
        pFlare2.SetActive(false);
    }
	
	void Update ()
    {
        if(Manager.Instance.IsUpdatable())//Only update if VR player is alive or game is not paused
        {
            //Flare Lifetime
            Flare1Delay += Time.deltaTime;
            Flare2Delay += Time.deltaTime;

            //Movement Inputs
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");
            Vector3 newVel = new Vector3(0,0,0);
            newVel += transform.forward * Speed * vertical;
            newVel += transform.right * Speed * 0.75f * horizontal;
            Rb.velocity = newVel;

            //Rotation Inputs
            Yaw += YawSpeed * Input.GetAxis("Mouse X");
            Pitch += (PitchSpeed * Input.GetAxis("Mouse Y"));

            //Cap pitch to make sure you dont flip upside-down 
            Pitch = Pitch <= MinPitchAngle ? MinPitchAngle : Pitch;
            Pitch = Pitch >= MaxPitchAngle ? MaxPitchAngle : Pitch;
            transform.localEulerAngles = new Vector3(Pitch, Yaw, 0.0f);

            //Shoot out raycast for flares
            RaycastHit info = new RaycastHit();
            //Debug.Log(info.collider.tag.ToString());

            if(Physics.Raycast(Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f)), out info, MaxRayDist))//cast form the middle of the screen
            {
                    if(info.collider.tag == "Enemy" || info.collider.tag == "Ammo")//if ray returns a valid target
                    {
                       if(Input.GetButtonDown("Flare1") && Flare1Delay > FlareCoolDown)//!!!!!!!refactor so that raycast only happens when the flare button is pressed!!!!!!!!
                        {
                            pFlare1.transform.position = info.point;
                            pFlare1.GetComponent<FlareScript>().Reset();//either make a flare script to auto deactivate over time or remove
                            Flare1Delay = 0.0f;
                        }
                       else if(Input.GetButtonDown("Flare2") && Flare2Delay > FlareCoolDown)
                        {
                            pFlare2.transform.position = info.point;
                            pFlare2.GetComponent<FlareScript>().Reset();
                            Flare2Delay = 0.0f;
                        }
                    }
            }
      }
    
        //Allows PC Player to pause game
        if( Input.GetButtonDown("Submit"))
        {
            Manager.Instance.TogglePause();
            //SceneManager.LoadScene("MainMenu",LoadSceneMode.Single);//Main Menu level unused
        }
        else if (Input.GetKeyDown(KeyCode.P))
        {
            Manager.Instance.StartGame();
        }
    }
}
