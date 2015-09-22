using UnityEngine;
using System.Collections;
using MKGlowSystem;

public class GloweynessModifier : MonoBehaviour 
{
    public float blurIntensityAmplitude = 0.008f;
    public float blurIntensityPeriod = 0.04f;
    public float glowIntensityAmplitude = 0.001f;
    public float glowIntensityPeriod = 0.023f;

    private MKGlow m;
    private int framesSinceInit;

	void Start () 
    {
        m = GetComponent<MKGlow>();
        framesSinceInit = 0;
	}
	
	void FixedUpdate () 
    {
        framesSinceInit++;
        m.GlowIntensity += glowIntensityAmplitude * Mathf.Sin(framesSinceInit * glowIntensityPeriod);
        m.BlurSpread += blurIntensityAmplitude * Mathf.Sin(framesSinceInit * blurIntensityPeriod);
	}
}
