using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
public class OrderData : MonoBehaviour {

    public TextMesh ID;
    public TextMesh Date;
    public TextMesh Customer;

    public TextMesh Status;

    public TextMesh OrderTotal;
    public int index;



    // Use this for initialization
    void Start () {
        Invoke("UpdateData", 1f);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void UpdateData()
    {
        var root = JSON.Parse(RestManager.jsonText);
        ID.text = "ID:" +root["orders"][index]["id"];
        Date.text = "Date Created:\n" + root["orders"][index]["dateCreated"];
        Customer.text = "Customer:" + root["orders"][index]["customer"]["name"];
        Status.text ="Order Status:" + root["orders"][index]["status"];
        OrderTotal.text = "Order Total: $" + root["orders"][index]["totals"][0]["value"];
    }
}
