using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class FileDataHandler
{
    private string dataDir = "";
    private string dataFileName = "";

    public FileDataHandler(string dataDir, string dataFileName)
    {
        this.dataDir = dataDir;
        this.dataFileName = dataFileName;
    }

    public GameData NewGame()
    {
        string fullPath = Path.Combine(dataDir, "NewGameData");
        GameData loadedDate = null;
        if (File.Exists(fullPath))
        {
            try
            {
                //Load Data from the file
                string dataToLoad = "";
                using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }

                loadedDate = JsonUtility.FromJson<GameData>(dataToLoad);
            }
            catch (Exception e)
            {
                Debug.LogError("Error occured when trying to load data to file" + fullPath + "\n" + e);
            }
        }
        return loadedDate;
    }
    public GameData Load()
    {
        //create direcotry path
        string fullPath = Path.Combine(dataDir, dataFileName);
        GameData loadedDate = null;
        if (File.Exists(fullPath))
        {
            try
            {
                //Load Data from the file
                string dataToLoad = "";
                using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }

                loadedDate = JsonUtility.FromJson<GameData>(dataToLoad);
            }
            catch (Exception e)
            {
                Debug.LogError("Error occured when trying to load data to file" + fullPath + "\n" + e);
            }
        }
        return loadedDate;
    }

    public void Save(GameData data)
    {
        //create direcotry path
        string fullPath = Path.Combine(dataDir, dataFileName);
        try
        {
            //create direcotry
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
            //change data into Json
            string dataToStore = JsonUtility.ToJson(data, true);
            //write data to file
            using (FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(dataToStore);
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Error occured when trying to save data to file" + fullPath + "\n" + e);
        }
    }
}
