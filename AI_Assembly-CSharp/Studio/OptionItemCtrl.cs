// Decompiled with JetBrains decompiler
// Type: Studio.OptionItemCtrl
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using IllusionUtility.GetUtility;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Studio
{
  public class OptionItemCtrl : MonoBehaviour
  {
    private Animator m_Animator;
    private HashSet<OptionItemCtrl.ItemInfo> hashItem;
    private bool m_OutsideVisible;

    public OptionItemCtrl()
    {
      base.\u002Ector();
    }

    public Animator animator
    {
      get
      {
        if (Object.op_Equality((Object) this.m_Animator, (Object) null))
          this.m_Animator = (Animator) ((Component) this).get_gameObject().GetComponentInChildren<Animator>();
        return this.m_Animator;
      }
      set
      {
        this.m_Animator = value;
      }
    }

    public OICharInfo oiCharInfo { get; set; }

    public HashSet<OptionItemCtrl.ItemInfo> HashItem
    {
      get
      {
        return this.hashItem;
      }
    }

    public bool visible
    {
      get
      {
        return this.oiCharInfo.animeOptionVisible;
      }
      set
      {
        this.oiCharInfo.animeOptionVisible = value;
        this.SetVisible(this.m_OutsideVisible & this.oiCharInfo.animeOptionVisible);
      }
    }

    public bool outsideVisible
    {
      get
      {
        return this.m_OutsideVisible;
      }
      set
      {
        this.m_OutsideVisible = value;
        this.SetVisible(this.m_OutsideVisible & this.oiCharInfo.animeOptionVisible);
      }
    }

    public float height
    {
      set
      {
        foreach (OptionItemCtrl.ItemInfo itemInfo in this.hashItem)
          itemInfo.height = value;
      }
    }

    public void LoadAnimeItem(
      Info.AnimeLoadInfo _info,
      string _clip,
      float _height,
      float _motion)
    {
      this.ReleaseAllItem();
      if (_info.option.IsNullOrEmpty<Info.OptionItemInfo>())
        return;
      for (int index1 = 0; index1 < _info.option.Count; ++index1)
      {
        Info.OptionItemInfo optionItemInfo = _info.option[index1];
        GameObject gameObject1 = Utility.LoadAsset<GameObject>(optionItemInfo.bundlePath, optionItemInfo.fileName, optionItemInfo.manifest);
        if (!Object.op_Equality((Object) gameObject1, (Object) null))
        {
          OptionItemCtrl.ItemInfo itemInfo = new OptionItemCtrl.ItemInfo(_height);
          itemInfo.gameObject = gameObject1;
          itemInfo.scale = gameObject1.get_transform().get_localScale();
          itemInfo.animator = (Animator) gameObject1.GetComponentInChildren<Animator>();
          if (Object.op_Inequality((Object) itemInfo.animator, (Object) null))
          {
            if (optionItemInfo.anmInfo.Check)
            {
              RuntimeAnimatorController animatorController = CommonLib.LoadAsset<RuntimeAnimatorController>(optionItemInfo.anmInfo.bundlePath, optionItemInfo.anmInfo.fileName, false, string.Empty);
              if (Object.op_Inequality((Object) animatorController, (Object) null))
                itemInfo.animator.set_runtimeAnimatorController(animatorController);
              AssetBundleManager.UnloadAssetBundle(optionItemInfo.anmInfo.bundlePath, false, (string) null, false);
              if (optionItemInfo.anmOveride.Check)
              {
                CommonLib.LoadAsset<RuntimeAnimatorController>(optionItemInfo.anmOveride.bundlePath, optionItemInfo.anmOveride.fileName, false, string.Empty).SafeProc<RuntimeAnimatorController>((Action<RuntimeAnimatorController>) (_rac => itemInfo.animator.set_runtimeAnimatorController((RuntimeAnimatorController) Illusion.Utils.Animator.SetupAnimatorOverrideController(itemInfo.animator.get_runtimeAnimatorController(), _rac))));
                AssetBundleManager.UnloadAssetBundle(optionItemInfo.anmOveride.bundlePath, false, (string) null, false);
              }
              itemInfo.animator.Play(_clip);
            }
            itemInfo.animator.SetFloat("height", _height);
            itemInfo.IsSync = optionItemInfo.isAnimeSync;
          }
          else
            itemInfo.IsSync = false;
          if (((IList<Info.ParentageInfo>) optionItemInfo.parentageInfo).IsNullOrEmpty<Info.ParentageInfo>())
          {
            GameObject gameObject2 = ((Component) this).get_gameObject();
            GameObject gameObject3 = gameObject1;
            gameObject3.get_transform().SetParent(gameObject2.get_transform());
            gameObject3.get_transform().set_localPosition(Vector3.get_zero());
            gameObject3.get_transform().set_localRotation(Quaternion.get_identity());
            if (optionItemInfo.counterScale)
              itemInfo.DefaultScaleOption = true;
            else
              gameObject3.get_transform().set_localScale(itemInfo.scale);
          }
          else
          {
            for (int index2 = 0; index2 < optionItemInfo.parentageInfo.Length; ++index2)
            {
              GameObject loop = ((Component) this).get_gameObject().get_transform().FindLoop(optionItemInfo.parentageInfo[index2].parent);
              GameObject gameObject2 = gameObject1;
              if (!optionItemInfo.parentageInfo[index2].child.IsNullOrEmpty())
              {
                gameObject2 = gameObject2.get_transform().FindLoop(optionItemInfo.parentageInfo[index2].child);
                itemInfo.child.Add(new OptionItemCtrl.ChildInfo(gameObject2.get_transform().get_localScale(), gameObject2));
              }
              gameObject2.get_transform().SetParent(loop.get_transform());
              gameObject2.get_transform().set_localPosition(Vector3.get_zero());
              gameObject2.get_transform().set_localRotation(Quaternion.get_identity());
              if (optionItemInfo.counterScale)
                itemInfo.DefaultScaleOption = true;
              else
                gameObject2.get_transform().set_localScale(itemInfo.scale);
            }
          }
          itemInfo.SetRender();
          this.hashItem.Add(itemInfo);
        }
      }
      this.SetVisible(this.visible);
    }

    public void ReleaseAllItem()
    {
      foreach (OptionItemCtrl.ItemInfo itemInfo in this.hashItem)
        itemInfo?.Release();
      this.hashItem.Clear();
    }

    public void PlayAnime()
    {
      foreach (OptionItemCtrl.ItemInfo itemInfo in this.hashItem.Where<OptionItemCtrl.ItemInfo>((System.Func<OptionItemCtrl.ItemInfo, bool>) (v => v.IsAnime)))
        itemInfo.RestartAnime();
    }

    public void SetMotion(float _motion)
    {
      foreach (OptionItemCtrl.ItemInfo itemInfo in this.hashItem)
      {
        if (Object.op_Implicit((Object) itemInfo.animator) && itemInfo.IsSync)
          itemInfo.animator.SetFloat("motion", _motion);
      }
    }

    public void ChangeScale(Vector3 _value)
    {
      foreach (OptionItemCtrl.ItemInfo itemInfo in this.hashItem)
        itemInfo.gameObject.get_transform().set_localScale(itemInfo.scale);
    }

    public void ReCounterScale()
    {
      foreach (OptionItemCtrl.ItemInfo itemInfo in this.hashItem)
        itemInfo.ReCounterScale();
    }

    private void SetVisible(bool _visible)
    {
      foreach (OptionItemCtrl.ItemInfo itemInfo in this.hashItem)
        itemInfo.active = _visible;
    }

    private void Awake()
    {
      this.m_OutsideVisible = true;
    }

    private void LateUpdate()
    {
      if (Object.op_Equality((Object) this.animator, (Object) null) || this.hashItem.Count == 0)
        return;
      AnimatorStateInfo animatorStateInfo = this.animator.GetCurrentAnimatorStateInfo(0);
      foreach (OptionItemCtrl.ItemInfo itemInfo in this.hashItem)
      {
        if (itemInfo.IsAnime && itemInfo.IsSync)
          itemInfo.SyncAnime(((AnimatorStateInfo) ref animatorStateInfo).get_shortNameHash(), 0, ((AnimatorStateInfo) ref animatorStateInfo).get_normalizedTime());
        itemInfo.CounterScale();
      }
    }

    public class ChildInfo
    {
      public Vector3 scale = Vector3.get_one();
      public GameObject obj;

      public ChildInfo(Vector3 _scale, GameObject _obj)
      {
        this.scale = _scale;
        this.obj = _obj;
      }
    }

    public class ItemInfo
    {
      public List<OptionItemCtrl.ChildInfo> child = new List<OptionItemCtrl.ChildInfo>();
      public float m_Height = 0.5f;
      private bool m_Active = true;
      public GameObject gameObject;
      public Animator animator;
      private Renderer[] renderer;
      private bool scaleOption;
      private Transform _transform;

      public ItemInfo(float _height)
      {
        this.m_Height = _height;
      }

      public Vector3 scale { get; set; }

      public bool IsScale { get; private set; }

      public bool DefaultScaleOption
      {
        get
        {
          return this.scaleOption;
        }
        set
        {
          this.scaleOption = value;
          this.IsScale = value;
        }
      }

      public bool IsSync { get; set; }

      public bool IsAnime
      {
        get
        {
          return Object.op_Inequality((Object) this.animator, (Object) null);
        }
      }

      private Transform Transform
      {
        get
        {
          return this._transform ?? (this._transform = this.gameObject.get_transform());
        }
      }

      public float height
      {
        get
        {
          return this.m_Height;
        }
        set
        {
          this.m_Height = value;
        }
      }

      public bool active
      {
        get
        {
          return this.m_Active;
        }
        set
        {
          if (this.m_Active == value)
            return;
          this.m_Active = value;
          for (int index = 0; index < this.renderer.Length; ++index)
            this.renderer[index].set_enabled(value);
        }
      }

      public AnimatorStateInfo CurrentAnimatorStateInfo
      {
        get
        {
          return this.animator.GetCurrentAnimatorStateInfo(0);
        }
      }

      public void Release()
      {
        Object.DestroyImmediate((Object) this.gameObject);
        for (int index = 0; index < this.child.Count; ++index)
          Object.DestroyImmediate((Object) this.child[index].obj);
      }

      public void SetRender()
      {
        List<Renderer> rendererList = new List<Renderer>();
        Renderer[] componentsInChildren1 = (Renderer[]) this.gameObject.GetComponentsInChildren<Renderer>();
        if (!((IList<Renderer>) componentsInChildren1).IsNullOrEmpty<Renderer>())
          rendererList.AddRange((IEnumerable<Renderer>) componentsInChildren1);
        for (int index = 0; index < this.child.Count; ++index)
        {
          Renderer[] componentsInChildren2 = (Renderer[]) this.child[index].obj.GetComponentsInChildren<Renderer>();
          if (!((IList<Renderer>) componentsInChildren2).IsNullOrEmpty<Renderer>())
            rendererList.AddRange((IEnumerable<Renderer>) componentsInChildren2);
        }
        this.renderer = rendererList.ToArray();
      }

      public void RestartAnime()
      {
        AnimatorStateInfo animatorStateInfo = this.CurrentAnimatorStateInfo;
        this.animator.Play(((AnimatorStateInfo) ref animatorStateInfo).get_shortNameHash(), 0, 0.0f);
      }

      public void SyncAnime(int stateNameHash, int layer, float normalizedTime)
      {
        this.animator.Play(stateNameHash, layer, normalizedTime);
      }

      public void CounterScale()
      {
        if (!this.IsScale)
          return;
        Vector3 localScale = this.Transform.get_localScale();
        Vector3 lossyScale = this.Transform.get_lossyScale();
        this.Transform.set_localScale(new Vector3((float) (localScale.x / lossyScale.x * this.scale.x), (float) (localScale.y / lossyScale.y * this.scale.y), (float) (localScale.z / lossyScale.z * this.scale.z)));
        this.IsScale = false;
      }

      public void ReCounterScale()
      {
        this.DefaultScaleOption = this.DefaultScaleOption;
      }
    }
  }
}
