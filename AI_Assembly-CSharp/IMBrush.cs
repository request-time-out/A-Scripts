// Decompiled with JetBrains decompiler
// Type: IMBrush
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

public abstract class IMBrush : MonoBehaviour
{
  public IncrementalModeling im;
  public float fadeRadius;
  public bool bSubtract;

  protected IMBrush()
  {
    base.\u002Ector();
  }

  public float PowerScale
  {
    get
    {
      return this.bSubtract ? -1f : 1f;
    }
  }

  [ContextMenu("Draw")]
  public void Draw()
  {
    if (Object.op_Equality((Object) this.im, (Object) null))
      this.im = Utils.FindComponentInParents<IncrementalModeling>(((Component) this).get_transform());
    if (Object.op_Inequality((Object) this.im, (Object) null))
      this.DoDraw();
    else
      Debug.LogError((object) "no IncrementalModeling component for this brush found in hierarchy.");
  }

  protected abstract void DoDraw();
}
