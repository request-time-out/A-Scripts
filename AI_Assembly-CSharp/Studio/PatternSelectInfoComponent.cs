// Decompiled with JetBrains decompiler
// Type: Studio.PatternSelectInfoComponent
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using Illusion.Extensions;
using UnityEngine;
using UnityEngine.UI;

namespace Studio
{
  public class PatternSelectInfoComponent : MonoBehaviour
  {
    public PatternSelectInfo info;
    public Image img;
    public Toggle tgl;

    public PatternSelectInfoComponent()
    {
      base.\u002Ector();
    }

    public void Disable(bool disable)
    {
      if (!Object.op_Implicit((Object) this.tgl))
        return;
      ((Selectable) this.tgl).set_interactable(!disable);
    }

    public void Disvisible(bool disvisible)
    {
      ((Component) this).get_gameObject().SetActiveIfDifferent(!disvisible);
    }
  }
}
