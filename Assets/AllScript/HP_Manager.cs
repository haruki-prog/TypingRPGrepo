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

    public Animator PlayerAnimator; //�v���C���[�A�j���[�^�[���g�p

    public PlayerController plc; //�X�N���v�g���p��


    private void Update()
    {
        if (HP <= 0)
        {
            HP = 0;
            audioSource.PlayOneShot(DeathSE);
            //var effect = Instantiate(Effect);
            //effect.transform.position = transform.position;
            //Destroy(effect, 5);
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
