using UnityEngine;
using UnityEngine.EventSystems;

public class BtnPlayerOffensiveBtns : MonoBehaviour/*, IPointerEnterHandler, IPointerExitHandler*/
{
    [SerializeField]Animator animator;
    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    /*
    public void OnPointerEnter(PointerEventData eventData)
    {
        print("Here to be hover");
        animator.SetBool("On", true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        animator.SetBool("On", false);
    }

    private void OnMouseEnter()
    {
        print("Here to be hover");
        animator.SetBool("On", true);
    }
    private void OnMouseExit()
    {
        animator.SetBool("On", false);
    }
    */
}
