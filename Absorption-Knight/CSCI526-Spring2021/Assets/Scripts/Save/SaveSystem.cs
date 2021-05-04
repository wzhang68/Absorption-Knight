using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.SceneManagement;


public static class SaveSystem
{
    // There exists some unicode issues with converting data to binary file, 
    // so I decide to store data via using JSON.
    // Hash Table and Dictionary are not supported by JSONUtility(Cannot convert from them to JSON string), so I need to find other ways.
    // Partially Solved: Imported JSON .NET, USING JSON UTILITY AND JSON.NET AT THE SAME TIME
    // JSON.NET and JSON Utility are shittttttttttttty stuff. MOTHERFUCKER!  STUPID SHIT;
    // NEED TO FIND A MORE CONVENIENT WAY/ A MORE GENERIC WAY TO STORE DATA.
    // I'M KIND LIKE HARD CODING CUZ MIDTERM IS ON WED (3/17????)
    // MIGHT ONLY FOR TEMPORARY USE... HIGHLY POSSIBLY

    // public static void Save(PlayerUnit player)
    // {
    //     BinaryFormatter formatter = new BinaryFormatter();
    //     string path = Application.dataPath + "/playerData.txt";
    //     FileStream stream = new FileStream(path, FileMode.Create);
    //
    //     PlayerData data = new PlayerData(player);
    //     formatter.Serialize(stream, data);
    //     stream.Close();
    //     Debug.Log("Saved to " + path);
    // }
    
    /// <summary>
    /// Storing Data via JSON
    /// It will be saved as a text file with JSON format.
    /// Location is something like this: C:\Users\zww\AppData\LocalLow\Seven Adventurers\Absorption_Knight
    /// </summary>
    /// <param name="player"></param>
    public static void Save(PlayerUnit player)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/playerData.txt";
        PlayerData data;
        if (File.Exists(path))
        {
            data = Load();
            data.level = SceneManager.GetActiveScene().name;
            data.SetPosInLevels(SceneManager.GetActiveScene().name,player);
            data.sanity = player.Sanity();
            data.energyBall = player.EB();
            data.health = player.HP();
            data.maxhealth = player.MaxHP();
            data.maxsanity = player.MaxSan();
            data.buffarray = player.temp2;
            data.musicVol = player.musicVol;
            data.effectVol = player.effectVol;
            data.activeskill = player.GetActiveSkillType();
            data.skillcount = player.GetSkillCount();
            data.speed = player.GetTeodoroSpeed();
            data.jumpforce = player.GetTeodoroJumpForce();
            data.musicVol = player.musicVol;
            data.effectVol = player.effectVol;
        }
        else
        {
            data = new PlayerData(player);
            data.Reset();
        }
        var settings = new JsonSerializerSettings();
        string JSONString = JsonConvert.SerializeObject(data,Formatting.Indented,settings);
        StreamWriter sw = new StreamWriter(path);
        sw.Write(JSONString);
        sw.Close();
        Debug.Log(JSONString);
        Debug.Log("-=-=-=-=-=Saved=-=-=-=-=-");
        Debug.Log(path);
    }

    // 没写完 还在测试
    public static void SaveMission(Dictionary<string, IMission> todo,Dictionary<string, IMission> complete)
    {
        string JSON_Todo = "";
        string JSON_Complete = "";
        string path_todo = Application.persistentDataPath + "/missionData_todo.txt";
        string path_complete = Application.persistentDataPath + "/missionData_complete.txt";

        BinaryFormatter formatter = new BinaryFormatter();
        var settings = new JsonSerializerSettings();
        string JSONString = JsonConvert.SerializeObject(todo,Formatting.Indented,settings);
        string JSONString2 = JsonConvert.SerializeObject(complete,Formatting.Indented,settings);
        StreamWriter sw = new StreamWriter(path_todo);
        StreamWriter sw2 = new StreamWriter(path_complete);
        sw.Write(JSONString);
        sw2.Write(JSONString);
        sw.Close();
        sw2.Close();
        Debug.Log(JSONString);
        Debug.Log(JSONString2);
        Debug.Log("-=-=-=-=-=Saved=-=-=-=-=-");


    }
    
    
    // Binary One : It doesn't work somehow.... Just a idea...
    // public static PlayerData Load()
    // {
    //     string path= Application.dataPath + "/playerData.txt";
    //     if (File.Exists(path))
    //     {
    //         BinaryFormatter formatter = new BinaryFormatter();
    //         FileStream stream = new FileStream(path, FileMode.Open);
    //         PlayerData data = formatter.Deserialize(stream) as PlayerData;
    //         stream.Close();
    //         return data;
    //     }
    //     else
    //     {
    //         Debug.Log("Save file not found in " + path);
    //         return null;
    //     }
    // }
    
    /// <summary>
    /// Loading Data via JSON
    /// </summary>
    /// <returns></returns>
    public static PlayerData Load()
    {
        string path= Application.persistentDataPath + "/playerData.txt";
        if (File.Exists(path))
        {
            StreamReader sr = new StreamReader(path);
            string JSONString = sr.ReadToEnd();
            sr.Close();
            PlayerData data =JsonUtility.FromJson<PlayerData>(JSONString);
            return data;
        }
        else
        {
            Debug.Log("Save file not found in " + path);
            return null;
        }
    }

}
