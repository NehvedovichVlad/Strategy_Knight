using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

    public enum SelectionState{
        UnitSelected,
        Frame,
        Other
    }

public class Managment : MonoBehaviour
{
    [SerializeField]private Camera _camera;
    [SerializeField]private  SelectableObject _howered;
    [SerializeField]private List<SelectableObject> _listOfSelected = new List<SelectableObject>();

    [SerializeField]private Image _frameImage;
    private Vector2 _frameStart;
    private Vector2 _frameEnd;

    [SerializeField]private SelectionState _currentSelectionState;
    void Update()
    {
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;
        if(Physics.Raycast(ray, out hit)){
            if(hit.collider.GetComponent<SelectableCollider>()){
                SelectableObject hitSelectable = hit.collider.GetComponent<SelectableCollider>().SelectableObject;
                if(_howered){
                    if(_howered != hitSelectable){
                        _howered.OnUnhover();
                        _howered = hitSelectable;
                        _howered.OnHover();
                    }
                }
                else {
                        _howered = hitSelectable;
                        _howered.OnHover();
                    }
            }
            else{  UnhoverCurrent();  }
        }
        else{ UnhoverCurrent();}

        if(Input.GetMouseButtonUp(0)){
            if(_howered){
                if(Input.GetKey(KeyCode.LeftControl) == false){
                    UnselectAll();
                }
                _currentSelectionState = SelectionState.UnitSelected;
                SelectObject(_howered);
            }
        }
        if(_currentSelectionState == SelectionState.UnitSelected){
            if(Input.GetMouseButtonUp(0)){
                if(hit.collider.tag == "Ground"){
                    for (int i = 0; i < _listOfSelected.Count; i++)
                    {
                        _listOfSelected[i].WhenClickOnGround(hit.point);
                    }
                }
            }
        }    

        if(Input.GetMouseButtonUp(1)){
            UnselectAll();
        }
        
        // Box Selection
        if(Input.GetMouseButtonDown(0)){
            _frameStart = Input.mousePosition;
        }

        if(Input.GetMouseButton(0)){

            _frameEnd = Input.mousePosition;

            Vector2 minStartPontFrame = Vector2.Min(_frameStart, _frameEnd);
            Vector2 maxEndPointFrame = Vector2.Max(_frameStart, _frameEnd);
            Vector2 size = maxEndPointFrame - minStartPontFrame;

            if(size.magnitude > 10){

                _frameImage.enabled = true; 
                _frameImage.rectTransform.anchoredPosition = minStartPontFrame;
                _frameImage.rectTransform.sizeDelta = size;

                Rect rect = new Rect(minStartPontFrame, size);

                UnselectAll();   
                Unit[] allUnits = FindObjectsOfType<Unit>();
                for (int i = 0; i < allUnits.Length; i++){
                    Vector2 screenPosition = _camera.WorldToScreenPoint(allUnits[i].transform.position);
                    if(rect.Contains(screenPosition)){
                        SelectObject(allUnits[i]);
                    }
                }
            _currentSelectionState = SelectionState.Frame;
            }
        }

        if(Input.GetMouseButtonUp(0)){
            _frameImage.enabled = false;
            if(_listOfSelected.Count > 0){
                _currentSelectionState = SelectionState.UnitSelected;
            }
            else{
                _currentSelectionState = SelectionState.Other;}    
        }
    }

    void SelectObject(SelectableObject selectableObject){
        if(_listOfSelected.Contains(selectableObject) == false){
            _listOfSelected.Add(selectableObject);
            selectableObject.Select(); 
        }
    }

    void UnselectAll(){
        for (int i = 0; i < _listOfSelected.Count; i++){
            _listOfSelected[i].UnSelect();
            }
        _listOfSelected.Clear();
        _currentSelectionState = SelectionState.Other;
    }
    void UnhoverCurrent(){
        if(_howered){
            _howered.OnUnhover();
            _howered = null;
        }
    }
}
