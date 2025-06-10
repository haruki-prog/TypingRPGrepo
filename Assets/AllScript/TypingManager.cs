using UnityEngine;
using UnityEngine.AI;

public class TypingManager : MonoBehaviour
{
    public PlayerController playerController; // ← プレイヤーの参照
    public EnemyManager currentEnemy;         // ← 現在攻撃対象の敵
    private string currentInput = "";
  

    void Update()
    {
        foreach (char c in Input.inputString)
        {
            currentInput += c;

            // 敵がいないなら無視
            if (currentEnemy == null) return;

            // ▼ ここがあなたの質問のコードです！ ▼
            if (currentInput.ToLower() == currentEnemy.typingWord.ToLower())
            {
                playerController.TriggerAttack();  // プレイヤー攻撃
                currentEnemy.typingCount--;

                currentInput = ""; // 入力リセット

                if (currentEnemy.typingCount <= 0)
                {
                    currentEnemy.Die(); // 敵を倒す処理
                }
            }
            // 間違ってる文字列ならリセット（任意）
            else if (!currentEnemy.typingWord.StartsWith(currentInput.ToLower()))
            {
                currentInput = "";
            }

            // 表示更新（任意）
            if (currentEnemy.typingText != null)
            {
                currentEnemy.typingText.text = currentEnemy.typingWord + "\n" + currentInput;
            }
        }
    }
}
