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
    private bool moveWithFrequency = true, keepMoving = true;

    private Vector2Int textureCoords;
    private Texture2D mask;
    private Color textureColor;

    private Vector3 direction;
    private Rigidbody2D rb;

    private void Awake()
    {
        ps = GetComponent<ParticleSystem>();
        _renderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        Destroy(gameObject, 5.0f);
    }
    public void MoveTowardsTarget(Vector3 _target, DrawableObject _objectToColor, Color _color, Vector2Int textureHitPoint, Texture2D _mask)
    {
        target = _target;
        objectToColor = _objectToColor;
        _renderer.color = _color;
        var main = ps.main;
        main.startColor = _color;
        textureCoords = textureHitPoint;
        mask = _mask;
        textureColor = _color;

        if(_objectToColor.GetComponent<Enemy>())
            _objectToColor._event.AddListener(EnemyHasDied);
    }

    private void EnemyHasDied()
    {
        Destroy(gameObject);
    }

    private void Update()
    {
        direction = target - transform.position;

        if (direction.magnitude < 0.1f && keepMoving)
            UpdateTargetPosition();
    }

    private void FixedUpdate()
    {   
        if(keepMoving)
            rb.velocity = direction.normalized * speed;
    }

    private void UpdateTargetPosition()
    {
        target = objectToColor.transform.position;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<DrawableObject>() == objectToColor)
        {
            objectToColor.ApplySplatter(textureCoords, mask, textureColor);
            ps.Play();
            moveWithFrequency = false;
            keepMoving = false;
            _renderer.enabled = false;
            rb.velocity /= 2.5f; // rb.velocity / 2.0f;
            GetComponent<CircleCollider2D>().enabled = false;
            Destroy(gameObject, 2.0f);
        }
    }
}
