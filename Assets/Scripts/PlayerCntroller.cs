using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCntroller : MonoBehaviour
{
    public static PlayerCntroller Inst;
    public float runSpeed;
    public float jumpSpeed;
    public float doulbJumpSpeed;
    public bool CanController;

    private Rigidbody2D myRigidbody;
    private Animator myAnim;
    private PolygonCollider2D PolygonCollider2D;
    public bool isGround;

    public bool _canRun;
    public bool CanRun
    {
        get
        {
            return _canRun;
        }
        set
        {
            _canRun = value;
            if (_canRun)
            {
                myAnim.runtimeAnimatorController = Run_Animator;
            }
            else
            {
                myAnim.runtimeAnimatorController = Walk_Animator;
            }
        }
    }
    private bool canDoubleJump;
    private bool isOneWayPlatform;

    public RuntimeAnimatorController Run_Animator;
    public RuntimeAnimatorController Walk_Animator;

    private PlayerInputActions controls;
    private Vector2 move;

    void Awake()
    {
        Inst = this;
        controls = new PlayerInputActions();

        controls.GamePlayer.Move.performed += ctx => move = ctx.ReadValue<Vector2>();
        controls.GamePlayer.Move.canceled += ctx => move = Vector2.zero;
        controls.GamePlayer.Jump.started += ctx => Jump();
        if (attackEnergy != null)
        {
            attackEnergy.gameObject.SetActive(false);   
        }
    }
    void OnEnable()
    {
        controls.GamePlayer.Enable();
    }

    void OnDisable()
    {
        controls.GamePlayer.Disable();
    }
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        myAnim = GetComponent<Animator>();
        PolygonCollider2D = GetComponent<PolygonCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (this.CanController)
        {
            Run();
            Flip();
            CheckGrounded();
            //Jump();
            //Attack();//攻击
            SwitchAnimation();//动画切换
            AttackWithEnergy();
        }

        if(_canRun && myAnim.runtimeAnimatorController != Run_Animator)
        {
            myAnim.runtimeAnimatorController = Run_Animator;
        }
        else if(!_canRun && myAnim.runtimeAnimatorController != Walk_Animator)
        {
            myAnim.runtimeAnimatorController = Walk_Animator;
        }

        OnFail();

    }
    void CheckGrounded()
    {
        isGround = PolygonCollider2D.IsTouchingLayers(LayerMask.GetMask("Ground"));
    }   


    void Flip()
    {
        bool plyerHasXAxisSpeed = Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon;
        if (plyerHasXAxisSpeed)
        {
            if (myRigidbody.velocity.x > 0.1f)
            {
                transform.localRotation = Quaternion.Euler(0, 0, 0);
            }

            if (myRigidbody.velocity.x < -0.1f)
            {
                transform.localRotation = Quaternion.Euler(0, 180, 0);
            }
        }
    }
    //}
    void Run()
    {
        //float moveDir = Input.GetAxis("Horizontal");
        //Vector2 playerVel = new Vector2(moveDir * runSpeed, myRigidbody.velocity.y);
        //myRigidbody.velocity = playerVel;
        //bool plyerHasXAxisSpeed = Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon;
        //myAnim.SetBool("Run", plyerHasXAxisSpeed);

        Debug.LogError(move);
        Vector2 playerVelocity = new Vector2(move.x * runSpeed, myRigidbody.velocity.y);
        myRigidbody.velocity = playerVelocity;
        bool playerHasXAxisSpeed = Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon;
        // if (CanRun)
        // {
        //     myAnim.SetBool("Run", playerHasXAxisSpeed);
        // }
        // else
        // {
        //     myAnim.SetBool("Walk", playerHasXAxisSpeed);
        // }
        
    }

    private bool DoubleJumpOnce = false;
    void Jump()
    {
        if (!CanController)
        {
            return;
        }
        //if (Input.GetButtonDown("Jump"))
        {
            if (isGround)
            {
                myAnim.SetBool("Jump", true);
                Vector2 jumpVel = new Vector2(0.0f, jumpSpeed);
                myRigidbody.velocity = Vector2.up * jumpVel;
                canDoubleJump = true;
            }
            else
            {
                if (canDoubleJump)
                {
                    myAnim.SetBool("DoubleJump", true);
                    Vector2 doubleJumpVel = new Vector2(0.0f, doulbJumpSpeed);
                    myRigidbody.velocity = Vector2.up * doubleJumpVel;
                    canDoubleJump = false;
                    DoubleJumpOnce = true;
                }
            }
        }
    }
    
    
    
    //void Attack()
    //{
    //    if(Input.GetButtonDown("Attack"))
    //    {
    //        myAnim.SetTrigger("Attack");
    //    }
    //} 
    void SwitchAnimation()
    {
        myAnim.SetBool("Idle", false);
        if (myAnim.GetBool("Jump"))
        {
            if (myRigidbody.velocity.y < 0.0f)
            {
                myAnim.SetBool("Jump", false);
                myAnim.SetBool("Fall", true);
            }
        }
        else if (isGround)
        {
            myAnim.SetBool("Fall", false);
            myAnim.SetBool("Idle", true);
        }

        if (myAnim.GetBool("DoubleJump"))
        {
            if (myRigidbody.velocity.y < 0.0f)
            {
                myAnim.SetBool("DoubleJump", false);
                myAnim.SetBool("DoubleFall", true);
            }
        }
        else if (isGround)
        {
            myAnim.SetBool("DoubleFall", false);
            myAnim.SetBool("Idle", true);
        }
    }
    
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

    public void ClosePlayerController()
    {
        PlayerCntroller.Inst.CanController = false;
        if (myAnim != null)
        {
            myAnim.SetBool("Idle" , true);
            if (CanRun)
            {
                myAnim.SetBool("Run", false);    
            }
            else
            {
                myAnim.SetBool("Walk", false);   
            }
            myAnim.SetBool("Jump", false);
            myAnim.SetBool("DoubleJump", false);
            myAnim.SetBool("Fall", false);
            myAnim.SetBool("DoubleFall", false);    
        }

        if (myRigidbody != null)
        {
            myRigidbody.velocity = Vector2.zero;
        }
        
    }

    public bool InDecryptRoom;
    public void OnPlayerEnterDecryptRoom()
    {
        this.InDecryptRoom = true;
    }

    public void OnFail()
    {
        
    }

    public GameObject energyFlow;
    // 能量跟随
    public void EnergyFlow()
    {
        if (energyFlow == null)
        {
            return;
        }
    }

    public GameObject attackEnergy;
    public void AttackWithEnergy()
    {
        if (attackEnergy == null)
        {
            return;
        }
    }

    IEnumerator delayCloseAttackEnergy()
    {
        yield return new WaitForSeconds(0.25f);
        attackEnergy.GetComponent<BoxCollider2D>().enabled = true;
        yield return new WaitForSeconds(0.3f);
        attackEnergy.gameObject.SetActive(false);
    }
}
