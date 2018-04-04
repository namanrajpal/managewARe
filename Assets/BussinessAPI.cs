using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BussinessAPI : MonoBehaviour {

    // Use this for initialization
    string coupon_url = "http://ab16d6c3.ngrok.io/api/sendCoupon";

    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    IEnumerator WaitForWWW_Order(WWW www)
    {
        yield return www;


        string txt = "";
        if (string.IsNullOrEmpty(www.error))
            txt = www.text;  //text of success
        else
            txt = www.error;  //error
        //Debug.Log("Order Result is: " + txt);

    }

    public void SendCoupon(string id)
    {
        Dictionary<string, string> headers = new Dictionary<string, string>();
        headers.Add("content-type", "application/json");

        string ourPostData = "{\n\t\"accessCode\": \"rrerere3443\",\n\t\"id\": \""+id+"\"\n\t\n}";
        byte[] pData = System.Text.Encoding.ASCII.GetBytes(ourPostData.ToCharArray());
        WWW api = new WWW(coupon_url, pData, headers);

        StartCoroutine(WaitForWWW_Order(api));
    }
}
