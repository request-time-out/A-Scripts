// Decompiled with JetBrains decompiler
// Type: Housing.ItemComponent
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIProject;
using AIProject.Animal;
using Manager;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Housing
{
  public class ItemComponent : MonoBehaviour
  {
    [Header("色替え関係")]
    public ItemComponent.Info[] renderers;
    public Color defColor1;
    public Color defColor2;
    public Color defColor3;
    [ColorUsage(false, true)]
    public Color defEmissionColor;
    [Header("行動関係")]
    public ActionPoint[] actionPoints;
    public FarmPoint[] farmPoints;
    public PetHomePoint[] petHomePoints;
    public JukePoint[] jukePoints;
    public CraftPoint[] craftPoints;
    public LightSwitchPoint[] lightSwitchPoints;
    public HPoint[] hPoints;
    [Header("オプション関係")]
    public GameObject[] objOption;
    [Header("遮蔽関係")]
    public ItemComponent.ColInfo[] colInfos;
    [Header("サイズ関係")]
    public bool autoSize;
    public Vector3 min;
    public Vector3 max;
    [Header("重なり関係")]
    public bool overlap;
    public Collider[] overlapColliders;
    [Header("初期関係")]
    public Vector3 initPos;

    public ItemComponent()
    {
      base.\u002Ector();
    }

    public Vector3 position
    {
      get
      {
        return ((Component) this).get_transform().get_position();
      }
    }

    public bool IsColor
    {
      get
      {
        return this.IsColor1 | this.IsColor2 | this.IsColor3 | this.IsEmissionColor;
      }
    }

    public bool IsColor1
    {
      get
      {
        return !((IList<ItemComponent.Info>) this.renderers).IsNullOrEmpty<ItemComponent.Info>() && ((IEnumerable<ItemComponent.Info>) this.renderers).Any<ItemComponent.Info>((Func<ItemComponent.Info, bool>) (_info => !((IList<ItemComponent.MaterialInfo>) _info.materialInfos).IsNullOrEmpty<ItemComponent.MaterialInfo>() && ((IEnumerable<ItemComponent.MaterialInfo>) _info.materialInfos).Any<ItemComponent.MaterialInfo>((Func<ItemComponent.MaterialInfo, bool>) (v => v.isColor1))));
      }
    }

    public bool IsColor2
    {
      get
      {
        return !((IList<ItemComponent.Info>) this.renderers).IsNullOrEmpty<ItemComponent.Info>() && ((IEnumerable<ItemComponent.Info>) this.renderers).Any<ItemComponent.Info>((Func<ItemComponent.Info, bool>) (_info => !((IList<ItemComponent.MaterialInfo>) _info.materialInfos).IsNullOrEmpty<ItemComponent.MaterialInfo>() && ((IEnumerable<ItemComponent.MaterialInfo>) _info.materialInfos).Any<ItemComponent.MaterialInfo>((Func<ItemComponent.MaterialInfo, bool>) (v => v.isColor2))));
      }
    }

    public bool IsColor3
    {
      get
      {
        return !((IList<ItemComponent.Info>) this.renderers).IsNullOrEmpty<ItemComponent.Info>() && ((IEnumerable<ItemComponent.Info>) this.renderers).Any<ItemComponent.Info>((Func<ItemComponent.Info, bool>) (_info => !((IList<ItemComponent.MaterialInfo>) _info.materialInfos).IsNullOrEmpty<ItemComponent.MaterialInfo>() && ((IEnumerable<ItemComponent.MaterialInfo>) _info.materialInfos).Any<ItemComponent.MaterialInfo>((Func<ItemComponent.MaterialInfo, bool>) (v => v.isColor3))));
      }
    }

    public bool IsEmissionColor
    {
      get
      {
        return !((IList<ItemComponent.Info>) this.renderers).IsNullOrEmpty<ItemComponent.Info>() && ((IEnumerable<ItemComponent.Info>) this.renderers).Any<ItemComponent.Info>((Func<ItemComponent.Info, bool>) (_info => !((IList<ItemComponent.MaterialInfo>) _info.materialInfos).IsNullOrEmpty<ItemComponent.MaterialInfo>() && ((IEnumerable<ItemComponent.MaterialInfo>) _info.materialInfos).Any<ItemComponent.MaterialInfo>((Func<ItemComponent.MaterialInfo, bool>) (v => v.isEmission))));
      }
    }

    public bool IsOption
    {
      get
      {
        return !((IList<GameObject>) this.objOption).IsNullOrEmpty<GameObject>() && ((IEnumerable<GameObject>) this.objOption).Where<GameObject>((Func<GameObject, bool>) (v => Object.op_Inequality((Object) v, (Object) null))).Count<GameObject>() > 0;
      }
    }

    public void SetupRendererInfo()
    {
      MeshRenderer[] componentsInChildren = (MeshRenderer[]) ((Component) this).GetComponentsInChildren<MeshRenderer>();
      if (((IList<MeshRenderer>) componentsInChildren).IsNullOrEmpty<MeshRenderer>())
        return;
      this.renderers = ((IEnumerable<MeshRenderer>) componentsInChildren).Select<MeshRenderer, ItemComponent.Info>((Func<MeshRenderer, ItemComponent.Info>) (_r => new ItemComponent.Info(_r))).ToArray<ItemComponent.Info>();
    }

    public void Setup(bool _force = false)
    {
      this.SetMinMax(_force);
      this.SetActionPoint();
      this.SetFarmPoint();
      this.SetHPoint();
      this.SetPetHomePoint();
      this.SetJukePoint();
      this.SetCraftPoint();
      this.SetLightSwitchPoint();
    }

    public void SetMinMax(bool _force = false)
    {
      if (!this.autoSize && !_force)
        return;
      MeshRenderer[] componentsInChildren = (MeshRenderer[]) ((Component) this).get_gameObject().GetComponentsInChildren<MeshRenderer>();
      if (((IList<MeshRenderer>) componentsInChildren).IsNullOrEmpty<MeshRenderer>())
        return;
      Bounds bounds = ((Renderer) componentsInChildren[0]).get_bounds();
      foreach (MeshRenderer meshRenderer in componentsInChildren)
        ((Bounds) ref bounds).Encapsulate(((Renderer) meshRenderer).get_bounds());
      this.min = ((Bounds) ref bounds).get_min();
      this.max = ((Bounds) ref bounds).get_max();
      this.min = new Vector3(this.Ceil((float) this.min.x), this.Ceil((float) this.min.y), this.Ceil((float) this.min.z));
      this.max = new Vector3(this.Ceil((float) this.max.x), this.Ceil((float) this.max.y), this.Ceil((float) this.max.z));
    }

    public void SetVisibleOption(bool _flag)
    {
      if (((IList<GameObject>) this.objOption).IsNullOrEmpty<GameObject>())
        return;
      foreach (GameObject self in this.objOption)
        self.SetActiveIfDifferent(_flag);
    }

    public void SetActionPoint()
    {
      this.actionPoints = (ActionPoint[]) ((Component) this).get_gameObject().GetComponentsInChildren<ActionPoint>(true);
    }

    public void SetFarmPoint()
    {
      this.farmPoints = (FarmPoint[]) ((Component) this).get_gameObject().GetComponentsInChildren<FarmPoint>(true);
    }

    public void SetHPoint()
    {
      if (((IList<HPoint>) this.hPoints).IsNullOrEmpty<HPoint>())
      {
        this.hPoints = (HPoint[]) ((Component) this).get_gameObject().GetComponentsInChildren<HPoint>(true);
        if (((IList<HPoint>) this.hPoints).IsNullOrEmpty<HPoint>())
          return;
      }
      if (!Singleton<Resources>.IsInstance())
        return;
      foreach (HPoint hPoint in this.hPoints)
      {
        hPoint.Init();
        hPoint.SetEffectActive(false);
      }
    }

    public void SetPetHomePoint()
    {
      this.petHomePoints = (PetHomePoint[]) ((Component) this).get_gameObject().GetComponentsInChildren<PetHomePoint>(true);
    }

    public void SetJukePoint()
    {
      this.jukePoints = (JukePoint[]) ((Component) this).get_gameObject().GetComponentsInChildren<JukePoint>(true);
    }

    public void SetCraftPoint()
    {
      this.craftPoints = (CraftPoint[]) ((Component) this).get_gameObject().GetComponentsInChildren<CraftPoint>(true);
    }

    public void SetLightSwitchPoint()
    {
      this.lightSwitchPoints = (LightSwitchPoint[]) ((Component) this).get_gameObject().GetComponentsInChildren<LightSwitchPoint>(true);
    }

    public void SetDefColor()
    {
      if (((IList<ItemComponent.Info>) this.renderers).IsNullOrEmpty<ItemComponent.Info>())
        return;
      ItemComponent.Info info1 = ((IEnumerable<ItemComponent.Info>) this.renderers).Where<ItemComponent.Info>((Func<ItemComponent.Info, bool>) (r => Object.op_Inequality((Object) r.meshRenderer, (Object) null) && !((IList<ItemComponent.MaterialInfo>) r.materialInfos).IsNullOrEmpty<ItemComponent.MaterialInfo>())).FirstOrDefault<ItemComponent.Info>((Func<ItemComponent.Info, bool>) (_i => ((IEnumerable<ItemComponent.MaterialInfo>) _i.materialInfos).Any<ItemComponent.MaterialInfo>((Func<ItemComponent.MaterialInfo, bool>) (_m => _m.isColor1))));
      if (info1 != null)
        this.defColor1 = info1.GetDefColor(0);
      ItemComponent.Info info2 = ((IEnumerable<ItemComponent.Info>) this.renderers).Where<ItemComponent.Info>((Func<ItemComponent.Info, bool>) (r => Object.op_Inequality((Object) r.meshRenderer, (Object) null) && !((IList<ItemComponent.MaterialInfo>) r.materialInfos).IsNullOrEmpty<ItemComponent.MaterialInfo>())).FirstOrDefault<ItemComponent.Info>((Func<ItemComponent.Info, bool>) (_i => ((IEnumerable<ItemComponent.MaterialInfo>) _i.materialInfos).Any<ItemComponent.MaterialInfo>((Func<ItemComponent.MaterialInfo, bool>) (_m => _m.isColor2))));
      if (info2 != null)
        this.defColor2 = info2.GetDefColor(1);
      ItemComponent.Info info3 = ((IEnumerable<ItemComponent.Info>) this.renderers).Where<ItemComponent.Info>((Func<ItemComponent.Info, bool>) (r => Object.op_Inequality((Object) r.meshRenderer, (Object) null) && !((IList<ItemComponent.MaterialInfo>) r.materialInfos).IsNullOrEmpty<ItemComponent.MaterialInfo>())).FirstOrDefault<ItemComponent.Info>((Func<ItemComponent.Info, bool>) (_i => ((IEnumerable<ItemComponent.MaterialInfo>) _i.materialInfos).Any<ItemComponent.MaterialInfo>((Func<ItemComponent.MaterialInfo, bool>) (_m => _m.isColor3))));
      if (info3 != null)
        this.defColor3 = info3.GetDefColor(2);
      ItemComponent.Info info4 = ((IEnumerable<ItemComponent.Info>) this.renderers).Where<ItemComponent.Info>((Func<ItemComponent.Info, bool>) (r => Object.op_Inequality((Object) r.meshRenderer, (Object) null) && !((IList<ItemComponent.MaterialInfo>) r.materialInfos).IsNullOrEmpty<ItemComponent.MaterialInfo>())).FirstOrDefault<ItemComponent.Info>((Func<ItemComponent.Info, bool>) (_i => ((IEnumerable<ItemComponent.MaterialInfo>) _i.materialInfos).Any<ItemComponent.MaterialInfo>((Func<ItemComponent.MaterialInfo, bool>) (_m => _m.isEmission))));
      if (info4 == null)
        return;
      this.defEmissionColor = info4.GetDefColor(4);
    }

    public void SetOverlapColliders(bool _flag)
    {
      if (((IList<Collider>) this.overlapColliders).IsNullOrEmpty<Collider>())
        return;
      using (IEnumerator<Collider> enumerator = ((IEnumerable<Collider>) this.overlapColliders).Where<Collider>((Func<Collider, bool>) (v => Object.op_Inequality((Object) v, (Object) null))).GetEnumerator())
      {
        while (((IEnumerator) enumerator).MoveNext())
        {
          Collider current = enumerator.Current;
          current.set_enabled(_flag);
          ((Component) current).get_gameObject().SetActiveIfDifferent(_flag);
          ((Component) current).get_gameObject().set_layer(LayerMask.NameToLayer("Housing/Overlap"));
        }
      }
    }

    private float Ceil(float _value)
    {
      bool flag = (double) _value < 0.0;
      return Mathf.Ceil(Mathf.Abs(_value)) * (!flag ? 1f : -1f);
    }

    [Serializable]
    public class MaterialInfo
    {
      public bool isColor1;
      public bool isColor2;
      public bool isColor3;
      public bool isEmission;
    }

    [Serializable]
    public class ColInfo
    {
      private bool visible = true;
      public Collider[] colliders;
      public Renderer[] renderers;

      public void CheckCollision(Collider _collider)
      {
        Vector3 position = ((Component) _collider).get_transform().get_position();
        Quaternion rotation = ((Component) _collider).get_transform().get_rotation();
        bool flag = false;
        using (IEnumerator<Collider> enumerator = ((IEnumerable<Collider>) this.colliders).Where<Collider>((Func<Collider, bool>) (v => Object.op_Inequality((Object) v, (Object) null))).GetEnumerator())
        {
          while (((IEnumerator) enumerator).MoveNext())
          {
            Collider current = enumerator.Current;
            Vector3 vector3;
            float num;
            flag |= Physics.ComputePenetration(_collider, position, rotation, current, ((Component) current).get_transform().get_position(), ((Component) current).get_transform().get_rotation(), ref vector3, ref num);
            MeshCollider meshCollider = current as MeshCollider;
            if (Object.op_Inequality((Object) meshCollider, (Object) null))
            {
              CapsuleCollider capsuleCollider = _collider as CapsuleCollider;
              RaycastHit raycastHit;
              flag |= ((Collider) meshCollider).Raycast(new Ray(position, ((Component) _collider).get_transform().TransformDirection(Vector3.get_forward())), ref raycastHit, Mathf.Abs(capsuleCollider.get_height()));
            }
          }
        }
        if (this.visible == !flag)
          return;
        this.Visible = !flag;
      }

      public bool Visible
      {
        set
        {
          if (((IList<Renderer>) this.renderers).IsNullOrEmpty<Renderer>())
            return;
          this.visible = value;
          using (IEnumerator<Renderer> enumerator = ((IEnumerable<Renderer>) this.renderers).Where<Renderer>((Func<Renderer, bool>) (v => Object.op_Inequality((Object) v, (Object) null))).GetEnumerator())
          {
            while (((IEnumerator) enumerator).MoveNext())
              enumerator.Current.set_enabled(value);
          }
        }
      }
    }

    [Serializable]
    public class Info
    {
      public MeshRenderer meshRenderer;
      public ItemComponent.MaterialInfo[] materialInfos;

      public Info()
      {
      }

      public Info(MeshRenderer _renderer)
      {
        this.meshRenderer = _renderer;
        if (Object.op_Equality((Object) this.meshRenderer, (Object) null))
          return;
        Material[] sharedMaterials = ((Renderer) this.meshRenderer).get_sharedMaterials();
        this.materialInfos = new ItemComponent.MaterialInfo[sharedMaterials.Length];
        for (int index = 0; index < sharedMaterials.Length; ++index)
          this.materialInfos[index] = new ItemComponent.MaterialInfo();
      }

      public Color GetDefColor(int _id)
      {
        Material[] sharedMaterials = ((Renderer) this.meshRenderer).get_sharedMaterials();
        for (int index = 0; index < sharedMaterials.Length; ++index)
        {
          ItemComponent.MaterialInfo materialInfo = this.materialInfos.SafeGet<ItemComponent.MaterialInfo>(index);
          switch (_id)
          {
            case 0:
              if (materialInfo.isColor1)
                return sharedMaterials[index].GetColor("_Color");
              break;
            case 1:
              if (materialInfo.isColor2)
                return sharedMaterials[index].GetColor("_Color2");
              break;
            case 2:
              if (materialInfo.isColor3)
                return sharedMaterials[index].GetColor("_Color3");
              break;
            case 4:
              if (materialInfo.isEmission)
                return sharedMaterials[index].GetColor("_EmissionColor");
              break;
          }
        }
        return Color.get_white();
      }
    }
  }
}
