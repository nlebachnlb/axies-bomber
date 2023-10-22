using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New AppRoot config", menuName = "Config/System/AppRoot")]
public class AppRootConfig : ScriptableObject
{
    public string startSceneName;
    public string playScene;
    public string homeScene;
    public AvailableAxieHeroesConfig availableAxies;
}
