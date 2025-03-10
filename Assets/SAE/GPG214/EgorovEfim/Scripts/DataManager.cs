using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using Gamekit2D;
using System.Collections;

[System.Serializable]
public class GameData
{
    public string scene;
    public int health;
    public float[] playerPositionArray = new float[] {0,0};
    public bool resetHealthOnSceneReload;

    public Vector2 ReturnPlayerPosition()
    {
        if (playerPositionArray.Length == 2)
        {
            return new Vector2(playerPositionArray[0], playerPositionArray[1]); 
        }
        else
        {
            return Vector2.zero;
        }
    }
}
public class DataManager : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Damageable damageable;
    GameData gameData;
    private string persistentDataPath;
    public string saveFileName = "gamedata.json";

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    void Start()
    {
        persistentDataPath = Application.persistentDataPath;
        gameData = new GameData();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.S))
        {
            SaveGameData();
        }
        if(Input.GetKeyDown(KeyCode.L))
        {
            LoadGameData();
            StartCoroutine(LoadSceneAndRestoreData());
        }
    }

    public void SaveGameData()
    {
        gameData.scene = SceneManager.GetActiveScene().name;

        Data<int, bool> data = (Data<int, bool>)damageable.SaveData();

        gameData.health = data.value0;
        gameData.resetHealthOnSceneReload = data.value1;
        
        if (playerTransform != null)
        {
            gameData.playerPositionArray[0] = playerTransform.position.x;
            gameData.playerPositionArray[1] = playerTransform.position.y;
        }

        string json = JsonUtility.ToJson(gameData, true);
        string savePath = Path.Combine(persistentDataPath, saveFileName);
        File.WriteAllText(savePath, json);
        Debug.Log("Game data saved in: " + savePath);
    }

        public void LoadGameData()
    {
        string savePath = Path.Combine(persistentDataPath, saveFileName);
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            gameData = JsonUtility.FromJson<GameData>(json);
            Debug.Log("Game data loaded: " + json);
        }
        else
        {
            Debug.Log("Save file not found.");
            gameData = new GameData();
            SaveGameData();
        }
    }

    IEnumerator LoadSceneAndRestoreData()
    {
        Debug.Log("started coroutine");
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(gameData.scene);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        Debug.Log("scene loaded");

        GameObject playerObject = GameObject.FindWithTag("Player");
        if (playerObject != null)
        {
            playerTransform = playerObject.transform;

            playerTransform.position = gameData.ReturnPlayerPosition();
            Debug.Log(playerTransform.position);

            damageable = playerObject.GetComponent<Damageable>();
            if (damageable != null)
            {
                Data<int, bool> data = new Data<int, bool>(gameData.health, gameData.resetHealthOnSceneReload);
                damageable.LoadData(data);
            }
        }
        else
        {
            Debug.LogWarning("Player object not found in the loaded scene.");
        }
    }
}
