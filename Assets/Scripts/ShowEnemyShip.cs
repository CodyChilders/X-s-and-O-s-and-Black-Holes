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

    private GUIStyle gs = new GUIStyle();
    private GUIStyle textStyle = new GUIStyle();

    void Start()
    {
        c = cam.GetComponent<Camera>();
        textStyle.normal.textColor = textColor;
        textStyle.font = font;
    }

    void OnGUI()
    {
        Vector3 screenPoint = c.WorldToScreenPoint(otherShip.transform.position);
        Vector2 displayPoint = new Vector2(screenPoint.x, screenPoint.y);
        displayPoint.x -= imageWidth  / 2f;
        displayPoint.y -= imageHeight / 2f;
        GUI.color = targetColor;
        GUI.Box(new Rect(displayPoint.x, displayPoint.y, imageWidth, imageHeight), 
                bullseye,
                gs);
        StringBuilder sb = new StringBuilder();
        sb.Append("Target: ");
        sb.Append(GetDistance());
        sb.Append(" meters");
        GUI.Label(new Rect(displayPoint.x, displayPoint.y + imageHeight, 1000, 1000),
                  sb.ToString(),
                  textStyle);
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
