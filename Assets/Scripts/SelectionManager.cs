using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QuizGame;

public class SelectionManager : MonoBehaviour
{
    [SerializeField, Tooltip("Make sure the tag specified exists.")]
    private string selectableTag = "Selectable";
    [SerializeField, Tooltip("The distance from which the selectable item can be selected.")]
    private float selectDistance = 0.0f;
    public static List<int> arrayList;
    public static string diamondName;
    XMLReader xmlReader;


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

    private void Start()
    {
        arrayList = new List<int>();
        xmlReader = GameObject.Find("GlobalGameManager").GetComponent<XMLReader>();

        Dictionary<string, int> doors = xmlReader.LoadAllDoors();
        foreach (int value in doors.Values)
        {
            if (!arrayList.Contains(value))
            {
                arrayList.Add(value);
            }

        }
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
            if (selection.CompareTag(selectableTag) && !uiManager.isQuestionCanvasActive)
            {
                _selection = selection;
                diamondName = _selection.name;
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
