// Decompiled with JetBrains decompiler
// Type: WaterUvAnimation
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

public class WaterUvAnimation : MonoBehaviour
{
  public bool IsReverse;
  public float Speed;
  public int MaterialNomber;
  private Material mat;
  private float deltaFps;
  private bool isVisible;
  private bool isCorutineStarted;
  private float offset;
  private float delta;

  public WaterUvAnimation()
  {
    base.\u002Ector();
  }

  private void Awake()
  {
    this.mat = ((Renderer) ((Component) this).GetComponent<Renderer>()).get_materials()[this.MaterialNomber];
  }

  private void Update()
  {
    if (this.IsReverse)
    {
      this.offset -= Time.get_deltaTime() * this.Speed;
      if ((double) this.offset < 0.0)
        this.offset = 1f;
    }
    else
    {
      this.offset += Time.get_deltaTime() * this.Speed;
      if ((double) this.offset > 1.0)
        this.offset = 0.0f;
    }
    Vector2 vector2;
    ((Vector2) ref vector2).\u002Ector(0.0f, this.offset);
    this.mat.SetTextureOffset("_BumpMap", vector2);
    this.mat.SetFloat("_OffsetYHeightMap", this.offset);
  }
}
