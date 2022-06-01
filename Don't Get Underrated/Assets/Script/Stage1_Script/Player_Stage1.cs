using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Stage1 : Player_Info
{
    [SerializeField]
    Sprite jump_sprite;

    [SerializeField]
    Sprite Stun_sprite;

    [SerializeField]
    Sprite Collide_sprite;

    [SerializeField]
    Sprite Up_sprite;

    [SerializeField]
    Sprite stop_sprite;

    [SerializeField]
    Sprite Down_sprite;

    [SerializeField]
    float Power_jump = 2.0f;

    float speed = 3.5f;

    Rigidbody2D rigid2D;

    [SerializeField]
    LayerMask groundLayer;

    [SerializeField]
    LayerMask wallLayer;

    [HideInInspector]
    public bool isJumped = false;

    bool isGrounded;

    bool isfainted;

    bool isStuck;

    bool Move = true;

    bool isJumping = false;


    public float Ch_y_position;
    private float Jump_start_position;
    public Animator animator;
    public GameObject Ground;
    public GameObject Wall;
    public float Prev_pos_y;

    //for debugging
    public float tp1;
    public float tp2;

    private AudioSource jump_sound;
    private AudioSource wall_collision_sound;
    private new void Awake()
    {
        base.Awake();

        if (GameObject.Find("Wall_Collision") && GameObject.Find("Wall_Collision").TryGetComponent(out AudioSource AS1))
            wall_collision_sound = AS1;

        if (GameObject.Find("Jump") && GameObject.Find("Jump").TryGetComponent(out AudioSource AS2))
            jump_sound = AS2;
        
        tp1 = My_Position.x;
        tp2 = My_Position.y;

        if (TryGetComponent(out Animator A))
            animator = A;
        if (TryGetComponent(out Rigidbody2D RB2D))
            rigid2D = RB2D;
    }
    private void Update()
    {
        isGrounded = Physics2D.OverlapBox(new Vector2(My_Position.x, My_Position.y - 0.3f), new Vector2(0.15f, 0.05f), 0f, groundLayer);
        Ch_y_position = My_Position.y;
        isStuck = Physics2D.OverlapBox(new Vector2(My_Position.x, My_Position.y), new Vector2(0.325f, 0.55f), 0f, wallLayer);

        if (isGrounded)
        {
            isJumping = false;
            animator.enabled = true;
            isfainted = false;
        }
        else
            animator.enabled = false;

        float x = Input.GetAxisRaw("Horizontal");

        if (x != 0 && isGrounded&&Move)
        {
            animator.SetBool("run", true);
            My_Scale = new Vector3(-1 * x, 1, 1);
        }
        else
            animator.SetBool("run", false);

        if (!isGrounded && (Ch_y_position > Prev_pos_y) && !isfainted)
        {
            animator.enabled = false;
            spriteRenderer.sprite = Up_sprite;
        }
        else if (!isGrounded && (Ch_y_position < Prev_pos_y) && !isfainted)
        {
            animator.enabled = false;
            spriteRenderer.sprite = Down_sprite;
        }
        else
        {

        }

        if (isGrounded && (Ch_y_position - Jump_start_position < -4.0f))
        {
            animator.enabled = false;
            spriteRenderer.sprite = Stun_sprite;
            Move = false;
            Invoke("stand_again", 1f);
        }
        if (!isGrounded && isStuck && isJumping)
        {
            isfainted = true;
            animator.enabled = false;
            spriteRenderer.sprite = Collide_sprite;
            wall_collision_sound.Play();
        }
        if (isGrounded && Move)
        {
            if (!isJumping && !Input.GetKey(KeyCode.Space))
                rigid2D.velocity = new Vector2(speed * x, rigid2D.velocity.y);

            Ready_to_Jump(x);
        }
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            My_Position = new Vector2(tp1, tp2);
        }
        Prev_pos_y = rigid2D.transform.position.y;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawCube(new Vector2(My_Position.x, My_Position.y - 0.3f), new Vector2(0.15f, 0.05f));
        Gizmos.DrawCube(new Vector2(My_Position.x, My_Position.y), new Vector2(0.325f, 0.55f));
        
    }
    public void Ready_to_Jump(float x)
    {
        isJumping = true;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rigid2D.velocity = new Vector2(0, rigid2D.velocity.y);
        }
        if (Input.GetKey(KeyCode.Space))
        {
            animator.enabled = false;
            spriteRenderer.sprite = jump_sprite;
            Power_jump += 0.25f;
            if (Power_jump > 8f)
                Power_jump = 8f;
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            if (Power_jump <= 3f)
                Jump(3f, x);

            else
                Jump(Power_jump, x);

            Power_jump = 0.0f;
            animator.enabled = true;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == Wall&&isGrounded)
            rigid2D.velocity = Vector2.zero;
    }
    public void Jump(float y, float x)
    {
        
        //rigid2D.velocity = new Vector2(x * speed, y*1.5f);
        //Debug.Log("이걸로 적용되야하는뎅");
        rigid2D.AddForce(Vector2.up * y * 1.25f, ForceMode2D.Impulse);
        //rigid2D.AddForce(new Vector2(x * speed*1.5f, y * 1.5f), ForceMode2D.Impulse);
        Jump_start_position = gameObject.transform.position.y;
        isJumping = true;
        jump_sound.Play();
    }
    void stand_again()
    {
        Jump_start_position = 0.0f;
        Move = true;
    }
}