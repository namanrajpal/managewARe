using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using System;

public class OrderManager : MonoBehaviour {


    Vector3 initial = new Vector3(-0.5f, 0.05f, 4f);
    public List<OrderData> orders;
    public static GameObject selected;
    public RestManager restAPI;
    public GameObject[] lateObjects;

	// Use this for initialization
	void Start () {
        orders = new List<OrderData>();
        
	}


    public void GetLateOrders()
    {
        orders = new List<OrderData>(GetComponentsInChildren<OrderData>());
        foreach(OrderData w in orders)
        {
            string id = w.ID.text;
            Debug.Log("id:"+id);
            restAPI.GetOrder(id.Substring(3));
            if (restAPI.IsCurrentOrderLate())
            {
                //Debug.Log(w.GetComponentInParent<Material>().name);
                Debug.Log(w.GetComponentInParent<Transform>().gameObject.name);
            }
        }
    }

    public void PaintlateOrders()
    {
        foreach(GameObject g in lateObjects)
        {
            Renderer rend = g.GetComponent<Renderer>();
            rend.material.shader = Shader.Find("Specular");
            rend.material.SetColor("_SpecColor", Color.red);
            rend.material.SetColor("_Color", Color.red);
        }
        

    }

    // Update is called once per frame
    void Update () {

	}
}
