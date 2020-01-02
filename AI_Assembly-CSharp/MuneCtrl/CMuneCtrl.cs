// Decompiled with JetBrains decompiler
// Type: MuneCtrl.CMuneCtrl
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using BoneSwayCtrl;
using System.Collections.Generic;
using UnityEngine;

namespace MuneCtrl
{
  public class CMuneCtrl : MonoBehaviour
  {
    public BoneSway[] m_aMune;
    private int m_nNumBone;
    private List<int> m_listNumLocater;
    private bool m_bSucceed;
    private CMuneParamCtrl m_ParamCtrl;
    private bool m_bAllocParam;
    private Vector2 m_Mouse;
    private float m_ftime;
    private int m_nCatchMode;
    private Transform m_transRef;

    public CMuneCtrl()
    {
      base.\u002Ector();
    }

    public bool Load(long nPtn, string strAsset, string strResource)
    {
      return true;
    }
  }
}
