using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class CameraControl : MonoBehaviour
{
    private GameControl gameControl;

    [SerializeField] private float moveSpeed = 50.0f;
    [SerializeField] private float scrollSpeed = 50.0f;

    private Renderer lastHighlightedCube;
    private bool IsUiSelected = false;

    private static Color normalColor = new(94f / 255f, 94f / 255f, 94f / 255f, 255f / 255f);
    private static Color selectedColor = new(25f / 255f, 25f / 255f, 25f / 255f, 255f / 255f);

    [SerializeField] private Vector3 maxPos = new Vector3(0, 0, 0);
    [SerializeField] private Vector3 minPos = new Vector3(0, 0, 0);

    private void Start()
    {
        GameInput.Instance.OnLeftButtonAction += OnLeftButton;
    }

    private void OnDestroy()
    {
        GameInput.Instance.OnLeftButtonAction -= OnLeftButton;
    }

    void FixedUpdate()
    {
        IsUiSelected = EventSystem.current.IsPointerOverGameObject();
        HandleSelectCube();
        HandleMovement();
        HandleScroll();
    }

    private void HandleSelectCube()
    {
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (!IsUiSelected && Physics.Raycast(ray, out RaycastHit hit))
        {
            MapCube mapCube = hit.transform.GetComponent<MapCube>();
            Renderer cubeRenderer = hit.transform.GetComponent<Renderer>();

            // Reset the last highlighted cube to white if it exists
            if (lastHighlightedCube != null && lastHighlightedCube != cubeRenderer)
            {
                lastHighlightedCube.material.color = normalColor;
                lastHighlightedCube = null;
            }

            if (mapCube != null && lastHighlightedCube != null && mapCube.IsBuilt())
            {
                lastHighlightedCube.material.color = normalColor;
                lastHighlightedCube = null;
            }

            if (mapCube != null && cubeRenderer != null && !mapCube.IsBuilt())
            {
                // Highlight the current cube to black
                cubeRenderer.material.color = selectedColor;
                lastHighlightedCube = cubeRenderer;
            }
        }
        else
        {
            if (lastHighlightedCube != null)
            {
                lastHighlightedCube.material.color = normalColor;
                lastHighlightedCube = null;
            }
        }
    }

    private void OnLeftButton(object sender, System.EventArgs e)
    {
        if (!IsUiSelected)
        {
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                MapCube mapCube = hit.transform.GetComponent<MapCube>();
                if (mapCube != null)
                {
                    if (mapCube.IsBuilt())
                    {
                        BuildManager.Instance.ShowUpgradeUI(mapCube, mapCube.transform.position, mapCube.IsUpgrade());
                    }
                    else
                    {
                        mapCube.Build(sender, e);
                    }
                }
            }
        }
    }

    private void HandleMovement()
    {
        Vector3 direction = GameInput.Instance.GetMovementDirectionNormalized();
        Vector3 tempTransform = transform.position + direction * Time.deltaTime * moveSpeed;
        transform.position = ClampPositionWithinBounds(tempTransform);
    }

    public void HandleScroll()
    {
        Vector3 direction = GameInput.Instance.GetScrollDirectionNormalized();
        Vector3 tempTransform = transform.position + direction * Time.deltaTime * scrollSpeed;
        transform.position = ClampPositionWithinBounds(tempTransform);
    }
    private Vector3 ClampPositionWithinBounds(Vector3 position)
    {
        position.x = Mathf.Clamp(position.x, minPos.x, maxPos.x);
        position.y = Mathf.Clamp(position.y, minPos.y, maxPos.y);
        position.z = Mathf.Clamp(position.z, minPos.z, maxPos.z);
        return position;
    }
}
