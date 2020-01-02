// Decompiled with JetBrains decompiler
// Type: FixShaderQueue
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

public class FixShaderQueue : MonoBehaviour
{
  public int AddQueue;
  private Renderer rend;

  public FixShaderQueue()
  {
    base.\u002Ector();
  }

  private void Start()
  {
    this.rend = (Renderer) ((Component) this).GetComponent<Renderer>();
    if (Object.op_Inequality((Object) this.rend, (Object) null))
    {
      Material sharedMaterial = this.rend.get_sharedMaterial();
      sharedMaterial.set_renderQueue(sharedMaterial.get_renderQueue() + this.AddQueue);
    }
    else
      this.Invoke("SetProjectorQueue", 0.1f);
  }

  private void SetProjectorQueue()
  {
    Material material = ((Projector) ((Component) this).GetComponent<Projector>()).get_material();
    material.set_renderQueue(material.get_renderQueue() + this.AddQueue);
  }

  private void OnDisable()
  {
    if (!Object.op_Inequality((Object) this.rend, (Object) null))
      return;
    this.rend.get_sharedMaterial().set_renderQueue(-1);
  }
}
