using UnityEngine;
using System.Collections;

public class Manager : MonoBehaviour {

    public static Manager Instance = null;

    //Pause menu management
    [SerializeField] Canvas VRPause;
    private bool paused = false;
    private bool updatable = true;

    //Ammo spawned from a prespawned pool
    [SerializeField] int AmmoNum = 10;
    public GameObject[] AmmoPackPool;
    [SerializeField] private GameObject pAmmo;

    //Enemies spawned from a prespawned pool
    [SerializeField] int EnemyNum = 50;
    public GameObject[] EnemyPool;
    [SerializeField] private GameObject pEnemy;
    private int nextAvaliableEnemy = 0;//faster than looping through to find next unactive enemy

    //Bullets spawned from a prespawned pool
    [SerializeField] int BulletNum = 20;
    public GameObject[] BulletPool;
    [SerializeField] private GameObject pBullet;
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

    //Sets pause state
    public void TogglePause()
    {
        if(VrPlayer.Instance.GetHealth()>0)//If VR player is alive than pause
        {
            paused = !paused;

            if (paused)//if now paused
            {
                GetComponentInChildren<Canvas>().enabled = true;
                VRPause.enabled = true;//show VR paused canvas
                Time.timeScale = 0;//pause time, nessesary??
                Cursor.lockState = CursorLockMode.None; //give the PC player their mouse back
                Cursor.visible = true; //hide crosshair

                updatable = false;
            }
            else
            {
                GetComponentInChildren<Canvas>().enabled = false;
                VRPause.enabled = false;//hide VR paused canvas
                Time.timeScale = 1;//unpause time
                Cursor.lockState = CursorLockMode.Locked; //lock the PC mouse to center of the screen
                Cursor.visible = false;//unhide crosshair

                updatable = true;
            }
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

        VrPlayer.Instance.Reset();//reset VR player
        PcPlayer.Instance.transform.position.Set(0, 1, -1.78f);//Reset PC player pos
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
            if (!AmmoPackPool[i].activeSelf) //active in hierarchy could be wrong call. use active self instead. Use a current place to speed up check
            {
                return AmmoPackPool[i];
            }
        }
        //Debug.Log("Potentially Not Enough Ammo Packs");
        return null;
    }

}
