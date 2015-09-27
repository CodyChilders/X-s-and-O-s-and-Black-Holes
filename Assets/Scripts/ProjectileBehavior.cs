using UnityEngine;
using System.Collections;

public class ProjectileBehavior : MonoBehaviour
{
    public float speed = 20f;
    public float followIntensity = 10f;
    public float maxLifetime = 20f;

    private GameObject target;
    private GameObject blackHole;
    private Vector3 velocity;

    public void Init(GameObject t, GameObject bh, Vector3 forward)
    {
        //Don't hold an internal timer, let Unity handle it when the time has elapsed
        Invoke("DestroyThis", maxLifetime);
        target = t;
        blackHole = bh;
        forward.Normalize();
        velocity = forward * speed;
    }

    void Update()
    {
        this.transform.Translate(velocity);
    }

    //This method gets invoked when its lifetime is up
    private void DestroyThis() 
    {
        Destroy(this.gameObject);
        //TODO: set off particle effects to indicate death
        //requires increasing the time to death slightly
    }
}
