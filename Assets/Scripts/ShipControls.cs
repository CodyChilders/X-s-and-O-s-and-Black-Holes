using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;

public class ShipControls : MonoBehaviour
{
    public GameObject gameController;
    public GameObject projectileContainer;
    public GameObject projectilePrefab;
    public GameObject otherShip;
    public GameObject blackHole;
    public GameObject engineParticles;
    private ParticleSystem engine;
    private AudioManager audio;

    public float blackHolePullStrength = 1f;

    public float maxSpeed = 2f;

    public int maxNumberOfProjectiles = 10;
    private LinkedList<GameObject> firedProjectiles;

    public bool ignoreBlackHolePull = false;

    private float pitchSpeed = 0.5f;
    private float yawSpeed = 0.5f;
    private float rollSpeed = 0.5f;
    private float moveSpeed = 2f;

    private int playerNumber;
    private Vector3 velocity;

    private bool engineOnThisFrame = false;

    public float weaponCooldownSeconds = 0.5f;
    private bool weaponCooldownElapsed = true;

    private string horizontalKey;
    private string verticalKey;
    private string primaryKey;
    private string secondaryKey;

    // Use this for initialization
    void Start()
    {
        AssignPlayerNumber();
        firedProjectiles = new LinkedList<GameObject>();
        SetKeys();
        engine = engineParticles.GetComponent<ParticleSystem>();
        audio = gameController.GetComponent<AudioManager>();
    }

    // Update is called once per frame
    void Update()
    {
        #region debug keyboard controls
        /*
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
        */
        #endregion
        #region resolving button presses
        float horizontalAxis = Input.GetAxis(horizontalKey);
        YawMove(horizontalAxis);
        float verticalAxis = Input.GetAxis(verticalKey);
        PitchMove(verticalAxis);
        bool firedWeapon = Input.GetButtonDown(primaryKey);
        bool firedEngine = Input.GetButton(secondaryKey);
        ProcessEngine(firedEngine);
        UpdateVelocityAndMove();
        if (firedWeapon)
        {
            FireWeapon();
        }
        #endregion
    }

    #region Button testing code
    /*
    void OnGUI()
    {
        if (playerNumber == 2)
            return;
        const float width = 500f;
        const float height = 100f;
        float posX = Screen.width / 2f - width / 2f;
        float posY = Screen.height / 2f - height / 2f;
        Rect screen = new Rect(posX, posY, width, height);
        string text;
        try
        {
            bool a = Input.GetButton("P1_Primary");
            float b = Input.GetAxis("P1_Secondary");
            float c = Input.GetAxis("P1_Horizontal");
            float d = Input.GetAxis("P1_Vertical");
            text = string.Format("Prim:\t{0}\nScnd:\t{1}\nHorz:\t{2}\nVert:\t{3}", a, b, c, d);
        }
        catch (System.Exception e)
        {
            text = e.ToString();
        }
        GUI.Box(screen, text);
    }
     * */
    #endregion

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

    private void SetKeys()
    {
        string prefix = "P" + playerNumber + "_";
        horizontalKey = prefix + "Horizontal";
        verticalKey = prefix + "Vertical";
        primaryKey = prefix + "Primary";
        secondaryKey = prefix + "Secondary";
    }

    private void ProcessEngine(bool pressed)
    {
        if (pressed)
        {
            FireEngine();
            engine.Emit(5); //Throw a few particles out to indicate that the engine is on
        }
    }

    private void FireEngine()
    {
        engineOnThisFrame = true;
    }

    private void FireWeapon()
    {
        if (!weaponCooldownElapsed)
        {
            audio.SWFailFireWeapon();
            return;
        }
        UpdateProjectileList();
        if (firedProjectiles.Count > maxNumberOfProjectiles)
        {
            audio.SWFailFireWeapon();
            return; //there are too many projectiles out, don't fire
        }
        Vector3 newPosition = this.transform.position;
        GameObject newProjectile = Instantiate(projectilePrefab, newPosition, Quaternion.identity) as GameObject;
        Vector3 newDirection = this.transform.TransformVector(Vector3.forward);
        newProjectile.GetComponent<ProjectileBehavior>().Init(otherShip, blackHole, newDirection, audio);
        newProjectile.transform.parent = projectileContainer.transform;
        firedProjectiles.AddLast(newProjectile);
        audio.SWFireWeapon();
        Invoke("ResetWeaponCooldownFlag", weaponCooldownSeconds);
    }

    //This method is invoked on a timer based on the weapon cooldown.  It just trips the flag to allow the gun to fire again
    private void ResetWeaponCooldownFlag()
    {
        weaponCooldownElapsed = true;
    }

    private void UpdateProjectileList()
    {
        //since they are created (then destroyed) in order, only check the first element
        LinkedListNode<GameObject> first = firedProjectiles.First;
        if (first == null)
            return; //the list is empty, this is totally fine
        GameObject firstGO = first.Value;
        if (firstGO == null) //Set to null when Unity cleans up the memory with Destroy()
        {
            firedProjectiles.RemoveFirst();
        }
    }

    private void PitchMove(float intensity)
    {
        this.transform.Rotate(Vector3.right, intensity * pitchSpeed * -1);
    }

    private void YawMove(float intensity)
    {
        this.transform.Rotate(Vector3.up, intensity * yawSpeed);
    }

    private void RollMove(float intensity)
    {
        this.transform.Rotate(Vector3.forward, intensity * rollSpeed);
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
            /*
            //no changes were made, so dampen the velocity so they don't fly off into infinity, 
            //also force them to move because this will lead back to the black hole
            velocity *= 0.99f; //0.99 because at 60ish fps, that adds up faster than you'd think
             * */
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
