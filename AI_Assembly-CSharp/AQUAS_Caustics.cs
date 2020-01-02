// Decompiled with JetBrains decompiler
// Type: AQUAS_Caustics
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

public class AQUAS_Caustics : MonoBehaviour
{
  public float fps;
  public Texture2D[] frames;
  public float maxCausticDepth;
  private int frameIndex;
  private Projector projector;

  public AQUAS_Caustics()
  {
    base.\u002Ector();
  }

  private void Start()
  {
    this.projector = (Projector) ((Component) this).GetComponent<Projector>();
    this.projector.set_material((Material) Object.Instantiate<Material>((M0) this.projector.get_material()));
    this.NextFrame();
    this.InvokeRepeating("NextFrame", 1f / this.fps, 1f / this.fps);
    this.projector.get_material().SetFloat("_WaterLevel", (float) ((Component) ((Component) this).get_transform().get_parent()).get_transform().get_position().y);
    this.projector.get_material().SetFloat("_DepthFade", (float) ((Component) ((Component) this).get_transform().get_parent()).get_transform().get_position().y - this.maxCausticDepth);
  }

  private void Update()
  {
    this.projector.get_material().SetFloat("_DepthFade", (float) ((Component) ((Component) this).get_transform().get_parent()).get_transform().get_position().y - this.maxCausticDepth);
  }

  private void NextFrame()
  {
    this.projector.get_material().SetTexture("_Texture", (Texture) this.frames[this.frameIndex]);
    this.frameIndex = (this.frameIndex + 1) % this.frames.Length;
  }
}
