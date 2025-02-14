using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class DataHandler : SingeltonBase<DataHandler>
{
    public string fileName;
    public stepRecord inGameData;
    public userDetails userData;
    private string path;
    private BinaryFormatter _binaryFormatter;
    public FileStream fileStream;

    // Start is called before the first frame update
    public override void Awake()
    {
        base.Awake();
        path = Path.Combine(Application.persistentDataPath, fileName);
        loadData();
    }

    public void saveData()
    {
        save();
    }

    private void loadData()
    {
        if (File.Exists(path))
        {
            //load Here
            _binaryFormatter = new BinaryFormatter();
            fileStream = new FileStream(path,FileMode.Open);
            try
            {
                inGameData = _binaryFormatter.Deserialize(fileStream) as stepRecord;
                userData = _binaryFormatter.Deserialize(fileStream) as userDetails;
                fileStream.Close();
            }
            catch (Exception e)
            {
                fileStream.Close();
                Console.WriteLine(e);
                throw;
            }
        }
        else
        {
            inGameData = new stepRecord();
            userData = new userDetails();
        }
    }

    private void save()
    {
        _binaryFormatter = new BinaryFormatter();
        fileStream = new FileStream(path,FileMode.Create);
        try
        {
            _binaryFormatter.Serialize(fileStream, inGameData);
            _binaryFormatter.Serialize(fileStream, userData);
            fileStream.Close();
        }
        catch (Exception e)
        {
            fileStream.Close();
            Debug.LogWarning(e);
            throw;
        }
            
    }

    private void OnApplicationQuit()
    {
        save();
    }
    public void resetData()
    {
        inGameData = new stepRecord();
    }

}

[System.Serializable]
public class stepData
{
    public int stepCounts=0;
    public float distanceWalked=0;
    public float time=0;
    public float calories=0;
    public string Day = "";
}
[System.Serializable]
public class userDetails
{
    public float weight = 50;
    public float height = 177;
    public int goal = 50;
}
[System.Serializable]

public class stepRecord
{
    public List<stepData> data= new List<stepData>();
}