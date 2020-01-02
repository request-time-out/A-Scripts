// Decompiled with JetBrains decompiler
// Type: Housing.List.ListInputProvider
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using Battlehub.UIControls;
using UnityEngine;

namespace Housing.List
{
  public class ListInputProvider : InputProvider
  {
    [SerializeField]
    [Tooltip("キーボード操作で対象の切り替えが出来るか")]
    private bool canAxis = true;

    public override float HorizontalAxis
    {
      get
      {
        return !this.canAxis ? 0.0f : (!Input.GetKey((KeyCode) 276) ? (!Input.GetKey((KeyCode) 275) ? 0.0f : 1f) : -1f);
      }
    }

    public override float VerticalAxis
    {
      get
      {
        return !this.canAxis ? 0.0f : (!Input.GetKey((KeyCode) 273) ? (!Input.GetKey((KeyCode) 274) ? 0.0f : -1f) : 1f);
      }
    }

    public override float HorizontalAxis2
    {
      get
      {
        return !this.canAxis ? 0.0f : (!Input.GetKey((KeyCode) 260) ? (!Input.GetKey((KeyCode) 262) ? 0.0f : 1f) : -1f);
      }
    }

    public override float VerticalAxis2
    {
      get
      {
        return !this.canAxis ? 0.0f : (!Input.GetKey((KeyCode) 264) ? (!Input.GetKey((KeyCode) 258) ? 0.0f : -1f) : 1f);
      }
    }

    public override bool IsFunctionalButtonPressed
    {
      get
      {
        return Input.GetKey((KeyCode) 306) | Input.GetKey((KeyCode) 305);
      }
    }

    public override bool IsFunctional2ButtonPressed
    {
      get
      {
        return Input.GetKey((KeyCode) 304) | Input.GetKey((KeyCode) 303);
      }
    }

    public override bool IsDeleteButtonDown
    {
      get
      {
        return Input.GetKeyDown((KeyCode) (int) sbyte.MaxValue);
      }
    }

    public override bool IsSelectAllButtonDown
    {
      get
      {
        return Input.GetKeyDown((KeyCode) 97);
      }
    }
  }
}
