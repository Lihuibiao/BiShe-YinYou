using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerCntroller : MonoBehaviour
{
    public static PlayerCntroller Inst;
    private Animator myAnim;
    private SpriteRenderer SpriteRenderer;
    private PolygonCollider2D PolygonCollider2D;
    private List<Vector2> points = new List<Vector2>();
    public bool isGround;
    public Text ScoreTxt;
    public Text HpTxt;
    
    private bool canDoubleJump;
    private Vector2 move;
    private int Score;

    private int Hp = 5;

    public static bool IsWin;
    void Awake()
    {
        Inst = this;
        IsWin = false;
    }
    
    void Start()
    {
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

        HpTxt.text = Hp.ToString();
        ScoreTxt.text = Score.ToString();
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
        Debug.LogError(col.transform);
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

    private void OnCollisionEnter2D(Collision2D other)
    {
        Debug.LogError(other);
        if (other.transform.tag.Equals("Enemy"))
        {
            // Destroy(other.transform.GetComponent<Rigidbody2D>());
            if (myAnim.GetCurrentAnimatorStateInfo(0).IsName("Attack") || myAnim.GetCurrentAnimatorStateInfo(0).IsName("Jump"))
            {
                Debug.LogError("砍死");
                Score++;
                Destroy(other.gameObject);
            }
            else
            {
                Hp--;
                Destroy(other.gameObject);
                if (Hp <= 0)
                {
                    IsWin = false;
                    GameOver();
                    return;
                }
                // Destroy(other.transform.GetComponent<Rigidbody2D>());
                // StartCoroutine(Delay2DestroyOtherRigibody(other));
            }
        }
        if (other.transform.gameObject.name.Equals("EndEnemy"))
        {
            IsWin = Hp > 0;
            GameOver();
        }
    }
    
    private void OnCollisionStay2D(Collision2D other)
    {
        Debug.LogError(other.transform);
        if (other.transform.tag.Equals("Enemy"))
        {
            if (myAnim.GetCurrentAnimatorStateInfo(0).IsName("Attack") || myAnim.GetCurrentAnimatorStateInfo(0).IsName("Jump"))
            {
                Debug.LogError("砍死");
                Score++;
                Destroy(other.gameObject);
            }   
        }
    }

    IEnumerator Delay2DestroyOtherRigibody(Collision2D other)
    {
        yield return new WaitForSeconds(0.1f);
        if (other != null)
        {
            Destroy(other.gameObject);   
        }
        // Destroy(other.transform.GetComponent<Rigidbody2D>());
    }

    public void GameOver()
    {
        SceneManager.LoadScene("GameOver");
    }
}
