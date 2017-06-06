using UnityEngine;
using System.Collections;

public class EnemyScript : MonoBehaviour
{
    private float Lifetime = 3;//needed?
    private float ChildLife = 0;
    private GameObject child;
    [SerializeField] private float hologramLife = 2;
    [SerializeField] private float life = 0;
    [SerializeField] private Shader unlit;//make a shader
    private Transform PlayerPos;
    private Rigidbody rb;
    private AudioSource DeathSound;
    private ParticleSystem PS;
    private ParticleSystem ChildPS;


    //
    private Renderer pRender;
    private Renderer cRender;
    private SphereCollider sCollider;
    
    // Use this for initialization
    void Awake ()
    {
       // GetComponent<Material>().shader = unlit;
        Lifetime = life;
        PlayerPos = VrPlayer.Instance.gameObject.transform;
        rb = GetComponent<Rigidbody>();
        DeathSound = GetComponent<AudioSource>();

        child = this.gameObject.transform.GetChild(0).gameObject;//enemy structure dependednt
        PS = GetComponent<ParticleSystem>();
        ChildPS = child.GetComponent<ParticleSystem>();

        pRender = this.gameObject.GetComponent<Renderer>();
        cRender = child.gameObject.GetComponent<Renderer>();
        cRender.enabled= false;
        sCollider = this.gameObject.GetComponent<SphereCollider>();
    }

	public void Reset()
    {
        Lifetime = life;
        this.gameObject.SetActive(true);
        rb.velocity = Vector3.zero;

        pRender.enabled = true;
        cRender.enabled = false;
        sCollider.enabled = true;
    }

    public void Die(bool explode = true)
    {
        cRender.enabled = false;
        pRender.enabled = false;
        sCollider.enabled = false;
        rb.velocity = Vector3.zero;
        if(explode)
        {
            PS.Play();
            ChildPS.Play();
            DeathSound.Play();
        }
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
                  cRender.enabled=false;//capture the child in awake
                }
            }


            Lifetime -= Time.deltaTime;
            if (Lifetime < 0.0f)
            {
                Die(false);
            }

            transform.LookAt(PlayerPos);
            rb.velocity = rb.velocity.magnitude * transform.forward;
        }
    }

    void OnMouseExit()//doesnt work
    {
        GetComponent<Material>().shader = unlit;
        Debug.Log("So so so wrong");
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Die();
            HealthManager.Instance.Damaged();
            if (--VrPlayer.Instance.Health <= 0)
            {
                Manager.Instance.SetUpdatable(false);
            }
        }
        else if(other.gameObject.tag == "Bullet")
        {
            Die();
        }
        else if(other.gameObject.tag == "Flare")
        {
            cRender.enabled=true;
            ChildLife = hologramLife;
            Debug.Log("Enemy hit a Flare!");
        }
    }
}
