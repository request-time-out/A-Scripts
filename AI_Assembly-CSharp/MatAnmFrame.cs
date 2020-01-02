// Decompiled with JetBrains decompiler
// Type: MatAnmFrame
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using IllusionUtility.GetUtility;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MatAnmFrame : MonoBehaviour
{
  public bool Usage;
  public Animation Anm;
  public MatAnmPtnInfo[] PtnInfo;
  private Renderer rendererData;
  private int _Color;

  public MatAnmFrame()
  {
    base.\u002Ector();
  }

  private void Awake()
  {
    this._Color = Shader.PropertyToID("_Color");
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
    if (!this.Usage)
      return;
    AnimationClip playingClip = this.Anm.GetPlayingClip();
    int index1 = -1;
    // ISSUE: object of a compiler-generated type is created
    foreach (\u003C\u003E__AnonType10<MatAnmPtnInfo, int> anonType10 in ((IEnumerable<MatAnmPtnInfo>) this.PtnInfo).Select<MatAnmPtnInfo, \u003C\u003E__AnonType10<MatAnmPtnInfo, int>>((Func<MatAnmPtnInfo, int, \u003C\u003E__AnonType10<MatAnmPtnInfo, int>>) ((value, index) => new \u003C\u003E__AnonType10<MatAnmPtnInfo, int>(value, index))))
    {
      if (!(anonType10.value.PtnName != ((Object) playingClip).get_name()))
      {
        index1 = anonType10.index;
        break;
      }
    }
    if (index1 == -1)
      return;
    MatAnmPtnInfo matAnmPtnInfo = this.PtnInfo[index1];
    float time = this.Anm.get_Item(((Object) playingClip).get_name()).get_time();
    while ((double) playingClip.get_length() < (double) time)
      time -= playingClip.get_length();
    int num1 = (int) Mathf.Lerp(0.0f, playingClip.get_frameRate() * playingClip.get_length(), Mathf.InverseLerp(0.0f, playingClip.get_length(), time));
    bool flag = false;
    for (int index2 = 0; index2 < matAnmPtnInfo.Param.Length - 1; ++index2)
    {
      if (matAnmPtnInfo.Param[index2].Frame <= num1 && matAnmPtnInfo.Param[index2 + 1].Frame >= num1)
      {
        float num2 = Mathf.InverseLerp((float) matAnmPtnInfo.Param[index2].Frame, (float) matAnmPtnInfo.Param[index2 + 1].Frame, (float) num1);
        Color32 color32 = (Color32) null;
        color32.r = (__Null) (int) (byte) Mathf.Lerp((float) matAnmPtnInfo.Param[index2].ColorVal.r, (float) matAnmPtnInfo.Param[index2 + 1].ColorVal.r, num2);
        color32.g = (__Null) (int) (byte) Mathf.Lerp((float) matAnmPtnInfo.Param[index2].ColorVal.g, (float) matAnmPtnInfo.Param[index2 + 1].ColorVal.g, num2);
        color32.b = (__Null) (int) (byte) Mathf.Lerp((float) matAnmPtnInfo.Param[index2].ColorVal.b, (float) matAnmPtnInfo.Param[index2 + 1].ColorVal.b, num2);
        color32.a = (__Null) (int) (byte) Mathf.Lerp((float) matAnmPtnInfo.Param[index2].ColorVal.a, (float) matAnmPtnInfo.Param[index2 + 1].ColorVal.a, num2);
        this.rendererData.get_material().SetColor(this._Color, Color32.op_Implicit(color32));
        flag = true;
        break;
      }
    }
    if (flag)
      ;
  }
}
