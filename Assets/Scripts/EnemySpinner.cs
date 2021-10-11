using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpinner : MonoBehaviour
{
    public int nrOfArms = 1;
    [Range(-0.5f, 0.5f)]
    public float rotationSpeed = 0.3f;

    public GameObject chain, ball;
    public int nrOfChains = 6;


    private GameObject CreateArm()
    {
        float offset = 0.4f;
        GameObject arm = new GameObject("Arm Parent");
        for (int i = 0; i < nrOfChains; i++)
        {
            Vector3 chainPosition = new Vector3(i * offset, 0, 0);
            var obj = Instantiate(chain, chainPosition, Quaternion.identity, arm.transform);

        }

        Vector3 ballPosition = new Vector3((nrOfChains + 1) * offset, 0, 0);
        var ballobj = Instantiate(ball, ballPosition, Quaternion.identity, arm.transform);
        
        return arm;
    }

    private void Start()
    {
        float angleBetweenArms = 2 * Mathf.PI / nrOfArms;
        for(int i = 0; i < nrOfArms; i++)
        {
            float angle = i * angleBetweenArms;
            float angleindeg = angle * 180 / Mathf.PI;
            //Vector2 pos = PolarToCartesian(angle, transform.localScale.x);

            GameObject arm = CreateArm();
            arm.transform.parent = transform;
            arm.transform.localPosition = new Vector3(0, 0, 0);
            arm.transform.eulerAngles = new Vector3(0, 0, -angleindeg);
        }
    }

    private void Update()
    {
        transform.RotateAround(transform.position, Vector3.forward, rotationSpeed * 360 * Time.deltaTime);
    }

    Vector2 PolarToCartesian(float angle, float radius)
    {
        float x = radius * Mathf.Cos(angle);
        float y = radius * Mathf.Sin(angle);

        return new Vector2(x, y);
    }

}
