using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GridManager))]
public class PlacementHandler : MonoBehaviour
{
    [Header("Instances")]
    [SerializeField] private GridManager gridManager;

    [Header("Layer Masks")]
    [SerializeField] private LayerMask mask;

    [Header("Placeable Objects")]
    [SerializeField] private List<GameObject> placeableObjects;
    [SerializeField] private Transform emptyObjectParent;

    private Camera _cam;
    private GameObject _heldObject;
    private List<GridObject> _activeGrids;

    private GameObject _selectedObjectPrefab;
    private bool _instantiateObject = false;
    private bool _canClick = true;
    private float _seconds = 0f;

    private RaycastHit _raycastHit;

    public void SelectCube()
    {
        foreach (GameObject placeableObject in placeableObjects)
        {
            if (placeableObject.GetComponent<PlaceableObject>().name == "Cube")
            {
                Destroy(_heldObject);
                _selectedObjectPrefab = placeableObject;
                _instantiateObject = true;
                return;
            }
        }
    }

    public void SelectSphere()
    {
        foreach (GameObject placeableObject in placeableObjects)
        {
            if (placeableObject.GetComponent<PlaceableObject>().name == "Sphere")
            {
                Destroy(_heldObject);
                _selectedObjectPrefab = placeableObject;
                _instantiateObject = true;
                return;
            }
        }
    }

    public void SelectCapsule()
    {
        foreach (GameObject placeableObject in placeableObjects)
        {
            if (placeableObject.GetComponent<PlaceableObject>().name == "Capsule")
            {
                Destroy(_heldObject);
                _selectedObjectPrefab = placeableObject;
                _instantiateObject = true;
                return;
            }
        }
    }

    private void Start()
    {
        _cam = Camera.main;
    }

    public void Update()
    {
        _activeGrids = gridManager.activeGrids;
        if (!_canClick)
        {
            _seconds += 1 * Time.deltaTime;
            if (_seconds > .5f)
            {
                _canClick = true;
                _seconds = 0f;
            }
        }
    }

    private void FixedUpdate()
    {
        InstantiatePreviewObject();
        RunPlacementLogic();
    }

    private void RunPlacementLogic()
    {
        Vector3 cameraPos = _cam.transform.position;
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = _cam.nearClipPlane + 1;
        Vector3 direction = _cam.ScreenToWorldPoint(mousePosition) - cameraPos;
        
        if (Physics.Raycast(cameraPos, direction, out _raycastHit, Mathf.Infinity, mask))
        {  
            if (_raycastHit.collider == null) return;

            ColliderHolder colliderHolder = _raycastHit.collider.GetComponent<ColliderHolder>();
            GameObject colliderObject = _raycastHit.collider.gameObject;
            if (_heldObject != null && _heldObject.tag == "PreviewObject")
                _heldObject.transform.position = colliderObject.transform.position;
            
            GridObject colliderGrid = colliderObject.GetComponent<ColliderHolder>().grid;

            if (Input.GetMouseButton(0) && _canClick)
            {
                _canClick = false;
                
                if (colliderGrid.isOccupied == true) return;
                colliderGrid.isOccupied = true;
                _instantiateObject = true;
                
                if (_heldObject != null)
                {
                    colliderHolder.placedObject = _heldObject;
                    PlaceableObject placeableObject = _heldObject.GetComponent<PlaceableObject>();
                    placeableObject.ChangeState(State.Built);
                    placeableObject.placedGrid = colliderGrid;

                    _heldObject.tag = "BuiltObject";
                }
                
                if (colliderGrid.isOccupied)
                    Debug.Log("This grid is now occupied");
            }
            if (Input.GetMouseButton(1) && _canClick)
            {
                _canClick = false;
                colliderGrid.isOccupied = false;

                if (colliderHolder.placedObject != null)
                    Destroy(colliderHolder.placedObject);

                if (!colliderGrid.isOccupied)
                    Debug.Log("This grid is now empty");
            }
        }
    }

    private void InstantiatePreviewObject()
    {
        if (_instantiateObject)
        {
            _instantiateObject = false;
            if (_selectedObjectPrefab == null) return;

            if (_raycastHit.collider != null)
            {
                _heldObject = Instantiate(_selectedObjectPrefab, 
                            _raycastHit.collider.transform.position, Quaternion.identity, emptyObjectParent); }
            else
            {
                _heldObject = Instantiate(_selectedObjectPrefab,
                            GameObject.FindGameObjectWithTag("GroundCollider").transform.position,
                            Quaternion.identity, emptyObjectParent); }
            }
        }
    }