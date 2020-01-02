// Decompiled with JetBrains decompiler
// Type: MatAnmFrameEx
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using IllusionUtility.GetUtility;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MatAnmFrameEx : MonoBehaviour
{
  public bool Usage;
  public Animation Anm;
  public MatAnmPtnInfoEx[] PtnInfo;
  private Renderer rendererData;
  private int _Color;

  public MatAnmFrameEx()
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
    foreach (\u003C\u003E__AnonType10<MatAnmPtnInfoEx, int> anonType10 in ((IEnumerable<MatAnmPtnInfoEx>) this.PtnInfo).Select<MatAnmPtnInfoEx, \u003C\u003E__AnonType10<MatAnmPtnInfoEx, int>>((Func<MatAnmPtnInfoEx, int, \u003C\u003E__AnonType10<MatAnmPtnInfoEx, int>>) ((value, index) => new \u003C\u003E__AnonType10<MatAnmPtnInfoEx, int>(value, index))))
    {
      if (!(anonType10.value.PtnName != ((Object) playingClip).get_name()))
      {
        index1 = anonType10.index;
        break;
      }
    }
    if (index1 == -1)
      return;
    MatAnmPtnInfoEx matAnmPtnInfoEx = this.PtnInfo[index1];
    float time = this.Anm.get_Item(((Object) playingClip).get_name()).get_time();
    while ((double) playingClip.get_length() < (double) time)
      time -= playingClip.get_length();
    float num = Mathf.InverseLerp(0.0f, playingClip.get_length(), time);
    Color color;
    color.r = matAnmPtnInfoEx.Value.Evaluate(num).r;
    color.g = matAnmPtnInfoEx.Value.Evaluate(num).g;
    color.b = matAnmPtnInfoEx.Value.Evaluate(num).b;
    color.a = matAnmPtnInfoEx.Value.Evaluate(num).a;
    this.rendererData.get_material().SetColor(this._Color, color);
  }
}
