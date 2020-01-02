// Decompiled with JetBrains decompiler
// Type: Studio.OCIItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using Manager;
using Studio.Sound;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Studio
{
  public class OCIItem : ObjectCtrlInfo
  {
    private Texture2D[] texturePattern = new Texture2D[3];
    private bool m_Visible = true;
    public GameObject objectItem;
    public Transform childRoot;
    public Animator animator;
    public ItemComponent itemComponent;
    public ParticleComponent particleComponent;
    public IconComponent iconComponent;
    public PanelComponent panelComponent;
    private Texture2D textureMain;
    public SEComponent seComponent;
    public ItemFKCtrl itemFKCtrl;
    public List<OCIChar.BoneInfo> listBones;
    public DynamicBone[] dynamicBones;
    public Renderer[] arrayRender;
    public ParticleSystem[] arrayParticle;

    public OIItemInfo itemInfo
    {
      get
      {
        return this.objectInfo as OIItemInfo;
      }
    }

    public bool isAnime
    {
      get
      {
        return Object.op_Inequality((Object) this.animator, (Object) null) && ((Behaviour) this.animator).get_enabled();
      }
    }

    public bool isChangeColor
    {
      get
      {
        bool flag = false;
        if (Object.op_Inequality((Object) this.itemComponent, (Object) null))
          flag |= this.itemComponent.check | this.itemComponent.checkGlass;
        if (Object.op_Inequality((Object) this.particleComponent, (Object) null))
          flag |= this.particleComponent.check;
        return flag;
      }
    }

    public bool[] useColor
    {
      get
      {
        bool[] result = Enumerable.Repeat<bool>(false, 3).ToArray<bool>();
        if (Object.op_Inequality((Object) this.itemComponent, (Object) null))
        {
          bool[] useColor = this.itemComponent.useColor;
          for (int i = 0; i < 3; ++i)
            useColor.SafeProc<bool>(i, (Action<bool>) (_b => result[i] = _b));
        }
        if (Object.op_Inequality((Object) this.particleComponent, (Object) null))
          result[0] |= this.particleComponent.UseColor1;
        return result;
      }
    }

    public bool useColor4
    {
      get
      {
        return Object.op_Inequality((Object) this.itemComponent, (Object) null) && this.itemComponent.checkGlass;
      }
    }

    public Color[] defColor
    {
      get
      {
        Color[] result = Enumerable.Repeat<Color>(Color.get_white(), 3).ToArray<Color>();
        if (Object.op_Inequality((Object) this.itemComponent, (Object) null) && !((IList<ItemComponent.Info>) this.itemComponent.info).IsNullOrEmpty<ItemComponent.Info>())
        {
          for (int i = 0; i < 3; ++i)
            this.itemComponent.info.SafeProc<ItemComponent.Info>(i, (Action<ItemComponent.Info>) (_i => result[i] = _i.defColor));
        }
        if (Object.op_Inequality((Object) this.particleComponent, (Object) null) && this.particleComponent.UseColor1)
          result[0] = this.particleComponent.defColor01;
        return result;
      }
    }

    public bool[] useMetallic
    {
      get
      {
        return Object.op_Equality((Object) this.itemComponent, (Object) null) ? new bool[3] : this.itemComponent.useMetallic;
      }
    }

    public bool[] usePattern
    {
      get
      {
        return Object.op_Equality((Object) this.itemComponent, (Object) null) ? new bool[3] : this.itemComponent.usePattern;
      }
    }

    public bool CheckAlpha
    {
      get
      {
        return Object.op_Inequality((Object) this.itemComponent, (Object) null) && this.itemComponent.checkAlpha;
      }
    }

    public bool CheckEmission
    {
      get
      {
        return Object.op_Inequality((Object) this.itemComponent, (Object) null) && this.itemComponent.CheckEmission;
      }
    }

    public bool CheckEmissionColor
    {
      get
      {
        return Object.op_Inequality((Object) this.itemComponent, (Object) null) && this.itemComponent.checkEmissionColor;
      }
    }

    public bool CheckEmissionPower
    {
      get
      {
        return Object.op_Inequality((Object) this.itemComponent, (Object) null) && this.itemComponent.checkEmissionStrength;
      }
    }

    public bool CheckLightCancel
    {
      get
      {
        return Object.op_Inequality((Object) this.itemComponent, (Object) null) && this.itemComponent.checkLightCancel;
      }
    }

    public bool IsParticle
    {
      get
      {
        return Object.op_Inequality((Object) this.particleComponent, (Object) null);
      }
    }

    public bool VisibleIcon
    {
      set
      {
        this.iconComponent.SafeProc<IconComponent>((Action<IconComponent>) (_ic => _ic.Active = value));
      }
    }

    public bool checkPanel
    {
      get
      {
        return Object.op_Inequality((Object) this.panelComponent, (Object) null);
      }
    }

    public bool isFK
    {
      get
      {
        return !this.listBones.IsNullOrEmpty<OCIChar.BoneInfo>();
      }
    }

    public bool isDynamicBone
    {
      get
      {
        return !(this.isFK & this.itemInfo.enableFK) && !((IList<DynamicBone>) this.dynamicBones).IsNullOrEmpty<DynamicBone>();
      }
    }

    public bool CheckOption
    {
      get
      {
        return Object.op_Inequality((Object) this.itemComponent, (Object) null) && this.itemComponent.CheckOption;
      }
    }

    public bool CheckAnimePattern
    {
      get
      {
        return Object.op_Inequality((Object) this.itemComponent, (Object) null) && this.itemComponent.CheckAnimePattern;
      }
    }

    public bool CheckAnim
    {
      get
      {
        return this.isChangeColor | this.checkPanel | this.isFK | this.isDynamicBone | this.CheckOption | this.CheckAnimePattern;
      }
    }

    public bool visible
    {
      get
      {
        return this.m_Visible;
      }
      set
      {
        this.m_Visible = value;
        for (int index = 0; index < this.arrayRender.Length; ++index)
          this.arrayRender[index].set_enabled(value);
        if (!((IList<ParticleSystem>) this.arrayParticle).IsNullOrEmpty<ParticleSystem>())
        {
          for (int index = 0; index < this.arrayParticle.Length; ++index)
          {
            if (value)
              this.arrayParticle[index].Play();
            else
              this.arrayParticle[index].Pause();
          }
        }
        if (!Object.op_Inequality((Object) this.seComponent, (Object) null))
          return;
        ((Behaviour) this.seComponent).set_enabled(value);
      }
    }

    public override void OnDelete()
    {
      if (!this.listBones.IsNullOrEmpty<OCIChar.BoneInfo>())
      {
        for (int index = 0; index < this.listBones.Count; ++index)
          Singleton<GuideObjectManager>.Instance.Delete(this.listBones[index].guideObject, true);
        this.listBones.Clear();
      }
      Singleton<GuideObjectManager>.Instance.Delete(this.guideObject, true);
      Object.Destroy((Object) this.objectItem);
      if (this.parentInfo != null)
        this.parentInfo.OnDetachChild((ObjectCtrlInfo) this);
      Studio.Studio.DeleteInfo(this.objectInfo, true);
    }

    public override void OnAttach(TreeNodeObject _parent, ObjectCtrlInfo _child)
    {
      if (_child.parentInfo == null)
        Studio.Studio.DeleteInfo(_child.objectInfo, false);
      else
        _child.parentInfo.OnDetachChild(_child);
      if (!this.itemInfo.child.Contains(_child.objectInfo))
        this.itemInfo.child.Add(_child.objectInfo);
      _child.guideObject.transformTarget.SetParent(this.childRoot);
      _child.guideObject.parent = this.childRoot;
      _child.guideObject.mode = GuideObject.Mode.World;
      _child.guideObject.moveCalc = GuideMove.MoveCalc.TYPE2;
      _child.objectInfo.changeAmount.pos = _child.guideObject.transformTarget.get_localPosition();
      _child.objectInfo.changeAmount.rot = _child.guideObject.transformTarget.get_localEulerAngles();
      _child.parentInfo = (ObjectCtrlInfo) this;
    }

    public override void OnLoadAttach(TreeNodeObject _parent, ObjectCtrlInfo _child)
    {
      if (_child.parentInfo == null)
        Studio.Studio.DeleteInfo(_child.objectInfo, false);
      else
        _child.parentInfo.OnDetachChild(_child);
      if (!this.itemInfo.child.Contains(_child.objectInfo))
        this.itemInfo.child.Add(_child.objectInfo);
      _child.guideObject.transformTarget.SetParent(this.childRoot, false);
      _child.guideObject.parent = this.childRoot;
      _child.guideObject.mode = GuideObject.Mode.World;
      _child.guideObject.moveCalc = GuideMove.MoveCalc.TYPE2;
      _child.objectInfo.changeAmount.OnChange();
      _child.parentInfo = (ObjectCtrlInfo) this;
    }

    public override void OnDetach()
    {
      this.parentInfo.OnDetachChild((ObjectCtrlInfo) this);
      this.guideObject.parent = (Transform) null;
      Studio.Studio.AddInfo(this.objectInfo, (ObjectCtrlInfo) this);
      this.objectItem.get_transform().SetParent(Singleton<Scene>.Instance.commonSpace.get_transform());
      this.objectInfo.changeAmount.pos = this.objectItem.get_transform().get_localPosition();
      this.objectInfo.changeAmount.rot = this.objectItem.get_transform().get_localEulerAngles();
      this.guideObject.mode = GuideObject.Mode.Local;
      this.guideObject.moveCalc = GuideMove.MoveCalc.TYPE1;
      this.treeNodeObject.ResetVisible();
    }

    public override void OnSelect(bool _select)
    {
      int layer = LayerMask.NameToLayer(!_select ? "Studio/Select" : "Studio/Col");
      if (this.listBones.IsNullOrEmpty<OCIChar.BoneInfo>())
        return;
      for (int index = 0; index < this.listBones.Count; ++index)
        this.listBones[index].layer = layer;
    }

    public override void OnDetachChild(ObjectCtrlInfo _child)
    {
      if (!this.itemInfo.child.Remove(_child.objectInfo))
        Debug.LogError((object) "情報の消去に失敗");
      _child.parentInfo = (ObjectCtrlInfo) null;
    }

    public override void OnSavePreprocessing()
    {
      base.OnSavePreprocessing();
      if (!this.isAnime || this.animator.get_layerCount() == 0)
        return;
      AnimatorStateInfo animatorStateInfo = this.animator.GetCurrentAnimatorStateInfo(0);
      this.itemInfo.animeNormalizedTime = ((AnimatorStateInfo) ref animatorStateInfo).get_normalizedTime();
    }

    public override float animeSpeed
    {
      get
      {
        return this.itemInfo.animeSpeed;
      }
      set
      {
        if (!Utility.SetStruct<float>(ref this.itemInfo.animeSpeed, value) || !Object.op_Implicit((Object) this.animator))
          return;
        this.animator.set_speed(this.itemInfo.animeSpeed);
      }
    }

    public override void OnVisible(bool _visible)
    {
      this.visible = _visible;
    }

    public void SetColor(Color _color, int _idx)
    {
      if (MathfEx.RangeEqualOn<int>(0, _idx, 3))
        this.itemInfo.colors[_idx].mainColor = _color;
      this.UpdateColor();
    }

    public void SetMetallic(int _idx, float _value)
    {
      if (MathfEx.RangeEqualOn<int>(0, _idx, 3))
        this.itemInfo.colors[_idx].metallic = _value;
      this.UpdateColor();
    }

    public void SetGlossiness(int _idx, float _value)
    {
      if (MathfEx.RangeEqualOn<int>(0, _idx, 3))
        this.itemInfo.colors[_idx].glossiness = _value;
      this.UpdateColor();
    }

    public void SetupPatternTex()
    {
      for (int _idx = 0; _idx < 3; ++_idx)
      {
        PatternInfo pattern = this.itemInfo.colors[_idx].pattern;
        if (!pattern.filePath.IsNullOrEmpty())
        {
          string fileName = Path.GetFileName(pattern.filePath);
          this.SetPatternTex(_idx, UserData.Path + "pattern/" + fileName);
        }
        else
          this.SetPatternTex(_idx, pattern.key);
      }
    }

    public string SetPatternTex(int _idx, int _key)
    {
      if (_key <= 0)
      {
        this.itemInfo.colors[_idx].pattern.key = _key;
        this.itemInfo.colors[_idx].pattern.filePath = string.Empty;
        if (Object.op_Implicit((Object) this.itemComponent))
          this.itemComponent.SetPatternTex(_idx, (Texture2D) null);
        this.ReleasePatternTex(_idx);
        return "なし";
      }
      PatternSelectInfo patternSelectInfo = Singleton<Studio.Studio>.Instance.patternSelectListCtrl.lstSelectInfo.Find((Predicate<PatternSelectInfo>) (p => p.index == _key));
      string str1 = "なし";
      if (patternSelectInfo != null)
      {
        if (patternSelectInfo.assetBundle.IsNullOrEmpty())
        {
          string str2 = UserData.Path + "pattern/" + patternSelectInfo.assetName;
          if (!File.Exists(str2))
            return "なし";
          this.texturePattern[_idx] = PngAssist.LoadTexture(str2);
          this.itemInfo.colors[_idx].pattern.key = -1;
          this.itemInfo.colors[_idx].pattern.filePath = patternSelectInfo.assetName;
          str1 = patternSelectInfo.assetName;
        }
        else
        {
          string assetBundleName = patternSelectInfo.assetBundle.Replace("thumb/", string.Empty);
          string assetName = patternSelectInfo.assetName.Replace("thumb_", string.Empty);
          this.texturePattern[_idx] = CommonLib.LoadAsset<Texture2D>(assetBundleName, assetName, false, string.Empty);
          this.itemInfo.colors[_idx].pattern.key = _key;
          this.itemInfo.colors[_idx].pattern.filePath = string.Empty;
          str1 = patternSelectInfo.name;
        }
      }
      this.itemComponent.SetPatternTex(_idx, this.texturePattern[_idx]);
      Resources.UnloadUnusedAssets();
      return str1;
    }

    public void SetPatternTex(int _idx, string _path)
    {
      if (_path.IsNullOrEmpty())
      {
        this.itemInfo.colors[_idx].pattern.key = 0;
        this.itemInfo.colors[_idx].pattern.filePath = string.Empty;
        this.itemComponent.SetPatternTex(_idx, (Texture2D) null);
        this.ReleasePatternTex(_idx);
      }
      else
      {
        this.itemInfo.colors[_idx].pattern.key = -1;
        this.itemInfo.colors[_idx].pattern.filePath = _path;
        if (File.Exists(_path))
          this.texturePattern[_idx] = PngAssist.LoadTexture(_path);
        this.itemComponent.SetPatternTex(_idx, this.texturePattern[_idx]);
        Resources.UnloadUnusedAssets();
      }
    }

    private void ReleasePatternTex(int _idx)
    {
      this.texturePattern[_idx] = (Texture2D) null;
    }

    public void SetPatternColor(int _idx, Color _color)
    {
      this.itemInfo.colors[_idx].pattern.color = _color;
      this.UpdateColor();
    }

    public void SetPatternClamp(int _idx, bool _flag)
    {
      if (!Utility.SetStruct<bool>(ref this.itemInfo.colors[_idx].pattern.clamp, _flag))
        return;
      this.UpdateColor();
    }

    public void SetPatternUT(int _idx, float _value)
    {
      // ISSUE: cast to a reference type
      if (!Utility.SetStruct<float>((float&) ref this.itemInfo.colors[_idx].pattern.uv.z, _value))
        return;
      this.UpdateColor();
    }

    public void SetPatternVT(int _idx, float _value)
    {
      // ISSUE: cast to a reference type
      if (!Utility.SetStruct<float>((float&) ref this.itemInfo.colors[_idx].pattern.uv.w, _value))
        return;
      this.UpdateColor();
    }

    public void SetPatternUS(int _idx, float _value)
    {
      // ISSUE: cast to a reference type
      if (!Utility.SetStruct<float>((float&) ref this.itemInfo.colors[_idx].pattern.uv.x, _value))
        return;
      this.UpdateColor();
    }

    public void SetPatternVS(int _idx, float _value)
    {
      // ISSUE: cast to a reference type
      if (!Utility.SetStruct<float>((float&) ref this.itemInfo.colors[_idx].pattern.uv.y, _value))
        return;
      this.UpdateColor();
    }

    public void SetPatternRot(int _idx, float _value)
    {
      if (!Utility.SetStruct<float>(ref this.itemInfo.colors[_idx].pattern.rot, _value))
        return;
      this.UpdateColor();
    }

    public void SetAlpha(float _value)
    {
      if (!Utility.SetStruct<float>(ref this.itemInfo.alpha, _value))
        return;
      this.UpdateColor();
    }

    public void SetEmissionColor(Color _color)
    {
      this.itemInfo.emissionColor = _color;
      this.UpdateColor();
    }

    public void SetEmissionPower(float _value)
    {
      this.itemInfo.emissionPower = _value;
      this.UpdateColor();
    }

    public void SetLightCancel(float _value)
    {
      this.itemInfo.lightCancel = _value;
      this.UpdateColor();
    }

    public void UpdateColor()
    {
      if (Object.op_Inequality((Object) this.itemComponent, (Object) null) && this.itemComponent.check | this.itemComponent.checkGlass)
        this.itemComponent.UpdateColor(this.itemInfo);
      if (Object.op_Inequality((Object) this.particleComponent, (Object) null) && this.particleComponent.check)
        this.particleComponent.UpdateColor(this.itemInfo);
      if (!Object.op_Inequality((Object) this.panelComponent, (Object) null))
        return;
      this.panelComponent.UpdateColor(this.itemInfo);
    }

    public void SetMainTex()
    {
      this.SetMainTex(this.itemInfo.panel.filePath);
    }

    public void SetMainTex(string _file)
    {
      if (Object.op_Equality((Object) this.panelComponent, (Object) null))
        return;
      if (_file.IsNullOrEmpty())
      {
        this.itemInfo.panel.filePath = string.Empty;
        this.panelComponent.SetMainTex((Texture2D) null);
        this.textureMain = (Texture2D) null;
      }
      else
      {
        this.itemInfo.panel.filePath = _file;
        string str = UserData.Path + BackgroundList.dirName + "/" + _file;
        if (!File.Exists(str))
          return;
        this.textureMain = PngAssist.LoadTexture(str);
        this.panelComponent.SetMainTex(this.textureMain);
        Resources.UnloadUnusedAssets();
      }
    }

    public void ActiveFK(bool _active)
    {
      if (Object.op_Equality((Object) this.itemFKCtrl, (Object) null))
        return;
      ((Behaviour) this.itemFKCtrl).set_enabled(_active);
      this.itemInfo.enableFK = _active;
      bool flag = !_active && this.itemInfo.enableDynamicBone;
      foreach (Behaviour dynamicBone in this.dynamicBones)
        dynamicBone.set_enabled(flag);
      foreach (OCIChar.BoneInfo listBone in this.listBones)
        listBone.active = _active;
    }

    public void UpdateFKColor()
    {
      if (this.listBones.IsNullOrEmpty<OCIChar.BoneInfo>())
        return;
      foreach (OCIChar.BoneInfo listBone in this.listBones)
        listBone.color = Studio.Studio.optionSystem.colorFKItem;
    }

    public void ActiveDynamicBone(bool _active)
    {
      this.itemInfo.enableDynamicBone = _active;
      if (((IList<DynamicBone>) this.dynamicBones).IsNullOrEmpty<DynamicBone>() || this.isFK & this.itemInfo.enableFK)
        return;
      foreach (Behaviour dynamicBone in this.dynamicBones)
        dynamicBone.set_enabled(_active);
    }

    public void SetOptionVisible(bool _visible)
    {
      int count = this.itemInfo.option.Count;
      for (int index = 0; index < count; ++index)
        this.itemInfo.option[index] = _visible;
      if (this.itemComponent == null)
        return;
      this.itemComponent.SetOptionVisible(_visible);
    }

    public void SetOptionVisible(int _idx, bool _visible)
    {
      if (MathfEx.RangeEqualOn<int>(0, _idx, this.itemInfo.option.Count - 1))
        this.itemInfo.option[_idx] = _visible;
      if (this.itemComponent == null)
        return;
      this.itemComponent.SetOptionVisible(_idx, _visible);
    }

    public void UpdateOption()
    {
      int count = this.itemInfo.option.Count;
      for (int _idx = 0; _idx < count; ++_idx)
      {
        if (this.itemComponent != null)
          this.itemComponent.SetOptionVisible(_idx, this.itemInfo.option[_idx]);
      }
    }

    public void SetAnimePattern(int _idx)
    {
      if (!this.isAnime)
        return;
      this.itemInfo.animePattern = _idx;
      ItemComponent.AnimeInfo animeInfo = this.itemComponent != null ? this.itemComponent.animeInfos.SafeGet<ItemComponent.AnimeInfo>(_idx) : (ItemComponent.AnimeInfo) null;
      if (animeInfo == null)
        return;
      this.animator.Play(animeInfo.state);
    }

    public void RestartAnime()
    {
      if (!this.isAnime || this.animator.get_layerCount() == 0)
        return;
      AnimatorStateInfo animatorStateInfo = this.animator.GetCurrentAnimatorStateInfo(0);
      this.animator.Play(((AnimatorStateInfo) ref animatorStateInfo).get_shortNameHash(), 0, 0.0f);
    }
  }
}
