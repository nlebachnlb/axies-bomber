using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[System.Serializable]
public class UserData
{
    public List<int> ownedAxieIds = new List<int>();
    public List<int> currentPickedAxies;

    public UserData()
    {
        currentPickedAxies = new List<int>()
        {
            -1, -1, -1
        };
    }
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

    public void ResetPickedAxies()
    {
        for (int i = 0; i < User.currentPickedAxies.Count; ++i)
            User.currentPickedAxies[i] = -1;
    }

    public void PickAxie(int slot, int axieId)
    {
        // Cancel the slots this axie occupies
        for (int i = 0; i < User.currentPickedAxies.Count; ++i)
        {
            if (User.currentPickedAxies[i] == axieId)
            {
                User.currentPickedAxies[i] = -1;
            }
        }

        // Put Axie into new slot
        User.currentPickedAxies[slot] = axieId;
    }

    public bool IsAxiePicked(int axieId)
    {
        return User.currentPickedAxies.Contains(axieId);
    }

    private void Awake()
    {
        User = GenerateDefaultUserData();

        EventBus.onPickAxie += OnPickAxie;
    }

    private void OnPickAxie(int slot, AxiePackedConfig config)
    {
        PickAxie(slot, config.id);
    }
}
