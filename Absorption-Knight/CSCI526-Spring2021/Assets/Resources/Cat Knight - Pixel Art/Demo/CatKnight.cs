using UnityEngine;
using System.Collections;

public class CatKnight : MonoBehaviour {

    [SerializeField] float      m_speed = 4.0f;
    [SerializeField] float      m_jumpForce = 7.5f;
    //[SerializeField] float      m_rollForce = 6.0f;

    private Animator            m_animator;
    private Rigidbody2D         m_body2d;
    private Sensor_CatKnight    m_groundSensor;
    private Sensor_CatKnight_Wall   m_leftWallSensor;
    private Sensor_CatKnight_Wall   m_rightWallSensor;
    public AttackZone           m_lightAttack;
    public AttackZone           m_heavyAttack;
    private bool                m_grounded = false;
    private bool                m_rolling = false;
    private int                 m_facingDirection = 1;
    //private int                 m_currentAttack = 0;
    private float               m_timeSinceAttack = 0.0f;
    private bool                m_mouseDownMarker = false;
    private float               m_timeSinceMouseDown = 0.0f;
    private float               m_delayToIdle = 0.0f;
    private bool                m_doubleJumpMarker = true;


    // Use this for initialization
    void Start ()
    {
        m_animator = GetComponent<Animator>();
        m_body2d = GetComponent<Rigidbody2D>();
        m_groundSensor = transform.Find("GroundSensor").GetComponent<Sensor_CatKnight>();
        m_leftWallSensor = transform.Find("PlayerLeftWallSensor").GetComponent<Sensor_CatKnight_Wall>();
        m_rightWallSensor = transform.Find("PlayerRightWallSensor").GetComponent<Sensor_CatKnight_Wall>();
    }

    // Update is called once per frame
    void Update ()
    {
        // Increase timer that controls attack combo
        m_timeSinceAttack += Time.deltaTime;
        
        if(m_mouseDownMarker == true) {
            m_timeSinceMouseDown += Time.deltaTime;
        }

        //Check if character just landed on the ground
        if (!m_grounded && m_groundSensor.State())
        {
            m_grounded = true;
            m_animator.SetBool("Grounded", m_grounded);

            m_doubleJumpMarker = true;
        }

        //Check if character just started falling
        if (m_grounded && !m_groundSensor.State())
        {
            m_grounded = false;
            m_animator.SetBool("Grounded", m_grounded);
        }

        // -- Handle input and movement --
        float inputX = Input.GetAxis("Horizontal");

        // Swap direction of sprite depending on walk direction
        if (inputX > 0)
        {
            GetComponent<SpriteRenderer>().flipX = false;
            m_facingDirection = 1;
        }
            
        else if (inputX < 0)
        {
            GetComponent<SpriteRenderer>().flipX = true;
            m_facingDirection = -1;
        }

        // Move
        if (!m_rolling ) {
            if(inputX > 0 && m_rightWallSensor.State()){
                m_rightWallSensor.OutputDebug("Right ");
                m_body2d.velocity = new Vector2(inputX * m_speed, m_body2d.velocity.y);
            } else if(inputX < 0 && m_leftWallSensor.State()) {
                m_rightWallSensor.OutputDebug("Left ");
                m_body2d.velocity = new Vector2(inputX * m_speed, m_body2d.velocity.y);
            } else {
                m_body2d.velocity = new Vector2(0, m_body2d.velocity.y);
            }
        }
            

        //Set AirSpeed in animator
        m_animator.SetFloat("AirSpeedY", m_body2d.velocity.y);

        // -- Handle Animations --
        //Attack
        if(Input.GetMouseButtonDown(0)) {
            m_mouseDownMarker = true;
        }

        
        else if(Input.GetMouseButtonUp(0)){
            if(m_timeSinceMouseDown > 0.35f) {
                m_animator.SetTrigger("Attack3");
                Instantiate(m_heavyAttack, transform.position + new Vector3(m_facingDirection * 0.9f, 0.8f, 0), transform.rotation);
            } else {
                if(m_timeSinceAttack > 0.5f){
                    Instantiate(m_lightAttack, transform.position + new Vector3(m_facingDirection * 1.0f, 0.8f, 0), transform.rotation);


                    m_animator.SetTrigger("Attack1");
                    m_timeSinceAttack = 0.0f;
                }
            }

            m_mouseDownMarker = false;
            m_timeSinceMouseDown = 0.0f;
        }

        /*
        if(Input.GetMouseButtonUp(0) && m_timeSinceAttack > 0.2f)
        {
            m_currentAttack++;

            // Loop back to one after third attack
            if (m_currentAttack > 3)
                m_currentAttack = 1;

            // Reset Attack combo if time since last attack is too large
            if (m_timeSinceAttack > 1.0f)
                m_currentAttack = 1;

            // Call one of three attack animations "Attack1", "Attack2", "Attack3"
            m_animator.SetTrigger("Attack" + m_currentAttack);

            // Reset timer
            m_timeSinceAttack = 0.0f;
        }
        */

        /*
        // Roll
        else if (Input.GetKeyDown("left shift") && !m_rolling)
        {
            m_rolling = true;
            m_animator.SetTrigger("Roll");
            m_body2d.velocity = new Vector2(m_facingDirection * m_rollForce, m_body2d.velocity.y);
        }
        */

        //Jump
        else if (Input.GetKeyDown("space") && m_grounded)
        {
            m_animator.SetTrigger("Jump");
            m_grounded = false;
            m_animator.SetBool("Grounded", m_grounded);
            m_body2d.velocity = new Vector2(m_body2d.velocity.x, m_jumpForce);
            m_groundSensor.Disable(0.2f);
        } 

        else if(Input.GetKeyDown("space") && !m_grounded)
        {
            if(m_doubleJumpMarker) {
                m_body2d.velocity = new Vector2(m_body2d.velocity.x, m_jumpForce);
                m_doubleJumpMarker = false;
            }
        }

        //Run
        else if (Mathf.Abs(inputX) > Mathf.Epsilon)
        {
            // Reset timer
            m_delayToIdle = 0.05f;
            m_animator.SetInteger("AnimState", 1);
        }

        //Idle
        else
        {
            // Prevents flickering transitions to idle
            m_delayToIdle -= Time.deltaTime;
                if(m_delayToIdle < 0)
                    m_animator.SetInteger("AnimState", 0);
        }
    }

    // Animation Events
    // Called in end of roll animation.
    void AE_ResetRoll()
    {
        m_rolling = false;
    }
}
