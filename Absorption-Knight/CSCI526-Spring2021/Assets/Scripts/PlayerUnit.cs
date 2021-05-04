using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
// using UnityEditor.iOS;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;
using Slider = UnityEngine.UI.Slider;

public class PlayerUnit : Unit
{

    private GameObject transImg;
    private Animator transAnim;

    [Header("PlayerUnit Properties")]
    [SerializeField] private float m_hitBackSpeed = 2.0f;

    [SerializeField] private float m_selfHitBackSpeed = 1.0f;
    [SerializeField] private bool m_duringHitback = false;
    [SerializeField] private float m_timeSinceHitback = 0.0f;

    [Header("Player Attack Properties")] [SerializeField]
    private UnitAttackZone m_attackZone1 = null;

    [SerializeField] private int m_attack_1_Damage = 0;
    [SerializeField] private float m_atk_1_Delay = 0.5f;
    [SerializeField] private float m_atk_1_Cooldown = 1.5f;
    [SerializeField] private float m_atk_1_TriggerEnableDelay = 0.1f;
    [SerializeField] private float m_atk_1_DestoryDelay = 1f;

    [Header("Player DEF Properties")] [SerializeField]
    private int m_def = 0;

    [Header("Player Absorb/Pray Properties")] [SerializeField]
    private float m_AP_Delay = 0.5f;

    [SerializeField] private float m_AP_Cooldown = 1.5f;
    [SerializeField] private float m_AP_TriggerEnableDelay = 0.1f;
    [SerializeField] private float m_AP_DestoryDelay = 1f;

    [Header("Player Climb Properties")] 
    [SerializeField] private float m_climb_speed = 2.0f;

    [Header("Player Skills")]
    [SerializeField] private float m_Skill_Delay = 0.5f;
    [SerializeField] private float m_Skill_Cooldown = 2f;
    [SerializeField] private GameObject skillcount_prefab;
    private int m_skillcount;

    [Header("Player Skill -- 1")] 
    [SerializeField] private int m_damagePerBullet = 10;
    [SerializeField] private int m_skillcount1 = 5;
    [SerializeField] private float m_bulletDestoryDelay = 1.0f;
    [SerializeField] private float m_bulletSpeed = 2.0f;
    [SerializeField] private Transform m_bulletSpawnPos = null;
    [SerializeField] private PlayerBullet m_bulletPrefab = null;

    [Header("Player Skill -- 2")]
    [SerializeField] private PlayerBomb m_bombPrefab = null;
    [SerializeField] private int m_skillcount2 = 2;
    [SerializeField] private int m_damagePerBomb = 50;
    [SerializeField] private float m_bombDestoryDelay = 1.0f;

    [Header("Player Debuffs")] 
    [SerializeField] private GameObject poisonPrefab = null;


    private TextWriter textwriter;

    protected Animator m_animator;
    protected Rigidbody2D m_rb2d;

    //Dialogue Related
    private Queue<string> sentences;
    private GameObject dialoguebox;
    private Text dialoguetext;
    private Camera camera;
    private int totalsentences;

    //Item Sys Related
    private int valuechange;
    private int itemcost;
    private int hp;
    private int san;
    private int eb;
    private string itemType = string.Empty;

    private float m_timeSinceAttack = 0.0f;

    //Same attribute for absorb and pray
    private float m_timeSinceAbsorb = 0.0f;
    private float m_timeSinceSkill = 0.0f;
    private ArrayList bufflist = new ArrayList();
    private GameObject buffgrid;
    private BuffManager buffmanager;
    private HashSet<string> uniquebuff = new HashSet<string>();
    private Teodoro teodoro;
    [HideInInspector] public string temp2;
    [HideInInspector] public string prevScene = String.Empty;

    [HideInInspector] public float musicVol;
    [HideInInspector] public float effectVol;

    // GameOver 
    private GameObject gameover;

    
    private Setting canvas_setting;
    private GameObject skillcountCanvas;
    private List<GameObject> skillcountContainer= new List<GameObject>();
    private float m_speed = 5f ;
    private float m_jumpforce = 7f;

