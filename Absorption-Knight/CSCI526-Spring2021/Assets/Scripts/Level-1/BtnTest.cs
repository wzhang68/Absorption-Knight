using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BtnTest : MonoBehaviour
{
    public GameObject player;
    public float speed = 0.35f;
    public int jumpHeight = 30;
    private Rigidbody2D rb;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = player.GetComponent<Rigidbody2D>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Jump()
    {
      rb.AddForce(Vector2.up * jumpHeight, ForceMode2D.Impulse);
    }
}
