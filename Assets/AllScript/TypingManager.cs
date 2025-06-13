using UnityEngine;
using TMPro;

public class TypingManager : MonoBehaviour
{
    public PlayerController playerController;//�v���C���[�̑���X�N���v�g�Q��
    public EnemyManager currentEnemy;//���^�[�Q�b�g�ɂ��Ă���G

    private string currentInput = "";//���͒��̕�������L������ϐ�

    public float detectionRadius = 20f; // �G��T���͈�

    void Start()
    {
        FindClosestEnemy(); // �܂��� SetTargetEnemy() �𖾎��I�ɌĂ�
    }
    void Update()
    {
        // 1. �ł��߂��G��T���ăZ�b�g�i�܂��^�[�Q�b�g�����Ȃ��Ƃ��j
        //���̏�Ԃ��ƃ^�[�Q�b�g���|���܂ōX�V����Ȃ�
        //�����̊֐������ւ���ƃ^�C�s���O�̃^�[�Q�b�g��؂�ւ����B
        FindClosestEnemy();
        if (currentEnemy == null)
        {
            return;
        }

        // �G������ꍇ�̃^�C�s���O����
        //�G�̖��O����Ȃ牽�����Ȃ�
        if (string.IsNullOrEmpty(currentEnemy.typingWord))
            return;

        foreach (char c in Input.inputString)//�ł����������P����������
        {
            currentInput += c;//���͂��ꂽ������currentInput�ɒǉ�
            //���͂��������ꍇ
            if (currentInput.ToLower() == currentEnemy.typingWord.ToLower())//currentInput �� typingWord �Ɗ��S��v�����ꍇ
            {
                if (playerController != null)
                    playerController.TriggerAttack();//TriggerAttack���Ăяo��

               // currentEnemy.typingCount--;�@//�J�E���g��������

                currentInput = "";//���Z�b�g����

              
            }
            //���͂��ԈႢ�̏ꍇ
            //���݂� currentInput �� typingWord �̐擪�ƈ�v���Ȃ��Ȃ�~�X�Ɣ���
            else if (!currentEnemy.typingWord.ToLower().StartsWith(currentInput.ToLower()))
            {
                currentInput = "";//�ԈႦ���u�ԃ��Z�b�g
            }

            if (currentEnemy.typingText != null)
            {
                //���͂��ꂽ������Ԃ�����
                    string full = currentEnemy.typingWord;
                    string typed = currentInput;//���͍ςݕ�����
                    string remaining = full.Substring(typed.Length);//�܂����͂��Ă��Ȃ��c��̕�����
                    //���͂��������ityped�j��Ԃ����āA�c��̕����iremaining�j�ƘA�����ĕ\��
                    string display = $"<color=red>{typed}</color>{remaining}";
                    currentEnemy.typingText.text = display;
                

            }
        }
    }

    // �ł��߂��G��T���֐�
    //�^�O��Enemy�̃I�u�W�F�N�g��T���A�����c���Ă邩�P�ԋ߂��G��currentEnemy�ɂ���
    void FindClosestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");//Enemy�^�O������ꂽ�I�u�W�F�N�g�����ׂĎQ��
        float minDistance = Mathf.Infinity;//��ԋ߂��G�Ƃ̋������ꎞ�I�ɕۑ����邽�߂̕ϐ��i�ŏ��͖�����)
        EnemyManager closest = null;//��ԋ߂��G�� EnemyManager ��ێ�����ϐ�

        foreach (GameObject enemy in enemies)//�S�Ă̓G�ɑ΂��� 1 ���������s�����[�v
        {
            EnemyManager manager = enemy.GetComponent<EnemyManager>();//�e enemy �� EnemyManager �R���|�[�l���g���t���Ă��邩�m�F
            if (manager == null || manager.typingCount <= 0) continue;//���񂾓G�͖���

            float dist = Vector3.Distance(transform.position, enemy.transform.position);//�v���C���[�̈ʒu (transform.position) �ƓG�̈ʒu�Ƃ̋������v�Z
            if (dist < minDistance && dist < detectionRadius)//���܂ł̒��ň�ԋ߂��A���v���C���[�̌��m�͈͓��idetectionRadius�j�Ȃ�X�V
            {
                minDistance = dist;//�ł��߂��G���Ƃ��Ĉꎞ�ۑ�
                closest = manager;
            }
        }

        if (closest != null && currentEnemy != closest)//�ł��߂��ɂ���G�� null �łȂ� ���A���ꂪ ���̃^�[�Q�b�g�ł͂Ȃ��icurrentEnemy�ƈႤ�j�Ȃ�
        {
            SetTargetEnemy(closest);//�^�[�Q�b�g�����̓G�ɍX�V����B
        }
    }

    public void SetTargetEnemy(EnemyManager enemy)
    {
        Debug.Log("SetTargetEnemy �Ăяo��:"+enemy.name);
        currentEnemy = enemy;//currentEnemy���X�V
        currentInput = "";//input�����Z�b�g
        
        if (enemy.typingText != null)//�G���\���p�e�L�X�g�itypingText�j�������Ă���΁A���̓��e�� typingWord�i���̓G�ɐݒ肳�ꂽ�P��j�ŏ㏑��
        {
            enemy.typingText.text = enemy.typingWord;
        }
        Debug.Log("�Z�b�g��@currentEnemy=" + currentEnemy);
    }
}
