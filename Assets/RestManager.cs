using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using SimpleJSON;

public class RestManager : MonoBehaviour {


    static string auth_url = "https://hackgt-api.ncrcloud.com/security/authentication/login";
    static string getOrdersurl = "https://hackgt-api.ncrcloud.com/order/orders/find?take=20&skip=0";
    static string getOrderByIDURL = "https://hackgt-api.ncrcloud.com/order/orders";
    public StringWrapper acces_token = new StringWrapper();

    public static string jsonText;
    public static List<Order> ordersRetrieved;
    public static string oneOrderJSON;

    public class StringWrapper
    {
        public string token;
        public StringWrapper()
        {

        }
        public StringWrapper(string x)
        {
            token = x;
        }
    }
    // Use this for initialization
    IEnumerator Start () {

        ordersRetrieved = new List<Order>();

        yield return StartCoroutine(GetAccessToken((tokenResult) =>
        {
            acces_token = tokenResult;

        }));

        Debug.Log("Token is : " + acces_token.token);
        GetOrders();


    }

    #region JSON Helper Classes
    [Serializable]
    public class TokenClassName
    {
        public string token;
    }

    [Serializable]
    public class Customer
    {
        public string id { get; set; }
        public string name { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
    }
    [Serializable]
    public class Total
    {
        public string type { get; set; }
        public double value { get; set; }
        public string lineId { get; set; }
    }
    [Serializable]
    public class Order
    {
        public DateTime dateCreated { get; set; }
        public DateTime dateUpdated { get; set; }
        public string id { get; set; }
        public DateTime expireAt { get; set; }
        public string comments { get; set; }
        public string channel { get; set; }
        public string currency { get; set; }
        public Customer customer { get; set; }
        public string errorDescription { get; set; }
        public string referenceId { get; set; }
        public string status { get; set; }
        public List<Total> totals { get; set; }
        public DateTime etag { get; set; }
    }
    [Serializable]
    public class RootObject
    {
        public int totalResults { get; set; }
        public List<Order> orders { get; set; }
    }

    #endregion

    IEnumerator WaitForWWW(WWW www)
    {
        yield return www;


        string txt = "";
        if (string.IsNullOrEmpty(www.error))
            txt = www.text;  //text of success
        else
            txt = www.error;  //error
        Debug.Log("All Orders are : " + txt);
        jsonText = txt;
        
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
        oneOrderJSON = txt;
        
    }

    public bool IsCurrentOrderLate()
    {
        Debug.Log("late order :" + oneOrderJSON);
        var root = JSON.Parse(oneOrderJSON);
        //Debug.Log("getting Order Data :" + w.Status.text + " " + w.Date.text.Substring(14, 10));
        DateTime oDate = DateTime.Parse(root["fulfillment"]["pickupDate"].Value.Substring(0, 10));
        TimeSpan timeDiff = DateTime.Today.Subtract(oDate);
        Debug.Log(oDate.ToString());
        Debug.Log("time Differene  " + timeDiff.Days);
        if (timeDiff.Days > 20)
            return true;
        else
            return false;
    }

    public void GetOrder(string id)
    {

        WWWForm form = new WWWForm();

        Dictionary<string, string> headers = new Dictionary<string, string>();

        headers.Add("authorization", "AccessToken " + acces_token.token);
        headers.Add("nep-application-key", "8a82859f5ef21870015ef2fa5e5f0000");
        headers.Add("nep-organization", "org-1-3");
        headers.Add("content-type", "application/json");

        WWW api = new WWW(getOrderByIDURL + "//"+id, null,headers);

        StartCoroutine(WaitForWWW_Order(api));
    }

    private void GetOrders()
    {
        //Dictionary<string, string> content = new Dictionary<string, string>();
        //Fill key and value
        //content.Add("postman-token", "994d47cb-ec45-71f0-1455-503bf5a8e0b1");

        WWWForm form = new WWWForm();
        string ourPostData = "{\"notes\":\"none\"}";
        Dictionary<string, string> headers = new Dictionary<string, string>();

        headers.Add("authorization", "AccessToken " +acces_token.token);
        headers.Add("nep-application-key", "8a82859f5ef21870015ef2fa5e5f0000");
        headers.Add("nep-organization","org-1-3");
        headers.Add("content-type", "application/json");
        byte[] pData = System.Text.Encoding.ASCII.GetBytes(ourPostData.ToCharArray());
        WWW api = new WWW(getOrdersurl, pData, headers);
        
        StartCoroutine(WaitForWWW(api));
    }

    private IEnumerator GetOrdersLine(Action<string> orders)
    {
        yield return null;
    }


    private IEnumerator GetAccessToken(Action<StringWrapper> result)
    {
        //Dictionary<string, string> content = new Dictionary<string, string>();
        //Fill key and value
        //content.Add("postman-token", "994d47cb-ec45-71f0-1455-503bf5a8e0b1");

        WWWForm form = new WWWForm();

        UnityWebRequest www = UnityWebRequest.Post(auth_url,form);
        www.SetRequestHeader("nep-application-key", "8a82859f5ef21870015ef2fa5e5f0000");
        www.SetRequestHeader("authorization", "Basic L29yZy0xL2FkbWluOkNoYW5nM20zISEtYWRtaW4tb3JnLTE=");
        
        //Send request
        yield return www.Send();

        if (!www.isNetworkError)
        {
            string resultContent = www.downloadHandler.text;

           Debug.Log("getting result from token query " +resultContent);
            TokenClassName json = JsonUtility.FromJson<TokenClassName>(resultContent);

            //Return result
            result(new StringWrapper(json.token));
            acces_token.token = json.token;
        }
        else
        {
            //Return null
            result(new StringWrapper());
        }
    }


    private void Update()
    {
#if UNITY_EDITOR
        if(Input.GetKeyDown(KeyCode.X))
        {
            var root = JSON.Parse(jsonText);
            string id = root["orders"][0]["id"].Value;
            GetOrder(id);
        }
#endif
    }
}
