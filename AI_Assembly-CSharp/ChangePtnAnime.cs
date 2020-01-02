// Decompiled with JetBrains decompiler
// Type: ChangePtnAnime
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

public class ChangePtnAnime : MonoBehaviour
{
  public Material[] MatChange;
  public Texture[] TexChange;
  public int ChangeTime;
  private int indexT;
  private int indexM;

  public ChangePtnAnime()
  {
    base.\u002Ector();
  }

  private void Start()
  {
  }

  public void Init(Texture[] tex)
  {
    this.TexChange = new Texture[tex.Length];
    for (int index = 0; index < tex.Length; ++index)
      this.TexChange[index] = tex[index];
  }

  private void Update()
  {
    if (!Object.op_Implicit((Object) ((Component) this).GetComponent<Renderer>()))
      return;
    int num1 = (int) ((long) ((double) Time.get_timeSinceLevelLoad() * 1000.0) % 1000000L) / this.ChangeTime;
    if (this.MatChange != null && this.MatChange.Length != 0)
    {
      int num2 = num1 % this.MatChange.Length;
      if (this.indexM != num2)
      {
        this.indexM = num2;
        ((Renderer) ((Component) this).GetComponent<Renderer>()).set_sharedMaterial(this.MatChange[this.indexM]);
      }
    }
    if (this.TexChange == null || this.TexChange.Length == 0)
      return;
    int num3 = num1 % this.TexChange.Length;
    if (this.indexT == num3)
      return;
    this.indexT = num3;
    ((Renderer) ((Component) this).GetComponent<Renderer>()).get_material().set_mainTexture(this.TexChange[this.indexT]);
  }
}
