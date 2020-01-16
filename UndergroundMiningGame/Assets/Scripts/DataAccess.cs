using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataAccess
{
    [DllImport("__Internal")]
    private static extern void SyncFiles();

    [DllImport("__Internal")]
    private static extern void WindowAlert(string message);

    public static void Save(SavaData data)
    {
        string dataPath = Application.persistentDataPath + "/Player.dat";
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        FileStream fileStream;

        try
        {
            if (File.Exists(dataPath))
            {
                File.WriteAllText(dataPath, string.Empty);
                fileStream = File.Open(dataPath, FileMode.Open);
            }
            else
            {
                fileStream = File.Create(dataPath);
            }

            binaryFormatter.Serialize(fileStream, data);
            fileStream.Close();

            if (Application.platform == RuntimePlatform.WebGLPlayer)
            {
                SyncFiles();
            }
        }
        catch (Exception e)
        {
            PlatformSafeMessage("Failed to Save: " + e.Message);
        }
    }

    public static SavaData Load()
    {
        SavaData data = null;
        string dataPath = Application.persistentDataPath + "/Player.dat";

        try
        {
            if (File.Exists(dataPath))
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                FileStream fileStream = File.Open(dataPath, FileMode.Open);

                data = (SavaData)binaryFormatter.Deserialize(fileStream);
                fileStream.Close();
            }
        }
        catch (Exception e)
        {
            PlatformSafeMessage("Failed to Load: " + e.Message);
        }

        return data;
    }

    public static void Delete()
    {
        string dataPath = Application.persistentDataPath + "/Player.dat";
        if (File.Exists(dataPath))
        {
            File.Delete(dataPath);
        }
    }

    public static bool HasSaveFile()
    {
        string dataPath = Application.persistentDataPath + "/Player.dat";
        return File.Exists(dataPath);
    }

    private static void PlatformSafeMessage(string message)
    {
        if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
            WindowAlert(message);
        }
        else
        {
            Debug.Log(message);
        }
    }
}
