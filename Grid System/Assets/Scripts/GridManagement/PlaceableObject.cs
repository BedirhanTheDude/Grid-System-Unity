using UnityEngine;
using System;

[Serializable]
public enum State {
    Preview,
    Built,
}

[Serializable]
public class PlaceableObject : MonoBehaviour
{
    [Header("Object Config")]
    public Material transparentMat;
    public Material opaqueMat;
    public string objectName;
    
    [HideInInspector] public GridObject placedGrid;
    [HideInInspector] public GameObject gObject;

    private State _state = State.Preview;
    private Renderer rend;

    private void Start()
    {
        gObject = gameObject;
        rend = gameObject.GetComponent<Renderer>();
        rend.material = transparentMat;
    }

    private void Update()
    {
        if (rend.material == opaqueMat)
            return;
        switch (_state)
        {
            case State.Preview:
                rend.material = transparentMat;
                break;
            case State.Built:
                rend.material = opaqueMat;
                break;
        }
    }

    public void ChangeState(State state)
    {
        _state = state;
    }
}
