// Decompiled with JetBrains decompiler
// Type: AIProject.TimeLinkedLightObject
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace AIProject
{
  public class TimeLinkedLightObject : Point
  {
    [SerializeField]
    private bool _isEmission = true;
    [SerializeField]
    private GameObject[] _onModeObjects = new GameObject[0];
    [SerializeField]
    private GameObject[] _offModeObjects = new GameObject[0];
    [SerializeField]
    private string _emissionParamName = "_EmissionColor";
    [SerializeField]
    private string _emissionKeyName = "_EMISSION";
    [SerializeField]
    private string _emissionStrengthName = "_EmissionStrength";
    [SerializeField]
    private TimeLinkedLightObject.EmissionInfo[] _emissionInfos = new TimeLinkedLightObject.EmissionInfo[0];
    [SerializeField]
    private bool _isMorning;
    [SerializeField]
    private bool _isDay;
    [SerializeField]
    private bool _isNight;

    protected virtual void Awake()
    {
      this.ResetMaterial();
    }

    protected override void Start()
    {
      if (Singleton<Manager.Map>.IsInstance())
      {
        Manager.Map instance = Singleton<Manager.Map>.Instance;
        this.Refresh(instance.Simulator.MapLightTimeZone);
        List<TimeLinkedLightObject> linkedLightObjectList = instance.TimeLinkedLightObjectList;
        if (!linkedLightObjectList.Contains(this))
          linkedLightObjectList.Add(this);
      }
      base.Start();
    }

    protected virtual void OnDestroy()
    {
      if (!Singleton<Manager.Map>.IsInstance())
        return;
      List<TimeLinkedLightObject> linkedLightObjectList = Singleton<Manager.Map>.Instance.TimeLinkedLightObjectList;
      if (!linkedLightObjectList.Contains(this))
        return;
      linkedLightObjectList.Remove(this);
    }

    public void Refresh(TimeZone timeZone)
    {
      bool isOn = false;
      switch (timeZone)
      {
        case TimeZone.Morning:
          isOn = this._isMorning;
          break;
        case TimeZone.Day:
          isOn = this._isDay;
          break;
        case TimeZone.Night:
          isOn = this._isNight;
          break;
      }
      if (this._isEmission)
        this.RefreshEmissionMode(isOn);
      else
        this.RefreshObjectMode(isOn);
    }

    private void RefreshObjectMode(bool isOn)
    {
      if (!this._onModeObjects.IsNullOrEmpty<GameObject>())
      {
        foreach (GameObject onModeObject in this._onModeObjects)
        {
          if (!Object.op_Equality((Object) onModeObject, (Object) null) && onModeObject.get_activeSelf() != isOn)
            onModeObject.SetActive(isOn);
        }
      }
      if (this._offModeObjects.IsNullOrEmpty<GameObject>())
        return;
      foreach (GameObject offModeObject in this._offModeObjects)
      {
        if (!Object.op_Equality((Object) offModeObject, (Object) null) && offModeObject.get_activeSelf() == isOn)
          offModeObject.SetActive(!isOn);
      }
    }

    private void RefreshEmissionMode(bool isOn)
    {
      if (this._emissionInfos.IsNullOrEmpty<TimeLinkedLightObject.EmissionInfo>())
        return;
      foreach (TimeLinkedLightObject.EmissionInfo emissionInfo in this._emissionInfos)
      {
        Material material = emissionInfo.Material;
        if (!Object.op_Equality((Object) material, (Object) null))
        {
          if (emissionInfo.IsStrengthSwitching)
          {
            if (isOn)
            {
              if (material.HasProperty(this._emissionStrengthName))
                material.SetFloat(this._emissionStrengthName, emissionInfo.EmissionStrength);
            }
            else if (material.HasProperty(this._emissionStrengthName))
              material.SetFloat(this._emissionStrengthName, 0.0f);
          }
          else if (emissionInfo.IsEmissionSwitching)
          {
            if (isOn)
            {
              if (!material.IsKeywordEnabled(this._emissionKeyName))
                material.EnableKeyword(this._emissionKeyName);
            }
            else if (material.IsKeywordEnabled(this._emissionKeyName))
              material.DisableKeyword(this._emissionKeyName);
          }
          else if (isOn)
          {
            if (!material.IsKeywordEnabled(this._emissionKeyName))
              material.EnableKeyword(this._emissionKeyName);
            if (material.HasProperty(this._emissionParamName))
              material.SetVector(this._emissionParamName, Color.op_Implicit(emissionInfo.OnColor));
          }
          else if (emissionInfo.IsOffModeDisable)
          {
            if (material.IsKeywordEnabled(this._emissionKeyName))
              material.DisableKeyword(this._emissionKeyName);
          }
          else
          {
            if (!material.IsKeywordEnabled(this._emissionKeyName))
              material.EnableKeyword(this._emissionKeyName);
            if (material.HasProperty(this._emissionParamName))
              material.SetVector(this._emissionParamName, Color.op_Implicit(emissionInfo.OffColor));
          }
        }
      }
    }

    private void ResetMaterial()
    {
      if (this._emissionInfos.IsNullOrEmpty<TimeLinkedLightObject.EmissionInfo>())
        return;
      foreach (TimeLinkedLightObject.EmissionInfo emissionInfo in this._emissionInfos)
      {
        if (emissionInfo != null && !Object.op_Equality((Object) emissionInfo.Renderer, (Object) null))
        {
          Material material = emissionInfo.Renderer.get_material();
          float emissionStrength = emissionInfo.EmissionStrength;
          if ((double) emissionStrength < 0.0 && material.HasProperty(this._emissionStrengthName))
            emissionStrength = material.GetFloat(this._emissionStrengthName);
          emissionInfo.SetInfo(material, emissionStrength);
        }
      }
    }

    [Serializable]
    public class EmissionInfo
    {
      [SerializeField]
      private float _emissionStrength = -1f;
      [SerializeField]
      private bool _isEmissionSwitching = true;
      [SerializeField]
      [ColorUsage(false, true)]
      private Color _onColor = new Color(0.0f, 0.0f, 0.0f, 1f);
      [SerializeField]
      [ColorUsage(false, true)]
      private Color _offColor = new Color(0.0f, 0.0f, 0.0f, 1f);
      [SerializeField]
      private Renderer _renderer;
      [ShowInInspector]
      [ReadOnly]
      private Material _material;
      [SerializeField]
      private bool _isStrengthSwitch;
      [SerializeField]
      private bool _isOffModeDisable;

      public EmissionInfo()
      {
      }

      public EmissionInfo(Renderer ren)
      {
        this._renderer = ren;
      }

      public Renderer Renderer
      {
        get
        {
          return this._renderer;
        }
      }

      public Material Material
      {
        get
        {
          return this._material;
        }
      }

      public float EmissionStrength
      {
        get
        {
          return this._emissionStrength;
        }
      }

      public Color OnColor
      {
        get
        {
          return this._onColor;
        }
      }

      public Color OffColor
      {
        get
        {
          return this._offColor;
        }
      }

      public bool IsStrengthSwitching
      {
        get
        {
          return this._isStrengthSwitch;
        }
      }

      public bool IsEmissionSwitching
      {
        get
        {
          return this._isEmissionSwitching;
        }
      }

      public bool IsOffModeDisable
      {
        get
        {
          return this._isOffModeDisable;
        }
      }

      public void SetMaterial(Material material)
      {
        this._material = material;
      }

      public void SetInfo(Material material, float strength)
      {
        this._material = material;
        this._emissionStrength = strength;
      }

      public void SetStrengthSwitching(bool flag)
      {
        this._isStrengthSwitch = flag;
      }

      public void SetEmissionSwithcing(bool flag)
      {
        this._isEmissionSwitching = flag;
      }

      public void SetOffModeDisable(bool flag)
      {
        this._isOffModeDisable = flag;
      }
    }
  }
}
