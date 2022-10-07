using UnityEngine;

public class SelectableObject : MonoBehaviour
{

    [SerializeField] private GameObject _selelectionIndicator;

    void Start()
    {
        _selelectionIndicator.SetActive(false);
    }

    public virtual void OnHover(){
        transform.localScale = Vector3.one * 1.1f;
    }

    public virtual void OnUnhover(){
        transform.localScale = Vector3.one;
    }

    public virtual void Select(){
        _selelectionIndicator.SetActive(true);
    }

    public virtual void UnSelect(){
        _selelectionIndicator.SetActive(false);
    }

    public virtual void WhenClickOnGround(Vector3 point){

    }
    
}
