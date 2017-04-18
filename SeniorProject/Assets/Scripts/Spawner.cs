using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour {


    [SerializeField] private GameObject pAmmo;//make a prefab
    [SerializeField] private float AmmoSpawnCoolDown = 0.5f;
    [SerializeField] private float AmmoDelay = 0;

    [SerializeField] private GameObject pEnemy;//make a prefab
    [SerializeField] private float EnemySpawnCoolDown = 0.5f;
    [SerializeField] private float EnemyDelay = 0.0f;
    [SerializeField] private float EnemyFlySpeed = 5.0f; //arbitrary starting numbers, playtest

    [SerializeField] private VrPlayer mPlayer;
    [SerializeField] private float minPos = 10.0f;
    [SerializeField] private float maxPos = 25.0f;

    [SerializeField] private BoxCollider PlayArea;

    // Use this for initialization
    void Awake ()
    {
        PlayArea = GameObject.FindGameObjectWithTag("PlayArea").GetComponent<BoxCollider>();
	}

    Vector3 SpawnablePos()
    {
        Vector3 pos = new Vector3(0, 0, 0); 
        bool spawnable = false;

        while (!spawnable)
        {
            pos = mPlayer.transform.position;
            pos += Random.insideUnitSphere*Random.Range(minPos,maxPos);
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
        if(UiManager.Instance.IsUpdatable())
        {
            EnemyDelay += Time.deltaTime;
            AmmoDelay += Time.deltaTime;

            if (EnemyDelay < EnemySpawnCoolDown)
            {
                EnemyDelay = 0;

                Vector3 enemyPos = SpawnablePos();
                GameObject p = Instantiate(pEnemy, enemyPos, this.transform.rotation) as GameObject; //this might wanna make an empty object infront of controller or with an offset

                EnemyScript e = p.GetComponent<EnemyScript>();
                e.playerPosition = mPlayer.transform.position;
                Rigidbody PRB = p.GetComponent<Rigidbody>();
                PRB.AddForce(PRB.transform.forward * EnemyFlySpeed, ForceMode.Force); //needs to be tested!!! (different force modes)
            }

            if (AmmoDelay < AmmoSpawnCoolDown)
            {
                AmmoDelay = 0;

                Vector3 ammoPos = SpawnablePos();
                Instantiate(pAmmo, ammoPos, transform.rotation);//this might wanna make an empty object infront of controller or with an offset
            }
        }
    }
}
