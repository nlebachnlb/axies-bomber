using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UserData
{
    public List<int> ownedAxieIds = new List<int>();
}

public class UserDataModel : MonoBehaviour
{
    public UserData User { get; private set; }

    public UserData GenerateDefaultUserData()
    {
        UserData user = new UserData();
        user.ownedAxieIds.Add(0);
        user.ownedAxieIds.Add(1);
        user.ownedAxieIds.Add(2);

        return user;
    }

    private void Awake()
    {
        User = GenerateDefaultUserData();
    }
}
