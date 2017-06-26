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
    [Space(10)]
    [SerializeField] private float MaxRayDist = 120.0f;//How far the ray should check (for flare creation)
    [SerializeField] private GameObject pFlare1;
    [SerializeField] private GameObject pFlare2;
    [SerializeField] private float FlareCoolDown = 1.0f;
    private float Flare1Delay = 0.0f;
    private float Flare2Delay = 0.0f;

    private void Start ()
    {
        //Basic Singleton
        if (Instance != null && Instance != this)
            Destroy(this.gameObject);
        Instance = this;

        //Set Variables
        Rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        //Create Flare 1 & 2, Design Idea: have flare 3 & 4 as well
        pFlare1 = Instantiate(pFlare1, this.transform.position, this.transform.rotation) as GameObject;
        pFlare1.SetActive(false);

        pFlare2 = Instantiate(pFlare2);
        pFlare2.SetActive(false);
    }
	
	void Update ()
    {
        //Only update if VR player is alive or game is not paused
        if(Manager.Instance.IsUpdatable())
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

            if (Input.GetButtonDown("Flare1") && Flare1Delay > FlareCoolDown)
            {
                Castflare(pFlare1);
                Flare1Delay = 0.0f;
            }
            else if(Input.GetButtonDown("Flare2") && Flare2Delay > FlareCoolDown)
            {
                Castflare(pFlare2);
                Flare2Delay = 0.0f;
            }
      }//end of updatable check
    
        //Allows PC Player to pause game
        if( Input.GetButtonDown("Submit"))
        {
            Manager.Instance.TogglePause();
            //SceneManager.LoadScene("MainMenu",LoadSceneMode.Single);//Main Menu level unused
        }
        else if (Input.GetKeyDown(KeyCode.P))
        {
            //Manager.Instance.StartGame();
            Spawner.Instance.SetSpawn(true);
        }
    }

    void Castflare(GameObject flare)
    {
        //Shoot out raycast for flares
        RaycastHit info = new RaycastHit();

        //cast form the middle of the screen
        if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f)), out info, MaxRayDist))
        {
            //if ray returns a valid target
            if (info.collider.tag == "Enemy" || info.collider.tag == "Ammo")
            {
                flare.transform.position = info.point;
                flare.GetComponent<FlareScript>().Reset();
            }

            //Debug.Log(info.collider.tag.ToString());//For testing
        }
    }
}//end of PC Class
