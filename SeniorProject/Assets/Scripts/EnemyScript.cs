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
    void Awake ()
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
        rb.velocity = Vector3.zero;
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

    void OnMouseExit()
    {
        //GetComponent<Material>().shader = unlit;
    }

    void OnMouseEnter()
    {
        //GetComponent<Material>().shader = lit;//testing required
    }

    void OnParticleCollision(GameObject other)
    {
        Debug.Log("Particle Collision!");
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            this.gameObject.SetActive(false);
     

            if (--VrPlayer.Instance.Health <= 0)
            {
                Manager.Instance.SetUpdatable(false);
            }
        }
    
        else if(other.gameObject.tag == "Bullet")
        {
            this.gameObject.SetActive(false);
            other.gameObject.SetActive(false);
        }
    }
     
    void OnFlareDetection()
    {
        //this.GetComponentInChildren
    }

}
