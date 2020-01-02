// Decompiled with JetBrains decompiler
// Type: SuperScrollView.GridViewLayoutParam
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

namespace SuperScrollView
{
  public class GridViewLayoutParam
  {
    public int mColumnOrRowCount;
    public float mItemWidthOrHeight;
    public float mPadding1;
    public float mPadding2;
    public float[] mCustomColumnOrRowOffsetArray;

    public bool CheckParam()
    {
      if (this.mColumnOrRowCount <= 0)
      {
        Debug.LogError((object) "mColumnOrRowCount shoud be > 0");
        return false;
      }
      if ((double) this.mItemWidthOrHeight <= 0.0)
      {
        Debug.LogError((object) "mItemWidthOrHeight shoud be > 0");
        return false;
      }
      if (this.mCustomColumnOrRowOffsetArray == null || this.mCustomColumnOrRowOffsetArray.Length == this.mColumnOrRowCount)
        return true;
      Debug.LogError((object) "mGroupOffsetArray.Length != mColumnOrRowCount");
      return false;
    }
  }
}
