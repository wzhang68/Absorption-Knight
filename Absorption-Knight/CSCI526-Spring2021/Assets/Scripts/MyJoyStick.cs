using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyJoyStick : MonoBehaviour
{
    protected Joystick joystick;
    public float moveSpeed = 20f;
    [SerializeField] private GameObject player;
    private Rigidbody2D characterBody;
    
    // Start is called before the first frame update
    void Start()
    {
        joystick = FindObjectOfType<Joystick>();
        characterBody = player.GetComponent<Rigidbody2D>();

    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(joystick.Horizontal);
        characterBody.velocity =new Vector2(joystick.Horizontal *moveSpeed, characterBody.velocity.y);
    }
}
