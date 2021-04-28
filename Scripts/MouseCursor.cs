using UnityEngine;
using UnityEngine.UI;

public class MouseCursor : MonoBehaviour
{
    [SerializeField] Image dayCursor;
    [SerializeField] Image nightCursor;

    void Start()
    {    
        //커서 안보이게
        Cursor.visible = false;
        dayCursor.gameObject.SetActive(true);
    }
    void Update()
    {
        //커서이미지 위치 마우스 위치로 조정
        dayCursor.rectTransform.position = Input.mousePosition;
        nightCursor.rectTransform.position = Input.mousePosition;
    }
    //낮이될 때 낮 커서 활성화
    public void setDayCursor()
    {
        dayCursor.gameObject.SetActive(true);
        nightCursor.gameObject.SetActive(false);
    }
    //밤이될 때 밤 커서 활성화
    public void setNightCursor()
    {
        nightCursor.gameObject.SetActive(true);
        dayCursor.gameObject.SetActive(false);  
    }
}
