using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionManager : MonoBehaviour
{
    [SerializeField, Tooltip("Make sure the tag specified exists.")]
    private string selectableTag = "Selectable";
    [SerializeField, Tooltip("The distance from which the selectable item can be selected.")]
    private float selectDistance = 0.0f;

    private ISelectionResponse _selectionResponse;
    private Transform _selection;
    private Camera _camera;

    private UIManager uiManager;

    private void Awake()
    {
        _selectionResponse = GetComponent<ISelectionResponse>();

        _camera = GameObject.Find("CustomFPSController").GetComponentInChildren<Camera>();
        if (!_camera) Debug.LogError("In SelectionManager::Awake(): Failed to find gameObject \"Player\".");

        uiManager = GameObject.Find("UIManager").GetComponent<UIManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(_selection) _selectionResponse.onDeselect(_selection);

        #region Raycasting

        // Create And Cast Ray
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

        // Selection Determination
        _selection = null;
        
        if (Physics.Raycast(ray, out var hit, selectDistance))
        {
            Transform selection = hit.transform;
            if(selection.CompareTag(selectableTag) && !uiManager.isQuestionCanvasActive)
            {
                _selection = selection;
            }
        }

        #endregion

        if (_selection) _selectionResponse.onSelect(_selection);

        #region Mouse Input

        if (_selection && Input.GetMouseButtonDown(0))
        {
            uiManager.GenerateQuestion();
        }

        #endregion
    }
}
