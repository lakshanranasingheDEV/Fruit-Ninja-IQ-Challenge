using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blade : MonoBehaviour
{

    public GameObject _bladeTrailPrefab;

    public float _mainCuttingVelocity = 0.001f;
    public bool _isCutting = false;
    
    public Vector2 _previousPosition;

    GameObject currentBladeTrail;


    Rigidbody2D rb;
    Camera cam;
    CircleCollider2D circleCollider;

    private void Start()
    {
        cam = Camera.main;
        rb = GetComponent<Rigidbody2D>();
        circleCollider = GetComponent<CircleCollider2D>();
    }


    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StartCutting();    
        }
        else if(Input.GetMouseButtonUp(0)) 
        {
            StopCutting();
        }

        if (_isCutting)
        {
            UpdateCut();
        }
    }

    public void UpdateCut()
    {
        Vector2 newPosition = cam.ScreenToWorldPoint(Input.mousePosition);
        rb.position = newPosition;
        float velocity = (newPosition - _previousPosition).magnitude*Time.deltaTime;

        if(velocity > _mainCuttingVelocity)
        {
            circleCollider.enabled = true;
        }else
        {
            circleCollider.enabled = false;
        }

        _previousPosition = newPosition;
    }


    public void StartCutting()
    {
        _isCutting = true;
        currentBladeTrail = Instantiate(_bladeTrailPrefab, transform);
        _previousPosition = cam.ScreenToWorldPoint(Input.mousePosition);
        circleCollider.enabled = false;


    }

    public void StopCutting()
    {
        _isCutting = false;
        currentBladeTrail.transform.SetParent(null);
        Destroy(currentBladeTrail, 2f);
        circleCollider.enabled=false;


    }
}
