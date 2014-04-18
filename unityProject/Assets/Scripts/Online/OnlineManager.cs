using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Parse;

public class OnlineManager : Singleton<OnlineManager>
{
    public bool testSaveObject = false;

    private ParseUser mTestUser = null;

    void Start()
    {
        DontDestroyOnLoad(gameObject);
        ParseAnalytics.TrackAppOpenedAsync();
        LoginTestUser();
        //TestTracking();
    }

    void Update()
    {
        if (testSaveObject)
        {
            testSaveObject = false;
            for (int i = 0; i < 1; i++)
            {
                SaveTestObject();
            }
        }
    }

    void SaveTestObject()
    {
        ParseObject testObject = new ParseObject("TestObject");
        testObject["foo"] = "bar";
        testObject.SaveAsync().ContinueWith(t =>
        {
            if (t.IsFaulted)
            {
                Debug.Log("saving failed!");
            }
            else
            {
                //success
                //Debug.Log("saving succeeded!");
            }
        });
    }

    void CreateTestUser()
    {
        var user = new ParseUser();
        user.Username = "Chandler55";
        user.Password = "bing";
        user.Email = "michaelle@gmail.com";
        user.SignUpAsync();
        //System.Threading.Tasks.Task<ParseUser> logInTask = ParseFacebookUtils.LogInAsync("23432", "3232", System.DateTime.Now);
        //ParseUser userfb = logInTask.Result;
    }

    void LoginTestUser()
    {
        ParseUser.LogInAsync("Chandler55", "bing").ContinueWith(t =>
        {
            if (t.IsFaulted || t.IsCanceled)
            {
                // The login failed. Check t.Exception to see why.
            }
            else
            {
                // Login was successful.
                Debug.Log("user login was successful");
                mTestUser = t.Result;

                //List<string> friendsList = new List<string>();
                //friendsList.Add("userid123123");
                //mTestUser["facebookFriendsList"] = friendsList;
                //mTestUser.SaveAsync();
            }
        });
    }

    void TestTracking()
    {
        var dimensions = new Dictionary<string, string>();
        dimensions.Add("level", "12");
        dimensions.Add("secondsPlayed", "184");
        dimensions.Add("completed", "false");
        ParseAnalytics.TrackEventAsync("LostLife", dimensions).ContinueWith(t =>
        {
            if (t.IsFaulted || t.IsCanceled)
            {
  
            }
            else
            {
                // Login was successful.
                Debug.Log("test tracking was successful");
            }
        });
    }
}
