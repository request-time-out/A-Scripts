// Decompiled with JetBrains decompiler
// Type: Battlehub.UIControls.InputProvider
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

namespace Battlehub.UIControls
{
  public class InputProvider : MonoBehaviour
  {
    public InputProvider()
    {
      base.\u002Ector();
    }

    public virtual float HorizontalAxis
    {
      get
      {
        if (Input.GetKey((KeyCode) 276))
          return -1f;
        return Input.GetKey((KeyCode) 275) ? 1f : 0.0f;
      }
    }

    public virtual float VerticalAxis
    {
      get
      {
        if (Input.GetKey((KeyCode) 273))
          return 1f;
        return Input.GetKey((KeyCode) 274) ? -1f : 0.0f;
      }
    }

    public virtual float HorizontalAxis2
    {
      get
      {
        if (Input.GetKey((KeyCode) 260))
          return -1f;
        return Input.GetKey((KeyCode) 262) ? 1f : 0.0f;
      }
    }

    public virtual float VerticalAxis2
    {
      get
      {
        if (Input.GetKey((KeyCode) 264))
          return 1f;
        return Input.GetKey((KeyCode) 258) ? -1f : 0.0f;
      }
    }

    public virtual bool IsHorizontalButtonDown
    {
      get
      {
        return Input.GetKeyDown((KeyCode) 276) || Input.GetKeyDown((KeyCode) 275);
      }
    }

    public virtual bool IsVerticalButtonDown
    {
      get
      {
        return Input.GetKeyDown((KeyCode) 273) || Input.GetKeyDown((KeyCode) 274);
      }
    }

    public virtual bool IsHorizontal2ButtonDown
    {
      get
      {
        return Input.GetKeyDown((KeyCode) 260) || Input.GetKeyDown((KeyCode) 262);
      }
    }

    public virtual bool IsVertical2ButtonDown
    {
      get
      {
        return Input.GetKeyDown((KeyCode) 264) || Input.GetKeyDown((KeyCode) 258);
      }
    }

    public virtual bool IsFunctionalButtonPressed
    {
      get
      {
        return Input.GetKey((KeyCode) 306);
      }
    }

    public virtual bool IsFunctional2ButtonPressed
    {
      get
      {
        return Input.GetKey((KeyCode) 304);
      }
    }

    public virtual bool IsSubmitButtonDown
    {
      get
      {
        return Input.GetKeyDown((KeyCode) 13);
      }
    }

    public virtual bool IsSubmitButtonUp
    {
      get
      {
        return Input.GetKeyUp((KeyCode) 13);
      }
    }

    public virtual bool IsCancelButtonDown
    {
      get
      {
        return Input.GetKeyDown((KeyCode) 27);
      }
    }

    public virtual bool IsDeleteButtonDown
    {
      get
      {
        return Input.GetKeyDown((KeyCode) (int) sbyte.MaxValue);
      }
    }

    public virtual bool IsSelectAllButtonDown
    {
      get
      {
        return Input.GetKeyDown((KeyCode) 97);
      }
    }

    public virtual bool IsAnyKeyDown
    {
      get
      {
        return Input.get_anyKeyDown();
      }
    }

    public virtual Vector3 MousePosition
    {
      get
      {
        return Input.get_mousePosition();
      }
    }

    public virtual bool IsMouseButtonDown(int button)
    {
      return Input.GetMouseButtonDown(button);
    }

    public virtual bool IsMousePresent
    {
      get
      {
        return Input.get_mousePresent();
      }
    }

    public virtual bool IsKeyboardPresent
    {
      get
      {
        return true;
      }
    }

    public virtual int TouchCount
    {
      get
      {
        return Input.get_touchCount();
      }
    }

    public virtual Touch GetTouch(int i)
    {
      return Input.GetTouch(i);
    }

    public virtual bool IsTouchSupported
    {
      get
      {
        return Input.get_touchSupported();
      }
    }
  }
}
