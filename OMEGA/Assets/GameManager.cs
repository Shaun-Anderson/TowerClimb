using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public GameObject UI;
    public Transform pauseMenu;
    public Transform endScreen;

    public bool pause;
    public bool noMove;

    [Header("LEVEL")]
    public int hi = 5;
    public Vector3 spawnPoint;
    public WorldType worldType;
    public int currentWorld;
    public int currentLvl;

    public ChunkType chunkType;

    public GameObject[] Tower_Middle_Chunks;
    public GameObject[] Tower_Left_Chunks;
    public GameObject[] Tower_Right_Chunks;


    public GameObject[] startChunks;
    public GameObject[] checkPointChunks;
    public GameObject[] lvlChunks;


    [Header("MISSIONS")]
    public Mission[] curMissions;
    public Mission[] missions;

    public float kills;
    public int coinGained;

    public static GameManager instance = null;              //Static instance of GameManager which allows it to be accessed by any other script.

    void Awake ()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
        Application.targetFrameRate = 60;
        spawnPoint = new Vector3(1, 0, 0);
        for (int i = 0; i <curMissions.Length; i++)
        {
            int rand = Random.Range(0, missions.Length);
            curMissions[i].title = missions[rand].title;
            curMissions[i].killsRequired = missions[rand].killsRequired;
            curMissions[i].missionType = missions[rand].missionType;
        }

	}

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }

    void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Level Loaded " + scene.name);
        if(scene.name == "Game")
        {
            UI = GameObject.Find("PlayerUI");
            pauseMenu = UI.transform.Find("PauseMenu");
            endScreen = UI.transform.Find("EndScreen");
            pauseMenu.GetComponent<PauseMenu>().mission1.text = curMissions[0].title;
            //ADD PAUSE BUTTON
            Button myselfButton = UI.transform.Find("PauseButton").GetComponentInChildren<Button>();
            myselfButton.onClick.AddListener(() => PauseSwitch());

            Debug.Log(pauseMenu.name);
            LoadLevel();
        }
    }

    public void StartGame()
    {
        Application.LoadLevel(1);
    }

    public void LoadLevel()
    {
        currentLvl += 1;
        int size = Random.Range(10, 20) + (2 * currentLvl);
        for (int i =0; i<size; i++)
        {
            //MIDDLE SECTIONS
            if (i != 0 && i != size -1)
            {
                GameObject chunk;
                if (chunkType == ChunkType.Middle)
                {
                    chunk = Tower_Middle_Chunks[Random.Range(0, Tower_Middle_Chunks.Length)];
                    GameObject lvlChunk = Instantiate(chunk, new Vector3(spawnPoint.x + chunk.GetComponent<LvlChunk>().offset.x, spawnPoint.y + chunk.GetComponent<LvlChunk>().offset.y, 0), Quaternion.identity);
                    chunkType = chunk.GetComponent<LvlChunk>().chunkType;
                }
                if (chunkType == ChunkType.Left)
                {
                    chunk = Tower_Left_Chunks[Random.Range(0, Tower_Left_Chunks.Length)];
                    GameObject lvlChunk = Instantiate(chunk, new Vector3(spawnPoint.x + chunk.GetComponent<LvlChunk>().offset.x, spawnPoint.y + chunk.GetComponent<LvlChunk>().offset.y, 0), Quaternion.identity);
                    chunkType = chunk.GetComponent<LvlChunk>().chunkType;
                }
                if (chunkType == ChunkType.Right)
                {
                    chunk = Tower_Right_Chunks[Random.Range(0, Tower_Right_Chunks.Length)];
                    GameObject lvlChunk = Instantiate(chunk, new Vector3(spawnPoint.x + chunk.GetComponent<LvlChunk>().offset.x, spawnPoint.y + chunk.GetComponent<LvlChunk>().offset.y, 0), Quaternion.identity);
                    chunkType = chunk.GetComponent<LvlChunk>().chunkType;
                }
            }
            //START
            else if (i == 0 && hi == 5)
            {
                GameObject chunk = startChunks[Random.Range(0, startChunks.Length)];
                GameObject lvlChunk = Instantiate(chunk, new Vector3(spawnPoint.x + chunk.GetComponent<LvlChunk>().offset.x, spawnPoint.y + chunk.GetComponent<LvlChunk>().offset.y, 0), Quaternion.identity);
                chunkType = chunk.GetComponent<LvlChunk>().chunkType;
            }
            //CHECKPOINT
            else if ( i == size -1 && hi >= 0)
            {
                GameObject chunk = checkPointChunks[Random.Range(0, checkPointChunks.Length)];
                GameObject lvlChunk = Instantiate(chunk, new Vector3(spawnPoint.x + chunk.GetComponent<LvlChunk>().offset.x, spawnPoint.y + chunk.GetComponent<LvlChunk>().offset.y, 0), Quaternion.identity);
                chunkType = chunk.GetComponent<LvlChunk>().chunkType;
                hi -= 1;
                LoadLevel();              
            }
            spawnPoint.y += 1.53f;
        }
    }

	public void PauseSwitch ()
    {
        if(pauseMenu.gameObject.activeSelf == false)
        {
            _GameManager.NoMove = true;
            pause = true;
            pauseMenu.gameObject.SetActive(true);
            Time.timeScale = 0f;
        }
        else
        {
            _GameManager.NoMove = false;
            pause = false;
            pauseMenu.gameObject.SetActive(false);
            Time.timeScale = 1f;
        }
	}

    public void CheckMissions()
    {
        for(int i = 0; i<curMissions.Length; i++)
        {
             switch(curMissions[i].missionType)
            {
                case MissionType.Kill:
                    PauseMenu PauseScript = pauseMenu.GetComponent<PauseMenu>();
                    float percent = kills / curMissions[i].killsRequired;
                    curMissions[i].precentComplete = percent;
                    PauseScript.mission1.text = curMissions[0].title + " (" + percent*100 + "%)";
                    PauseScript.mission1Percent.transform.localScale = new Vector3 (percent, PauseScript.mission1Percent.transform.localScale.y, PauseScript.mission1Percent.transform.localScale.z);
                    if (kills >= curMissions[i].killsRequired)
                    {
                        
                    }
                    break;
                case MissionType.World:
                    break;
                case MissionType.Coins:
                    break;
            }
        }
    }

    public void Death(int gold)
    {
        _GameManager.NoMove = true;
        pause = true;
        endScreen.gameObject.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Restart()
    {
        SceneManager.LoadScene(1);
    }
}

public enum WorldType
{
    Tower,
    Cave,
    Hell
}

public enum ChunkType
{
    Left,
    Right,
    Middle
}
