// Decompiled with JetBrains decompiler
// Type: Battlehub.UIControls.RectTransformChangeListener
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine.EventSystems;

namespace Battlehub.UIControls
{
  public class RectTransformChangeListener : UIBehaviour
  {
    public RectTransformChangeListener()
    {
      base.\u002Ector();
    }

    public event Battlehub.UIControls.RectTransformChanged RectTransformChanged;

    protected virtual void OnRectTransformDimensionsChange()
    {
      if (this.RectTransformChanged == null)
        return;
      this.RectTransformChanged();
    }
  }
}
