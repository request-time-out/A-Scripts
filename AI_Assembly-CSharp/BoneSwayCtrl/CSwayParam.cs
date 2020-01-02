// Decompiled with JetBrains decompiler
// Type: BoneSwayCtrl.CSwayParam
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;

namespace BoneSwayCtrl
{
  [Serializable]
  public class CSwayParam
  {
    public string strName = string.Empty;
    public List<CSwayParamDetail> listDetail = new List<CSwayParamDetail>();
    public bool bEntry;
    public int nPtn;
    public float fBlendTime;
    public bool bCalc;
    public byte nCatch;
    public float fMoveRate;
  }
}
