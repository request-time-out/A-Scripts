// Decompiled with JetBrains decompiler
// Type: TexAnmPtn
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

public class TexAnmPtn : MonoBehaviour
{
  public int UvNumX;
  public int UvNumY;
  public int ChangeTime;
  private int passTime;
  private int separateTime;
  private int ptnNum;
  private Vector2 uvSize;
  private Renderer rendererData;
  private int lastIndex;

  public TexAnmPtn()
  {
    base.\u002Ector();
  }

  private void Start()
  {
    this.rendererData = (Renderer) ((Component) this).GetComponent<Renderer>();
    if (Object.op_Equality((Object) null, (Object) this.rendererData))
      ((Behaviour) this).set_enabled(false);
    this.ptnNum = this.UvNumX * this.UvNumY;
    this.separateTime = this.ChangeTime / this.ptnNum;
    this.uvSize = new Vector2(1f / (float) this.UvNumX, 1f / (float) this.UvNumY);
  }

  private void Update()
  {
    this.passTime += (int) ((double) Time.get_deltaTime() * 1000.0);
    while (this.passTime >= this.ChangeTime)
      this.passTime -= this.ChangeTime;
    int num1 = Mathf.Clamp(this.ChangeTime != 0 ? this.passTime % this.ChangeTime / this.separateTime : 0, 0, this.ptnNum);
    if (num1 == this.lastIndex)
      return;
    int num2 = num1 % this.UvNumX;
    int num3 = num1 / this.UvNumX;
    Vector2 vector2;
    ((Vector2) ref vector2).\u002Ector((float) num2 * (float) this.uvSize.x, (float) (1.0 - (double) num3 * this.uvSize.y));
    this.rendererData.get_material().SetTextureOffset("_MainTex", vector2);
    this.rendererData.get_material().SetTextureScale("_MainTex", this.uvSize);
    this.lastIndex = num1;
  }
}
