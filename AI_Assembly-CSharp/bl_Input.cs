// Decompiled with JetBrains decompiler
// Type: bl_Input
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class bl_Input : ScriptableObject
{
  public List<bl_KeyInfo> AllKeys;
  private static bl_Input m_Instance;

  public bl_Input()
  {
    base.\u002Ector();
  }

  public static bl_Input Instance
  {
    get
    {
      if (Object.op_Equality((Object) bl_Input.m_Instance, (Object) null))
      {
        bl_Input.m_Instance = Resources.Load("InputManager", typeof (bl_Input)) as bl_Input;
        Debug.Log((object) "Get Input");
      }
      return bl_Input.m_Instance;
    }
  }

  public void InitInput()
  {
    for (int index = 0; index < this.AllKeys.Count; ++index)
    {
      string str = string.Format("Key.{0}", (object) this.AllKeys[index].Function);
      this.AllKeys[index].Key = (KeyCode) PlayerPrefs.GetInt(str, (int) this.AllKeys[index].Key);
    }
  }

  public static bool GetKeyDown(string function)
  {
    return Input.GetKeyDown(bl_Input.Instance.GetKeyCode(function));
  }

  public static bool GetKey(string function)
  {
    return Input.GetKey(bl_Input.Instance.GetKeyCode(function));
  }

  public static bool GetKeyUp(string function)
  {
    return Input.GetKeyUp(bl_Input.Instance.GetKeyCode(function));
  }

  public bool SetKey(string function, KeyCode newKey)
  {
    for (int index = 0; index < this.AllKeys.Count; ++index)
    {
      if (this.AllKeys[index].Function == function)
      {
        this.AllKeys[index].Key = newKey;
        PlayerPrefs.SetInt(string.Format("Key.{0}", (object) function), (int) newKey);
        Debug.Log((object) string.Format("Done, replace key function: {0} with {1} key.", (object) function, (object) newKey.ToString()));
        return true;
      }
    }
    Debug.LogError((object) string.Format("Function {0} is not setup.", (object) function));
    return false;
  }

  public KeyCode GetKeyCode(string function)
  {
    for (int index = 0; index < this.AllKeys.Count; ++index)
    {
      if (this.AllKeys[index].Function == function)
        return this.AllKeys[index].Key;
    }
    Debug.LogError((object) string.Format("Key for {0} is not setup.", (object) function));
    return (KeyCode) 0;
  }

  public static float VerticalAxis
  {
    get
    {
      if (bl_Input.GetKey("Up") && !bl_Input.GetKey("Down"))
        return 1f;
      if (!bl_Input.GetKey("Up") && bl_Input.GetKey("Down"))
        return -1f;
      return bl_Input.GetKey("Up") && bl_Input.GetKey("Down") ? 0.5f : 0.0f;
    }
  }

  public static float HorizontalAxis
  {
    get
    {
      if (bl_Input.GetKey("Right") && !bl_Input.GetKey("Left"))
        return 1f;
      if (!bl_Input.GetKey("Right") && bl_Input.GetKey("Left"))
        return -1f;
      return bl_Input.GetKey("Right") && bl_Input.GetKey("Left") ? 0.5f : 0.0f;
    }
  }

  public bool isKeyUsed(KeyCode newKey)
  {
    for (int index = 0; index < this.AllKeys.Count; ++index)
    {
      if (this.AllKeys[index].Key == newKey)
        return true;
    }
    return false;
  }
}
