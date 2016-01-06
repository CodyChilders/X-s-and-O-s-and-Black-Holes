using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class DisableObject : MonoBehaviour {

    public float fadeInTime = .25f;
    public float displayTime = 2f;
    public float fadeOutTime = 1f;

    /*
    void Awake()
    {
        DOTween.Init(); 
    }

    void OnEnable()
    {
        this.gameObject.GetComponent<Image>().DOFade(1, fadeInTime);
        Invoke("Fade", displayTime);
        Invoke("Disable", displayTime + fadeOutTime);
    }

    private void Disable()
    {
        this.gameObject.SetActive(false);
    }

    private void Fade()
    {
        Image i = this.gameObject.GetComponent<Image>();
        i.DOFade(0, fadeOutTime);
    }
    */
}
