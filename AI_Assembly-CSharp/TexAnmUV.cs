// Decompiled with JetBrains decompiler
// Type: TexAnmUV
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

public class TexAnmUV : MonoBehaviour
{
  public YS_Timer yst;
  public TexAnmUVParam ScrollU;
  public TexAnmUVParam ScrollV;
  private int passTimeU;
  private int passTimeV;
  private Renderer rendererData;

  public TexAnmUV()
  {
    base.\u002Ector();
  }

  private void Start()
  {
    this.rendererData = (Renderer) ((Component) this).GetComponent<Renderer>();
    if (!Object.op_Equality((Object) null, (Object) this.rendererData))
      return;
    ((Behaviour) this).set_enabled(false);
  }

  private void Update()
  {
    float num1 = 0.0f;
    float num2 = 0.0f;
    if (this.ScrollU.Usage)
    {
      float num3;
      if (Object.op_Equality((Object) null, (Object) this.yst))
      {
        this.passTimeU += (int) ((double) Time.get_deltaTime() * 1000.0);
        while (this.passTimeU >= this.ScrollU.ChangeTime)
          this.passTimeU -= this.ScrollU.ChangeTime;
        num3 = Mathf.InverseLerp(0.0f, (float) this.ScrollU.ChangeTime, (float) this.passTimeU);
      }
      else
        num3 = this.yst.rate;
      if (this.ScrollU.Plus)
        num3 = 1f - num3;
      num1 = num3 + this.ScrollU.correct;
      if ((double) num1 > 1.0)
        --num1;
    }
    if (this.ScrollV.Usage)
    {
      float num3;
      if (Object.op_Equality((Object) null, (Object) this.yst))
      {
        this.passTimeV += (int) ((double) Time.get_deltaTime() * 1000.0);
        while (this.passTimeV >= this.ScrollV.ChangeTime)
          this.passTimeV -= this.ScrollV.ChangeTime;
        num3 = Mathf.InverseLerp(0.0f, (float) this.ScrollV.ChangeTime, (float) this.passTimeV);
      }
      else
        num3 = this.yst.rate;
      if (this.ScrollV.Plus)
        num3 = 1f - num3;
      num2 = num3 + this.ScrollV.correct;
      if ((double) num2 > 1.0)
        --num2;
    }
    Vector2 vector2;
    ((Vector2) ref vector2).\u002Ector(num1, 1f - num2);
    this.rendererData.get_material().SetTextureOffset("_MainTex", vector2);
  }
}
