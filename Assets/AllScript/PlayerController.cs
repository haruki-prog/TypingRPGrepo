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

    [Header("Speed Settings")]
    public float PlayerSpeed;
    Vector3 speed = Vector3.zero;
    Vector3 rot = Vector3.zero;
    public float RotationSpeed;         //RotationSpeed:視点移動の感度を格納する変数

    public Transform Camera;

    bool isRun;
    public bool canMove = true;    //移動できるかどうかを判定するbool型変数

    [Header("Attack Animation Setting")]
    public Animator PlayerAnimator;
    public Collider WeaponCollider;



    [Header("Audio Settings")]
    public AudioSource audioSource;
    public AudioClip SwingSE;

    [Header("Particle Settings")]
    //[SerializeField] private ParticleSystem SwingEffect;
    public ParticleSystem Attack1Effect;
    public ParticleSystem Attack2Effect;
    public ParticleSystem Attack3Effect;

    // エイムアシスト関連の追加変数
    [Header("Aim Assist Settings")]
    public float aimAssistSpeed = 10f; // エイムアシスト時の移動速度
    public float aimAssistStopDistance = 1.5f; // 敵の手前で止まる距離
    public float aimAssistDuration = 0.5f; // エイムアシストが持続する最大時間
    private Transform currentAimAssistTarget = null; // 現在のエイムアシストターゲット
    private float aimAssistTimer = 0f; // エイムアシストのタイマー
    
    
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (currentAimAssistTarget == null)
        {
            Movement();
            Rotation();
        }
        else
        {
            // エイムアシスト中の移動処理
            HandleAimAssistMovement();
        }
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
        // 攻撃終了時にエイムアシストをリセット
        currentAimAssistTarget = null;
        aimAssistTimer = 0f;
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
    //エイムアシストの設定
    public void StartAimAssist(Transform target)
    {
        if (target == null) return; // ターゲットが無ければ何もしない

        currentAimAssistTarget = target;
        aimAssistTimer = aimAssistDuration; // タイマーをリセット
        canMove = false; // エイムアシスト中は移動を制限
        // 敵の方向を向く
        Vector3 lookDirection = (target.position - transform.position).normalized; // .normalizedは単位ベクトルに変換してくれるやつ
        lookDirection.y = 0; // Y軸は無視
        if (lookDirection != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(lookDirection);// 方向転換
        }
    }
    //アシスト中の移動処理
    private void HandleAimAssistMovement()
    {
        if (currentAimAssistTarget == null) return;

       
        // タイマーを減らす
        aimAssistTimer -= Time.deltaTime;

        // ターゲットまでの距離を計算
        Vector3 targetPos = currentAimAssistTarget.position;
        targetPos.y = transform.position.y; // 高さ方向の移動を無視

        float distanceToTarget = Vector3.Distance(transform.position, targetPos); // ターゲットまでの距離を計算

        // 敵の手前で止まる距離(aimAssistStopDistance)よりも近ければ、移動を停止
        if (distanceToTarget <= aimAssistStopDistance || aimAssistTimer <= 0f)
        {
            currentAimAssistTarget = null; // エイムアシスト終了
            aimAssistTimer = 0f;
            canMove = true; // 移動を許可
            rigidBody.linearVelocity = Vector3.zero; // 慣性をなくす
            return;
        }

       
        Vector3 direction = (targetPos - transform.position).normalized; // 方角を決定
        rigidBody.MovePosition(transform.position + direction * aimAssistSpeed * Time.deltaTime); // ターゲットに向かって移動

        // プレイヤーの向きもターゲットに向ける
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, RotationSpeed * Time.deltaTime); // 線形補完してくれる関数slerp
    }
}
