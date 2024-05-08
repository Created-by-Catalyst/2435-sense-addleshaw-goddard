using UnityEngine;

public class KeyboardManager : MonoBehaviour
{
    [SerializeField] WinState winState;
    public void KeyPress(string key)
    {
        winState.AddCharacter(key);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
