using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Audio;
using static UnityEngine.GraphicsBuffer;
using TMPro;


public class EnemyManager : MonoBehaviour
{
   
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
    public GameObject DamageEffect;

    public string Target;
    Transform target;

    /*float Timer;
    public float EnemySpeed;
    public float ChangeTime;
    */
    GameObject playerObj;

    [SerializeField] public string typingWord ;//与える文字列
    [SerializeField] public int typingCount;//タイピングをする回数
    [SerializeField] public TMP_Text typingText;
    private string currentInput = "";

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

        if(!playerObj)
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
        //AttackEffect.Play();
        AttackingPlayerCollider.enabled = true;
    }
    void AttackModeOFF()
    {
        AttackingPlayerCollider.enabled = false;
        EnemyAnimator.SetBool("Attack", false);
    }
 
    private void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Weapon")
        {
           
            // TypingManager の currentEnemy を取得
            TypingManager typingManager = Object.FindFirstObjectByType<TypingManager>();
            Debug.Log("== Trigger 時の確認 ==");
            Debug.Log($"TypingManager: {typingManager}");
            Debug.Log($"currentEnemy: {typingManager?.currentEnemy}");
            Debug.Log($"This:{this},CrrentEnemy:{typingManager.currentEnemy}");
            if (typingManager == null)
            {
                Debug.LogWarning("TypingManager が見つかりません");
                return;
            }
            if (typingManager.currentEnemy == null)
            {
                Debug.LogWarning("currentEnemy が null です");
                return;
            }
         

            // 対象の敵以外は無視
            if (typingManager.currentEnemy.gameObject != this.gameObject)
            {
                Debug.Log("攻撃されたが、対象外のため無視: " + gameObject.name);
                return;
            }
            Debug.Log("Hit2!");
            typingCount--;  // タイピングカウントをここで減らす

            //与ダメ時のeffect,SE
            audioSource.PlayOneShot(HitSE);
            Vector3 effectPosition = transform.position + new Vector3(0, 1.5f, 0);
            GameObject effect = Instantiate(DamageEffect, effectPosition, Quaternion.identity);
            Destroy(effect, 2);


            // typingCount が 0 以下なら死亡処理
            if (typingCount <= 0)
            {
                /*if (audioSource != null && DeathSE != null)
                {
                    audioSource.PlayOneShot(DeathSE);
                }*/

                if (DeathEffect != null)
                {
                    Vector3 effectPosition1 = transform.position + new Vector3(0, 1.5f, 0);
                    GameObject effect1 = Instantiate(DeathEffect, effectPosition1, Quaternion.identity);
                    Destroy(effect1, 3);
                   
                }

                /*if (audioSource != null)
                {
                    AudioSource deathSE = Instantiate(audioSource, transform.position, Quaternion.identity);
                    deathSE.transform.SetParent(null);
                    deathSE.Play();
                    Destroy(deathSE.gameObject, deathSE.clip.length);
                }*/

                Destroy(gameObject); // 敵を削除
                Debug.Log("倒れました");
            }
            else
            {

                Debug.Log("まだ倒れません。残り：" + typingCount);
                EnemyAnimator.SetTrigger("Die");
               
            }
        }
    }




}
