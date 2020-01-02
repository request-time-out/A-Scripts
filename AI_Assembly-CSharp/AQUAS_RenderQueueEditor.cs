// Decompiled with JetBrains decompiler
// Type: AQUAS_RenderQueueEditor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("AQUAS/Render Queue Controller")]
public class AQUAS_RenderQueueEditor : MonoBehaviour
{
  public int renderQueueIndex;

  public AQUAS_RenderQueueEditor()
  {
    base.\u002Ector();
  }

  private void Update()
  {
    ((Renderer) ((Component) this).get_gameObject().GetComponent<Renderer>()).get_sharedMaterial().set_renderQueue(this.renderQueueIndex);
  }
}
