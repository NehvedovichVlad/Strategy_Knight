using UnityEngine;

public class SelectableCollider : MonoBehaviour
{
    [SerializeField] private SelectableObject _selectableObject;
    public SelectableObject SelectableObject{ get => _selectableObject;}

}
