// Decompiled with JetBrains decompiler
// Type: FlockWaypointTrigger
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

public class FlockWaypointTrigger : MonoBehaviour
{
  public float _timer;
  public FlockChild _flockChild;

  public FlockWaypointTrigger()
  {
    base.\u002Ector();
  }

  public void Start()
  {
    if (Object.op_Equality((Object) this._flockChild, (Object) null))
      this._flockChild = (FlockChild) ((Component) ((Component) this).get_transform().get_parent()).GetComponent<FlockChild>();
    float num = Random.Range(this._timer, this._timer * 3f);
    this.InvokeRepeating("Trigger", num, num);
  }

  public void Trigger()
  {
    this._flockChild.Wander(0.0f);
  }
}
