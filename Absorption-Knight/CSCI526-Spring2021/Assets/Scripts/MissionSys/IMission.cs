using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class IMission : MonoBehaviour
{
    public string missionName;
    public string reward;
    public List<missionTracker> requirements;

    public string getMissionName(){
        return missionName;
    }

    //更新所有tracker
    public void updateTrackers(string q){
        for(int i = 0; i < requirements.Count; i++){
            if(requirements[i].requireStr == q){
                requirements[i].requireNum --;
                Debug.Log("one goblin is killed and erased, now " + requirements[i].requireNum + " left");
            }
            if(requirements[i].isFinished()){
                requirements.RemoveAt(i);
                i--;
            }
        }
    }

    public bool missionComplete(){
        return requirements.Count <= 0;
    }
}


