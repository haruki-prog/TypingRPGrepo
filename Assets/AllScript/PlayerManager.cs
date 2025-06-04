using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerControler : MonoBehaviour
{
    private Rigidbody rigidBody;
    private Animator animator;
    
    private int currentHP;


    public float PlayerSpeed;
    Vector3 speed = Vector3.zero;
    Vector3 rot = Vector3.zero;
    public Transform Camera;

    bool isRun;


    public Animator PlayerAnimator;
    public Collider WeaponCollider;


    public float RotationSpeed;         //RotationSpeed:���_�ړ��̊��x���i�[����ϐ�

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
        AttackMotion();

    }

    void Movement()
    {
        speed = Vector3.zero;
        rot = Vector3.zero;
        isRun = false;
        /*if (!canMove)
        {
            return;
        }*/

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
        //���΍��W
        transform.Translate(speed);
        PlayerAnimator.SetBool("run", isRun);
    }

    void MoveSet()
    {
        speed.z = PlayerSpeed;
        transform.eulerAngles = Camera.transform.eulerAngles + rot;
        isRun = true;
    }


    //�U�����[�V�����̐ݒ�
    void AttackMotion()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            animator.SetBool("attack", true);
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            animator.SetBool("attack", false);
        }
    }
    
    //�A�j���[�V�����̓����蔻��̐ݒ�B�@�@�ԈႢ���肩��
    
    void WeaponON()
    {
        WeaponCollider.enabled = true;
    }
    void WeaponOFF()
    {
        WeaponCollider.enabled = false;
        PlayerAnimator.SetBool("attack", false);
    }


    //���_�ړ��̊֐�
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


}
