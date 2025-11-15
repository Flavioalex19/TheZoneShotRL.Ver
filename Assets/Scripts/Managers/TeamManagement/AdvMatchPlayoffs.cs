using DG.Tweening.Core.Easing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdvMatchPlayoffs : MonoBehaviour
{

    GameManager gameManager;
    LeagueManager leagueManager;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        leagueManager = GameObject.Find("League/Season Manager").GetComponent<LeagueManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void AdvanceToMatch()
    {

        StartCoroutine(AdvanceToMatchRoutine());

    }
    private IEnumerator AdvanceToMatchRoutine()
    {
        if (GameObject.Find("TransitionToMatch"))
        {
            Animator animator = GameObject.Find("TransitionToMatch").GetComponent<Animator>();
            //GameObject.Find("PhaseText").GetComponent<TextMeshProUGUI>().text = " Next Phase";
            animator.SetTrigger("Go");
        }

        float timer = 3f;
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            //print("On Transition");
            yield return null; // Wait a frame instead of freezing
        }
        if (leagueManager.isOnR8)
        {
            gameManager.GoToPlayoffs();
        }
        else gameManager.GoToMatch();


    }
}
