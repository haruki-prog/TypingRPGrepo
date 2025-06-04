using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//敵の自動生成、多分Vtuberからそのまま
public class EnemyCreative : MonoBehaviour
{
    public GameObject Enemy1;
    public GameObject Enemy2;

    public Transform EnemyPlace1;
    public Transform EnemyPlace2;
    [SerializeField] Transform target;/*追いかけるターゲットをヒエラルキー内から宣言しているためプレハブ化できなくて不便
     なので、タグかレイヤーで設定する方がいいと思う。今後メインキャラを増やすことも考えたら後者の方がよい。
    注意：シリアライズとプレハブ化は共存できない！プレハブ化したいなら追尾のソースコードを変えるしかない。*/


    float TimeCount;

    public int Count, MaxCount;     //Count:現在の敵の数を格納する変数
                                    //MaxCount:Scean上に発生できる敵の数の上限値を格納する変数
                                   
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (MaxCount <= Count)  //敵の数が上限値を超えたらreturn
        {
            return;
        }

        TimeCount += Time.deltaTime;
        if(TimeCount > 5)
        {
            Instantiate(Enemy1, EnemyPlace1.position, Quaternion.identity);
            Count++;    //敵の数の値を１増加
            Instantiate(Enemy2, EnemyPlace2.position, Quaternion.identity);
            Count++;    //敵の数の値を１増加
            TimeCount = 0;
        }
        
    }

    
}
