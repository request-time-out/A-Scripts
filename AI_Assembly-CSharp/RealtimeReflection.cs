// Decompiled with JetBrains decompiler
// Type: RealtimeReflection
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

public class RealtimeReflection : MonoBehaviour
{
  private ReflectionProbe probe;
  private Transform camT;

  public RealtimeReflection()
  {
    base.\u002Ector();
  }

  private void Awake()
  {
    this.probe = (ReflectionProbe) ((Component) this).GetComponent<ReflectionProbe>();
    this.camT = ((Component) Camera.get_main()).get_transform();
  }

  private void Update()
  {
    Vector3 position = this.camT.get_position();
    ((Component) this.probe).get_transform().set_position(new Vector3((float) position.x, (float) (position.y * -1.0), (float) position.z));
    this.probe.RenderProbe();
  }
}
