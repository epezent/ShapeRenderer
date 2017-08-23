using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateShape : MonoBehaviour {

    PolygonShape polygonShape;
    ShapeRenderer sr;
    public float size;
    public float radius;

    public Vector2[] points = new Vector2[3];
    public float sum = 0;

    // Use this for initialization
    void Start () {
        polygonShape = GetComponent<PolygonShape>();
        sr = GetComponent<ShapeRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
        size = 200 + 50 * Mathf.Sin(2 * Mathf.PI * 0.25f * Time.time);
        radius = 25 + 24 * Mathf.Sin(2 * Mathf.PI * 0.25f * Time.time);
        polygonShape.parameterValue = size;
        polygonShape.cornerRadius = radius;
        sr.fillAngle = 5.0f * Time.time;
	}
}
