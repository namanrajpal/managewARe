using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;

public class UpdateOrder : MonoBehaviour {


    public TextMesh name;

    public TextMesh city;
    public TextMesh state;

    public TextMesh email;
    OrderData d;
    void Start()
    {
        d = GetComponent<OrderData>();
    }

    void OnSelect()
    {
        var root = JSON.Parse(RestManager.jsonText);
        city.text = "Date Created:" + root["orders"][d.index]["dateCreated"].Value.Substring(0,10);
        name.text = "Customer:" + root["orders"][d.index]["customer"]["name"];
        state.text = "Order Status:" + root["orders"][d.index]["status"];
        email.text = "Order Total: $" + root["orders"][d.index]["totals"][0]["value"];
    }
    

	// Update is called once per frame
	void Update () {
		
	}
}
