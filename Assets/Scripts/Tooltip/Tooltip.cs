using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode()]
public class Tooltip : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI text_headerField;
    [SerializeField] TextMeshProUGUI text_contentField;

    [SerializeField] LayoutElement le_layoutElement;

    [SerializeField] int _characterWrapLimit;


    [SerializeField] RectTransform _rectTransform;

    public void SetText(string content, string header = "")
    {
        if (string.IsNullOrEmpty(header))
        {
            text_headerField.gameObject.SetActive(false);
        }
        else
        {
            text_headerField.gameObject.SetActive(true);
            text_headerField.text = header;

        }

        text_contentField.text = content;

        int headerLenght = text_headerField.text.Length;
        int contentLength = text_contentField.text.Length;

        le_layoutElement.enabled = (headerLenght > _characterWrapLimit || contentLength > _characterWrapLimit) ? true : false;
    }
    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
    }
    // Update is called once per frame
    void Update()
    {
        Vector2 position = Input.mousePosition;

        float pivotX = position.x / Screen.width;
        float pivotY = position.y / Screen.height;

        _rectTransform.pivot = new Vector2(pivotX, pivotY);

        transform.position = position;
    }
}
