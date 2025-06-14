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

    public Animator EnemyAnimator;  //�G�̃A�j���[�^�[���i�[����ϐ�
    public Collider AttackingPlayerCollider;    //�v���C���[���U�����̓G�̓����蔻����i�[����Collider�^�ϐ�

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

    [SerializeField] public string typingWord ;//�^���镶����
    [SerializeField] public int typingCount;//�^�C�s���O�������
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

    


    //�G���ǂ������Ă���͈͂̐ݒ�
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
    
    //�U�����[�V�����̐ݒ�
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

    void AttackModeON() //�G�̓����蔻���؂�ւ���֐�
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
           
            // TypingManager �� currentEnemy ���擾
            TypingManager typingManager = Object.FindFirstObjectByType<TypingManager>();
            Debug.Log("== Trigger ���̊m�F ==");
            Debug.Log($"TypingManager: {typingManager}");
            Debug.Log($"currentEnemy: {typingManager?.currentEnemy}");
            Debug.Log($"This:{this},CrrentEnemy:{typingManager.currentEnemy}");
            if (typingManager == null)
            {
                Debug.LogWarning("TypingManager ��������܂���");
                return;
            }
            if (typingManager.currentEnemy == null)
            {
                Debug.LogWarning("currentEnemy �� null �ł�");
                return;
            }
         

            // �Ώۂ̓G�ȊO�͖���
            if (typingManager.currentEnemy.gameObject != this.gameObject)
            {
                Debug.Log("�U�����ꂽ���A�ΏۊO�̂��ߖ���: " + gameObject.name);
                return;
            }
            Debug.Log("Hit2!");
            typingCount--;  // �^�C�s���O�J�E���g�������Ō��炷

            //�^�_������effect,SE
            audioSource.PlayOneShot(HitSE);
            Vector3 effectPosition = transform.position + new Vector3(0, 1.5f, 0);
            GameObject effect = Instantiate(DamageEffect, effectPosition, Quaternion.identity);
            Destroy(effect, 2);


            // typingCount �� 0 �ȉ��Ȃ玀�S����
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

                Destroy(gameObject); // �G���폜
                Debug.Log("�|��܂���");
            }
            else
            {

                Debug.Log("�܂��|��܂���B�c��F" + typingCount);
                EnemyAnimator.SetTrigger("Die");
               
            }
        }
    }




}
