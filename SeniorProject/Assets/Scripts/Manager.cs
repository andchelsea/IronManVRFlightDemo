using UnityEngine;
using System.Collections;

public class Manager : MonoBehaviour {

    public static Manager Instance = null;

    private bool paused = false;
    [SerializeField] Canvas VRPause;
    [SerializeField] int BulletNum = 20;
    [SerializeField] int EnemyNum = 50;
    [SerializeField] int AmmoNum = 10;
    public GameObject[] EnemyPool;
    [SerializeField] private GameObject pEnemy;
    public GameObject[] BulletPool;
    [SerializeField] private GameObject pBullet;
    public GameObject[] FlarePool;
    public GameObject pFlare1;//move into pc player
    public GameObject pFlare2;//move into pc player
    public GameObject[] AmmoPackPool;
    [SerializeField] private GameObject pAmmo;

    private bool updatable = true;

	// Use this for initialization
	private void Awake ()
    {
        if (Instance != null && Instance != this)
            Destroy(this.gameObject);

        Instance = this;

        paused = false;
        updatable = true;

        EnemyPool = new GameObject[EnemyNum];
        for(int i =0; i<EnemyNum; ++i)
        {
            EnemyPool[i] =  Instantiate(pEnemy, this.transform.position, this.transform.rotation) as GameObject;
            EnemyPool[i].SetActive(false);
        }

        BulletPool = new GameObject[BulletNum];
        for (int i = 0; i < BulletNum; ++i)
        {
            BulletPool[i] = Instantiate(pBullet, this.transform.position, this.transform.rotation) as GameObject;
            BulletPool[i].SetActive(false);
        }

        FlarePool = new GameObject[2];
        for (int i = 0; i < 2; ++i)
        {
            FlarePool[i] = Instantiate(pBullet, this.transform.position, this.transform.rotation) as GameObject;
            FlarePool[i].SetActive(false);
        }

        AmmoPackPool = new GameObject[AmmoNum];
        for (int i = 0; i < AmmoNum; ++i)
        {
            AmmoPackPool[i] = Instantiate(pAmmo, this.transform.position, this.transform.rotation) as GameObject;
            AmmoPackPool[i].SetActive(false);
        }
    }
	
    public bool IsUpdatable() { return updatable; }
    public void SetUpdatable(bool update) { updatable = update; }

    public void TogglePause()
    {
        paused = !paused;
        if (paused)
        {

            GetComponentInChildren<Canvas>().enabled = true;
            VRPause.enabled = true;
            Time.timeScale = 0;
            Cursor.lockState = CursorLockMode.None; 
            //hide crosshair
            Cursor.visible = true;

            updatable = false;
        }
        else
        {
            GetComponentInChildren<Canvas>().enabled = false;
            VRPause.enabled = false;
            Time.timeScale = 1;
            Cursor.lockState = CursorLockMode.Locked; 
            //unhide crosshair
             Cursor.visible = false;

            updatable = true;
        }

    }

    public GameObject GetEnemy()
    {
        for (int i = 0; i < EnemyNum; ++i)
        {
            if (!EnemyPool[i].activeInHierarchy)
            {
                return EnemyPool[i];
            }
        }
        Debug.Log("Not Enough Enemies");
        return null;
    }

    public GameObject GetBullet()
    {
        for (int i = 0; i < BulletNum; ++i)
        {
            if (!BulletPool[i].activeInHierarchy)
            {
                return BulletPool[i];
            }
        }
        return null;
    }

    public GameObject GetAmmoPack()
    {
        for (int i = 0; i < AmmoNum; ++i)
        {
            if (!AmmoPackPool[i].activeInHierarchy)
            {
                return AmmoPackPool[i];
            }
        }
        return null;
    }

    // Update is called once per frame
    void Update ()
    {
	
	}
}
