using UnityEngine;
using System.Collections;

public class EnemyScript : MonoBehaviour
{
    //Keeps track of enemies alive
    private float Lifetime = 3;//Should the enemies expire over time?
    [SerializeField] private float life = 0;

    //Enemy Object holds a copy of itself with a different 'hologram' shader 
    //so the VR player can see the enemy if shot with a flare
    private float ChildLife = 0;
    private GameObject child;
    [SerializeField] private float hologramLife = 2;
    private ParticleSystem ChildPS;//plays an explosion on VR layer
    private Renderer cRender;

    //Unused, trying to get the enemy to highlight when the PC player mouses over it
    [SerializeField] private Shader unlit;//make a shader
    [SerializeField] private Shader lit;//make a shader

    //Enemy Variables
    private Transform PlayerPos;
    private Rigidbody rb;
    private AudioSource DeathSound;
    private ParticleSystem PS;//plays an explostion on PC layer
    private Renderer pRender;
    private SphereCollider sCollider;
    
    //Set Variables
    void Awake ()
    {
       //GetComponent<Material>().shader = unlit;
        Lifetime = life;
        PlayerPos = VrPlayer.Instance.gameObject.transform;
        rb = GetComponent<Rigidbody>();
        DeathSound = GetComponent<AudioSource>();

        child = this.gameObject.transform.GetChild(0).gameObject;//Not the best, since it's enemy structure dependent
        PS = GetComponent<ParticleSystem>();
        ChildPS = child.GetComponent<ParticleSystem>();

        pRender = this.gameObject.GetComponent<Renderer>();
        cRender = child.gameObject.GetComponent<Renderer>();
        cRender.enabled= false;
        sCollider = this.gameObject.GetComponent<SphereCollider>();
    }

    //Called everytime a enemy is spawned
	public void Reset()
    {
        Lifetime = life;
        this.gameObject.SetActive(true);
        rb.velocity = Vector3.zero;

        pRender.enabled = true;
        cRender.enabled = false;
        sCollider.enabled = true;
    }

    //Called to turn off an enemy (where explode is whether to play an explosion)
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

	void FixedUpdate()
    {
        if (Manager.Instance.IsUpdatable())//Only update if VR player is alive or game is not paused
        { 
            if(child.activeSelf)//If the enemy has been hit by a flare
            {
                ChildLife -= Time.deltaTime;
                if (ChildLife < 0.0f)//If hologram expires turn it off
                {
                  cRender.enabled=false;
                }
            }

            Lifetime -= Time.deltaTime;
            if (Lifetime < 0.0f)//if enemy expires silently turn it off
            {
                Die(false);
            }

            //Makes enemy a seeking missle type
            transform.LookAt(PlayerPos);
            rb.velocity = rb.velocity.magnitude * transform.forward;
        }
    }

    //Unused, for PC player to highlight Enemy on mouse over 
    void OnMouseExit()
    {
        //GetComponent<Material>().shader = unlit;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")//If enemy collides with player, hurt player
        {
            Die();
            HealthManager.Instance.Damaged();
            if (--VrPlayer.Instance.Health <= 0)
            {
                Manager.Instance.SetUpdatable(false);
            }
        }
        else if(other.gameObject.tag == "Bullet")//If enemy collides with a bullet, kill self
        {
            Die();
            VrPlayer.Instance.AddScore(100);
        }
        else if(other.gameObject.tag == "Flare")//If enemy collides with a flare, show hologram
        {
            cRender.enabled=true;
            ChildLife = hologramLife;
            //Debug.Log("Enemy hit a Flare!");
        }
    }
}
