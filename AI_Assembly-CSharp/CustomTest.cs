// Decompiled with JetBrains decompiler
// Type: CustomTest
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIChara;
using Manager;
using System.Collections.Generic;
using UnityEngine;

public class CustomTest : BaseLoader
{
  private int nowPose = -1;
  private ChaControl chaCtrl;

  private void Start()
  {
    this.chaCtrl = Singleton<Character>.Instance.CreateChara((byte) 1, (GameObject) null, 0, (ChaFileControl) null);
    this.chaCtrl.releaseCustomInputTexture = false;
    this.chaCtrl.fileHair.parts[1].id = 1;
    this.chaCtrl.Load(false);
    this.ChangeAnimation(1);
  }

  public bool ChangeAnimation(int pose)
  {
    if (Object.op_Equality((Object) null, (Object) this.chaCtrl) || this.nowPose == pose)
      return false;
    Dictionary<int, ListInfoBase> categoryInfo = Singleton<Character>.Instance.chaListCtrl.GetCategoryInfo(this.chaCtrl.sex != (byte) 0 ? ChaListDefine.CategoryNo.custom_pose_f : ChaListDefine.CategoryNo.custom_pose_m);
    string empty1 = string.Empty;
    string empty2 = string.Empty;
    string empty3 = string.Empty;
    string empty4 = string.Empty;
    ListInfoBase listInfoBase1;
    if (!categoryInfo.TryGetValue(pose, out listInfoBase1))
      return false;
    string info1 = listInfoBase1.GetInfo(ChaListDefine.KeyType.MainManifest);
    string info2 = listInfoBase1.GetInfo(ChaListDefine.KeyType.MainAB);
    string info3 = listInfoBase1.GetInfo(ChaListDefine.KeyType.MainData);
    string info4 = listInfoBase1.GetInfo(ChaListDefine.KeyType.Clip);
    bool flag = true;
    ListInfoBase listInfoBase2;
    if (0 <= this.nowPose && categoryInfo.TryGetValue(this.nowPose, out listInfoBase2) && (listInfoBase2.GetInfo(ChaListDefine.KeyType.MainManifest) == info1 && listInfoBase2.GetInfo(ChaListDefine.KeyType.MainAB) == info2) && listInfoBase2.GetInfo(ChaListDefine.KeyType.MainData) == info3)
      flag = false;
    if (flag)
      this.chaCtrl.LoadAnimation(info2, info3, info1);
    this.chaCtrl.AnimPlay(info4);
    return true;
  }

  private void Update()
  {
  }
}
