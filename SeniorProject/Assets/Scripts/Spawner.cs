using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour {

    public static Spawner Instance;

    [SerializeField] private float AmmoSpawnCoolDown = 0.5f;
    private float AmmoDelay = 0;

    [SerializeField] private float EnemySpawnCoolDown = 0.5f;
    private float EnemyDelay = 0.0f;
    [SerializeField] private float EnemyFlySpeed = 5.0f; //arbitrary starting numbers, playtest

    [SerializeField] private float minPos = 10.0f;
    [SerializeField] private float maxPos = 25.0f;

    [SerializeField] private BoxCollider PlayArea;

    // Use this for initialization
    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this.gameObject);
        Instance = this;

        PlayArea = GameObject.FindGameObjectWithTag("PlayArea").GetComponent<BoxCollider>();
	}

    Vector3 SpawnablePos()
    {
        Vector3 pos = new Vector3(0, 0, 0); 
        bool spawnable = false;

        while (!spawnable)
        {
            pos = VrPlayer.Instance.transform.position;
            pos += (Random.insideUnitSphere*Random.Range(minPos,maxPos));
            if(PlayArea.bounds.Contains(pos))
            {
                spawnable = true;
            }
        }

        return pos;
    }

	
	// Update is called once per frame
	void Update ()
    {
        if(Manager.Instance.IsUpdatable())
        {
            EnemyDelay += Time.deltaTime;
            AmmoDelay += Time.deltaTime;

            if (EnemyDelay < EnemySpawnCoolDown)
            {
                GameObject e = Manager.Instance.GetEnemy();

                if(e != null)
                {
                    EnemyDelay = 0;

                    e.transform.position = SpawnablePos();
                    //e.GetComponent<EnemyScript>().playerPosition = VrPlayer.Instance.transform.position;
                    e.GetComponent<Rigidbody>().AddForce(transform.forward * EnemyFlySpeed, ForceMode.Force);//needs to be tested!!! (different force modes)
                    //move the above to the enemy script???
                }
                else
                    Debug.Log("NOT ENOUGH ENEMIES");
            }

            if (AmmoDelay < AmmoSpawnCoolDown)
            {
                GameObject e = Manager.Instance.GetAmmoPack();
                if (e != null)
                {
                    AmmoDelay = 0;

                    e.transform.position = SpawnablePos();//this might wanna make an empty object infront of controller or with an offset
                }
                else
                    Debug.Log("NOT ENOUGH AMMO PACKS");
            }
        }
    }
}
