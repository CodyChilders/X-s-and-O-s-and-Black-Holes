/******************************************
 * This is just code for killing players/ *
 * projectiles.  If you are looking for   *
 * how the black hole affects movement of *
 * projectiles or ships, check their      *
 * respective behavior scripts.           *
 *****************************************/

using UnityEngine;
using System.Collections;

public class BlackHoleBehavior : MonoBehaviour
{
    public GameObject p1Ship;
    public GameObject p2Ship;
    public GameObject projectileContainer;

    public float eventHorizon = 55f;

    private Vector3 thisPosition; //constant since this doesn't move
    private InputManager im;

    void Start()
    {
        thisPosition = this.transform.position;
        im = GameObject.Find("Controller Scripts").GetComponent<InputManager>();
    }

    void Update()
    {
        bool p1Hit = CheckForKill(p1Ship);
        bool p2Hit = CheckForKill(p2Ship);
        if (p1Hit)
        {
            im.ResolveContestedBoard(Board.CellStates.P1);
            return;
        }
        if (p2Hit)
        {
            im.ResolveContestedBoard(Board.CellStates.P2);
            return;
        }
        EatProjectiles();
    }

    private bool CheckForKill(GameObject ship)
    {
        Vector3 thatPosition = ship.transform.position;
        float distance = Vector3.Distance(thisPosition, thatPosition);
        return distance <= eventHorizon;
    }

    //check every projectile, destroy the ones that fell in
    private void EatProjectiles()
    {
        GameObject[] allProjectiles = GetAllChildGameObjects.GetAllChildren(projectileContainer);
        for (int i = 0; i < allProjectiles.Length; i++)
        {
            GameObject currentProjectile = allProjectiles[i];
            Vector3 projectilePosition = currentProjectile.transform.position;
            float distance = Vector3.Distance(thisPosition, projectilePosition);
            if (distance < eventHorizon)
            {
                Destroy(currentProjectile);
            }
        }
    }
}