    private void Awake()
    {
        m_rb2d = GetComponent<Rigidbody2D>();
        m_animator = GetComponent<Animator>();
        m_unitType = "player";
        buffgrid = GameObject.Find("Canvas").transform.GetChild(11).gameObject.transform.GetChild(0).gameObject;
        textwriter = FindObjectOfType<TextWriter>();
        transImg = GameObject.Find("Canvas").transform.GetChild(15).gameObject;
        transAnim = GameObject.Find("Canvas").transform.GetChild(15).GetComponent<Animator>();
        skillcountCanvas = GameObject.Find("Canvas").transform.GetChild(1).transform.GetChild(1).transform.GetChild(1).gameObject;

        //Dialogue Related
        if (GameObject.Find("DialogueUIPanel_Blue") != null)
        {
            sentences = new Queue<string>();
            dialoguebox = GameObject.Find("DialogueUIPanel_Blue");
            dialoguetext = dialoguebox.GetComponentInChildren<Text>();

        }

        if (GameObject.Find("BuffManager") != null)
        {
            buffmanager = GameObject.Find("BuffManager").GetComponent<BuffManager>();
        }

        teodoro = GetComponent<Teodoro>();
        gameover = GameObject.Find("Canvas").transform.GetChild(16).gameObject;

    }

    private void Update()
    {
        m_timeSinceAttack += Time.deltaTime;
        m_timeSinceAbsorb += Time.deltaTime;
        m_timeSinceSkill += Time.deltaTime;
        UpdateHitBackState();
        hp = m_currentHealth;
        san = m_currentSanity;
        eb = m_currentEB;
        // musicVol = teodoro.music.value;
        // effectVol = teodoro.soundeffect.value;
        

        if (m_currentHealth <= 0)
        {
            Destroy(this.gameObject);
            gameover.SetActive(true);

        }

        DebuffSelector();

        //BUFF ICONs
        if (GameObject.Find("BuffManager") != null)
        {
            if (bufflist.Count > 0)
            {
                foreach (string buffname in bufflist)
                {
                    if (!uniquebuff.Contains(buffname) && buffname != "")
                    {
                        Sprite img = buffmanager.buffs.Find(x => x.name == buffname);
                        GameObject temp = new GameObject();
                        temp.AddComponent<Image>().sprite = img;
                        temp.AddComponent<Buff>().buffName = buffname;
                        Instantiate(temp, Vector3.zero, Quaternion.identity);
                        temp.transform.SetParent(buffgrid.transform);
                        uniquebuff.Add(buffname);
                        BUFF(buffname);
                        // Add data into JSON
                        if (!temp2.Contains(buffname))
                        {
                            temp2 += buffname + ",";
                        }
                    }
                }
            }
        }
    }


    public void LoadLevel()
    {
        //Load to a Specific Scene
        StartCoroutine(LoadToLevel());
    }

    public void Attack()
    {
        if (m_timeSinceAttack > m_atk_1_Cooldown)
        {
            StartCoroutine(DoAttack());
            SoundManager.PlaySound("playerAttack");
        }
    }

    public bool Purchase()
    {
        if (CheckEnoughEB(itemcost))
        {
            StartCoroutine(UseItem(valuechange, itemcost));
            return true;
        }

        return false;
    }


    /// <summary>
    ///Climb Up & Down when Teodoro encounters ladders
    /// </summary>
    public void ClimbUp()
    {
        StartCoroutine(DoClimbUp());
    }

    public void ClimbDown()
    {
        StartCoroutine(DoClimbDown());
    }

    /// <summary>
    ///UNCOMPLETED --> ABSORB & PRAY
    /// </summary>
    public void Absorb()
    {
        if (m_timeSinceAbsorb > m_AP_Cooldown)
        {
            StartCoroutine(DoAbsorb());
        }

    }

    //------------------------------The Number of Skill Usage-------------------------------------
    /// <summary>
    /// Create the number of skill usage into the horizontal layout group in Canvas
    /// Current default value is 2 --->  Haven't connected to absorb btn completely.
    /// This method is called by OnAbsorbZoneHitUnit in PlayerUnit.cs
    /// </summary>
    /// <param name="val"></param>
    private void CreateSkillCount(int val)
    {
        skillcountContainer.Clear();
        for (int i = 0; i < val; i++)
        {
            GameObject temp = Instantiate(skillcount_prefab, skillcountCanvas.transform.position, Quaternion.identity,
                skillcountCanvas.transform);
            skillcountContainer.Add(temp);
        }
    }

