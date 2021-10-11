using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorCloud : DrawableObject
{
    private const float min = 5, max = 15;

    [Range(min, max)]
    public float lifeTime = 15.0f;

    private float elapsedTime, t;
    Vector3 startvalue;

    private bool isShrinking = false;

    private void Awake()
    {
        gameObject.SetActive(false);
    }


    protected override void Start()
    {
        base.Start();
        elapsedTime = 0.0f;
        startvalue = transform.localScale;
        isShrinking = true;
    }

    private void Update()
    {
        if (isShrinking)
        {
            t = elapsedTime / lifeTime;
            transform.localScale = Vector3.Lerp(startvalue, Vector3.zero, t);
            elapsedTime += Time.deltaTime;

            if (elapsedTime >= lifeTime)
            {
                isShrinking = false;
                Destroy(gameObject, 5);
            }
        }
    }

    protected override void FullyColored()
    {
        if (canGivePoints)
        {
            int bonus = Mathf.RoundToInt(pointsToGive * (1 - (lifeTime / (max + min)) )); // extra points given at points * [0.25, 0.75]
            int points = Mathf.RoundToInt(pointsToGive * (1 - t)) + bonus;
            PointSystem.Instance.AddPoints(points);
            canGivePoints = false;
            isShrinking = false;
            //Destroy(gameObject, 5);
        }
    }
}
