﻿using UnityEngine;
using System.Collections;

public class ProjectileBehavior : MonoBehaviour
{
    public float speed = 20f;
    public float followIntensity = 10f;
    public float blackHoleGravity = 8f;
    public float maxLifetime = 20f;

    public float distanceNeededForKill = 5f;

    public enum WeaponPower { zero, over9000 };
    public WeaponPower damage = WeaponPower.over9000;

    private GameObject target;
    private GameObject blackHole;
    private Vector3 velocity;

    private InputManager im;

    public void Init(GameObject t, GameObject bh, Vector3 forward)
    {
        //Don't hold an internal timer, let Unity handle it when the time has elapsed
        Invoke("DestroyThis", maxLifetime);
        target = t;
        blackHole = bh;
        forward.Normalize();
        velocity = forward * speed;
        im = GameObject.Find("Controller Scripts").GetComponent<InputManager>();
    }

    void Update()
    {
        UpdateVelocity();
        this.transform.Translate(velocity);
        CheckForKill();
    }

    private void UpdateVelocity()
    {
        Vector3 normalizedVectorToBH = GetDirection(this.transform.position, blackHole.transform.position);
        Vector3 normalizedVectorToTarget = GetDirection(this.transform.position, target.transform.position);
        Vector3 bhPull = normalizedVectorToBH * blackHoleGravity;
        Vector3 targetPull = normalizedVectorToTarget * followIntensity;
        velocity += bhPull + targetPull;
        velocity.Normalize();
        velocity *= speed;
    }

    private Vector3 GetDirection(Vector3 a, Vector3 b)
    {
        Vector3 direction = b - a;
        direction.Normalize();
        return direction;
    }

    //This method gets invoked when its lifetime is up
    private void DestroyThis() 
    {
        Destroy(this.gameObject);
        //TODO: set off particle effects to indicate death
        //requires increasing the time to death slightly
    }

    private void CheckForKill()
    {
        if (damage == WeaponPower.zero && Debug.isDebugBuild)
            return;
        Vector3 here = this.transform.position;
        Vector3 there = target.transform.position;
        float distance = Vector3.Distance(here, there);
        if (distance < distanceNeededForKill)
        {
            im.ResolveContestedBoard(target);
        }
    }
}