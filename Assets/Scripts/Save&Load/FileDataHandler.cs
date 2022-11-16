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


    public GameData Load()
    {
        //create direcotry path
        string fullPath = Path.Combine("Assets/Scripts/Save&Load/Data", dataFileName);
        //string fullPath = Path.Combine(dataDir, dataFileName);
        GameData loadedData = null;
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

                loadedData = JsonUtility.FromJson<GameData>(dataToLoad);
            }
            catch (Exception e)
            {
                Debug.LogError("Error occured when trying to load data to file" + fullPath + "\n" + e);
            }
        }
        return loadedData;
    }

    public void Save(GameData data)
    {
        //create direcotry path
        string fullPath = Path.Combine("Assets/Scripts/Save&Load/Data", dataFileName);
        //string fullPath = Path.Combine(dataDir, dataFileName);
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
