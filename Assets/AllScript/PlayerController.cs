using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rigidBody;
    private Animator animator;
    
    private int currentHP;


    public float PlayerSpeed;
    Vector3 speed = Vector3.zero;
    Vector3 rot = Vector3.zero;
    public Transform Camera;

    bool isRun;
    public bool canMove = true;    //移動できるかどうかを判定するbool型変数


    public Animator PlayerAnimator;
    public Collider WeaponCollider;


    public float RotationSpeed;         //RotationSpeed:視点移動の感度を格納する変数

    public AudioSource audioSource;
    public AudioClip SwingSE;

    //[SerializeField] private ParticleSystem SwingEffect;
    public ParticleSystem Attack1Effect;
    public ParticleSystem Attack2Effect;
    public ParticleSystem Attack3Effect;
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        Movement();
        Rotation();
        Camera.transform.position = transform.position;
        //AttackMotion();

    }

    void Movement()
    {
        speed = Vector3.zero;
        rot = Vector3.zero;
        isRun = false;
        if (!canMove)   //移動できなくする
        {
            return;
        }

        if (Input.GetKey(KeyCode.W))
        {
            rot.y = 0;
            MoveSet();
            
        }
        if (Input.GetKey(KeyCode.S))
        {
            rot.y = 180;
            MoveSet();
        }
        if (Input.GetKey(KeyCode.A))
        {
            rot.y = -90;
            MoveSet();
        }
        if (Input.GetKey(KeyCode.D))
        {
            rot.y = 90;
            MoveSet();
        }
        //相対座標
        transform.Translate(speed);
        PlayerAnimator.SetBool("run", isRun);
    }

    void MoveSet()
    {
        speed.z = PlayerSpeed;
        transform.eulerAngles = Camera.transform.eulerAngles + rot;
        isRun = true;
    }

    // 移動を許可する関数
    void CanMove()
    {
        canMove = true;
    }

    //攻撃モーションの設定
    /*void AttackMotion()
    {
        if (Input.GetKeyDown(KeyCode.Space))    
        {
            PlayerAnimator.SetBool("attack", true);
            canMove = false;    //移動できなくする

        }
    }*/
    public void TriggerAttack()
    {
        PlayerAnimator.SetBool("attack", true);
        canMove = false;
    }
       

    //アニメーションの当たり判定の設定。　　間違いありかも

    void WeaponON()
    {
        //SwingEffect.Play();
        WeaponCollider.enabled = true;
        audioSource.PlayOneShot(SwingSE);
        
    }
    void WeaponOFF()
    {
        WeaponCollider.enabled = false;
        PlayerAnimator.SetBool("attack", false);
        PlayerAnimator.SetBool("combo", false);
    }
 


    //視点移動の関数
    void Rotation()
    {
        var speed = Vector3.zero;
        if (Input.GetKey(KeyCode.RightArrow))
        {
            speed.y = RotationSpeed;
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            speed.y = -RotationSpeed;
        }
        Camera.transform.eulerAngles += speed;
    }

    void PlayAttack1Effect()
    {
        Attack1Effect.Play();
    }
    void PlayAttack2Effect()
    {
        Attack2Effect.Play();
    }
    void PlayAttack3Effect()
    {
        Attack3Effect.Play();
    }


}
