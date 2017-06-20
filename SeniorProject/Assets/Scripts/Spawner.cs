using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour {

    public static Spawner Instance;

    //How often the ammo packs spawn
    [SerializeField] private float AmmoSpawnCoolDown = 0.5f;
    private float AmmoDelay = 0;

    //How often the enemies spawn
    [SerializeField] private float EnemySpawnCoolDown = 0.5f;
    private float EnemyDelay = 0.0f;
    [SerializeField] private float EnemyFlySpeed = 5.0f; //arbitrary starting numbers, playtest

    //spawn area parameters
    [SerializeField] private float minPos = 10.0f;//min distance from VR player
    [SerializeField] private float maxPos = 25.0f;//max distance from VR player
    [SerializeField] private BoxCollider PlayArea;//game map area

    //Variables
    private Transform VRPlayer;

    //Basic Singleton
    private void Start()
    {
        if (Instance != null && Instance != this)
            Destroy(this.gameObject);
        Instance = this;

        PlayArea = GameObject.FindGameObjectWithTag("PlayArea").GetComponent<BoxCollider>();
        VRPlayer = VrPlayer.Instance.gameObject.transform;
    }

    //Find a suitable spawnable position for ammo or enemy
    Vector3 SpawnablePos()
    {
        Vector3 pos = new Vector3(0, 0, 0); 
        bool spawnable = false;

        while (!spawnable)
        {
            pos = VRPlayer.position;
            pos += (Random.insideUnitSphere*Random.Range(minPos,maxPos));
            if(PlayArea.bounds.Contains(pos))
            {
                spawnable = true;
            }
        }

        return pos;
    }

	void Update ()
    {
        if(Manager.Instance.IsUpdatable() && Manager.Instance.IsGameStarted())//Only update if VR player is alive, game is not paused, and the PC player has started the game
        {
            EnemyDelay += Time.deltaTime;//how often to spawn a enemy
            AmmoDelay += Time.deltaTime;//how often to spawn ammo, !!!!!!repalce with coroutines!!!!!!!!!!!

            if (EnemyDelay > EnemySpawnCoolDown)
            {
                GameObject e = Manager.Instance.GetEnemy();//find and spawn an enemy

                EnemyDelay = 0;
                e.GetComponent<EnemyScript>().Reset();
                e.transform.position = SpawnablePos();
                e.GetComponent<Rigidbody>().velocity = e.transform.forward * EnemyFlySpeed;//kept in spawner to make the fly speed centeralized
            }

            //make an int that keeps track of how many ammo packs are active to avoid a unnessesary for loop
            if (AmmoDelay > AmmoSpawnCoolDown)
            {
                GameObject e = Manager.Instance.GetAmmoPack();
                if (e != null)//Can return null
                {
                    AmmoDelay = 0;

                    e.transform.position = SpawnablePos();
                    e.SetActive(true);
                }
                else
                    Debug.Log("NOT ENOUGH AMMO PACKS");
            }
        }
    }
}
