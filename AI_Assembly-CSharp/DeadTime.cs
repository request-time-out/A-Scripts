// Decompiled with JetBrains decompiler
// Type: DeadTime
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

public class DeadTime : MonoBehaviour
{
  public float deadTime;
  public bool destroyRoot;

  public DeadTime()
  {
    base.\u002Ector();
  }

  private void Awake()
  {
    Object.Destroy(this.destroyRoot ? (Object) ((Component) ((Component) this).get_transform().get_root()).get_gameObject() : (Object) ((Component) this).get_gameObject(), this.deadTime);
  }
}
