using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[RequireComponent(typeof(DestroyColliderObjects))]
public class GridManager : MonoBehaviour
{
    [HideInInspector] public List<GridObject> activeGrids;

    [Header("Instances")]
    [SerializeField] DestroyColliderObjects destroyColliders;
    [SerializeField] private GameObject gridObjectPrefab;
    [SerializeField] private Transform parent;
    
    [Header("Grid Properties")]
    [SerializeField] private Vector2 dimensions;
    [SerializeField] private float size;

    public GridObject grid;
    
    private void OnDrawGizmos()
    {
        foreach (GridObject grid in activeGrids)
        {
            grid.DrawGridGizmos();
        }
    }

    private void OnValidate()
    {
        RunGridInitialization();
    }

    
    private void RunGridInitialization()
    {
        activeGrids = GridObject.GenerateGrids(dimensions.x, dimensions.y, size);
        gridObjectPrefab.transform.localScale = new Vector3(activeGrids[0].Size, .01f, activeGrids[0].Size);
        int iteration = 0;
        destroyColliders.destroy = true;
        foreach (GameObject colliderObject in GameObject.FindGameObjectsWithTag("GroundCollider"))
        {
            colliderObject.tag = "OldCollider";
        }
        foreach (GridObject grid in activeGrids)
        {
            GameObject collider = Instantiate(gridObjectPrefab, grid.GetPosition(), Quaternion.identity, parent);
            ColliderHolder colliderHolder = collider.gameObject.GetComponent<ColliderHolder>();
            collider.name = $"Grid Collider '{iteration}'";   
            colliderHolder.grid = activeGrids[iteration];        
            iteration++;
        }
    }
}
