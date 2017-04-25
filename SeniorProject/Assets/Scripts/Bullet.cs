using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour
{
    private float Lifetime = 3;

    [SerializeField]   private float Life = 0;

    // Use this for initialization
    void Start ()
    {
        Lifetime = Life;
    }
	
    public void  Reset()
    {
        Lifetime = Life;
        this.gameObject.SetActive(true);
    }

	// Update is called once per frame
	void Update ()
    {
        if(Manager.Instance.IsUpdatable())
        {
            //if(gameObject.)
                {
                Lifetime -= Time.deltaTime;
                if (Lifetime < 0.0f)
                   this.gameObject.SetActive(false);
                 }
        }
    }
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            Debug.Log("Bullet hit an Enemy!");
            this.gameObject.SetActive(false);
        }
    }
}
