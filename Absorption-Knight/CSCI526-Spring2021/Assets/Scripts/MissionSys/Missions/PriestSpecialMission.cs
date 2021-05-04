using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PriestSpecialMission : MonoBehaviour
{
    public Dialogue sp;
    private GameObject thisPlayer;
    private DialogueTrigger thisDt;
    // Start is called before the first frame update
    void Start()
    {
        if (thisPlayer == null)
        {
            thisPlayer = GameObject.Find("Teodoro_ZLVer");
        }
        thisDt = GameObject.Find("Priest").GetComponent<DialogueTrigger>();
        //he is from the hole of level1
        if (GameManager.prevScene == "Level-1")
        {
            //update mission status first
            MissionManager.getInstance().updateAllMissionStates("findpath");
            thisDt.dialogueOfTakingQuest = sp;
            thisDt.earlyFinish();
        }
    }
}
