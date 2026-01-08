using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrealoadManager : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        gameManager.ToTitleScreen();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
