using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SendCoupon : MonoBehaviour {

    public bool isFocusedOn;

    public GameObject _screenToOpen;
    public GameObject _textToUpdate;
    public BussinessAPI api;
    public TextMesh idText;
    Animator anim;
    void OnSelect()
    {
        //Keep the screen null if you dont wanna use this feature
        if (_screenToOpen != null)
        {
            if (_screenToOpen.activeInHierarchy == false)
                _screenToOpen.SetActive(true);
            else
                _screenToOpen.SetActive(false);
        }

        OrderManager.selected = this.gameObject;

        if (_textToUpdate != null)
            _textToUpdate.GetComponent<TextMesh>().text = GetComponent<OrderData>().ID.text;

        api.SendCoupon(idText.GetComponent<TextMesh>().text.Substring(3));

    }

    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void OnGazeEnter()
    {
        //Debug.Log("Gaze Enter at" + this.gameObject.name);
        if (anim != null)
        {
            anim.SetBool("GazedAt", true);
        }

    }

    public void OnGazeExit()
    {
        //Debug.Log("Gaze Exit at" + this.gameObject.name);
        if (anim != null)
        {
            anim.SetBool("GazedAt", false);
        }
    }

    private void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.G))
        {
            OnSelect();
        }
#endif

    }
}
