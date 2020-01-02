// Decompiled with JetBrains decompiler
// Type: BoneSwayCtrl.CBoneData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System.Collections.Generic;

namespace BoneSwayCtrl
{
  public class CBoneData
  {
    public List<CFrameInfo> listLocater = new List<CFrameInfo>();
    public CFrameInfo Bone = new CFrameInfo();
    public CFrameInfo Reference = new CFrameInfo();
    public byte[] anLocaterTIdx = new byte[2];
    public byte[] anLocaterRIdx = new byte[2];
    public float fScaleT = 1f;
    public float fScaleYT = 1f;
    public float fScaleR = 1f;
    public int nNumLocater;
    public FakeTransform transResult;
    public float fLerp;
    public byte nCalcKind;
    public byte nTransformKind;
  }
}
