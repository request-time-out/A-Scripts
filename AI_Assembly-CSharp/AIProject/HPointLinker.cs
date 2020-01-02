// Decompiled with JetBrains decompiler
// Type: AIProject.HPointLinker
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

namespace AIProject
{
  public class HPointLinker : MonoBehaviour
  {
    [SerializeField]
    private HPoint _hPoint;

    public HPointLinker()
    {
      base.\u002Ector();
    }

    private void Start()
    {
      ((ActionPoint) ((Component) this).GetComponent<ActionPoint>()).HPoint = this._hPoint;
    }
  }
}
