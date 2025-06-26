using UnityEngine;

public class FireBreath : MonoBehaviour
{
    // InspectorでFirePointの子オブジェクトであるFX_Fire_06_2を割り当てる
    public ParticleSystem fireBreathParticleSystem;

    // 任意: ドラゴンが炎を吐くアニメーションを持っている場合
    // public Animator dragonAnimator; 

    void Update()
    {
        // ここをスペースキーの入力検出に変更します
        if (Input.GetKeyDown(KeyCode.Space)) // スペースキーが押された瞬間
        {
            
                Debug.Log("火をふく (スペースキー)"); // ログも修正して分かりやすく
                fireBreathParticleSystem.Play(); // 炎を再生
                // 任意: アニメーションを再生する場合
                // if (dragonAnimator != null)
                // {
                //     dragonAnimator.SetTrigger("FireBreathTrigger"); // 例: アニメーターのトリガー
                // }
            
        }
        if (Input.GetKeyUp(KeyCode.Space)) // スペースキーが離された瞬間
        {
            
                Debug.Log("火を止める (スペースキー)"); // ログも追加
                fireBreathParticleSystem.Stop(); // 炎を停止
                // 炎の余韻を残したい場合は、StopではなくStop(true, ParticleSystemStopBehavior.StopEmitting)などを使うと良い場合もあります。
            
        }
    }

    // アニメーションイベントから呼び出すためのメソッド
    public void StartFireBreath()
    {
        if (fireBreathParticleSystem != null)
        {
            fireBreathParticleSystem.Play();
        }
    }

    public void StopFireBreath()
    {
        if (fireBreathParticleSystem != null)
        {
            fireBreathParticleSystem.Stop();
        }
    }
}
