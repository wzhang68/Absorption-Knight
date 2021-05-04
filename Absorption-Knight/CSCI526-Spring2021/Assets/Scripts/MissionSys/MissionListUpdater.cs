using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

//attach this script to missionpanel
public class MissionListUpdater : MonoBehaviour
{
    // public GameObject childContext;
    
    //下面这些是添加的 原版code只有上面一个
    public GameObject mission;
    public GameObject missionlist;
    private Dictionary<string, IMission> todo;
    private Dictionary<string, IMission> clear;
    List<string> instantiated_todo= new List<string>();
    List<string> unique2= new List<string>();
    
    void Update()
    {
        if(this.isActiveAndEnabled == true){
            todo = MissionManager.getInstance().getTodo();
            // clear = MissionManager.getInstance().getComplete();
            // updateMissionArea();
            test1(todo);
            

        }        
    }

    // void updateMissionArea(){
    //     childContext.GetComponent<Text>().text = MissionManager.getInstance().checkToDoList();
    // }

    
    // 临时的function，用于对mission prefab instantiation的测试
    // 记得看一下 看要在IMission和 MissionManager里面加什么东西
    // 任务状态的判断不太对
    void test1(Dictionary<string, IMission> todo)
    {

        if (todo.Count > 0)
        {
            foreach (KeyValuePair<string, IMission> m in todo)
            {
                if (!instantiated_todo.Contains(m.Key))
                {
                    Debug.Log(m.Key);
                    string missionContent = String.Empty;
                    string reward = "Reward: ";
                    switch (m.Key)
                    {
                        case "home sweet home":
                            missionContent = "Kill 2 Goblins";
                            reward += m.Value.reward;
                            break;
                        case "the hidden path":
                            missionContent = "Find the Hidden Path";
                            reward += m.Value.reward;
                            break;
                        case "cat knight":
                            missionContent = "Defeat a Cat Knight";
                            reward += m.Value.reward;
                            break;
                        case "the oracle":
                            missionContent = "Talk to Oracle";
                            reward += m.Value.reward;
                            break;
                        case "road to food":
                            missionContent = "Defeat LV2 Boss";
                            reward += m.Value.reward;
                            break;
                    }
                    mission.transform.GetChild(0).GetComponent<Text>().text = missionContent;
                    mission.transform.GetChild(1).GetComponent<Text>().text = reward; 
                    mission.transform.GetChild(2).GetComponent<Text>().text = "OnGoing";
                    mission.gameObject.name = m.Key;
                    Instantiate(mission, Vector3.zero, Quaternion.identity, missionlist.transform);
                    instantiated_todo.Add(m.Key);
                }
                else
                {
                    missionlist.transform.Find(m.Key+"(Clone)").gameObject.transform.GetChild(2).GetComponent<Text>().text = m.Value.missionComplete()?"Clear":"OnGoing";
                }

            }
        }
        // if (clear.Count > 0)
        // {
        //     foreach (KeyValuePair<string, IMission> m in clear)
        //     {
        //         Debug.Log("Completed  "+m.Key);
        //         if (instantiated_todo.Contains(m.Key))
        //         {
        //             transform.Find(m.Key+"(Clone)").gameObject.transform.GetChild(2).GetComponent<Text>().text = "Clear";
        //             // unique2.Add(m.Key);
        //             // instantiated_todo.Remove(m.Key);
        //         }
        //     }
        // }
    }


}
