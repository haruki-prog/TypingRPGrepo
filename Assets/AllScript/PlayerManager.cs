using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerControler : MonoBehaviour
{
    private Rigidbody rigidBody;
    private Animator animator;
    //private float speed = 100f;
    private int currentHP;


    public float PlayerSpeed;
    Vector3 speed = Vector3.zero;
    Vector3 rot = Vector3.zero;
    public Transform Camera;

    bool isRun;


    public Animator PlayerAnimator;


    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        Movement();
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
        //ëäëŒç¿ïW
        transform.Translate(speed);
        PlayerAnimator.SetBool("run", isRun);
    }
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


    void MoveSet()
    {
        speed.z = PlayerSpeed;
        transform.eulerAngles = Camera.transform.eulerAngles + rot;
        isRun = true;
    }
}
