using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class MyFileDataHandler
{
    private string _DataDirectoryPath;
    private string _DataFileName;

    public MyFileDataHandler(string DataDirectoryPath, string DataFileName)
    {
        _DataDirectoryPath = DataDirectoryPath;
        _DataFileName = DataFileName;
    }

    public GameData Load()
    {
        string fullPath = Path.Combine(_DataDirectoryPath, _DataFileName);
        GameData loadedData = null;
        if(File.Exists(fullPath))
        {
            try
            {
                string dataToLoad = "";
                using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                {
                    using(StreamReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }

                loadedData = JsonUtility.FromJson<GameData>(dataToLoad);
            }
            catch (Exception e)
            {

                Debug.Log("Error occured when trying to save data to file" + fullPath + "\n" + e);
            }
        }
        return loadedData;
    }

    public void Save(GameData data)
    {
        string fullPath = Path.Combine(_DataDirectoryPath,_DataFileName);
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
            string dataToStore = JsonUtility.ToJson(data,true);
            using (FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(dataToStore);
                }
            }
        }
        catch(Exception e)
        {
            Debug.Log("Error occured when trying to save data to file" + fullPath + "\n" + e);
        }
    }
}
