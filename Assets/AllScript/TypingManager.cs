using UnityEngine;
using UnityEngine.AI;

public class TypingManager : MonoBehaviour
{
    public PlayerController playerController; // �� �v���C���[�̎Q��
    public EnemyManager currentEnemy;         // �� ���ݍU���Ώۂ̓G
    private string currentInput = "";
  

    void Update()
    {
        foreach (char c in Input.inputString)
        {
            currentInput += c;

            // �G�����Ȃ��Ȃ疳��
            if (currentEnemy == null) return;

            // �� ���������Ȃ��̎���̃R�[�h�ł��I ��
            if (currentInput.ToLower() == currentEnemy.typingWord.ToLower())
            {
                playerController.TriggerAttack();  // �v���C���[�U��
                currentEnemy.typingCount--;

                currentInput = ""; // ���̓��Z�b�g

                if (currentEnemy.typingCount <= 0)
                {
                    currentEnemy.Die(); // �G��|������
                }
            }
            // �Ԉ���Ă镶����Ȃ烊�Z�b�g�i�C�Ӂj
            else if (!currentEnemy.typingWord.StartsWith(currentInput.ToLower()))
            {
                currentInput = "";
            }

            // �\���X�V�i�C�Ӂj
            if (currentEnemy.typingText != null)
            {
                currentEnemy.typingText.text = currentEnemy.typingWord + "\n" + currentInput;
            }
        }
    }
}
