using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Teodoro : MonoBehaviour
{
    [SerializeField] private float m_speed = 5.0f;
    [SerializeField] private float m_jumpForce = 10.0f;

    protected Joystick joystick;

    private CapsuleCollider2D m_collider;
    private Animator m_animator;
    private Rigidbody2D m_rb;
    private GroundChecker m_groundChecker;
    private Sensor_CatKnight_Wall m_leftWallChecker;
    private Sensor_CatKnight_Wall m_rightWallChecker;
    private Transform m_bulletSpawnPos;
    private GameObject climbPack;
    private GameObject btnPack;
    private GameObject APBtn;
    [HideInInspector]
    public Slider music;
    [HideInInspector]
    public Slider soundeffect;
    

    private bool m_isGround = false;

    private bool m_doubleJump = false;

    private string objTag; //Object's Tag
    private bool endable = false;
    private bool teleportable = false;   //To determine whether teleporter can be used
    private bool climbable = false;      //To determine whether ladder can be used
    private bool talkable = false;       //To determine whether Teodoro can talk to NPC
    private int talk_count = 0;
    private bool purchasable = false;    //To determine whether Teodoro can purchase items
    private bool isOnLadder = false;
    // private bool buttonHold = false;

    //private int  m_facingDirection = 1;
    private float m_delayToIdle = 0.0f;

    private PlayerUnit m_playerUnit;
    private LevelTransitions leveltrans;

    private GameObject[] npcs = null;
    private GameObject closestNpc;
    private DialogueTrigger dtrigger;
    private GameObject dialoguebox;
    private string lastNPC;  // The last npc whom you talked to
    private Animator dialogueAnim;

    private List<GameObject> itemsCollection;
    private string lastItem;
    private Vector3 distance =Vector3.zero;

    /// <summary>
    /// HP & Sanity & EB
    /// </summary>
    private GameObject hpslider;
    private GameObject sanslider;
    private Slider hpslider_slider;
    private Slider sanslider_slider;
    private Text hpslider_text;
    private Text sanslider_text;
    private float currHP;
    private float currSan;
    private float maxHP;
    private float maxSan;
    private Text currEB;


    private GameObject prevArrow = null;
    private GameObject closestArrow;
    //Only for village
    private GameObject[] Arrows;
    
    private GameObject end;
    
    private void Awake()
    {
        m_playerUnit = GetComponent<PlayerUnit>();
        m_collider = GetComponent<CapsuleCollider2D>();
        leveltrans = GameObject.Find("Canvas").transform.GetChild(15).gameObject.GetComponent<LevelTransitions>();
        hpslider = GameObject.Find("Canvas").transform.GetChild(2).gameObject;
        sanslider = GameObject.Find("Canvas").transform.GetChild(3).gameObject;
        currEB = GameObject.Find("Canvas").transform.GetChild(5).gameObject.transform.GetChild(1).GetComponent<Text>();
        APBtn =GameObject.Find("Canvas").transform.GetChild(12).gameObject;
        music = GameObject.Find("Canvas").transform.GetChild(9).transform.GetChild(2).GetComponent<Slider>();
        soundeffect = GameObject.Find("Canvas").transform.GetChild(9).transform.GetChild(3).GetComponent<Slider>();
        hpslider_slider = hpslider.GetComponent<Slider>();
        sanslider_slider = sanslider.GetComponent<Slider>();
        hpslider_text = hpslider.transform.GetChild(3).GetComponent<Text>();
        sanslider_text = sanslider.transform.GetChild(3).GetComponent<Text>();
        
        end = GameObject.Find("Canvas").transform.GetChild(17).gameObject;
        
        if (GameObject.Find("ItemsManager") != null)
        {
            itemsCollection = GameObject.Find("ItemsManager").GetComponent<ItemManager>().items;
        }

        if (GameObject.Find("ArrowManager") != null)
        {
            Arrows = GameObject.Find("ArrowManager").GetComponent<ArrowManager>().Arrows;
        }
        
        if (GameObject.Find("DialogueManager") != null)
        {
            npcs = GameObject.Find("DialogueManager").GetComponent<NpcContainer>().NPCs;
            dialoguebox = GameObject.Find("DialogueUIPanel_Blue");

        }
        
    }

    // Start is called before the first frame update
    void Start()
    {
        m_animator = GetComponent<Animator>();
        m_rb = GetComponent<Rigidbody2D>();
        m_groundChecker = transform.Find("GroundChecker").GetComponent<GroundChecker>();
        m_leftWallChecker = transform.Find("WallChecker_L").GetComponent<Sensor_CatKnight_Wall>();
        m_rightWallChecker = transform.Find("WallChecker_R").GetComponent<Sensor_CatKnight_Wall>();
        m_bulletSpawnPos = transform.Find("FirePoint").transform;
        joystick = FindObjectOfType<Joystick>();
        btnPack = GameObject.Find("Canvas").transform.GetChild(1).gameObject;
        btnPack.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(jumpBtn);
        btnPack.transform.GetChild(2).GetComponent<Button>().onClick.AddListener(atkBtn);
        btnPack.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(skillBtn);
        APBtn.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(prayBtn);
        APBtn.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(absorbBtn);
        m_speed = m_playerUnit.GetTeodoroSpeed();
        m_jumpForce = m_playerUnit.GetTeodoroJumpForce();
        if (SceneManager.GetActiveScene().name == "Level-1" || SceneManager.GetActiveScene().name == "LV-1Store" ||SceneManager.GetActiveScene().name == "LV-1BossRoom" )
        {
            climbPack = GameObject.Find("Canvas").transform.GetChild(4).gameObject;
            climbPack.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(climbUpBtn);
            climbPack.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(climbDownBtn);
        }

        if (m_playerUnit.prevScene == "LV-1Store" || m_playerUnit.prevScene == "LV-2Store-1" ||
            m_playerUnit.prevScene == "LV-2Store-2" || m_playerUnit.prevScene == "LV-2Store-3")
        {
        }
        else
        {
            switch (SceneManager.GetActiveScene().name)
            {
                case "Level-0":
                    FindObjectOfType<MusicManager>().Play("LV0");
                    break;
                case "Tutorial":
                    FindObjectOfType<MusicManager>().Play("Tutorial");
                    break;
                case "Level-1":
                    FindObjectOfType<MusicManager>().Play("Lv1");
                    break;
                case "Level-2":
                    FindObjectOfType<MusicManager>().Play("Lv2");
                    break;
                case "Level-2-village":
                    FindObjectOfType<MusicManager>().Play("Village");
                    break;
                case "Level-3":
                    FindObjectOfType<MusicManager>().Play("Lv3");
                    break;
                case "LV-1BossRoom":
                    FindObjectOfType<MusicManager>().Play("Boss");
                    break;
                case "LV-2BossRoom":
                    FindObjectOfType<MusicManager>().Play("Boss2");
                    break;
                case "Level-4":
                    FindObjectOfType<MusicManager>().Play("Lv4");
                    break;
                case "LV-5":
                    FindObjectOfType<MusicManager>().Play("Lv5");
                    break;   
            }   
        }
    }

    private void FixedUpdate()
    {
        if (isOnLadder)
        {
            m_rb.gravityScale = 0;
            m_collider.enabled = false;

        }
        else
        {
            m_rb.gravityScale = 1;
            m_collider.enabled = true;
        }
    }


    // Update is called once per frame
    void Update()
    {

        currHP = m_playerUnit.HP();
        currSan = m_playerUnit.Sanity();
        maxHP = m_playerUnit.MaxHP();
        maxSan = m_playerUnit.MaxSan();
        hpslider_slider.value = currHP;
        sanslider_slider.value = currSan;
        hpslider_slider.maxValue = maxHP;
        sanslider_slider.maxValue = maxSan;
        hpslider_text.text = currHP + "%";
        sanslider_text.text = currSan + "%";
        currEB.text = m_playerUnit.EB().ToString();




        if (!m_isGround && m_groundChecker.State())
        {
            m_isGround = true;
            m_animator.SetBool("isground", m_isGround);
        }

        if (m_isGround && !m_groundChecker.State())
        {
            m_isGround = false;
            m_animator.SetBool("isground", m_isGround);
        }

        float inputX = joystick.Horizontal; //Joystick Input

        //Keyboard control
        float hor = Input.GetAxis("Horizontal");
        if (Mathf.Abs(hor) > Mathf.Abs(inputX))
        {
            inputX = hor;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            jumpBtn();
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            atkBtn();
        }


        if (inputX > 0)
        {
            transform.localScale = new Vector3(1.3f, 1.3f, 1.3f);
            m_rightWallChecker.transform.localScale = new Vector3(1f, 1f, 1f);
            m_leftWallChecker.transform.localScale = new Vector3(1f, 1f, 1f);
            m_bulletSpawnPos.localScale= new Vector3(1f, 1f, 1f);
            
        }

        else if (inputX < 0)
        {
            transform.localScale = new Vector3(-1.3f, 1.3f, 1.3f);
            m_rightWallChecker.transform.localScale = new Vector3(-1f, 1f, 1f);
            m_leftWallChecker.transform.localScale = new Vector3(-1f, 1f, 1f);
            m_bulletSpawnPos.localScale= new Vector3(-1f, 1f, 1f);
        }

        //RUN
        if (!m_playerUnit.IsDuringHitback())
        {
            if (inputX > 0 && m_rightWallChecker.State())
            {
                m_rb.velocity = new Vector2(inputX * m_speed, m_rb.velocity.y);
            }
            else if (inputX < 0 && m_leftWallChecker.State())
            {
                m_rb.velocity = new Vector2(inputX * m_speed, m_rb.velocity.y);
            }
            else
            {
                m_animator.SetInteger("AnimState", 1);
                m_rb.velocity = new Vector2(0, m_rb.velocity.y);
            }
        }


        if (Mathf.Abs(inputX) > Mathf.Epsilon)
        {
            // Reset timer
            m_delayToIdle = 0.005f;
            m_animator.SetInteger("AnimState", 1);
        }
        else
        {
            // Prevents flickering transitions to idle
            m_delayToIdle -= Time.deltaTime;
            if (m_delayToIdle < 0)
                m_animator.SetInteger("AnimState", 0);
        }

        // Dialogue Related
        if (npcs != null && npcs.Length > 0)
        {
            // Approaches to an NPC close enough --> Communication can be started
            closestNpc = GetClosestNPC();
            distance = closestNpc.transform.position - transform.position;
            dtrigger = closestNpc.GetComponent<DialogueTrigger>();
            if (distance.sqrMagnitude <= 1f)
            {
                talkable = true;
                if (closestNpc.name != lastNPC)
                {
                    talk_count = 0;
                    m_playerUnit.ClearDialogueText();
                }
            }
            else
            {
                talkable = false;
                talk_count = 0;
                dialoguebox.transform.localScale = new Vector3(0f, 0f, 0f);
                if (dtrigger.walk)
                {
                    closestNpc.GetComponent<Viliagers>().Wandering();

                }

                m_playerUnit.ClearDialogueText();
            }
        }

        // Only For Level-2 Village ---- Arrow Sign Detection
        if (Arrows != null && Arrows.Length > 0)
        {
            closestArrow = GetClosestArrow();
            if (prevArrow != null)
            {
                if (closestArrow != prevArrow)
                {
                    prevArrow.SetActive(false);
                }
            }
            if (!closestArrow.activeSelf)
            {
                closestArrow.SetActive(true);
                prevArrow = closestArrow;
            }
        }

    }
    
    //JUMP BUTTON
    public void jumpBtn()
    {
        if (m_isGround)
        {
            m_animator.SetTrigger("jump");
            m_isGround = false;
            m_doubleJump = true;
            m_animator.SetBool("isground", m_isGround);
            m_rb.velocity = new Vector2(m_rb.velocity.x, m_jumpForce);
            SoundManager.PlaySound("jumpSound");
            m_groundChecker.Disable(0.1f);
        
        }
        // Double Jump
        else if (!m_isGround)
        {
            if (m_doubleJump)
            {
                m_animator.SetTrigger("doublejump");
                m_rb.velocity = new Vector2(m_rb.velocity.x, m_jumpForce);
                SoundManager.PlaySound("jumpSound");
                m_doubleJump = false;
            }
        }
    }

    // BASIC ATTACK AND INTERACTION BUTTON
    // IF MEET NPC ---> INTERACTION
    // OTHERWISE, ATTACK
    public void atkBtn()
    {
        // Basic Attack & Interact
        // Teleporter & NPCs ---> Interactable
        if (teleportable)
        {
            m_playerUnit.LoadLevel();
        }
        //If the number of talk is more than sentences count, reset
        else if (talkable)
        {
            if (dtrigger.walk)
            {
                closestNpc.GetComponent<Viliagers>().StopWandering();
            }

            dialoguebox.transform.localScale = new Vector3(1f, 1f, 1f);
            lastNPC = closestNpc.name;

            if (talk_count > m_playerUnit.GetTotalSentenceCount())
            {
                dialoguebox.transform.localScale = new Vector3(0f, 0f, 0f);
                m_playerUnit.ClearDialogueText();
                talk_count = -1;
            }

            if (talk_count == 0)
            {
                dtrigger.TriggerDialogue();
            }
            else if (talk_count > 0)
            {
                m_playerUnit.DisplayNext();
            }

            talk_count++;
        }
        else if (purchasable)
        {

            if (m_playerUnit.Purchase())
            {
                itemsCollection.ElementAt(CheckIndex(lastItem)).GetComponent<Item>().isPurchased = true;
            }
        }
        else if (endable)
        {
            end.SetActive(true);    
        }
        else

        {
            m_playerUnit.Attack();
        }
    }


    // SKILL BUTTON AND ABSORB BUTTON
    public void skillBtn()
    {
        // If the number of skill usage is large or equal to 1, then execute skill btn action.... 
        if (m_playerUnit.GetSkillCount() > 0)
        {
            // m_playerUnit.Shoot();
            // m_playerUnit.Bomb();
            // m_playerUnit.HPSteal();
            m_playerUnit.SkillSelector();
        }
    }



    public void absorbBtn()
    {
        m_playerUnit.Absorb();
    }

     public void prayBtn()
    {
        m_playerUnit.Pray();
    }
    
    public void climbUpBtn()
    {
        isOnLadder = true;
        m_animator.SetBool("climb",true);
        transform.position = new Vector2(transform.position.x, transform.position.y + m_playerUnit.ClimbSpeed());

    }
    
    public void climbDownBtn()
    {   
        isOnLadder = true;
        m_animator.SetBool("climb",true);
        // m_rb.velocity = new Vector2(m_rb.velocity.x,  -m_playerUnit.ClimbSpeed());
        transform.position = new Vector2(transform.position.x, transform.position.y + m_playerUnit.ClimbSpeed());

    }

 

    private void OnTriggerEnter2D(Collider2D other)
    {
        //Special Case for water in Level-1
        if (other.gameObject.tag == "teleport" )
        {
            GameManager.prevScene = SceneManager.GetActiveScene().name;
            objTag = "teleport";
            leveltrans.levelName = other.name;
            teleportable = true;
            if (other.name == "Level-2-village" && SceneManager.GetActiveScene().name == "Level-1")
            {
                m_playerUnit.LoadLevel();
            }

            if (other.name == "Level-1" && SceneManager.GetActiveScene().name == "Level-2-village")
            {
                m_playerUnit.LoadLevel();
            }
            if (other.name == "Level-2" && SceneManager.GetActiveScene().name=="Level-3")
            {
                m_playerUnit.LoadLevel();
            }

            if (other.name == "Level-3" && SceneManager.GetActiveScene().name == "LV-2TransitionToLV3")
            {
                m_playerUnit.LoadLevel();
            }

            if (other.name == "Level-4" && SceneManager.GetActiveScene().name == "Level-3")
            {
                m_playerUnit.LoadLevel();
            }
            
        }
        else
        {
            teleportable = false;
        }

        if (other.gameObject.tag == "ladder")
        {
            climbable = true;
            btnPack.SetActive(false);
            climbPack.SetActive(true);
        } 
        
    

        if (other.gameObject.tag == "purchasable")
        {
            purchasable = true;
            m_playerUnit.SetItemValueChange(GetItemInfo(other.gameObject));
        }

        if (other.gameObject.tag == "GameOver")
        {
            m_playerUnit.forceGameOver();
        }

        if (other.gameObject.tag == "End")
        {
            endable = true;
        }

    }

    private void OnTriggerStay2D(Collider2D other)
    {    
        if (other.gameObject.tag == "ladder")
        {
            isOnLadder = true;

        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        climbable = false;
        m_animator.SetBool("climb",false);
        isOnLadder = false;
        teleportable = false;
        purchasable = false;
        endable = false;
        lastItem = String.Empty;
        m_playerUnit.ResetValueChange();
        objTag = " ";

        if (SceneManager.GetActiveScene().name == "Level-1")
        {
            btnPack.SetActive(true);
            if (climbPack != null)
            {
                climbPack.SetActive(false);
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        // level-2 moving wood on water
        if (collision.gameObject.tag == "MovePlatform")
        {
            transform.position = new Vector2(collision.gameObject.GetComponent<ObjectMoveController>().CopySpeed *Time.deltaTime + transform.position.x, transform.position.y);
        }
        
        if (collision.gameObject.tag == "Enemy")
        {
            if (collision.gameObject.GetComponent<EnemyDeath>() != null && collision.gameObject.GetComponent<EnemyDeath>().isDeath)
            {
                btnPack.SetActive(false);
                APBtn.SetActive(true);   
            }
        }
    }    
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            btnPack.SetActive(true);
            APBtn.SetActive(false);   
        }
    }
    


    private GameObject GetClosestNPC()
    {
        GameObject betTarget = null;
        float closestDistance = Mathf.Infinity;
        foreach (GameObject npc in npcs)
        {
            Vector3 directionToTarget = npc.transform.position - transform.position;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if (dSqrToTarget < closestDistance)
            {
                closestDistance = dSqrToTarget;
                betTarget = npc;
            }
        }
        return betTarget;
    }
    
    private GameObject GetClosestArrow()
    {
        GameObject betTarget = null;
        float closestDistance = Mathf.Infinity;
        foreach (GameObject arrow in Arrows)
        {
            Vector3 directionToTarget = arrow.transform.position - transform.position;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if (dSqrToTarget < closestDistance)
            {
                closestDistance = dSqrToTarget;
                betTarget = arrow;
            }
        }
        //Debug.Log(betTarget.name);
        return betTarget;
    }
    
    // IMPORTANT FUNCTION FOR ITEMS
    private Tuple<Tuple<int,int>,string> GetItemInfo(GameObject item)
    {
        Item temp = item.GetComponent<Item>();
        string flag = temp.ItemsType.ToString();
        lastItem = item.name;
        int result = 0;
        int cost = 0;
        switch (flag)
        {
            case "HpPortion":
                result = temp.hpchange;
                cost = temp.cost;

                break;
            case "SanPortion":
                result = temp.sanchange;
                cost = temp.cost;
                break;
            case "Weapon":
                result = temp.ATKchange;
                cost = temp.cost;
                break;
            case "Armor":
                result = temp.DEFchange;
                cost = temp.cost;
                break;
            case "Special":
                result = temp.NumOfSkillchange;
                cost = temp.cost;
                break;
            case "EnergyBall":
                result = temp.EBReward;
                cost = temp.cost;
                break;
        }
        return Tuple.Create(Tuple.Create(result,cost*-1),flag);
    }

    public int CheckIndex(string temp)
    {
        int count = 0;
        foreach (GameObject item in itemsCollection)
        {
            if (temp == item.name)
            {
                return count;
            }

            count++;
        }

        return count;
    }

    public void AddSpeed(int val)
    {
        m_speed += val;
    }

    public void AddJumpForce(int val)
    {
        m_jumpForce += val;
    }

    public float GetSpeed()
    {
        return m_speed;
    }

    public float GetJumpForce()
    {
        return m_jumpForce;
    }
}
