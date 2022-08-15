using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveAndLoadSystem
{
    static string totalCoinsPathSuffix = "/TotalCoinsData.NHK";
    static string selectedCharacterPathSuffix = "/SelectedCharacterData.NHK";

    //public static void InitialSave()
    //{
    //    string path = Application.persistentDataPath + pathSuffix;
    //    FileStream fileStream = new FileStream(path, FileMode.Create);
    //    DataFile dataFile = new DataFile();
    //    BinaryFormatter formatter = new BinaryFormatter();
    //    formatter.Serialize(fileStream, dataFile);
    //    fileStream.Close();
    //}

    public static void SaveTotalCoins(int totalCoins)
    {
        string path = Application.persistentDataPath + totalCoinsPathSuffix;
        FileStream fileStream = new FileStream(path, FileMode.Create);
        TotalCoinsData dataFile = new TotalCoinsData();
        dataFile.TotalCoins = totalCoins;
        //dataFile.SetTotalCoin(totalCoins);
        BinaryFormatter formatter = new BinaryFormatter();
        formatter.Serialize(fileStream, dataFile);
        fileStream.Close();
    }

    public static void SaveSelectedCharacter(int chosenCharacter)
    {
        string path = Application.persistentDataPath + selectedCharacterPathSuffix;

        FileStream fileStream = new FileStream(path, FileMode.Create);
        SelectedCharacterData dataFile = new SelectedCharacterData();
        dataFile.SelectedCharacter = chosenCharacter;
        //dataFile.SetSelectedCharacter(chosenCharacter);
        BinaryFormatter formatter = new BinaryFormatter();
        formatter.Serialize(fileStream, dataFile);
        fileStream.Close();
    }

    public static SelectedCharacterData LoadSelectedCharacter()
    {
        //InitialSave();
        string path = Application.persistentDataPath + selectedCharacterPathSuffix;


        FileStream fileStream = new FileStream(path, FileMode.Open);

        if (fileStream == null)
        {
            throw new System.Exception("File is empty");
        }

        if (!File.Exists(path))
        {
            throw new System.Exception("File does not exist");
        }

        BinaryFormatter formatter = new BinaryFormatter();
        SelectedCharacterData dataFile = formatter.Deserialize(fileStream) as SelectedCharacterData;
        if (dataFile == null)
        {
            MonoBehaviour.print("dataFile is null!");
        }


        fileStream.Close();
        return dataFile;


    }

    public static TotalCoinsData LoadTotalCoinsData()
    {
        string path = Application.persistentDataPath + totalCoinsPathSuffix;


        FileStream fileStream = new FileStream(path, FileMode.Open);

        if (fileStream == null)
        {
            throw new System.Exception("File is empty");
        }

        if (!File.Exists(path))
        {
            throw new System.Exception("File does not exist");
        }

        BinaryFormatter formatter = new BinaryFormatter();
        TotalCoinsData dataFile = formatter.Deserialize(fileStream) as TotalCoinsData;
        if (dataFile == null)
        {
            MonoBehaviour.print("dataFile is null!");
        }


        fileStream.Close();
        return dataFile;
    }

    public static bool DoesTotalCoinsDataExist()
    {
        string path = Application.persistentDataPath + totalCoinsPathSuffix;

        if (File.Exists(path))
        {
            return true;
        }
        return false;
    }

    public static bool DoesSelectedCharacterDataExist()
    {
        string path = Application.persistentDataPath + selectedCharacterPathSuffix;

        if (File.Exists(path))
        {
            return true;
        }
        return false;
    }
}
