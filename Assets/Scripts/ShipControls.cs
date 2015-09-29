using UnityEngine;
using System.Collections;

public class ShipControls : MonoBehaviour
{
    public GameObject projectileContainer;
    public GameObject projectilePrefab;
    public GameObject otherShip;
    public GameObject blackHole;

    public float blackHolePullStrength = 1f;

    public float maxSpeed = 2f;

    public bool ignoreBlackHolePull = false;

    private float pitchSpeed = 0.5f;
    private float yawSpeed = 0.5f;
    private float rollSpeed = 0.5f;
    private float moveSpeed = 2f;

    private int playerNumber;
    private Vector3 velocity;

    private bool engineOnThisFrame = false;

    private enum Pitch { Up, Down };
    private enum Yaw { Left, Right };
    private enum Roll { Left, Right };

    // Use this for initialization
    void Start()
    {
        AssignPlayerNumber();
    }

    // Update is called once per frame
    void Update()
    {
        if (Debug.isDebugBuild) //debug keyboard controls to be taken out later
        {
            if (Input.GetKeyDown(KeyCode.P)) //primary action
            {
                FireWeapon();
            }
            if (Input.GetKey(KeyCode.S)) //secondary action
            {
                FireEngine();
            }
            if (Input.GetKey(KeyCode.UpArrow))
            {
                PitchMove(Pitch.Down);
            }
            if (Input.GetKey(KeyCode.DownArrow))
            {
                PitchMove(Pitch.Up);
            }
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                YawMove(Yaw.Left);
            }
            if (Input.GetKey(KeyCode.RightArrow))
            {
                YawMove(Yaw.Right);
            }
        }
        else
        {
            //TODO: controller controls
        }
        UpdateVelocityAndMove();
    }

    private void AssignPlayerNumber()
    {
        string name = this.gameObject.name;
        if (name.Contains("1"))
        {
            playerNumber = 1;
        }
        else
        {
            playerNumber = 2;
        }
    }

    private void FireEngine()
    {
        engineOnThisFrame = true;
    }

    private void FireWeapon()
    {
        Vector3 newPosition = this.transform.position;
        GameObject newProjectile = Instantiate(projectilePrefab, newPosition, Quaternion.identity) as GameObject;
        Vector3 newDirection = this.transform.TransformVector(Vector3.forward);
        newProjectile.GetComponent<ProjectileBehavior>().Init(otherShip, blackHole, newDirection);
        newProjectile.transform.parent = projectileContainer.transform;
    }

    private void PitchMove(Pitch p)
    {
        if (p == Pitch.Down)
        {
            this.transform.Rotate(Vector3.right, pitchSpeed);
        }
        else
        {
            this.transform.Rotate(Vector3.right, -pitchSpeed);
        }
    }

    private void YawMove(Yaw y)
    {
        if (y == Yaw.Left)
        {
            this.transform.Rotate(Vector3.up, -yawSpeed);
        }
        else
        {
            this.transform.Rotate(Vector3.up, yawSpeed);
        }
    }

    private void RollMove(Roll r)
    {
        if (r == Roll.Right)
        {
            this.transform.Rotate(Vector3.forward, -rollSpeed);
        }
        else
        {
            this.transform.Rotate(Vector3.forward, rollSpeed);
        }
    }

    private void UpdateVelocityAndMove()
    {
        int velocityChanges = 0;
        //Update if the engine was fired this frame
        if (engineOnThisFrame)
        {
            Vector3 move = this.transform.TransformDirection(Vector3.forward) * moveSpeed;
            velocity += move;
            engineOnThisFrame = false;
            velocityChanges++;
        }
        //update if the black hole isn't turned off for debugging
        if( ! (ignoreBlackHolePull && Debug.isDebugBuild) )
        {
            Vector3 blackHolePull = GetBHPull();
            velocity += blackHolePull;
            velocityChanges++;
        }
        //calculate and move
        if (velocityChanges > 0)
        {
            velocity.Normalize();
            velocity *= maxSpeed;
            this.transform.Translate(velocity, Space.World);
        }
        else
        {
            //no changes were made, so dampen the velocity so they don't fly off into infinity, 
            //also force them to move because this will lead back to the black hole
            velocity *= 0.95f;
        }
    }

    private Vector3 GetBHPull()
    {
        Vector3 here = this.transform.position;
        Vector3 there = blackHole.transform.position;
        Vector3 delta = there - here;
        delta.Normalize();
        delta *= blackHolePullStrength;
        return delta;
    }
}
