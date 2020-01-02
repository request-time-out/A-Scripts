// Decompiled with JetBrains decompiler
// Type: IKhandle
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

public class IKhandle : MonoBehaviour
{
  public Transform target;

  public IKhandle()
  {
    base.\u002Ector();
  }

  public void Init()
  {
    ((Component) this).get_transform().set_position(this.target.get_position());
    ((Component) this).get_transform().set_rotation(this.target.get_rotation());
  }

  public void BaseReset()
  {
    this.target = (Transform) null;
  }
}
