using UnityEngine;
using System.Collections;

public class PcPlayer : MonoBehaviour
{
    [SerializeField]
    private float speed = 10.0f;

    private Rigidbody rb;

    private float pitch = 0.0f;
    private float yaw = 0.0f;
    [SerializeField]
    private float pitchSpeed = 2.0f;
    [SerializeField]
    private float yawSpeed = 2.0f;


    // Use this for initialization
    void Start ()
    {
        rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        Vector3 newVel = new Vector3(0,0,0);

        //Movement Inputs
        rb.velocity = new Vector3(0.0f,0.0f,0.0f);
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // rb.AddForce(transform.forward * speed* Vertical);
        // rb.AddForce(transform.right * speed * 0.75f * Horizontal);
        //testing purposes
        newVel += transform.forward * speed * vertical;
        newVel += transform.right * speed * 0.75f * horizontal;
        rb.AddForce(newVel);

        yaw += yawSpeed * Input.GetAxis("Mouse X");
        pitch += pitchSpeed * Input.GetAxis("Mouse Y");
        transform.eulerAngles = new Vector3(pitch, yaw, 0.0f);
        //need to lock mouse to center of the screen (add some kind of targeting UI)
    }
}
