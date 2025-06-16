using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroManager : MonoBehaviour
{
    [SerializeField] bool _canProgressToMainMenu = false;
    [SerializeField] bool _canPressEnter = false;
    [SerializeField] Animator _introAnimator;
    // Start is called before the first frame update
    void Start()
    {
        _canProgressToMainMenu = false;
        _canPressEnter = false;
        StartCoroutine(ProgressToMainMenu());

        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)&& _canPressEnter == true)
        {
            _canProgressToMainMenu=true;
        }
        
    }
    IEnumerator ProgressToMainMenu()
    {
        yield return new WaitForSeconds(3.5f);
        _introAnimator.SetTrigger("ToStart");
        _canPressEnter = true;
        yield return new WaitUntil(() => _canProgressToMainMenu);
        _introAnimator.SetTrigger("ToMain");
    }
}
