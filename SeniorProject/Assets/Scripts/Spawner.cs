using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour {

    public static Spawner Instance;

    //How often the ammo packs spawn
    [SerializeField] private float AmmoSpawnCoolDown = 0.5f;
    private IEnumerator AmmoSpawning = null;//controlls spawning ammo

    //How often the enemies spawn
    [SerializeField] private float EnemySpawnCoolDown = 0.5f;
    [SerializeField] private float EnemyFlySpeed = 5.0f; //arbitrary starting numbers, playtest
    private IEnumerator EnemySpawning = null;//controls spawning enemies

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

        EnemySpawning = SpawnEnemies();
        AmmoSpawning = SpawnAmmo();
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

    //replaces update
    IEnumerator SpawnEnemies()
    {
        GameObject enemy = Manager.Instance.GetEnemy();//find and spawn an enemy

        enemy.GetComponent<EnemyScript>().Reset();
        enemy.transform.position = SpawnablePos();
        enemy.GetComponent<Rigidbody>().velocity = enemy.transform.forward * EnemyFlySpeed;//kept in spawner to make the fly speed centeralized
        yield return new WaitForSeconds(EnemySpawnCoolDown);
    }

    //replaces update
    IEnumerator SpawnAmmo()
    {
        GameObject ammo = Manager.Instance.GetAmmoPack();
        if (ammo != null)//Can return null
        {
            ammo.transform.position = SpawnablePos();
            ammo.SetActive(true);
        }

        yield return new WaitForSeconds(AmmoSpawnCoolDown);
    }

    //replaces update
    public void SetSpawn(bool spawn)
    {
        if(spawn)
        {
            StartCoroutine(AmmoSpawning);
            StartCoroutine(EnemySpawning);
        }
        else
        {
            StopCoroutine(EnemySpawning);
            StopCoroutine(AmmoSpawning);
        }
    }
}
