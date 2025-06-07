using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Audio;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] Transform target;
   // [SerializeField] EnemyStatusSO enemyStatusSO;
    private NavMeshAgent agent;
    private Animator animator;
    private float speed = 3f;
    private float distance;
   
    private int currentHP;

    public Animator EnemyAnimator;  //敵のアニメーターを格納する変数
    public Collider AttackingPlayerCollider;    //プレイヤーを攻撃時の敵の当たり判定を格納するCollider型変数

    public AudioSource audioSource;
    public AudioClip HitSE;


    public GameObject DeathEffect;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        agent.speed = speed;
       // currentHP = enemyStatusSO.HP;

    }
    // Update is called once per frame
    void Update()
    {
        Distance();
        AttackMotion();

    }
    //敵が追いかけてくる範囲の設定
    void Distance()
    {
        distance = Vector3.Distance(target.position, this.transform.position);
        if (distance < 10)
        {
            agent.destination = target.position;
            animator.SetBool("Found", true);
        }
        else
        {
            animator.SetBool("Found", false);
        }
    }
    
    //攻撃モーションの設定
    void AttackMotion()
    {
        if (distance < 2&& distance>1)
        {
            
            animator.SetBool("Attack", true);
        }
        else
        {
            animator.SetBool("Attack", false);
        }
    }

    void AttackModeON() //敵の当たり判定を切り替える関数
    {
        AttackingPlayerCollider.enabled = true;
    }
    void AttackModeOFF()
    {
        AttackingPlayerCollider.enabled = false;
        EnemyAnimator.SetBool("Attack", false);
    }


    //ダメージモーションの設定、間違いありかも [追記:調整しました]
    private void OnTriggerEnter(Collider col)
    {
        if (col.tag== "Weapon")
        {
            Debug.Log("Hit!");
            audioSource.PlayOneShot(HitSE);
            //animator.SetTrigger("Damage");
            var effect = Instantiate(DeathEffect);
            var pos = transform.position;
            pos.y += 1.0f;
            effect.transform.position = pos;
            Destroy(effect, 5);
            Destroy(gameObject);    //一撃で死ぬようにしてます

        }
        

    }
  
}
