// Decompiled with JetBrains decompiler
// Type: Battlehub.UIControls.InputProviderAdapter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

namespace Battlehub.UIControls
{
  public class InputProviderAdapter : InputProvider
  {
    [SerializeField]
    private InputProvider m_inputProvider;

    public InputProvider InputProvider
    {
      get
      {
        return this.m_inputProvider;
      }
      set
      {
        this.m_inputProvider = value;
      }
    }

    public override float HorizontalAxis
    {
      get
      {
        return this.m_inputProvider.HorizontalAxis;
      }
    }

    public override float VerticalAxis
    {
      get
      {
        return this.m_inputProvider.VerticalAxis;
      }
    }

    public override float HorizontalAxis2
    {
      get
      {
        return this.m_inputProvider.HorizontalAxis2;
      }
    }

    public override float VerticalAxis2
    {
      get
      {
        return this.m_inputProvider.VerticalAxis2;
      }
    }

    public override bool IsHorizontalButtonDown
    {
      get
      {
        return this.m_inputProvider.IsHorizontalButtonDown;
      }
    }

    public override bool IsVerticalButtonDown
    {
      get
      {
        return this.m_inputProvider.IsVerticalButtonDown;
      }
    }

    public override bool IsHorizontal2ButtonDown
    {
      get
      {
        return this.m_inputProvider.IsHorizontal2ButtonDown;
      }
    }

    public override bool IsVertical2ButtonDown
    {
      get
      {
        return this.m_inputProvider.IsVertical2ButtonDown;
      }
    }

    public override bool IsFunctionalButtonPressed
    {
      get
      {
        return this.m_inputProvider.IsFunctionalButtonPressed;
      }
    }

    public override bool IsFunctional2ButtonPressed
    {
      get
      {
        return this.m_inputProvider.IsFunctional2ButtonPressed;
      }
    }

    public override bool IsSubmitButtonDown
    {
      get
      {
        return this.m_inputProvider.IsSubmitButtonDown;
      }
    }

    public override bool IsSubmitButtonUp
    {
      get
      {
        return this.m_inputProvider.IsSubmitButtonUp;
      }
    }

    public override bool IsCancelButtonDown
    {
      get
      {
        return this.m_inputProvider.IsCancelButtonDown;
      }
    }

    public override bool IsDeleteButtonDown
    {
      get
      {
        return this.m_inputProvider.IsDeleteButtonDown;
      }
    }

    public override bool IsSelectAllButtonDown
    {
      get
      {
        return this.m_inputProvider.IsSelectAllButtonDown;
      }
    }

    public override bool IsAnyKeyDown
    {
      get
      {
        return this.m_inputProvider.IsAnyKeyDown;
      }
    }

    public override Vector3 MousePosition
    {
      get
      {
        return this.m_inputProvider.MousePosition;
      }
    }

    public override bool IsMouseButtonDown(int button)
    {
      return this.m_inputProvider.IsMouseButtonDown(button);
    }

    public override bool IsMousePresent
    {
      get
      {
        return this.m_inputProvider.IsMousePresent;
      }
    }

    public override bool IsKeyboardPresent
    {
      get
      {
        return this.m_inputProvider.IsKeyboardPresent;
      }
    }

    public override int TouchCount
    {
      get
      {
        return this.m_inputProvider.TouchCount;
      }
    }

    public override Touch GetTouch(int i)
    {
      return this.m_inputProvider.GetTouch(i);
    }

    public override bool IsTouchSupported
    {
      get
      {
        return this.m_inputProvider.IsTouchSupported;
      }
    }
  }
}