    /// <summary>
    /// If the player use skill once, the number of skill usage count will be decreased by one
    /// </summary>
    public void RemoveSkillCount()
    {
        if (skillcountContainer.Count > 0)
        {
            Destroy(skillcountContainer[0]);
            skillcountContainer.RemoveAt(0);
            m_skillcount--;
            if (m_skillcount == 0)
            {
                m_activeSkillType = String.Empty;
            }
        }
    }

    public int GetSkillCount()
    {
        return m_skillcount;
    }
    //------------------------------The Number of Skill Usage (End)-------------------------------------


    public void Pray()
    {
        if (m_timeSinceAbsorb > m_AP_Cooldown)
        {
            StartCoroutine(DoPray());
        }
    }

    // Attempting to use the same attack zone as a trigger
    // Need New Instantiate method and Status Tag in Unit.cs
    private IEnumerator DoAbsorb()
    {
        m_timeSinceAbsorb = 0.0f;
        yield return new WaitForSeconds(m_AP_Delay);
        if (m_graphics.GetComponent<SpriteRenderer>().flipX == true)
        {
            m_rb2d.velocity = new Vector2(0.1f, m_rb2d.velocity.y);
        }
        else
        {
            m_rb2d.velocity = new Vector2(-0.1f, m_rb2d.velocity.y);
        }

        m_animator.SetTrigger("absorb");
        m_animator.SetTrigger("absorb_effect");
        UnitAttackZone attackZone = Instantiate(m_attackZone1, transform, false);
        attackZone.InitializeAbsorbZone(this, m_AP_TriggerEnableDelay, m_AP_DestoryDelay);
    }

    private IEnumerator DoPray()
    {
        m_timeSinceAbsorb = 0.0f;
        yield return new WaitForSeconds(m_AP_Delay);

        if (m_graphics.GetComponent<SpriteRenderer>().flipX == true)
        {
            m_rb2d.velocity = new Vector2(0.1f, m_rb2d.velocity.y);
        }
        else
        {
            m_rb2d.velocity = new Vector2(-0.1f, m_rb2d.velocity.y);
        }

        m_animator.SetTrigger("absorb");
        m_animator.SetTrigger("pray");
        UnitAttackZone attackZone = Instantiate(m_attackZone1, transform, false);
        attackZone.InitializePrayZone(this, m_AP_TriggerEnableDelay, m_AP_DestoryDelay);

    }

    /// <summary>
    ///Naturally Load to Other Level
    /// </summary>
    private IEnumerator LoadToLevel()
    {
        transImg.SetActive(true);
        transAnim.SetBool("Fade", true);
        yield return new WaitUntil(() => Color.black.a == 1);
        SavePlayer();

    }

    /// <summary>
    ///Climb Up & Down when Teodoro encounters ladders
    /// </summary>
    private IEnumerator DoClimbUp()
    {
        m_animator.SetBool("climb", true);
        // m_rb2d.gravityScale = 0;
        m_rb2d.velocity = new Vector2(0f, Time.deltaTime * m_climb_speed);
        // m_rb2d.velocity = new Vector2(0f,Time.deltaTime * m_climb_speed);
        yield return null;
    }

    private IEnumerator DoClimbDown()
    {
        m_animator.SetBool("climb", true);
        // m_rb2d.gravityScale = 0;
        m_rb2d.velocity = new Vector2(0f, Time.deltaTime * -m_climb_speed);
        // m_rb2d.velocity = new Vector2(0f,Time.deltaTime * -m_climb_speed);
        yield return null;
    }

