using UnityEngine;



public class CallAnim : MonoBehaviour
{

    [SerializeField]Animator animator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void OpenAnim()
    {
        animator.SetTrigger("Open");
    }
    public void CloseAnim()
    {
        animator.SetTrigger("Close");
    }
}
