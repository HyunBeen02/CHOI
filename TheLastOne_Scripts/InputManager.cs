using UnityEngine;

public class InputManager : MonoBehaviour
{
    public bool fire()
    {
        return Input.GetMouseButton(0);
    }
    public bool throwGrenade()
    {
        return Input.GetKeyDown(KeyCode.F);
    }
    public bool scrollUp()
    {
        return Input.GetAxis("Mouse ScrollWheel") > 0;
    }
    public bool scrollDown()
    {
        return Input.GetAxis("Mouse ScrollWheel") < 0;
    }
    public bool moveKey()
    {
        return Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D);
    }
    public bool KeyDownNum(int num)
    {
        switch (num)
        {
            case 1: return Input.GetKeyDown(KeyCode.Alpha1);
            case 2: return Input.GetKeyDown(KeyCode.Alpha2);
            case 3: return Input.GetKeyDown(KeyCode.Alpha3);
        }
        return false;
    }
    public bool reload()
    {
        return Input.GetKeyDown(KeyCode.R);
    }
    public bool BackToMenu()
    {
        return Input.GetKeyDown(KeyCode.Escape);
    }
}