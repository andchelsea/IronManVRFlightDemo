using UnityEngine;
using System.Collections;

public class EnemyScript : MonoBehaviour
{
    [SerializeField] private float Lifetime = 3;

    // Use this for initialization
    void Start ()
    {
	}
	
	// Update is called once per frame
	void Update ()
    {
        Lifetime -= Time.deltaTime;
        if (Lifetime < 0.0f)
            Destroy(this.gameObject);
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
