// Decompiled with JetBrains decompiler
// Type: SetProjectioinQueue
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

public class SetProjectioinQueue : MonoBehaviour
{
  [SerializeField]
  private int _queue;

  public SetProjectioinQueue()
  {
    base.\u002Ector();
  }

  private void Awake()
  {
    ((Projector) ((Component) this).GetComponent<Projector>()).get_material().set_renderQueue(this._queue);
  }
}
