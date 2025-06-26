using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Audio;
using static UnityEngine.GraphicsBuffer;
using TMPro;


public class BossManager : MonoBehaviour
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
    public GameObject FirebreathEffect;

    public float Searchrange = 0;

    public string Target;
    Transform target;

   
    GameObject playerObj;
    [Header("Typing")]
    [SerializeField] public string typingWord;//与える文字列
    [SerializeField] public int typingCount;//タイピングをする回数
    [SerializeField] public TMP_Text typingText;
    private string currentInput = "";

    [Header("Attack")]
    public float attackdistanse;
    // 攻撃遅延関連の変数
    public float attackDelayTime = 1.0f; // 攻撃を開始するまでの遅延時間（秒）
    private float currentAttackDelayTimer = 0f; // 現在の攻撃遅延タイマー
    
    public GameObject HighrightLine;

    // ノックバック関連の変数
    [Header("Knockback Settings")]
    public float knockbackForce = 10f; // ノックバックの強さ
    public float knockbackDuration = 0.2f; // ノックバックが持続する時間（NavMeshAgent無効化時間）
    private bool isKnockedBack = false; // ノックバック中かどうかのフラグ


    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        agent.speed = speed;
        // currentHP = enemyStatusSO.HP;
        playerObj = GameObject.FindWithTag(Target);
        currentAttackDelayTimer = attackDelayTime;
    }
    // Update is called once per frame
    void Update()
    {

        if (isKnockedBack) return; // ノックバック中は全ての動作をスキップ
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
            if (distance < Searchrange)
            {
                agent.destination = target.position;
                animator.SetBool("Found", true);
                animator.SetBool("Follow", true);

            }
            else
            {
                animator.SetBool("Follow", false);

            }
        }


    }

    //攻撃モーションの設定
    void AttackMotion()
    {
        if (distance < attackdistanse && distance > 0.5)
        {
            currentAttackDelayTimer -= Time.deltaTime;
            // タイマーが0以下になったら攻撃を開始
            if (typingCount >= 3)
            {
                if (currentAttackDelayTimer <= 0f)
                {
                    animator.SetBool("Attack1", true);
                    currentAttackDelayTimer = attackDelayTime;
                }
            }
            if (typingCount >= 2&& typingCount <3)
            {
                if (currentAttackDelayTimer <= 0f)
                {
                    animator.SetBool("Attack2", true);
                    currentAttackDelayTimer = attackDelayTime;
                }
            }
            if (typingCount >= 1&& typingCount < 2)
            {
                if (currentAttackDelayTimer <= 0f)
                {
                    animator.SetBool("Attack3", true);
                    currentAttackDelayTimer = attackDelayTime;
                   // Vector3 FirePos = transform.position + new Vector3(0, 1.5f, 1.5f);
                   // GameObject fire = Instantiate(DamageEffect, FirePos, Quaternion.identity);
                    //Destroy(fire, 3f);
                }
            }


        }
        else
        {
            animator.SetBool("Attack1", false);
            animator.SetBool("Attack2", false);
            animator.SetBool("Attack3", false);
            currentAttackDelayTimer = attackDelayTime;
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
        currentAttackDelayTimer = attackDelayTime;
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


            ApplyKnockback(col.transform);// ノックバック処理の呼び出し

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
                return;
            }
            else
            {

                Debug.Log("まだ倒れません。残り：" + typingCount);
                EnemyAnimator.SetTrigger("Die");

            }
        }
    }

    private void ApplyKnockback(Transform attackerTransform)
    {
        // ノックバック中ならリターン
        if (isKnockedBack) return;

        isKnockedBack = true;
        // Rigidbodyを取得
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            isKnockedBack = false;
            return;
        }

        // NavMeshAgentを一時停止（ノックバック中にAIが追跡しないように）
        if (agent != null && agent.enabled)
        {
            agent.isStopped = true; // ナビゲーションの停止
            agent.enabled = false;
        }

        // ノックバックの方向を計算 (敵自身から攻撃者への方向)
        Vector3 knockbackDirection = (transform.position - attackerTransform.position).normalized;

        knockbackDirection.y = 0.3f; // 少し浮き上がるように
        knockbackDirection = knockbackDirection.normalized; // 正規化

        // 力を加える
        rb.AddForce(knockbackDirection * knockbackForce, ForceMode.Impulse);

        // ノックバック終了を待つ関数を開始
        StartCoroutine(ResetKnockback());
    }


    // ノックバック状態をリセットする関数
    private IEnumerator ResetKnockback() // この関数をコルーチンとして定義
    {
        // 指定されたノックバック持続時間だけ待つ
        yield return new WaitForSeconds(knockbackDuration); // yieldで継続的returnを実現(戻り値の返却のみ行う)

        // ノックバック状態を解除
        isKnockedBack = false;

        // Rigidbodyの速度をリセット（ノックバックの余韻を残さない場合）
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        // NavMeshAgentを再開
        if (agent != null && !agent.enabled) // agent.enabled が false なら有効化
        {
            agent.enabled = true;
        }
        if (agent != null && agent.isStopped) // agent.isStopped が true なら停止解除
        {
            agent.isStopped = false;
        }
    }



}
