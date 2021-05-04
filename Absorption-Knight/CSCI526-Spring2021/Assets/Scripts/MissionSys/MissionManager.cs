using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionManager
{
    private static MissionManager mmInstance;

    private Dictionary<string, IMission> toDoList;
    private Dictionary<string, IMission> completeList; 

    public static MissionManager getInstance(){
        if(mmInstance == null){
            mmInstance = new MissionManager();
        }
        return mmInstance;
    }

    private MissionManager(){
        toDoList = new Dictionary<string, IMission>();
        completeList = new Dictionary<string, IMission>();
        readData();
    }

    public string checkToDoList(){
        string res = "";
        foreach(KeyValuePair<string, IMission> m in toDoList){
            res += m.Key;
            if(m.Value.missionComplete()){
                res += " Clear\n";
            } else {
                res += " On going\n";
            }
        }
        return res;
    }

    //从NPC处调用，如果返回false说明任务不可接，否则接受任务且返回true
    public bool takeMission(IMission m){
        if(toDoList.ContainsKey(m.getMissionName()) || completeList.ContainsKey(m.getMissionName())) return false;
        toDoList[m.getMissionName()] = m;
        Debug.Log("mission is taken: " + m.getMissionName());
        return true;
    }

    //传入一个mission并且立即将其完成
    public void takeMissionAndFinish(IMission m){
        toDoList.Remove(m.getMissionName());
        completeList[m.getMissionName()] = m;
    }

    //从怪物死亡处调用，更新所有任务状态
    public void updateAllMissionStates(string q){
        foreach(KeyValuePair<string, IMission> m in toDoList){
            m.Value.updateTrackers(q);
        }
    }

    //从npc调用，检查任务是否在待完成项中，是否已可以完成，若可，移动到完成列并返回true，否则返回false
    public bool finMission(string m){
        if(toDoList.ContainsKey(m)){
            IMission temp = toDoList[m];
            if(temp.missionComplete()){
                toDoList.Remove(m);
                completeList[temp.getMissionName()] = temp;
                return true;
            }
        }
        return false;
    }

    //从npc调用，检查任务是否已经在已完成项目中，是则返回true，否则false
    public bool findMissionFinished(string m){
        return completeList.ContainsKey(m);
    }

    //用来测试 MissionPanel的instantiation
    //唯两个加了这个用来测试
    public Dictionary<string, IMission> getTodo()
    {
        return toDoList;
    }
    public Dictionary<string, IMission> getComplete()
    {
        return completeList;
    }

    //read和save用于存储状态
    private void readData(){

    }
    private void saveData(){
        SaveSystem.SaveMission(toDoList,completeList);
    }
}
