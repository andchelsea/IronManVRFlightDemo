using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PcPlayer : MonoBehaviour
{
    private Rigidbody Rb;

    private float Pitch = 0.0f;
    private float Yaw = 0.0f;
    [SerializeField] private float PitchSpeed = 2.0f;
    [SerializeField] private float YawSpeed = 2.0f;
    [SerializeField] private float Speed = 10.0f;
    [SerializeField] private float MaxPitchAngle = 120.0f;
    [SerializeField] private float MinPitchAngle = -120.0f;

    [SerializeField] private float MaxRayDist = 120.0f;

    [SerializeField] private GameObject pFlare1;//make a prefab
    [SerializeField] private GameObject pFlare2;//make a prefab
    [SerializeField] private float FlareCoolDown = 1.0f;
    private float Flare1Delay = 0.0f;
    private float Flare2Delay = 0.0f;
    // Use this for initialization
    void Start ()
    {
	}

    void Awake()
    {
        Rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked; //add crosshair
        Cursor.visible = false;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if(UiManager.Instance.IsUpdatable())
        {
            Flare1Delay += Time.deltaTime;
            Flare2Delay += Time.deltaTime;
            Vector3 newVel = new Vector3(0,0,0);

            //Movement Inputs
            Rb.velocity = new Vector3(0.0f,0.0f,0.0f);
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            //testing purposes
            newVel += transform.forward * Speed * vertical;
            newVel += transform.right * Speed * 0.75f * horizontal;
            Rb.AddForce(newVel);

            Yaw += YawSpeed * Input.GetAxis("Mouse X");

            Pitch += (PitchSpeed * Input.GetAxis("Mouse Y"));

            Pitch = Pitch <= MinPitchAngle ? MinPitchAngle : Pitch;

            Pitch = Pitch >= MaxPitchAngle ? MaxPitchAngle : Pitch;

            transform.localEulerAngles = new Vector3(Pitch, Yaw, 0.0f);

            //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit info = new RaycastHit();
            Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out info, MaxRayDist);

            if(info.collider.tag == "Enemy")
            {
               // info.collider.gameObject.GetComponent<Material>().shader = lit;
               if(Input.GetButtonDown("Flare1") && Flare1Delay > FlareCoolDown)
                {
                    Instantiate(pFlare1, info.transform.position, transform.rotation);
                    Flare1Delay = 0.0f;
                }
               else if(Input.GetButtonDown("Flare2") && Flare2Delay > FlareCoolDown)
                {
                    Instantiate(pFlare2, info.transform.position, this.transform.rotation);
                    Flare2Delay = 0.0f;
                }
            }
            else if(info.collider.tag == "Ammo")
            {
                if (Input.GetButtonDown("Flare1") && Flare1Delay > FlareCoolDown)
                {
                    Instantiate(pFlare1, info.transform.position, this.transform.rotation);
                    Flare1Delay = 0.0f;
                }
                else if (Input.GetButtonDown("Flare2") && Flare2Delay > FlareCoolDown)
                {
                    Instantiate(pFlare2, info.transform.position, this.transform.rotation);
                    Flare2Delay = 0.0f;
                }
            }
        }

      if( Input.GetButtonDown("Submit"))//either make into axis or getbutton down?
        {
            Debug.Log("FOUND");
            UiManager.Instance.TogglePause();
            //SceneManager.LoadScene("MainMenu",LoadSceneMode.Single);
        }
    }
}
