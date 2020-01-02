// Decompiled with JetBrains decompiler
// Type: CreateThumbnail
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIChara;
using Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CreateThumbnail : BaseLoader
{
  public Dictionary<int, CreateThumbnail.FacePaintLayout> dictFacePaintLayout;
  public Dictionary<int, CreateThumbnail.MoleLayout> dictMoleLayout;
  public CameraControl_Ver2 camCtrl;
  public Camera camMain;
  public Camera camBack;
  public Camera camFront;
  public GameObject objImgBack;
  public GameObject objImgFront;
  public ChaControl chaCtrl;

  private void Start()
  {
    this.ReloadChara(1);
    ChaListControl chaListCtrl = Singleton<Character>.Instance.chaListCtrl;
    this.dictFacePaintLayout = chaListCtrl.GetCategoryInfo(ChaListDefine.CategoryNo.facepaint_layout).Select<KeyValuePair<int, ListInfoBase>, CreateThumbnail.FacePaintLayout>((Func<KeyValuePair<int, ListInfoBase>, CreateThumbnail.FacePaintLayout>) (dict => new CreateThumbnail.FacePaintLayout()
    {
      index = dict.Value.Id,
      x = dict.Value.GetInfoFloat(ChaListDefine.KeyType.PosX),
      y = dict.Value.GetInfoFloat(ChaListDefine.KeyType.PosY),
      s = dict.Value.GetInfoFloat(ChaListDefine.KeyType.Scale)
    })).ToDictionary<CreateThumbnail.FacePaintLayout, int, CreateThumbnail.FacePaintLayout>((Func<CreateThumbnail.FacePaintLayout, int>) (v => v.index), (Func<CreateThumbnail.FacePaintLayout, CreateThumbnail.FacePaintLayout>) (v => v));
    this.dictMoleLayout = chaListCtrl.GetCategoryInfo(ChaListDefine.CategoryNo.mole_layout).Select<KeyValuePair<int, ListInfoBase>, CreateThumbnail.MoleLayout>((Func<KeyValuePair<int, ListInfoBase>, CreateThumbnail.MoleLayout>) (dict => new CreateThumbnail.MoleLayout()
    {
      index = dict.Value.Id,
      x = dict.Value.GetInfoFloat(ChaListDefine.KeyType.PosX),
      y = dict.Value.GetInfoFloat(ChaListDefine.KeyType.PosY),
      s = dict.Value.GetInfoFloat(ChaListDefine.KeyType.Scale)
    })).ToDictionary<CreateThumbnail.MoleLayout, int, CreateThumbnail.MoleLayout>((Func<CreateThumbnail.MoleLayout, int>) (v => v.index), (Func<CreateThumbnail.MoleLayout, CreateThumbnail.MoleLayout>) (v => v));
  }

  public void ReloadChara(int sex)
  {
    if (Object.op_Implicit((Object) this.chaCtrl))
      Singleton<Character>.Instance.DeleteChara(this.chaCtrl, false);
    this.chaCtrl = Singleton<Character>.Instance.CreateChara((byte) sex, ((Component) this).get_gameObject(), 0, (ChaFileControl) null);
    int length = Enum.GetNames(typeof (ChaFileDefine.ClothesKind)).Length;
    for (int index = 0; index < length; ++index)
      this.chaCtrl.nowCoordinate.clothes.parts[index].id = 0;
    this.chaCtrl.releaseCustomInputTexture = false;
    this.chaCtrl.Load(false);
    this.chaCtrl.hideMoz = true;
    this.chaCtrl.loadWithDefaultColorAndPtn = true;
    this.chaCtrl.ChangeEyesOpenMax(1f);
    this.chaCtrl.ChangeEyesBlinkFlag(false);
    this.chaCtrl.LoadAnimation(ChaABDefine.CustomAnimAssetBundle(sex), ChaABDefine.CustomAnimAsset(sex), string.Empty);
    this.chaCtrl.AnimPlay("mannequin");
  }

  private void Update()
  {
    if ((double) QualitySettings.get_shadowDistance() == 80.0)
      return;
    QualitySettings.set_shadowDistance(80f);
  }

  public class FacePaintLayout
  {
    public int index = -1;
    public float x;
    public float y;
    public float s;
  }

  public class MoleLayout
  {
    public int index = -1;
    public float x;
    public float y;
    public float s;
  }
}
