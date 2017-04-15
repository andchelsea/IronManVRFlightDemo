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
        Vector3 newVel = new Vector3(0,0,0);

        //Movement Inputs
        Rb.velocity = new Vector3(0.0f,0.0f,0.0f);
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // rb.AddForce(transform.forward * speed* Vertical);
        // rb.AddForce(transform.right * speed * 0.75f * Horizontal);
        //testing purposes
        newVel += transform.forward * Speed * vertical;
        newVel += transform.right * Speed * 0.75f * horizontal;
        Rb.AddForce(newVel);

        Yaw += YawSpeed * Input.GetAxis("Mouse X");

        Pitch += (PitchSpeed * Input.GetAxis("Mouse Y"));

        Pitch = Pitch <= MinPitchAngle ? MinPitchAngle : Pitch;

        Pitch = Pitch >= MaxPitchAngle ? MaxPitchAngle : Pitch;

        transform.localEulerAngles = new Vector3(Pitch, Yaw, 0.0f);

      if( Input.GetButton("Submit"))//either make into axis or getbutton down?
        {
            Debug.Log("FOUND");
            //SceneManager.LoadScene("PauseMenu",LoadSceneMode.Additive);
        }
    }
}
