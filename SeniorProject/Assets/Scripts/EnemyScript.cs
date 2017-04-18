using UnityEngine;
using System.Collections;

public class EnemyScript : MonoBehaviour
{
    [SerializeField] private float Lifetime = 3;//needed?

    public Vector3 playerPosition;

    [SerializeField] private Shader unlit;//make a shader
    [SerializeField] private Shader lit;//make a shader

    // Use this for initialization
    void Awake ()
    {
        GetComponent<Material>().shader = unlit;
    }
	
	// Update is called once per frame
	void Update()
    {
        Lifetime -= Time.deltaTime;
        if (Lifetime < 0.0f)
            Destroy(this.gameObject);

        transform.LookAt(playerPosition);            

    }

    void OnMouseEnter()
    {
        GetComponent<Material>().shader = lit;
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
            Destroy(this.gameObject);
        }
        else if(other.gameObject.tag == "Bullet")
        {
            Debug.Log("Enemy hit a Bullet!");
            Destroy(this.gameObject);
        }
    }

}
