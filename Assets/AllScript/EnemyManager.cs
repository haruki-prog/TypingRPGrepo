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

    public Animator EnemyAnimator;  //�G�̃A�j���[�^�[���i�[����ϐ�
    public Collider AttackingPlayerCollider;    //�v���C���[���U�����̓G�̓����蔻����i�[����Collider�^�ϐ�

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
    //�G���ǂ������Ă���͈͂̐ݒ�
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
        AttackingPlayerCollider.enabled = true;
    }
    void AttackModeOFF()
    {
        AttackingPlayerCollider.enabled = false;
        EnemyAnimator.SetBool("Attack", false);
    }


    //�_���[�W���[�V�����̐ݒ�A�ԈႢ���肩�� [�ǋL:�������܂���]
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
            Destroy(gameObject);    //�ꌂ�Ŏ��ʂ悤�ɂ��Ă܂�

        }
        

    }
  
}
