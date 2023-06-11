using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MyDataManager : MonoBehaviour
{
    [Header("File Storage Config")]
    [SerializeField] private string _FileName;

    private GameData gameData;
    private List<IMyDataManager> myDataManagerObjects;

    private MyFileDataHandler myDataHandler;
    public static MyDataManager instance { get; private set; }

    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogError("More than one MyDataManagers in the scene!!!");
        }
        instance = this;
    }

    private void Start()
    {
        myDataHandler = new MyFileDataHandler(Application.persistentDataPath,_FileName);
        myDataManagerObjects = FindAllMyDataManagerObjects();
        LoadGame();
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }

    public void NewGame() => gameData = new GameData();

    public void LoadGame()
    {
        gameData = myDataHandler.Load();

        if(gameData == null)
        {
            Debug.Log("No data found. Initializig data to defaults.");
            NewGame();
        }
        
        foreach(IMyDataManager myDataManagerObj in myDataManagerObjects)
        {
            myDataManagerObj.LoadData(gameData);
        }
    }

    public void SaveGame()
    {
        foreach (IMyDataManager myDataManagerObj in myDataManagerObjects)
        {
            myDataManagerObj.SaveData(ref gameData);
        }
        myDataHandler.Save(gameData);
    }

    private List<IMyDataManager> FindAllMyDataManagerObjects()
    {
        IEnumerable<IMyDataManager> myDataManagerObjects = FindObjectsOfType<MonoBehaviour>().
            OfType<IMyDataManager>();

        return new List<IMyDataManager>(myDataManagerObjects);
    }
}
