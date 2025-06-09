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

    public Animator EnemyAnimator;  //�G�̃A�j���[�^�[���i�[����ϐ�
    public Collider AttackingPlayerCollider;    //�v���C���[���U�����̓G�̓����蔻����i�[����Collider�^�ϐ�

    public AudioSource audioSource;
    public AudioClip HitSE;


    public string typingWord = "slime"; // �X���C���Ȃ� "slime"
    public TMP_Text typingText;         // Canvas��ɕ\������Text�i���[���h�X�y�[�X�ł��j

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




    //�G���ǂ������Ă���͈͂̐ݒ�
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

    //�U�����[�V�����̐ݒ�
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


    //�_���[�W���[�V�����̐ݒ�A�ԈႢ���肩�� [�ǋL:�������܂���]
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
            deathSE.transform.SetParent(null); // ��������e����O��
            deathSE.Play();
            Destroy(deathSE.gameObject, deathSE.clip.length); // �Đ����I������^�C�~���O�ŏ����悤�ɂ���


            Destroy(gameObject);    //�ꌂ�Ŏ��ʂ悤�ɂ��Ă܂�

        }


    }

}
