using UnityEngine;
using System.Collections;

public class ShipControls : MonoBehaviour
{
    public GameObject projectileContainer;
    public GameObject projectilePrefab;
    public GameObject otherShip;
    public GameObject blackHole;

    private float pitchSpeed = 0.5f;
    private float yawSpeed = 0.5f;
    private float rollSpeed = 0.5f;
    private float moveSpeed = 2f;

    private int playerNumber;

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
        Vector3 move = Vector3.forward * moveSpeed;
        this.transform.Translate(move);
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
}
