using UnityEngine;
using TMPro;

public class TypingManager : MonoBehaviour
{
    public PlayerController playerController;
    public EnemyManager currentEnemy;

    private string currentInput = "";

    public float detectionRadius = 15f; // �G��T���͈�

    void Update()
    {
        // 1. �ł��߂��G��T���ăZ�b�g�i�܂��^�[�Q�b�g�����Ȃ��Ƃ��j
        FindClosestEnemy();
        if (currentEnemy == null)
        {
            return;
        }

        // �G������ꍇ�̃^�C�s���O����
        if (string.IsNullOrEmpty(currentEnemy.typingWord))
            return;

        foreach (char c in Input.inputString)
        {
            currentInput += c;

            if (currentInput.ToLower() == currentEnemy.typingWord.ToLower())
            {
                if (playerController != null)
                    playerController.TriggerAttack();

                currentEnemy.typingCount--;

                currentInput = "";

                /*if (currentEnemy.typingCount <= 0)
                {
                    currentEnemy.Die();
                    currentEnemy = null;
                }*/
            }
            else if (!currentEnemy.typingWord.ToLower().StartsWith(currentInput.ToLower()))
            {
                currentInput = "";
            }

            if (currentEnemy.typingText != null)
            {
                currentEnemy.typingText.text = currentEnemy.typingWord + "\n" + currentInput;
            }
        }
    }

    // �ł��߂��G��T���֐�
    void FindClosestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        float minDistance = Mathf.Infinity;
        EnemyManager closest = null;

        foreach (GameObject enemy in enemies)
        {
            EnemyManager manager = enemy.GetComponent<EnemyManager>();
            if (manager == null || manager.typingCount <= 0) continue;

            float dist = Vector3.Distance(transform.position, enemy.transform.position);
            if (dist < minDistance && dist < detectionRadius)
            {
                minDistance = dist;
                closest = manager;
            }
        }

        if (closest != null && currentEnemy != closest)
        {
            SetTargetEnemy(closest);
        }
    }

    public void SetTargetEnemy(EnemyManager enemy)
    {
        currentEnemy = enemy;
        currentInput = "";

        if (enemy.typingText != null)
        {
            enemy.typingText.text = enemy.typingWord;
        }
    }
}