    private IEnumerator UseItem(int val, int cost)
    {
        m_animator.SetTrigger("acquire");
            switch (itemType)
            {
                case "HpPortion":
                    AddHp(val);
                    break;
                case "SanPortion":
                    AddSan(val);
                    break;
                case "Weapon":
                    AddATK(val);
                    break;
                case "Armor":
                    AddDEF(val);
                    break;
                case "EnergyBall":
                    AddEB(val);
                    Debug.Log("GET EB REWARD");
                    break;
                case "Special":
                    if (m_activeSkillType != String.Empty)
                    {
                        m_skillcount += val;
                        for (int i = 0; i < val; i++)
                        {
                            GameObject temp = Instantiate(skillcount_prefab, skillcountCanvas.transform.position,
                                Quaternion.identity,
                                skillcountCanvas.transform);
                            skillcountContainer.Add(temp);
                        }
                    }
                    break;
            }

            //Under Debuff Situation
            if (m_currentSanity < 70)
            {
                AddEB((int)Math.Floor(cost*1.25));
            }
            else
            {
                AddEB(cost);   
            }
            yield return true;
    }

    /// <summary>
    /// Check if you have enough Energy Balls
    /// </summary>
    /// <param name="cost"></param>
    /// <returns></returns>
    public bool CheckEnoughEB(int cost)
    {
        return (m_currentEB) >= (Math.Abs(cost));
    }

    private IEnumerator DestroyAfterSeconds(GameObject gameObject)
    {
        yield return new WaitForSeconds(1.8f);
        Destroy(gameObject);

    }


    private IEnumerator DoAttack()
    {
        m_timeSinceAttack = 0.0f;
        yield return new WaitForSeconds(m_atk_1_Delay);

        if (m_graphics.GetComponent<SpriteRenderer>().flipX == true)
        {
            m_rb2d.velocity = new Vector2(0.1f, m_rb2d.velocity.y);
        }
        else
        {
            m_rb2d.velocity = new Vector2(-0.1f, m_rb2d.velocity.y);
        }

        m_animator.SetTrigger("attack");
        UnitAttackZone attackZone = Instantiate(m_attackZone1, transform, false);
        //attackZone.transform.localScale = new Vector3(1f, 1f, 1f);
        attackZone.InitializeAttackZone(this, m_attack_1_Damage, m_atk_1_TriggerEnableDelay, m_atk_1_DestoryDelay);
    }

    protected override void OnHitBack(GameObject source, GameObject attacker, Vector2 dir, float hitBackSpeed)
    {
        dir.Normalize();
        if (Vector2.Dot(dir, m_rb2d.velocity) >= 0)
        {
            m_rb2d.velocity = m_rb2d.velocity + dir * hitBackSpeed;
        }
        else
        {
            m_rb2d.velocity = dir * hitBackSpeed;
        }

        m_duringHitback = true;
        m_timeSinceHitback = 0.0f;
        SoundManager.PlaySound("playerGetHit");
    }

    public override void OnAttackZoneHitUnit(Unit hitUnit, GameObject source, int damage)
    {
        Hit(hitUnit, source, damage);
    }

    public override void OnAbsorbZoneHitUnit(Unit hitUnit, GameObject source)
    {
        if (hitUnit.GetCurrentHP() <= 0)
        {
            AddEB(hitUnit.GetEBAfterKill());
            AddSan(hitUnit.GetSanAfterKill());
            m_skillType = hitUnit.GetSkill();
            m_activeSkillType = hitUnit.GetActiveSkill();
            SkillCountSelector();
            if (!bufflist.Contains(m_skillType))
            {
                bufflist.Add(m_skillType);
            }

            Debug.Log("Get BUFF: " + m_skillType);
            Debug.Log("Get Skill: " + m_activeSkillType);
            StartCoroutine(DestroyAfterSeconds(hitUnit.gameObject));
            
            //Set skill count from 1 to as many as you want
            if (m_activeSkillType != String.Empty)
            {
                CreateSkillCount(m_skillcount);   
            }

        }
    }

    public override void OnPrayZoneHitUnit(Unit hitUnit, GameObject source)
    {
        if (hitUnit.GetCurrentHP() <= 0)
        {
            AddSan(5);
            Debug.Log("Recovered Sanity:  5");
            StartCoroutine(DestroyAfterSeconds(hitUnit.gameObject));

        }
    }

