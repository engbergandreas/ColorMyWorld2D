using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
public class ColorGun : MonoBehaviour
{
    public static ColorGun Instance;
    public Color color;
    public float fireRate = 10.0f;
    public bool continiousFire;
    public Image crosshair;
    public Transform fireGunPosition;
    public GameObject colorblob;
    public List<Texture2D> maskList;
    public UnityEvent<RGBChannel> rgbChannelEvent;
    public UnityEvent<Color> colorChannelEvent;

    private Camera _cam;
    private float fireTimer;

    private LineRenderer lr;

    private bool hasRed = false, hasGreen = false, hasBlue = false;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        _cam = Camera.main;
        fireTimer = 0;
        Cursor.visible = false; //make cursor pointer invisible
        lr = GetComponent<LineRenderer>();
        lr.startWidth = 0.05f;
        lr.endWidth = 0.05f;
    }

    // Update is called once per frame
    void Update()
    {
        if (hasRed && Input.GetKeyDown(KeyCode.Alpha1))
            ChangeColorGun(RGBChannel.Red);
        if (hasGreen && Input.GetKeyDown(KeyCode.Alpha2))
            ChangeColorGun(RGBChannel.Green);
        if (hasBlue && Input.GetKeyDown(KeyCode.Alpha3))
            ChangeColorGun(RGBChannel.Blue);

        if (fireTimer > 0)
            fireTimer -= Time.deltaTime;

        //if (fireTimer <= 0)
        //{
        //    //if (continiousFire && Input.GetMouseButton(0)) //can hold down mouse
        //    //    FireGun();
        //    //else if (Input.GetMouseButtonDown(0)) //single taps only
        //    //    FireGun();
        //}
        //else
        //{
        //    fireTimer -= Time.deltaTime;
        //}

        RectTransform crosshairRec = crosshair.GetComponent<RectTransform>();
        Vector3 screenMousePos = Input.mousePosition;
        crosshairRec.position = screenMousePos;

        CheckIntersectingObjectsBetweenPlayerAndMouse();
    }
    /// <summary>
    /// Raycast from the player towards the mouse to se if we intercept any drawable objects
    /// Crosshair alpha value set to 1 if we are hovering a drawable object, otherwise lower value.
    /// </summary>
    private void CheckIntersectingObjectsBetweenPlayerAndMouse()
    {
        //Set crosshair alpha 
        Color c = crosshair.color;
        c.a = 0.2f;
        crosshair.color = c;
        crosshair.rectTransform.eulerAngles = new Vector3(0, 0, 0);

        //Reset line renderer
        lr.SetPosition(0, Vector3.zero);
        lr.SetPosition(1, Vector3.zero);

        //Raycast mouseposition 
        RaycastHit2D rayMouseToWorld = Physics2D.GetRayIntersection(_cam.ScreenPointToRay(Input.mousePosition));
        if (!rayMouseToWorld || rayMouseToWorld.transform.tag == "Player")
            return;
        
        Vector3 mouseHitPoint = rayMouseToWorld.point;
        Vector3 playertoMouseDirection = mouseHitPoint - fireGunPosition.position; 

        //Filter raycast to not include player & ladder
        ContactFilter2D filter = new ContactFilter2D { useTriggers = true };
        filter.SetLayerMask(~LayerMask.GetMask("Player", "Ladder"));
        List<RaycastHit2D> hits = new List<RaycastHit2D>();
        
        Physics2D.Raycast(fireGunPosition.position, playertoMouseDirection.normalized, filter, hits, 40.0f);

        if (hits.Count == 0)
            return;

        RaycastHit2D rayPlayerToMouse = hits[0]; //Take the first hit excluding the player -> closest obj hit

        var objmousehit = rayMouseToWorld.transform.GetComponent<DrawableObject>();
        var interceptedObj = rayPlayerToMouse.transform.GetComponent<DrawableObject>();

        //Object under the mouse is the same as the intercepted object between the player and mouseposition 
        //If the intercepted object is a walkable surface or a ladder, it counts as correct 
        //Sets the linerenderer from player to mouse position
        if ((interceptedObj && interceptedObj == objmousehit) ||
            rayPlayerToMouse.transform.GetComponent<WalkableSurface>())
        {
            lr.SetPosition(0, fireGunPosition.position);
            lr.SetPosition(1, mouseHitPoint);
            lr.startColor = Color.cyan;
            lr.endColor = Color.cyan;
            
            //Can we fire gun?
            if (fireTimer <= 0)
            {
                if (continiousFire && Input.GetMouseButton(0)) //can hold down mouse
                    FireGun(objmousehit, mouseHitPoint);
                else if (Input.GetMouseButtonDown(0)) //single taps only
                    FireGun(objmousehit, mouseHitPoint);
            }
        }
        //Another object blocks the ray from player to mouse
        //Sets the linerender from player to the intercepted object
        else
        {
            lr.SetPosition(0, fireGunPosition.position);
            lr.SetPosition(1, rayPlayerToMouse.point);
            lr.startColor = Color.red;
            lr.endColor = Color.red;
        }

        //if the player hovers a drawable surface, wiggle the crosshair 
        if(objmousehit)
        {
            c.a = 1.0f;
            crosshair.color = c;

            float deg = 15.0f;
            float speed = 4.0f;
            crosshair.rectTransform.eulerAngles = new Vector3(0, 0, Mathf.Sin(Time.time * speed) * deg );
        }
    }

    private void FireGun(DrawableObject objmousehit, Vector3 mouseHitPoint)
    {
        fireTimer = 1 / fireRate;

        Vector3 mouseHitPointScreenCoords = _cam.WorldToScreenPoint(mouseHitPoint);
        Vector2Int textureHitPoint = objmousehit.ColorTarget(mouseHitPointScreenCoords, color, _cam);

        var obj = Instantiate(colorblob, fireGunPosition.position, Quaternion.identity);
        obj.GetComponent<ColorBlobProjectile>().MoveTowardsTarget(mouseHitPoint, objmousehit, color, textureHitPoint, GetRandomSplatterMask());
    }

    /// <summary>
    /// Fires gun from player to mouse location 
    /// </summary>
    //private void FireGun()
    //{
    //    fireTimer = 1 / fireRate;

    //    RaycastHit2D hitinfo = Physics2D.GetRayIntersection(_cam.ScreenPointToRay(Input.mousePosition));
    //    if (!hitinfo)
    //        return;

    //    //Check we hit drawable target
    //    DrawableObject objectHit = hitinfo.transform.gameObject.GetComponent<DrawableObject>();

    //    if (objectHit)
    //    {
    //        //Fire a ray from the player towards the mouse in world coordinates
    //        //check if the ray intersects any other objects on the path
    //        Vector3 worldHitPoint = hitinfo.point;
    //        Vector3 dir = worldHitPoint - fireGunPosition.position;

    //        //RaycastHit2D info = Physics2D.Raycast(fireGunPosition.position, dir.normalized);
    //        //if (!info)
    //        //    return;

    //        //Filter raycast to not include player 
    //        ContactFilter2D filter = new ContactFilter2D { useTriggers = true };
    //        filter.SetLayerMask(~LayerMask.GetMask("Player"));
    //        List<RaycastHit2D> hits = new List<RaycastHit2D>();
    //        Physics2D.Raycast(fireGunPosition.position, dir.normalized, filter, hits, 40.0f);

    //        RaycastHit2D info = hits[0]; //Take the first hit -> closest obj hit
    //        if (!info)
    //            return;

    //        DrawableObject interception = info.transform.GetComponent<DrawableObject>();

    //        if (!interception)
    //            return;

    //        if (interception == objectHit || info.transform.GetComponent<WalkableSurface>() ||
    //            info.transform.tag == "Ladder") //Is it the same drawable object that we clicked on then fire the gun
    //        {
    //            Vector3 hitPoint = _cam.WorldToScreenPoint(hitinfo.point);
    //            objectHit.ColorTarget(hitPoint, color, _cam, GetRandomSplatterMask());
    //        }
    //    }
    //}

    Texture2D GetRandomSplatterMask()
    {
        return maskList[Random.Range(0, maskList.Count)];
    }

    //TODO: broadcast onchanged to other objects in a better way?
    //UnityEvent<RGBChannel> <Color>
    private void ChangeColorGun(RGBChannel channel)
    {
        color = RGBChannelToColor(channel);
        //var colorableObjects = FindObjectsOfType<ColorableObject>();
        //foreach(var obj in colorableObjects)
        //{
        //    obj.OnChangedColorGun(color);
        //}

        //var channelCollisions= FindObjectsOfType<ChannelCollision>();
        //foreach(var obj in channelCollisions)
        //{
        //    obj.OnChannelChange(channel);
        //}
        crosshair.color = color;
        rgbChannelEvent.Invoke(channel);
        colorChannelEvent.Invoke(color);
    }

    public Color RGBChannelToColor(RGBChannel channel)
    {
        switch (channel)
        {
            case RGBChannel.Red:
                return Color.red;
            case RGBChannel.Green:
                return Color.green;
            case RGBChannel.Blue:
                return Color.blue;
            default:
                return Color.black;
        }
    }
  
    public void PickUpColor(RGBChannel color)
    {
        switch (color)
        {
            case RGBChannel.Red:
                hasRed = true;
                break;
            case RGBChannel.Green:
                hasGreen = true;
                break;
            case RGBChannel.Blue:
                hasBlue = true;
                break;
            default:
                break;
        }
    }
}
