// Decompiled with JetBrains decompiler
// Type: HitCollision
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class HitCollision : MonoBehaviour
{
  public List<GameObject> lstObj;

  public HitCollision()
  {
    base.\u002Ector();
  }

  private void Reset()
  {
    for (int index = 0; index < ((Component) this).get_transform().get_childCount(); ++index)
      this.lstObj.Add(((Component) ((Component) this).get_transform().GetChild(index)).get_gameObject());
  }
}