    private void Hit(Unit hitUnit, GameObject source, int damage)
    {
        //self hit back
        Vector2 hitBackDir;
        if (transform.position.x >= hitUnit.transform.position.x)
        {
            hitBackDir = new Vector2(-1f, 0f);
        }
        else
        {
            hitBackDir = new Vector2(1f, 0f);
        }

        m_rb2d.velocity = -hitBackDir * m_selfHitBackSpeed;

        hitUnit.OnHit(this.gameObject, source, damage, hitBackDir, m_hitBackSpeed);
    }

    private void UpdateHitBackState()
    {
        if (m_duringHitback)
        {
            m_timeSinceHitback += Time.deltaTime;
            if (m_timeSinceHitback > 2.0f || Mathf.Abs(m_rb2d.velocity.x) <= 0.01f)
            {
                m_duringHitback = false;
            }
        }
    }

    public bool IsDuringHitback()
    {
        return m_duringHitback;
    }

    /// <summary>
    /// NPC聊天
    /// 不同的NPC音效 暂时这样 之后修改
    /// </summary>
    /// <returns></returns>
    public void StartDialogue(Dialogue dialogue)
    {
        Debug.Log("Starting conversation with " + dialogue.npcName);
        sentences.Clear();

        switch (dialogue.npcName)
        {
            case "Bard":
                SoundManager.PlaySound("MGreeting1");
                break;
            case "Guide":
                SoundManager.PlaySound("MGreeting2");
                break;
            case "Viliager-1":
                SoundManager.PlaySound("MGreeting3");
                break;
            case "BlackSmith":
                SoundManager.PlaySound("MGreeting4");
                break;
            case "Priest":
                SoundManager.PlaySound("MGreeting5");
                break;
            case "Merchant":
                SoundManager.PlaySound("MGreeting7");
                break;
            case "Farmer":
                SoundManager.PlaySound("MGreeting6");
                break;


        }

        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }


