// Decompiled with JetBrains decompiler
// Type: SetRenderQueue
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

[AddComponentMenu("Rendering/SetRenderQueue")]
public class SetRenderQueue : MonoBehaviour
{
  [SerializeField]
  protected int[] m_queues;

  public SetRenderQueue()
  {
    base.\u002Ector();
  }

  protected void Awake()
  {
    Material[] materials = ((Renderer) ((Component) this).GetComponent<Renderer>()).get_materials();
    for (int index = 0; index < materials.Length && index < this.m_queues.Length; ++index)
      materials[index].set_renderQueue(this.m_queues[index]);
  }
}
