using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewsManager : MonoBehaviour
{
    [SerializeField] TeamManagerUI teamManager;
    [SerializeField] Image image_news;
    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
    }

    public void ChangeSprint()
    {
        int index;
        index = Random.Range(0, teamManager.sprites_newsSprites.Count);
        image_news.sprite = teamManager.sprites_newsSprites[index];


    }
}
