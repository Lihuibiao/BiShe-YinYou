using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCntroller : MonoBehaviour
{
    public static PlayerCntroller Inst;
    public bool CanController;

    private Rigidbody2D myRigidbody;
    private Animator myAnim;
    private SpriteRenderer SpriteRenderer;
    private PolygonCollider2D PolygonCollider2D;
    private List<Vector2> points = new List<Vector2>();
    public bool isGround;
    
    private bool canDoubleJump;
    private Vector2 move;

    void Awake()
    {
        Inst = this;
        
    }
    
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        myAnim = GetComponent<Animator>();
        SpriteRenderer = GetComponent<SpriteRenderer>();
        PolygonCollider2D = GetComponent<PolygonCollider2D>();
    }

    void fixedCollider()
    {
        SpriteRenderer.sprite.GetPhysicsShape(0, points);
        PolygonCollider2D.SetPath(0 , points);
    }

    public GameObject BgMoveObj;
    
    void Update()
    {
        fixedCollider();
        AniCtrl();
        if (BgMoveObj.transform.position.x > -57)
        {
            BgMoveObj.transform.Translate(-1 * Time.deltaTime , 0f , 0f);   
        }
        
        
        if (this.CanController)
        {
            // Run();
            // Flip();
            // CheckGrounded();
            // //Jump();
            // //Attack();//攻击
            // SwitchAnimation();//动画切换
            // AttackWithEnergy();
        }

        // if(_canRun && myAnim.runtimeAnimatorController != Run_Animator)
        // {
        //     myAnim.runtimeAnimatorController = Run_Animator;
        // }
        // else if(!_canRun && myAnim.runtimeAnimatorController != Walk_Animator)
        // {
        //     myAnim.runtimeAnimatorController = Walk_Animator;
        // }

        // OnFail();
        // PolygonCollider2D.
    }

    private void AniCtrl()
    {
        if (myAnim.GetBool("Jump"))
        {
            myAnim.SetBool("Jump" , false);
        }
        
        if (myAnim.GetBool("Attack"))
        {
            myAnim.SetBool("Attack" , false);
        }
        
        if (Input.GetKeyDown(KeyCode.Space) && myAnim.GetCurrentAnimatorStateInfo(0).IsName("Run"))
        {
            myAnim.SetBool("Jump" , true);
        }
        
        if (Input.GetMouseButton(0) && myAnim.GetCurrentAnimatorStateInfo(0).IsName("Run"))
        {
            myAnim.SetBool("Attack" , true);
        }
    }
    
    void CheckGrounded()
    {
        // isGround = PolygonCollider2D.IsTouchingLayers(LayerMask.GetMask("Ground"));
    }   
    
    private bool DoubleJumpOnce = false;
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag.Equals("FigureInStone"))
        {

        }

        CheckGrounded();
        if (DoubleJumpOnce && isGround)
        {
            DoubleJumpOnce = false;
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag.Equals("FigureInStone"))
        {
        }
    }

    public void OnFail()
    {
        
    }
}
