using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public bool walk;
    public Dialogue dialogueOfNormalTalking;
    public Dialogue dialogueOfTakingQuest;
    public Dialogue dialogueOfUrging;
    public Dialogue dialogueOfThanking;
    public GameObject thisMission;
    private bool thisMissionTaken = false;
    private bool thisMissionFinished = false;
    private bool finishInAdvance = false;
    public GameObject reward;
    
    public void TriggerDialogue()
    {
        //如果从外部调用得到该任务需要被做完，调用这个脚本一次
        if(finishInAdvance){
            Dia(dialogueOfTakingQuest);
            MissionManager.getInstance().takeMissionAndFinish(thisMission.GetComponent<IMission>());
            finishInAdvance = false;
            if(reward != null) reward.SetActive(true);
            return;
        }

        //更新任务是否已经被完成
        if(thisMission != null){
            thisMissionFinished = MissionManager.getInstance().findMissionFinished(thisMission.GetComponent<IMission>().getMissionName());
        }

        //如果没有任务或任务已经做完，使用普通谈话
        if(thisMission == null || thisMissionFinished){
            Dia(dialogueOfNormalTalking);
        } else {
            //如果有任务
            //更新任务能否接受状态
            thisMissionTaken = MissionManager.getInstance().takeMission(thisMission.GetComponent<IMission>());
            //如果能接受，即接受，并且使用接任务对话
            if(thisMissionTaken){
                Dia(dialogueOfTakingQuest);
            } else {
                //如果不能接受，有两种可能，已接受未完成或已完成
                //如果已经可以完成且完成了，使用完成对话并激活奖励
                if(this.gameObject.name == "Oracle") MissionManager.getInstance().updateAllMissionStates("talktooracle");
                if(MissionManager.getInstance().finMission(thisMission.GetComponent<IMission>().getMissionName())){
                    Dia(dialogueOfThanking);
                    thisMissionFinished = true;
                    if(reward != null) reward.SetActive(true);
                } else {
                    //否则使用催促对话
                    Dia(dialogueOfUrging);
                }
            }
        }
        
    }

    public void earlyFinish(){
        finishInAdvance = true;
    }

    private void Dia(Dialogue t){
        FindObjectOfType<PlayerUnit>().StartDialogue(t);
    }
    
    
}
