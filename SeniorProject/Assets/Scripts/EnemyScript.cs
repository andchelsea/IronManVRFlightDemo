using UnityEngine;
using System.Collections;

public class EnemyScript : MonoBehaviour
{
    private float Lifetime = 3;//needed?
    [SerializeField] private float life = 0;
    [SerializeField] private Shader unlit;//make a shader
    [SerializeField] private Shader lit;//make a shader
    Transform PlayerPos;
    Rigidbody rb;
    // Use this for initialization
    void Start ()
    {
       // GetComponent<Material>().shader = unlit;
        Lifetime = life;
        PlayerPos = VrPlayer.Instance.gameObject.transform;
        rb = GetComponent<Rigidbody>();
    }

	public void Reset()
    {
        Lifetime = life;
        this.gameObject.SetActive(true);
    }

	// Update is called once per frame
	void FixedUpdate()
    {
        if (Manager.Instance.IsUpdatable())
        { 
            Lifetime -= Time.deltaTime;
            if (Lifetime < 0.0f)
                this.gameObject.SetActive(false);//will this work??

            transform.LookAt(PlayerPos);
            rb.velocity = rb.velocity.magnitude * transform.forward;
            
        }
    }

    void OnMouseEnter()
    {
        //GetComponent<Material>().shader = lit;//testing required
    }

    void OnMouseExit()
    {
        //GetComponent<Material>().shader = unlit;
    }


    void OnCollisionEnter(Collision other)
    {
        if (other.collider.tag == "Player")
        {
            Debug.Log("Enemy hit VR Player!");
            //Destroy(this.gameObject);
            this.gameObject.SetActive(false);//will this work??
        }
        else if (other.collider.tag == "Bullet")
        {
            Debug.Log("Enemy hit a Bullet!");
            this.gameObject.SetActive(false);//will this work??
        }
    }

    
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("Enemy hit VR Player!");
            //Destroy(this.gameObject);
            this.gameObject.SetActive(false);//will this work??
        }
        else if(other.gameObject.tag == "Bullet")
        {
            Debug.Log("Enemy hit a Bullet!");
            this.gameObject.SetActive(false);//will this work??
        }
    }
     

}
