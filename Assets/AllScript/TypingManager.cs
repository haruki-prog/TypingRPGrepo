using UnityEngine;
using TMPro;

public class TypingManager : MonoBehaviour
{
    public PlayerController playerController;//プレイヤーの操作スクリプト参照
    public EnemyManager currentEnemy;//今ターゲットにしている敵

    private string currentInput = "";//入力中の文字列を記憶する変数

    public float detectionRadius = 20f; // 敵を探す範囲

    void Start()
    {
        FindClosestEnemy(); // または SetTargetEnemy() を明示的に呼ぶ
    }
    void Update()
    {
        // 1. 最も近い敵を探してセット（まだターゲットがいないとき）
        //今の状態だとターゲットが倒れるまで更新されない
        //ここの関数を入れ替えるとタイピングのターゲットを切り替えれる。
        FindClosestEnemy();
        if (currentEnemy == null)
        {
            return;
        }

        // 敵がいる場合のタイピング処理
        //敵の名前が空なら何もしない
        if (string.IsNullOrEmpty(currentEnemy.typingWord))
            return;

        foreach (char c in Input.inputString)//打った文字を１文字ずつ処理
        {
            currentInput += c;//入力された文字をcurrentInputに追加
            //入力が正しい場合
            if (currentInput.ToLower() == currentEnemy.typingWord.ToLower())//currentInput が typingWord と完全一致した場合
            {
                if (playerController != null)
                    playerController.TriggerAttack();//TriggerAttackを呼び出す

               // currentEnemy.typingCount--;　//カウントを下げる

                currentInput = "";//リセットする

              
            }
            //入力が間違いの場合
            //現在の currentInput が typingWord の先頭と一致しないならミスと判定
            else if (!currentEnemy.typingWord.ToLower().StartsWith(currentInput.ToLower()))
            {
                currentInput = "";//間違えた瞬間リセット
            }

            if (currentEnemy.typingText != null)
            {
                //入力された文字を赤くする
                    string full = currentEnemy.typingWord;
                    string typed = currentInput;//入力済み文字列
                    string remaining = full.Substring(typed.Length);//まだ入力していない残りの文字列
                    //入力した文字（typed）を赤くして、残りの文字（remaining）と連結して表示
                    string display = $"<color=red>{typed}</color>{remaining}";
                    currentEnemy.typingText.text = display;
                

            }
        }
    }

    // 最も近い敵を探す関数
    //タグがEnemyのオブジェクトを探し、生き残ってるかつ１番近い敵をcurrentEnemyにする
    void FindClosestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");//Enemyタグをつけられたオブジェクトをすべて参照
        float minDistance = Mathf.Infinity;//一番近い敵との距離を一時的に保存するための変数（最初は無限大)
        EnemyManager closest = null;//一番近い敵の EnemyManager を保持する変数

        foreach (GameObject enemy in enemies)//全ての敵に対して 1 つずつ処理を行うループ
        {
            EnemyManager manager = enemy.GetComponent<EnemyManager>();//各 enemy に EnemyManager コンポーネントが付いているか確認
            if (manager == null || manager.typingCount <= 0) continue;//死んだ敵は無視

            float dist = Vector3.Distance(transform.position, enemy.transform.position);//プレイヤーの位置 (transform.position) と敵の位置との距離を計算
            if (dist < minDistance && dist < detectionRadius)//今までの中で一番近く、かつプレイヤーの検知範囲内（detectionRadius）なら更新
            {
                minDistance = dist;//最も近い敵情報として一時保存
                closest = manager;
            }
        }

        if (closest != null && currentEnemy != closest)//最も近くにいる敵が null でない かつ、それが 今のターゲットではない（currentEnemyと違う）なら
        {
            SetTargetEnemy(closest);//ターゲットをその敵に更新する。
        }
    }

    public void SetTargetEnemy(EnemyManager enemy)
    {
        Debug.Log("SetTargetEnemy 呼び出し:"+enemy.name);
        currentEnemy = enemy;//currentEnemyを更新
        currentInput = "";//inputをリセット
        
        if (enemy.typingText != null)//敵が表示用テキスト（typingText）を持っていれば、その内容を typingWord（その敵に設定された単語）で上書き
        {
            enemy.typingText.text = enemy.typingWord;
        }
        Debug.Log("セット後　currentEnemy=" + currentEnemy);
    }
}
