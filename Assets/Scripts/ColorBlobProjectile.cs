using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorBlobProjectile : MonoBehaviour
{
    public float amplitude, frequency;
    private Vector3 target;
    private DrawableObject objectToColor;
    private float speed = 10.0f;
    private ParticleSystem ps;
    private SpriteRenderer _renderer;
    private bool moveWithFrequency = true;

    private void Awake()
    {
        ps = GetComponent<ParticleSystem>();
        _renderer = GetComponent<SpriteRenderer>();
        Destroy(gameObject, 5.0f);
    }
    public void MoveTowardsTarget(Vector3 _target, DrawableObject _objectToColor, Color _color)
    {
        target = _target;
        objectToColor = _objectToColor;
        _renderer.color = _color;
        var main = ps.main;
        main.startColor = _color;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = target - transform.position;

        if (moveWithFrequency)
            direction.y += Mathf.Sin(Time.time * frequency) * amplitude;

        transform.Translate(direction.normalized * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<DrawableObject>() == objectToColor)
        {
            objectToColor.ApplySplatter();
            moveWithFrequency = false;
            ps.Play();
            _renderer.enabled = false;
            GetComponent<CircleCollider2D>().enabled = false;
            Destroy(gameObject, 2.0f);
        }
    }
}
