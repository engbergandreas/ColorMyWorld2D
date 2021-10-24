using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PointSystem : MonoBehaviour
{
    public static PointSystem Instance;
    public TextMeshProUGUI scoreText;
    private int points;
    // Start is called before the first frame update
    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        points = 0;
    }

    public void AddPoints(int p)
    {
        points += p;
        UpdateUIText();
    }
    public int GetPoints()
    {
        return points;
    }

    private void UpdateUIText()
    {
        scoreText.SetText("Score: " + points);
    }

    //private void Update()
    //{
    //    if (Input.GetMouseButtonDown(1))
    //        AddPoints(1);
    //}
}
