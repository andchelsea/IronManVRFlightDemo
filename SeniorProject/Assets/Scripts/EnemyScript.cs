using UnityEngine;
using System.Collections;

public class EnemyScript : MonoBehaviour
{
    private float Lifetime = 3;//needed?
    private float ChildLife = 0;
    [SerializeField]
    private GameObject child;
    [SerializeField] private float hologramLife = 2;
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
        child = this.gameObject.transform.GetChild(0).gameObject;//enemy structure dependednt
        child.SetActive(false);
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
            if(child.activeSelf)
            {
                ChildLife -= Time.deltaTime;
                if (ChildLife < 0.0f)
                {
                   child.SetActive(false);//capture the child in awake
                }
            }


            Lifetime -= Time.deltaTime;
            if (Lifetime < 0.0f)
            {
                child.SetActive(false);
                this.gameObject.SetActive(false);
            }

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
            child.SetActive(false);
            this.gameObject.SetActive(false);

            if (--VrPlayer.Instance.Health <= 0)
            {
                Manager.Instance.SetUpdatable(false);
            }
        }
        else if(other.gameObject.tag == "Bullet")
        {
            child.SetActive(false);
            other.gameObject.SetActive(false);
            this.gameObject.SetActive(false);
        }
        else if(other.gameObject.tag == "Flare")
        {
            child.SetActive(true);
            ChildLife = hologramLife;
            Debug.Log("Enemy hit a Flare!");
        }
    }
}
