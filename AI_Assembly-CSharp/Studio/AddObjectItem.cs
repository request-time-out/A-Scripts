// Decompiled with JetBrains decompiler
// Type: Studio.AddObjectItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using Manager;
using Studio.Sound;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Studio
{
  public static class AddObjectItem
  {
    public static OCIItem Add(int _group, int _category, int _no)
    {
      int newIndex = Studio.Studio.GetNewIndex();
      Singleton<UndoRedoManager>.Instance.Do((ICommand) new AddObjectCommand.AddItemCommand(_group, _category, _no, newIndex, Studio.Studio.optionSystem.initialPosition));
      return Studio.Studio.GetCtrlInfo(newIndex) as OCIItem;
    }

    public static OCIItem Load(
      OIItemInfo _info,
      ObjectCtrlInfo _parent,
      TreeNodeObject _parentNode)
    {
      ChangeAmount _source = _info.changeAmount.Clone();
      OCIItem ociItem = AddObjectItem.Load(_info, _parent, _parentNode, false, -1);
      _info.changeAmount.Copy(_source, true, true, true);
      AddObjectAssist.LoadChild(_info.child, (ObjectCtrlInfo) ociItem, (TreeNodeObject) null);
      return ociItem;
    }

    public static OCIItem Load(
      OIItemInfo _info,
      ObjectCtrlInfo _parent,
      TreeNodeObject _parentNode,
      bool _addInfo,
      int _initialPosition)
    {
      OCIItem _ociItem = new OCIItem();
      Info.ItemLoadInfo loadInfo = AddObjectItem.GetLoadInfo(_info.group, _info.category, _info.no);
      if (loadInfo == null)
      {
        Debug.LogWarningFormat("存在しない : G[{0}] : C[{1}] : N[{2}]", new object[3]
        {
          (object) _info.group,
          (object) _info.category,
          (object) _info.no
        });
        loadInfo = AddObjectItem.GetLoadInfo(0, 0, 399);
      }
      _ociItem.objectInfo = (ObjectInfo) _info;
      GameObject gameObject = CommonLib.LoadAsset<GameObject>(loadInfo.bundlePath, loadInfo.fileName, true, loadInfo.manifest);
      if (Object.op_Equality((Object) gameObject, (Object) null))
      {
        Debug.LogError((object) string.Format("読み込み失敗 : {0} : {1} : {2}", (object) loadInfo.manifest, (object) loadInfo.bundlePath, (object) loadInfo.fileName));
        Studio.Studio.DeleteIndex(_info.dicKey);
        return (OCIItem) null;
      }
      gameObject.get_transform().SetParent(Singleton<Scene>.Instance.commonSpace.get_transform());
      _ociItem.objectItem = gameObject;
      _ociItem.itemComponent = (ItemComponent) gameObject.GetComponent<ItemComponent>();
      _ociItem.arrayRender = ((IEnumerable<Renderer>) gameObject.GetComponentsInChildren<Renderer>()).Where<Renderer>((Func<Renderer, bool>) (v => v.get_enabled())).ToArray<Renderer>();
      ParticleSystem[] componentsInChildren = (ParticleSystem[]) gameObject.GetComponentsInChildren<ParticleSystem>();
      if (!((IList<ParticleSystem>) componentsInChildren).IsNullOrEmpty<ParticleSystem>())
        _ociItem.arrayParticle = ((IEnumerable<ParticleSystem>) componentsInChildren).Where<ParticleSystem>((Func<ParticleSystem, bool>) (v => v.get_isPlaying())).ToArray<ParticleSystem>();
      MeshCollider component = (MeshCollider) gameObject.GetComponent<MeshCollider>();
      if (Object.op_Implicit((Object) component))
        ((Collider) component).set_enabled(false);
      _ociItem.dynamicBones = (DynamicBone[]) gameObject.GetComponentsInChildren<DynamicBone>();
      GuideObject guideObject = Singleton<GuideObjectManager>.Instance.Add(gameObject.get_transform(), _info.dicKey);
      guideObject.isActive = false;
      guideObject.scaleSelect = 0.1f;
      guideObject.scaleRot = 0.05f;
      guideObject.isActiveFunc += new GuideObject.IsActiveFunc(((ObjectCtrlInfo) _ociItem).OnSelect);
      guideObject.enableScale = !Object.op_Inequality((Object) _ociItem.itemComponent, (Object) null) || _ociItem.itemComponent.isScale;
      guideObject.SetVisibleCenter(true);
      _ociItem.guideObject = guideObject;
      if (Object.op_Inequality((Object) _ociItem.itemComponent, (Object) null) && Object.op_Inequality((Object) _ociItem.itemComponent.childRoot, (Object) null))
        _ociItem.childRoot = _ociItem.itemComponent.childRoot;
      if (Object.op_Equality((Object) _ociItem.childRoot, (Object) null))
        _ociItem.childRoot = gameObject.get_transform();
      _ociItem.animator = (Animator) gameObject.GetComponentInChildren<Animator>();
      if (Object.op_Implicit((Object) _ociItem.animator))
        ((Behaviour) _ociItem.animator).set_enabled(Object.op_Inequality((Object) _ociItem.itemComponent, (Object) null) && _ociItem.itemComponent.isAnime);
      if (Object.op_Inequality((Object) _ociItem.itemComponent, (Object) null))
      {
        _ociItem.itemComponent.SetGlass();
        _ociItem.itemComponent.SetEmission();
        if (_addInfo && _ociItem.itemComponent.check)
        {
          Color[] defColorMain = _ociItem.itemComponent.defColorMain;
          for (int index = 0; index < 3; ++index)
            _info.colors[index].mainColor = defColorMain[index];
          Color[] defColorPattern = _ociItem.itemComponent.defColorPattern;
          for (int index = 0; index < 3; ++index)
          {
            _info.colors[index].pattern.color = defColorPattern[index];
            _info.colors[index].metallic = _ociItem.itemComponent.info[index].defMetallic;
            _info.colors[index].glossiness = _ociItem.itemComponent.info[index].defGlossiness;
            _info.colors[index].pattern.clamp = _ociItem.itemComponent.info[index].defClamp;
            _info.colors[index].pattern.uv = _ociItem.itemComponent.info[index].defUV;
            _info.colors[index].pattern.rot = _ociItem.itemComponent.info[index].defRot;
          }
          _info.colors[3].mainColor = _ociItem.itemComponent.defGlass;
          _info.emissionColor = _ociItem.itemComponent.DefEmissionColor;
          _info.emissionPower = _ociItem.itemComponent.defEmissionStrength;
          _info.lightCancel = _ociItem.itemComponent.defLightCancel;
        }
        _ociItem.itemComponent.SetupSea();
      }
      _ociItem.particleComponent = (ParticleComponent) gameObject.GetComponent<ParticleComponent>();
      if (Object.op_Inequality((Object) _ociItem.particleComponent, (Object) null) && _addInfo)
        _info.colors[0].mainColor = _ociItem.particleComponent.defColor01;
      _ociItem.iconComponent = (IconComponent) gameObject.GetComponent<IconComponent>();
      if (Object.op_Inequality((Object) _ociItem.iconComponent, (Object) null))
        _ociItem.iconComponent.Layer = LayerMask.NameToLayer("Studio/Camera");
      _ociItem.VisibleIcon = Singleton<Studio.Studio>.Instance.workInfo.visibleGimmick;
      _ociItem.panelComponent = (PanelComponent) gameObject.GetComponent<PanelComponent>();
      if (_addInfo && Object.op_Inequality((Object) _ociItem.panelComponent, (Object) null))
      {
        _info.colors[0].mainColor = _ociItem.panelComponent.defColor;
        _info.colors[0].pattern.uv = _ociItem.panelComponent.defUV;
        _info.colors[0].pattern.clamp = _ociItem.panelComponent.defClamp;
        _info.colors[0].pattern.rot = _ociItem.panelComponent.defRot;
      }
      _ociItem.seComponent = (SEComponent) gameObject.GetComponent<SEComponent>();
      if (_addInfo && Object.op_Inequality((Object) _ociItem.itemComponent, (Object) null) && !((IList<ItemComponent.OptionInfo>) _ociItem.itemComponent.optionInfos).IsNullOrEmpty<ItemComponent.OptionInfo>())
        _info.option = Enumerable.Repeat<bool>(true, _ociItem.itemComponent.optionInfos.Length).ToList<bool>();
      if (_addInfo)
        Studio.Studio.AddInfo((ObjectInfo) _info, (ObjectCtrlInfo) _ociItem);
      else
        Studio.Studio.AddObjectCtrlInfo((ObjectCtrlInfo) _ociItem);
      TreeNodeObject _parent1 = !Object.op_Inequality((Object) _parentNode, (Object) null) ? (_parent == null ? (TreeNodeObject) null : _parent.treeNodeObject) : _parentNode;
      TreeNodeObject treeNodeObject = Studio.Studio.AddNode(loadInfo.name, _parent1);
      treeNodeObject.treeState = _info.treeState;
      treeNodeObject.onVisible += new TreeNodeObject.OnVisibleFunc(((ObjectCtrlInfo) _ociItem).OnVisible);
      treeNodeObject.enableVisible = true;
      treeNodeObject.visible = _info.visible;
      guideObject.guideSelect.treeNodeObject = treeNodeObject;
      _ociItem.treeNodeObject = treeNodeObject;
      if (!loadInfo.bones.IsNullOrEmpty<string>())
      {
        _ociItem.itemFKCtrl = (ItemFKCtrl) gameObject.AddComponent<ItemFKCtrl>();
        _ociItem.itemFKCtrl.InitBone(_ociItem, loadInfo, _addInfo);
      }
      else
        _ociItem.itemFKCtrl = (ItemFKCtrl) null;
      if (_initialPosition == 1)
        _info.changeAmount.pos = Singleton<Studio.Studio>.Instance.cameraCtrl.targetPos;
      _info.changeAmount.OnChange();
      Studio.Studio.AddCtrlInfo((ObjectCtrlInfo) _ociItem);
      _parent?.OnLoadAttach(!Object.op_Inequality((Object) _parentNode, (Object) null) ? _parent.treeNodeObject : _parentNode, (ObjectCtrlInfo) _ociItem);
      if (Object.op_Implicit((Object) _ociItem.animator))
      {
        if (_info.animePattern != 0)
          _ociItem.SetAnimePattern(_info.animePattern);
        _ociItem.animator.set_speed(_info.animeSpeed);
        if ((double) _info.animeNormalizedTime != 0.0 && _ociItem.animator.get_layerCount() != 0)
        {
          _ociItem.animator.Update(1f);
          AnimatorStateInfo animatorStateInfo = _ociItem.animator.GetCurrentAnimatorStateInfo(0);
          _ociItem.animator.Play(((AnimatorStateInfo) ref animatorStateInfo).get_shortNameHash(), 0, _info.animeNormalizedTime);
        }
      }
      _ociItem.SetupPatternTex();
      _ociItem.SetMainTex();
      _ociItem.UpdateColor();
      _ociItem.ActiveFK(_ociItem.itemInfo.enableFK);
      _ociItem.UpdateFKColor();
      _ociItem.ActiveDynamicBone(_ociItem.itemInfo.enableDynamicBone);
      _ociItem.UpdateOption();
      _ociItem.particleComponent?.PlayOnLoad();
      return _ociItem;
    }

    private static Info.ItemLoadInfo GetLoadInfo(int _group, int _category, int _no)
    {
      Dictionary<int, Dictionary<int, Info.ItemLoadInfo>> dictionary1 = (Dictionary<int, Dictionary<int, Info.ItemLoadInfo>>) null;
      if (!Singleton<Info>.Instance.dicItemLoadInfo.TryGetValue(_group, out dictionary1))
        return (Info.ItemLoadInfo) null;
      Dictionary<int, Info.ItemLoadInfo> dictionary2 = (Dictionary<int, Info.ItemLoadInfo>) null;
      if (!dictionary1.TryGetValue(_category, out dictionary2))
        return (Info.ItemLoadInfo) null;
      Info.ItemLoadInfo itemLoadInfo = (Info.ItemLoadInfo) null;
      if (dictionary2.TryGetValue(_no, out itemLoadInfo))
        return itemLoadInfo;
      Debug.LogWarning((object) string.Format("存在しない番号[{0}]", (object) _no));
      return (Info.ItemLoadInfo) null;
    }
  }
}
