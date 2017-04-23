using UnityEngine;
using System.Collections;

public class EnemyScript : MonoBehaviour
{
    [SerializeField] private float Lifetime = 3;//needed?

    [SerializeField] private Shader unlit;//make a shader
    [SerializeField] private Shader lit;//make a shader

    // Use this for initialization
    void Start ()
    {
        GetComponent<Material>().shader = unlit;
    }
	
	// Update is called once per frame
	void Update()
    {
        if (Manager.Instance.IsUpdatable())
        { 
            Lifetime -= Time.deltaTime;
            if (Lifetime < 0.0f)
                this.gameObject.SetActive(false);//will this work??

            transform.LookAt(VrPlayer.Instance.transform.position);
        }
    }

    void OnMouseEnter()
    {
        GetComponent<Material>().shader = lit;//testing required
    }

    void OnMouseExit()
    {
        GetComponent<Material>().shader = unlit;
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "MainCamera")
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