        totalsentences = sentences.Count;
        DisplayNext();

    }

    public void DisplayNext()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        // Text Animation
        textwriter.AddWriter(dialoguetext, sentence, 0.05f);
    }

    void EndDialogue()
    {
        Debug.Log("End of conversation");
    }

    public int GetTotalSentenceCount()
    {
        return totalsentences;
    }

    public void ClearDialogueText()
    {
        dialoguetext.text = String.Empty;
    }


    /// <summary>
    /// Item Sys Related
    /// </summary>
    /// <param name="val"></param>
    public int HP()
    {
        return m_currentHealth;
    }

    public int MaxHP()
    {
        return m_maxHealth;
    }

    public int MaxSan()
    {
        return m_maxSanity;
    }

    public int Sanity()
    {
        return m_currentSanity;
    }

    public float ClimbSpeed()
    {
        return m_climb_speed;
    }

    public int EB()
    {
        return m_currentEB;
    }

    public void SetItemValueChange(Tuple<Tuple<int, int>, string> tuple)
    {
        valuechange = tuple.Item1.Item1;
        itemcost = tuple.Item1.Item2;
        itemType = tuple.Item2;
    }

    public void ResetValueChange()
    {
        valuechange = 0;
        itemcost = 0;
        itemType = string.Empty;
    }

    private void AddATK(int value)
    {
        m_attack_1_Damage += value;
    }

    private void AddDEF(int value)
    {
        m_def += value;
    }

    //BUFF 效果的实施
    private void BUFF(string name)
    {
        switch (name)
        {
            case "AddMaxHP":
                AddMaxHp(20);
                AddHp(20);
                break;
            case "SanityUp":
                AddMaxSan(20);
                AddSan(20);
                break;
            case "SpeedUp":
                teodoro.AddSpeed(2);
                break;
            case "ATKUp":
                m_attack_1_Damage += 10;
                break;
            case "JumpForceUp":
                teodoro.AddJumpForce(1);
                break;
        }
    }

    public void SavePlayer()
    {
        m_speed = teodoro.GetSpeed();
        m_jumpforce = teodoro.GetJumpForce();
        musicVol = GameObject.Find("Canvas").transform.GetChild(9).transform.GetChild(2).transform.GetChild(1)
            .GetComponent<Slider>().value;
        effectVol = GameObject.Find("Canvas").transform.GetChild(9).transform.GetChild(3).transform.GetChild(1)
            .GetComponent<Slider>().value;
        SaveSystem.Save(this);
    }

    public void LoadPlayer()
    {
        PlayerData data = SaveSystem.Load();
        string currLV = SceneManager.GetActiveScene().name;
        prevScene = data.level;
        switch (currLV)
        {
            case "TempUI":
                transform.position = data.posUI;
                break;
            case "Tutorial":
                transform.position = data.posTutorial;
                break;
            case "Level-1":
                if (prevScene == "Level-2-village")
                {
                    transform.position = new Vector3(15.4f, -3.11f, 0f);
                    break;
                }
                transform.position = data.posLV1;
                break;
            case "Level-2":
                transform.position = data.posLV2;
                break;
            case "Level-3":
                transform.position = data.posLV3;
                break;
            case "Level-4":
                transform.position = data.posLV4;
                break;
            case "LV-5":
                transform.position = data.posLV5;
                break;
            case "LV-1Store":
                transform.position = data.posLV1_Store;
                break;
            case "LV-1BossRoom":
                transform.position = data.posLV1_BossRoom;

                break;
            case "LV-2BossRoom":
                transform.position = data.posLV2_BossRoom;
                break;
            case "Level-2-village":
                if (prevScene == "Level-1")
                {
                    transform.position = new Vector3(68.42f, -1.89f, 0f);
                    break;
                }
                else if (prevScene == "LV-1BossRoom")
                {
                    transform.position = new Vector3(-9.8f, -2.7f, 0f);
                    break;
                }

                transform.position = data.posLV2_Village;
                break;
            case "LV-2Store-1":
                transform.position = data.posLV2_Store1;
                break;
            case "LV-2Store-2":
                transform.position = data.posLV2_Store2;
                break;
            case "LV-2Store-3":
                transform.position = data.posLV2_Store3;
                break;
            case "LV-2Store-4":
                transform.position = data.posLV2_Store4;
                break;
            case "LV-2TransitionToLV3":
                transform.position = data.posLV2_Transition;
                break;
        }

        m_currentHealth = data.health;
        m_currentSanity = data.sanity;
        m_currentEB = data.energyBall;
        m_maxHealth = data.maxhealth;
        m_maxSanity = data.maxsanity;
        m_activeSkillType = data.activeskill;
        m_skillcount = data.skillcount;
        CreateSkillCount(m_skillcount);
        m_speed = data.speed;
        m_jumpforce = data.jumpforce;
        musicVol = data.musicVol;
        effectVol = data.effectVol;
        GameObject.Find("Canvas").transform.GetChild(9).transform.GetChild(2).transform.GetChild(1)
            .GetComponent<Slider>().value = musicVol;
        GameObject.Find("Canvas").transform.GetChild(9).transform.GetChild(3).transform.GetChild(1)
            .GetComponent<Slider>().value = effectVol;
        
        temp2 = data.buffarray;
        if (temp2.Length > 0)
        {
            List<string> result = temp2.Split(',').ToList();
            foreach (string i in result)
            {
                if (i != "")
                {
                    Sprite img = buffmanager.buffs.Find(x => x.name == i);
                    GameObject temp = new GameObject();
                    temp.AddComponent<Image>().sprite = img;
                    temp.AddComponent<Buff>().buffName = i;
                    Instantiate(temp, Vector3.zero, Quaternion.identity);
                    temp.transform.SetParent(buffgrid.transform);
                }

            }
        }

    }


    // 强制游戏结束
    public void forceGameOver()
    {
        AddHp(-1000);
    }


    
    //----------------------------SKILLS----------------------------------
    /// <summary>
    /// Skill Btn in Teodoro.cs calls this method.
    /// </summary>
    public string GetActiveSkillType()
    {
        return m_activeSkillType;
    }

    public void SkillSelector()
    {
        switch (m_activeSkillType)
        {
            case "bullet":
                Shoot();
                return;
            case "bomb":
                Bomb();
                return;
            case "hpsteal":
                HPSteal();
                return;
            default:
                Debug.Log("Currently you don't have a skill to use.");
                return;
        }
    }

    private void SkillCountSelector()
    {
        switch (m_activeSkillType)
        {
            case "bullet":
                m_skillcount = m_skillcount1;
                return;
            case "bomb":
                m_skillcount = m_skillcount2;
                return;
            case "hpsteal":
                m_skillcount = 2;
                return;
            default:
                m_skillcount = 0;
                Debug.Log("Currently you don't have a skill to use.");
                return;
        }
    }

    // 远距离技能
    public void Shoot()
    {
        if (m_timeSinceSkill > m_Skill_Cooldown)
        {
            StartCoroutine(DoShoot());
            RemoveSkillCount();
        }
    }

    // 远距离技能
    private IEnumerator DoShoot()
    {
        m_timeSinceSkill = 0.0f;
        yield return new WaitForSeconds(m_Skill_Delay);
        m_animator.SetTrigger("skill3");
        PlayerBullet bullet = Instantiate(m_bulletPrefab, m_bulletSpawnPos.position, m_bulletSpawnPos.rotation);
        bullet.InitializePlayerBullet(this, m_damagePerBullet, m_bulletSpeed, m_bulletDestoryDelay);

    }

    //埋炸弹
    public void Bomb()
    {
        if (m_timeSinceSkill > m_Skill_Cooldown)
        {
            StartCoroutine(DoBomb());
            RemoveSkillCount();
        }
    }

    private IEnumerator DoBomb()
    {
        m_timeSinceSkill = 0.0f;
        yield return new WaitForSeconds(m_Skill_Delay);
        m_animator.SetTrigger("skill3");
        PlayerBomb bomb = Instantiate(m_bombPrefab, m_bulletSpawnPos.position, m_bulletSpawnPos.rotation);
        bomb.InitializePlayerBomb(this, m_damagePerBomb, m_bombDestoryDelay);
    }


    // 生命偷取
    // 偷取的HP = 基础攻击的30%
    // 关联到UnitAttackZone.cs #116-132
    public void HPSteal()
    {
        if (m_timeSinceSkill > m_Skill_Cooldown)
        {
            StartCoroutine(DoHPSteal());
            RemoveSkillCount();
        }
    }
    
    private IEnumerator DoHPSteal()
    {
        m_timeSinceSkill = 0.0f;
        yield return new WaitForSeconds(m_Skill_Delay);
        m_animator.SetTrigger("skill2");
        UnitAttackZone attackZone = Instantiate(m_attackZone1, transform, false);
        attackZone.InitializeSkillZone(this, m_attack_1_Damage, m_atk_1_TriggerEnableDelay, m_atk_1_DestoryDelay);
    }
    //----------------------------DEBUFF----------------------------------
    bool poisonEffect = true;
    int poisonDamage = 5 ;
    private float poisonTimer = 10f;
    private bool poisonInstantiated = false;
    IEnumerator Poisoning(){
        if(poisonEffect){
            Debug.Log("Poisoning");
            AddHp(-poisonDamage);
            if (!poisonInstantiated)
            {
               GameObject poi = Instantiate(poisonPrefab, transform.position, Quaternion.identity,transform);
               poisonInstantiated = true;
            }
            poisonEffect = false ;
            yield return new WaitForSeconds(poisonTimer) ;
            poisonEffect = true ;
        }
    }

    // 根据Sanity值的不同 选择不同的buff或者buff叠加
    // 准备把毒附到普通攻击上，可以作为技能
    // Demo设定（暂时）：根据程度依次叠加
    // <90 中毒 | <70 获得80%的EB | 
    private void DebuffSelector()
    {
        if (m_currentSanity >= 90)
        {
            UnityEngine.Object.Destroy(GameObject.Find("Poison(Clone)"));
            poisonInstantiated = false;
        } 
        if (m_currentSanity < 90)
        {
            StartCoroutine(Poisoning());
        }
        
    }

    public float GetTeodoroSpeed()
    {
        return m_speed;
    }

    public float GetTeodoroJumpForce()
    {
        return m_jumpforce;
    }
    
}
