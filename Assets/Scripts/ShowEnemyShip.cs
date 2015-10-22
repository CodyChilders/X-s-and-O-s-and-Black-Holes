using UnityEngine;
using System.Collections;
using System.Text;

public class ShowEnemyShip : MonoBehaviour
{
    public GameObject cam;
    private Camera c;

    public GameObject otherShip;

    public Texture bullseye;
    public float imageWidth = 100f;
    public float imageHeight = 100f;

    public Color targetColor = Color.red;
    public Color textColor = Color.red;

    public Font font;

    public int playerNumber = 0;

    private GUIStyle gs = new GUIStyle();
    private GUIStyle textStyle = new GUIStyle();

    private StringBuilder builder = new StringBuilder();

    void Start()
    {
        c = cam.GetComponent<Camera>();
        textStyle.normal.textColor = textColor;
        textStyle.font = font;
    }

    void OnGUI()
    {
        Vector3 screenPoint = c.WorldToScreenPoint(otherShip.transform.position);
        Vector2 displayPoint = new Vector2(screenPoint.x, Screen.height - screenPoint.y);
        displayPoint.x -= imageWidth  / 2f;
        displayPoint.y -= imageHeight / 2f;
        //it might be on the screen, but on the other player's side.  skip the rest of the method if so
        if (ShouldSkipDrawingHud(screenPoint, displayPoint))
        {
            return;
        }
        GUI.color = targetColor;
        GUI.Box(new Rect(displayPoint.x, displayPoint.y, imageWidth, imageHeight), 
                bullseye,
                gs);
        builder.Length = 0;
        builder.Append("Target: ");
        builder.Append(GetDistance());
        builder.Append(" meters");
        GUI.Label(new Rect(displayPoint.x, displayPoint.y + imageHeight, 1000, 1000),
                  builder.ToString(),
                  textStyle);
    }

    private bool ShouldSkipDrawingHud(Vector3 screenPoint, Vector2 displayPoint)
    {
        return IsHudOnWrongPlayersSide(displayPoint) || IsPointInWrongDirection(screenPoint);
    }

    private bool IsHudOnWrongPlayersSide(Vector2 displayPoint)
    {
        float xCoordinate = displayPoint.x + imageWidth / 2;
        if (playerNumber == 1)
        {
            if (xCoordinate > Screen.width / 2)
                return true;
        }
        else if (playerNumber == 2)
        {
            if (xCoordinate < Screen.width / 2)
                return true;
        }
        else
        {
            return true;
        }
        return false;
    }

    private bool IsPointInWrongDirection(Vector3 screenPoint)
    {
        Vector3 heading = otherShip.transform.position - cam.transform.position;
        if (Vector3.Dot(cam.transform.forward, heading) > 0) 
        {
            // Object is in front.
            return false;
        }
        else
        {
            return true;
        }
    }

    private string GetDistance()
    {
        Vector3 thisPos = this.transform.position;
        Vector3 thatPos = otherShip.transform.position;
        float distance = Vector3.Distance(thisPos, thatPos);
        string val = string.Format("{0:0.0}", distance);
        return val;
    }
}
