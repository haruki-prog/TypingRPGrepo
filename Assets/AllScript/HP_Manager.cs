using UnityEngine;
using UnityEngine.UI;

public class HP_Manager : MonoBehaviour
{
    
    public int HP;
    public int MaxHP;
    
    public Image HPGage;


    //public GameObject Effect;
    //public AudioSource audioSource;
    //public AudioClip HitSE;

    public Collider collider;
    public float ResetTime = 0;

   
    private void Update()
    {
        if (HP <= 0)
        {
            HP = 0;
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
        if (other.tag == "AttackingEnemy")
        {
            Damage();
            collider.enabled = false;
            Invoke("ColliderReset", ResetTime);
        }
    }

    void Damage()
    {
        //audioSource.PlayOneShot(HitSE);
        HP--;
    }

    void ColliderReset()
    {
        collider.enabled = true;
    }
}
