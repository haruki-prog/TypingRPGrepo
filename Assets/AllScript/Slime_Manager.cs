using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Audio;
using static UnityEngine.GraphicsBuffer;

public class Slime_Manager : MonoBehaviour
{
    //[SerializeField] Transform target;
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


    public string typingWord = "slime"; // スライムなら "slime"
    public TMP_Text typingText;         // Canvas上に表示するText（ワールドスペースでも可）

    public GameObject DeathEffect;

    public string Target;
    Transform target;

    float Timer;
    public float EnemySpeed;
    public float ChangeTime;

    GameObject playerObj;


    //[SerializeField] private ParticleSystem AttackEffect;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        agent.speed = speed;
        // currentHP = enemyStatusSO.HP;
        playerObj = GameObject.FindWithTag(Target);
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

        if (!playerObj)
        {
            return;
        }

        if (playerObj != null)
        {
            target = playerObj.transform;

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


    }

    //攻撃モーションの設定
    void AttackMotion()
    {
        if (distance < 2 && distance > 1)
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
        //AttackEffect.Play();
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
        if (col.tag == "Weapon")
        {
            Debug.Log("Hit!");
            audioSource.PlayOneShot(HitSE);
            //animator.SetTrigger("Damage");

            var pos = transform.position;
            pos.y += 1.2f;
            var effect = Instantiate(DeathEffect);
            effect.transform.position = pos;
            Destroy(effect, 5);

            AudioSource deathSE = Instantiate(audioSource, transform.position, Quaternion.identity);
            deathSE.transform.SetParent(null); // いったん親から外す
            deathSE.Play();
            Destroy(deathSE.gameObject, deathSE.clip.length); // 再生が終わったタイミングで消すようにした


            Destroy(gameObject);    //一撃で死ぬようにしてます

        }


    }

}
