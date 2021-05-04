using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class missionTracker{
    public string requireStr;
    public int requireNum;

    public missionTracker(string name, int number){
        this.requireStr = name;
        this.requireNum = number;
    }

    public void oneDone(){
        requireNum --;
    }

    public bool isFinished(){
        return requireNum <= 0;
    }
}
