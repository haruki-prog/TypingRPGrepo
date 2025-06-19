using UnityEngine;
using UnityEngine.SceneManagement;

public class SceanChanger : MonoBehaviour
{
    public string SceanName;
    public AudioSource audioSource;
    public AudioClip ClickSE;
    public void SceanChange()
    {
        audioSource.PlayOneShot(ClickSE);
        SceneManager.LoadScene(SceanName);
    }
}

