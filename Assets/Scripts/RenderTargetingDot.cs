using UnityEngine;
using System.Collections;

public class RenderTargetingDot : MonoBehaviour
{
    public Texture dot;
    public GameObject cameraObject;
    private Camera cam;

    public int imageWidth = 16;
    public int imageHeight = 16;
    public Color color = Color.white;

    private GUIStyle gs = new GUIStyle();

    void Start()
    {
        cam = cameraObject.GetComponent<Camera>();
    }

    void OnGUI()
    {
        Vector3 pos = cam.WorldToScreenPoint(this.transform.position);
        Vector2 point = new Vector2(pos.x, pos.y);
        point.x -= imageWidth / 2;
        point.y -= imageHeight / 2;
        GUI.Box(new Rect(point.x, point.y, imageWidth, imageHeight),
                dot,
                gs);
    }
}
