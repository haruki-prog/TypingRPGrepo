using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class HP_Manager : MonoBehaviour
{
    
    public int HP;
    public int MaxHP;
    
    public Image HPGage;


    //public GameObject Effect;
    public AudioSource audioSource;
    public AudioClip HitSE;
    public AudioClip DeathSE;

    public Collider collider;
    public float ResetTime = 0;

    public Animator PlayerAnimator; //プレイヤーアニメーターを使用

    public PlayerController plc; //スクリプトを継承

    public GameObject DamageEffect;
    private void Update()
    {
        if (HP <= 0)
        {
            HP = 0;
            audioSource.PlayOneShot(DeathSE);
            
            Destroy(gameObject);
        }
        float percent = (float)HP / MaxHP;
        HPGage.fillAmount = percent;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "AttackingPlayer")
        {
            Damage();
            collider.enabled = false;
            Invoke("ColliderReset", ResetTime);
        }
    }

    void Damage()
    {
        audioSource.PlayOneShot(HitSE);
        var effect = Instantiate(DamageEffect);
        var pos= transform.position;
        pos.y += 1.0f;
        effect.transform.position = pos;
        Destroy(effect, 5);
        HP--;
        PlayerAnimator.SetBool("hittobody", true);
        plc.canMove = false;

    }

    void StandUp()
    {
        PlayerAnimator.SetBool("hittobody", false);
        plc.canMove = true;

    }

    void ColliderReset()
    {
        collider.enabled = true;
    }
}
