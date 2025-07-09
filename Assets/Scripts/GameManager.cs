using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public static GameManager gm;
    public GameObject enemy;
    public GameObject player;
    WaveFunctionCollapse wfc;
    private int wave;
    public float timeCountDown;
    TextMeshProUGUI text;
    private float spawnCoolDown;
    public float spawnRate;
    public int enemyEachWave;
    bool haveSpawn;
    public GameObject gameSetting;
    int bonus;
    

    // Start is called before the first frame update
    void Start()
    {
        GetComponent();
        wfc.StartWaveFunction();
        SpawnPlayer();
        gameSetting.SetActive(false);
        bonus = 5;
    }
    public void MenuSetting()
    {
        Time.timeScale = 0f;
        gameSetting.SetActive(true);
    }
    public void Resume()
    {
        Time.timeScale = 1f;
        gameSetting.SetActive(false);
    }
    public void MainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("StartScene");
    }
    void GetComponent()
    {
        gm = this;
        wfc = GetComponent<WaveFunctionCollapse>();
        text = GameObject.Find("Timer").GetComponent<TextMeshProUGUI>();
        wave = 1;
        spawnCoolDown = 0;
        enemyEachWave = 0;
        haveSpawn = true;
    }

    // Update is called once per frame
    //Thiết lập điều kiện sau mỗi bao nhiêu giây đó thì sẽ tiếp tục tạo ra thêm nhiều quái vật để tiến công
    void Update()
    {
        if(wave == 5)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            SceneManager.LoadScene("EndScene");
        }
        if(GameObject.FindGameObjectWithTag("Enemy") && haveSpawn)
        {
            haveSpawn = false;
            enemyEachWave++;
        }
        
        if(wfc.end && !GameObject.FindGameObjectWithTag("Enemy") && timeCountDown>=0)
        {
            timeCountDown -= Time.deltaTime;
            text.text = timeCountDown.ToString("F2");
        }
        if(timeCountDown <= 0)
        {
            spawnCoolDown -= Time.deltaTime;
            text.text = "Wave " + wave;
            if(spawnCoolDown <= 0)
            {
                if(enemyEachWave < wave + 4 + bonus)
                {
                    SpawnEnemy();
                    haveSpawn = true;
                    spawnCoolDown = spawnRate;
                }else 
                {
                    wave++;
                    enemyEachWave = 0;
                    timeCountDown =5f;
                }
            }
        }   
    }
    //Tạo quái vật
    public void SpawnEnemy()
    {
        Instantiate(enemy,wfc.cellForWay[0].transform.position - new Vector3(3,-1,0),transform.rotation);
    }
    //Tạo ra người chơi để di chuyển    
    public void SpawnPlayer()
    {
            Cell mid = wfc.thatMidCell;
            Vector3 spawnLocation =  mid.transform.position + new Vector3(0,10,0);
            Instantiate(player,spawnLocation,player.transform.rotation); 
    }
}
