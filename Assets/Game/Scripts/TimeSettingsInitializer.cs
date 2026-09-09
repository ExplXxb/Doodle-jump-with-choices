using UnityEngine;
using VContainer.Unity;

public class TimeSettingsInitializer : IInitializable
{
    public void Initialize()
    {
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;
        Debug.Log("Time.timeScale = 1f;\r\n        Time.fixedDeltaTime = 0.02f;");
    }
}
