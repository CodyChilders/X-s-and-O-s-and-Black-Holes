using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UICountdownTimer : MonoBehaviour {
    public float time = 1f;
    public Text nextElement;

    void OnEnable()
    {
        Invoke("Transition", time);
    }

    public void Transition()
    {
        DisableThis();
        EnableNext();
    }

    public void DisableThis()
    {
        this.gameObject.SetActive(false);
    }

    public void EnableNext()
    {
        if (nextElement != null)
            nextElement.gameObject.SetActive(true);
    }
}
