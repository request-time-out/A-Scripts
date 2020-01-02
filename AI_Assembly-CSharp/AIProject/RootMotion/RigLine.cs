// Decompiled with JetBrains decompiler
// Type: AIProject.RootMotion.RigLine
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

namespace AIProject.RootMotion
{
  public class RigLine : MonoBehaviour
  {
    [SerializeField]
    private Transform _parent;

    public RigLine()
    {
      base.\u002Ector();
    }

    private void Start()
    {
      if (!Object.op_Equality((Object) this._parent, (Object) null))
        return;
      this._parent = ((Component) this).get_transform();
    }
  }
}
