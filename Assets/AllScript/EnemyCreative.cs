using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�G�̎��������A����Vtuber���炻�̂܂�
public class EnemyCreative : MonoBehaviour
{
    public GameObject Enemy1;
    public GameObject Enemy2;

    public Transform EnemyPlace1;
    public Transform EnemyPlace2;
    [SerializeField] Transform target;/*�ǂ�������^�[�Q�b�g���q�G�����L�[������錾���Ă��邽�߃v���n�u���ł��Ȃ��ĕs��
     �Ȃ̂ŁA�^�O�����C���[�Őݒ肷����������Ǝv���B���チ�C���L�����𑝂₷���Ƃ��l�������҂̕����悢�B
    ���ӁF�V���A���C�Y�ƃv���n�u���͋����ł��Ȃ��I�v���n�u���������Ȃ�ǔ��̃\�[�X�R�[�h��ς��邵���Ȃ��B*/


    float TimeCount;

    public int Count, MaxCount;     //Count:���݂̓G�̐����i�[����ϐ�
                                    //MaxCount:Scean��ɔ����ł���G�̐��̏���l���i�[����ϐ�
                                   
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (MaxCount <= Count)  //�G�̐�������l�𒴂�����return
        {
            return;
        }

        TimeCount += Time.deltaTime;
        if(TimeCount > 5)
        {
            Instantiate(Enemy1, EnemyPlace1.position, Quaternion.identity);
            Count++;    //�G�̐��̒l���P����
            Instantiate(Enemy2, EnemyPlace2.position, Quaternion.identity);
            Count++;    //�G�̐��̒l���P����
            TimeCount = 0;
        }
        
    }

    
}
