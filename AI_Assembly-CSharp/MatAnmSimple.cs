// Decompiled with JetBrains decompiler
// Type: MatAnmSimple
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

public class MatAnmSimple : MonoBehaviour
{
  public MatAnmSimpleInfo[] ptn;
  private int _Color;

  public MatAnmSimple()
  {
    base.\u002Ector();
  }

  private void Awake()
  {
    this._Color = Shader.PropertyToID("_Color");
  }

  private void Start()
  {
  }

  private void Update()
  {
    foreach (MatAnmSimpleInfo matAnmSimpleInfo in this.ptn)
    {
      if (!Object.op_Equality((Object) null, (Object) matAnmSimpleInfo.mr))
        matAnmSimpleInfo.Update(this._Color);
    }
  }
}
