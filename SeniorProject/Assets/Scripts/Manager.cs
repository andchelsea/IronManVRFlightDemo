using UnityEngine;
using System.Collections;

public class Manager : MonoBehaviour {

    public static Manager Instance = null;
    private bool GameStarted = false;

    //Pause menu management
    [SerializeField] Canvas VRPause = null;
    private bool paused = false;
    private bool updatable = true;

    //Ammo spawned from a prespawned pool
    [SerializeField] int AmmoNum = 10;
    public GameObject[] AmmoPackPool;
    [SerializeField] private GameObject pAmmo=null;

    //Enemies spawned from a prespawned pool
    [SerializeField] int EnemyNum = 50;
    public GameObject[] EnemyPool;
    [SerializeField] private GameObject pEnemy=null;
    private int nextAvaliableEnemy = 0;//faster than looping through to find next unactive enemy

    //Bullets spawned from a prespawned pool
    [SerializeField] int BulletNum = 20;
    public GameObject[] BulletPool;
    [SerializeField] private GameObject pBullet=null;
    private int nextAvaliableBullet = 0;//faster than looping through to find next unactive bullet

    //Basic Singleton
    private void Awake ()
    {
        if (Instance != null && Instance != this)
            Destroy(this.gameObject);
        Instance = this;

        paused = false;
        updatable = true;

        //spawn enemies
        EnemyPool = new GameObject[EnemyNum];
        for(int i =0; i<EnemyNum; ++i)
        {
            EnemyPool[i] =  Instantiate(pEnemy, this.transform.position, this.transform.rotation) as GameObject;
            EnemyPool[i].GetComponent<EnemyScript>().Die(false);
        }

        //spawn bullets
        BulletPool = new GameObject[BulletNum];
        for (int i = 0; i < BulletNum; ++i)
        {
            BulletPool[i] = Instantiate(pBullet, this.transform.position, this.transform.rotation) as GameObject;
            BulletPool[i].SetActive(false);
        }

        //spawn ammo packs
        AmmoPackPool = new GameObject[AmmoNum];
        for (int i = 0; i < AmmoNum; ++i)
        {
            AmmoPackPool[i] = Instantiate(pAmmo, this.transform.position, this.transform.rotation) as GameObject;
            AmmoPackPool[i].SetActive(false);
        }
    }
	
    //Allows outsiders to set/get updatable
    public bool IsUpdatable() { return updatable; }
    public void SetUpdatable(bool update) { updatable = update; }

    //Allows outsiders to set/get playgame
    public bool IsGameStarted() { return GameStarted; }
    public void StartGame()
    {
        GameStarted = true;
        GetComponentsInChildren<Canvas>()[1].enabled = false;
    }

    //Sets pause state
    public void TogglePause()
    {
        paused = !paused;

        //If VR player is alive than pause
        if(VrPlayer.Instance.GetHealth()>0)
        {
            //show pause menu if paused
            GetComponentsInChildren<Canvas>()[0].enabled = paused;

            //show VR paused canvas if paused
            VRPause.enabled = paused;

            //set timescale to 0 if paused
            Time.timeScale = paused? 0 : 1;

            //center the mouse during gameplay
            Cursor.lockState = paused ? CursorLockMode.None : CursorLockMode.Locked;

            //if paused dont update
            updatable = !paused;


            if (!GameStarted)
            {
                //if game hasn't started and is paused hide game instructions on PC side
                GetComponentsInChildren<Canvas>()[1].enabled = !paused;
            }
            else
            {
                //if game paused dont spawn
                Spawner.Instance.SetSpawn(!paused);
            }
        }
        //for death state
        else
        {
            //activate death screeen on PC side
            GetComponentsInChildren<Canvas>()[2].enabled = true;

            Time.timeScale = 0;

            //give the PC player their mouse back
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            updatable = false;
        }
    }

    public void ResetGame()
    {
        //Kill off each enemy quietly
        foreach( GameObject e in EnemyPool)
        {
            e.GetComponent<EnemyScript>().Die(false);
        }
        //set each bullet to be inactive
        foreach (GameObject B in BulletPool)
        {
            B.SetActive(false);
        }
        //set each ammo pack to be inactive
        foreach (GameObject A in AmmoPackPool)
        {
            A.SetActive(false);
        }

        //reset the game
        VrPlayer.Instance.Reset();
        PcPlayer.Instance.transform.position = new Vector3(0, 1, -1.78f);
        GameStarted = false;

        //turn off death menu
        GetComponentsInChildren<Canvas>()[2].enabled = false;
        TogglePause();
    }
    public GameObject GetEnemy()
    {
        //Has risk of taking enemy that is currently in use but, speed makes it a worthy trade off
        ++nextAvaliableEnemy;
        nextAvaliableEnemy %= EnemyNum;

        return EnemyPool[nextAvaliableEnemy];
    }

    public GameObject GetBullet()
    {
        //Has risk of taking bullet that is currently in use but, speed makes it a worthy trade off
        ++nextAvaliableBullet;
        nextAvaliableBullet %= BulletNum;

        return BulletPool [nextAvaliableBullet];
    }

    public GameObject GetAmmoPack()
    {
        //Ammo pack is expected to be a low number ~5 so popping a ammo pack could be devistating
        for (int i = 0; i < AmmoNum; ++i)
        {
            //replace with spawning 5 and then only spawning each time one is collected?
            if (!AmmoPackPool[i].activeSelf)
            {
                return AmmoPackPool[i];
            }
        }
        //Debug.Log("Consider More Ammo Packs");
        return null;
    }

}
