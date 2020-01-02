// Decompiled with JetBrains decompiler
// Type: Debug_CountRaycastTargetOn
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

public class Debug_CountRaycastTargetOn : MonoBehaviour
{
  [Button("CountRaycastTargetOn", "RaycastTargetOn数取得", new object[] {})]
  public int countRaycastTargetOn;
  public int raycastTargetOnNum;

  public Debug_CountRaycastTargetOn()
  {
    base.\u002Ector();
  }

  public void CountRaycastTargetOn()
  {
    this.raycastTargetOnNum = 0;
    foreach (Image componentsInChild in (Image[]) ((Component) this).GetComponentsInChildren<Image>(true))
      ++this.raycastTargetOnNum;
  }
}
