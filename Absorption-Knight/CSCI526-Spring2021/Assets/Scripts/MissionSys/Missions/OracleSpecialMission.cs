using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OracleSpecialMission : MonoBehaviour
{
    public Dialogue sp;
    private DialogueTrigger thisDt;
    void Start()
    {
        thisDt = GameObject.Find("Oracle").GetComponent<DialogueTrigger>();
        //if the mission is not taken yet
        if(MissionManager.getInstance().findMissionFinished(thisDt.thisMission.GetComponent<IMission>().getMissionName())){
            return;
        }
        if(!MissionManager.getInstance().finMission(thisDt.thisMission.GetComponent<IMission>().getMissionName())){
            thisDt.earlyFinish();
        }
    }
}
