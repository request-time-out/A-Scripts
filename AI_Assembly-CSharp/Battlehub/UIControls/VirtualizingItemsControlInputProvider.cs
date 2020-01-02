// Decompiled with JetBrains decompiler
// Type: Battlehub.UIControls.VirtualizingItemsControlInputProvider
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

namespace Battlehub.UIControls
{
  public class VirtualizingItemsControlInputProvider : InputProvider
  {
    public KeyCode MultiselectKey = (KeyCode) 306;
    public KeyCode RangeselectKey = (KeyCode) 304;
    public KeyCode SelectAllKey = (KeyCode) 97;
    public KeyCode DeleteKey = (KeyCode) (int) sbyte.MaxValue;

    public override bool IsFunctionalButtonPressed
    {
      get
      {
        return Input.GetKey(this.MultiselectKey);
      }
    }

    public override bool IsFunctional2ButtonPressed
    {
      get
      {
        return Input.GetKey(this.RangeselectKey);
      }
    }

    public override bool IsDeleteButtonDown
    {
      get
      {
        return Input.GetKeyDown(this.DeleteKey);
      }
    }

    public override bool IsSelectAllButtonDown
    {
      get
      {
        return Input.GetKeyDown(this.SelectAllKey);
      }
    }
  }
}
