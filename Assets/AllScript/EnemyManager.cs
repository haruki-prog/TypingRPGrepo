using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] Transform target;
   // [SerializeField] EnemyStatusSO enemyStatusSO;
    private NavMeshAgent agent;
    private Animator animator;
    private float speed = 3f;
    private float distance;
   
    private int currentHP;
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
        if (distance < 2)
        {
            
            animator.SetBool("Attack", true);
        }
        else
        {
            animator.SetBool("Attack", false);
        }
    }
    //�_���[�W���[�V�����̐ݒ�A�ԈႢ���肩��
    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Weapon"))
        {
            Debug.Log("Hit!");
            animator.SetTrigger("Damage");

        }
        

    }
  
}
