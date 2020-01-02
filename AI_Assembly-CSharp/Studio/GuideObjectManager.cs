// Decompiled with JetBrains decompiler
// Type: Studio.GuideObjectManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Studio
{
  public class GuideObjectManager : Singleton<GuideObjectManager>
  {
    private HashSet<GuideObject> hashSelectObject = new HashSet<GuideObject>();
    private Dictionary<Transform, GuideObject> dicGuideObject = new Dictionary<Transform, GuideObject>();
    private Dictionary<Transform, Light> dicTransLight = new Dictionary<Transform, Light>();
    private Dictionary<GuideObject, Light> dicGuideLight = new Dictionary<GuideObject, Light>();
    [SerializeField]
    private GameObject objectOriginal;
    [SerializeField]
    private GuideInput m_GuideInput;
    [SerializeField]
    private Transform transformWorkplace;
    [SerializeField]
    private DrawLightLine m_DrawLightLine;
    private int m_Mode;

    public GuideInput guideInput
    {
      get
      {
        return this.m_GuideInput;
      }
    }

    public DrawLightLine drawLightLine
    {
      get
      {
        return this.m_DrawLightLine;
      }
    }

    public GuideObject selectObject
    {
      get
      {
        return this.hashSelectObject.Count != 0 ? this.hashSelectObject.ToArray<GuideObject>()[0] : (GuideObject) null;
      }
      set
      {
        this.SetSelectObject(value, true);
      }
    }

    public GuideObject deselectObject
    {
      set
      {
        this.SetDeselectObject(value);
      }
    }

    public GuideObject[] selectObjects
    {
      get
      {
        return this.hashSelectObject.Count != 0 ? this.hashSelectObject.ToArray<GuideObject>() : (GuideObject[]) null;
      }
    }

    public ChangeAmount[] selectObjectChangeAmount
    {
      get
      {
        return this.hashSelectObject.Select<GuideObject, ChangeAmount>((Func<GuideObject, ChangeAmount>) (v => v.changeAmount)).ToArray<ChangeAmount>();
      }
    }

    public int[] selectObjectKey
    {
      get
      {
        return this.hashSelectObject.Select<GuideObject, int>((Func<GuideObject, int>) (v => v.dicKey)).ToArray<int>();
      }
    }

    public Dictionary<int, ChangeAmount> selectObjectDictionary
    {
      get
      {
        return this.hashSelectObject.ToDictionary<GuideObject, int, ChangeAmount>((Func<GuideObject, int>) (v => v.dicKey), (Func<GuideObject, ChangeAmount>) (v => v.changeAmount));
      }
    }

    public GuideObject operationTarget { get; set; }

    public bool isOperationTarget
    {
      get
      {
        return Object.op_Inequality((Object) this.operationTarget, (Object) null);
      }
    }

    public int mode
    {
      get
      {
        return this.m_Mode;
      }
      set
      {
        if (!Utility.SetStruct<int>(ref this.m_Mode, value))
          return;
        this.SetMode(this.m_Mode);
        if (this.ModeChangeEvent == null)
          return;
        this.ModeChangeEvent((object) this, EventArgs.Empty);
      }
    }

    public static int GetMode()
    {
      return !Singleton<GuideObjectManager>.IsInstance() ? 0 : Singleton<GuideObjectManager>.Instance.mode;
    }

    public event EventHandler ModeChangeEvent;

    public GuideObject Add(Transform _target, int _dicKey)
    {
      GameObject gameObject = (GameObject) Object.Instantiate<GameObject>((M0) this.objectOriginal);
      gameObject.get_transform().SetParent(this.transformWorkplace);
      GuideObject component1 = (GuideObject) gameObject.GetComponent<GuideObject>();
      component1.transformTarget = _target;
      component1.dicKey = _dicKey;
      this.dicGuideObject.Add(_target, component1);
      Light component2 = (Light) ((Component) _target).GetComponent<Light>();
      if (Object.op_Implicit((Object) component2) && component2.get_type() != 1)
        this.dicTransLight.Add(_target, component2);
      return component1;
    }

    public void Delete(GuideObject _object, bool _destroy = true)
    {
      if (Object.op_Equality((Object) _object, (Object) null))
        return;
      if (this.hashSelectObject.Contains(_object))
        this.SetSelectObject((GuideObject) null, false);
      this.dicGuideObject.Remove(_object.transformTarget);
      this.dicTransLight.Remove(_object.transformTarget);
      this.dicGuideLight.Remove(_object);
      if (_destroy)
        Object.DestroyImmediate((Object) ((Component) _object).get_gameObject());
      if (!Object.op_Equality((Object) this.operationTarget, (Object) _object))
        return;
      this.operationTarget = (GuideObject) null;
    }

    public void DeleteAll()
    {
      this.hashSelectObject.Clear();
      this.operationTarget = (GuideObject) null;
      GameObject[] array = ((IEnumerable<KeyValuePair<Transform, GuideObject>>) this.dicGuideObject).Where<KeyValuePair<Transform, GuideObject>>((Func<KeyValuePair<Transform, GuideObject>, bool>) (v => Object.op_Inequality((Object) v.Value, (Object) null))).Select<KeyValuePair<Transform, GuideObject>, GameObject>((Func<KeyValuePair<Transform, GuideObject>, GameObject>) (v => ((Component) v.Value).get_gameObject())).ToArray<GameObject>();
      for (int index = 0; index < array.Length; ++index)
      {
        if (Object.op_Implicit((Object) array[index]))
          Object.DestroyImmediate((Object) array[index]);
      }
      this.dicGuideObject.Clear();
      this.dicTransLight.Clear();
      this.dicGuideLight.Clear();
      this.drawLightLine.Clear();
      this.guideInput.Stop();
    }

    public void AddSelectMultiple(GuideObject _object)
    {
      if (Object.op_Equality((Object) _object, (Object) null) || this.hashSelectObject.Contains(_object) || this.hashSelectObject.Count != 0 && !_object.enableMaluti)
        return;
      this.AddObject(_object, this.hashSelectObject.Count == 0);
      this.guideInput.AddSelectMultiple(_object);
    }

    public void SetScale()
    {
      using (Dictionary<Transform, GuideObject>.Enumerator enumerator = this.dicGuideObject.GetEnumerator())
      {
        while (enumerator.MoveNext())
          enumerator.Current.Value.SetScale();
      }
    }

    public void SetVisibleTranslation()
    {
      bool visibleAxisTranslation = Singleton<Studio.Studio>.Instance.workInfo.visibleAxisTranslation;
      using (Dictionary<Transform, GuideObject>.Enumerator enumerator = this.dicGuideObject.GetEnumerator())
      {
        while (enumerator.MoveNext())
          enumerator.Current.Value.visibleTranslation = visibleAxisTranslation;
      }
    }

    public void SetVisibleCenter()
    {
      bool visibleAxisCenter = Singleton<Studio.Studio>.Instance.workInfo.visibleAxisCenter;
      using (Dictionary<Transform, GuideObject>.Enumerator enumerator = this.dicGuideObject.GetEnumerator())
      {
        while (enumerator.MoveNext())
          enumerator.Current.Value.visibleCenter = visibleAxisCenter;
      }
    }

    private void SetMode(int _mode)
    {
      foreach (GuideObject guideObject in this.hashSelectObject)
        guideObject.SetMode(_mode, true);
    }

    private void SetSelectObject(GuideObject _object, bool _multiple = true)
    {
      bool flag = Input.GetKey((KeyCode) 306) || Input.GetKey((KeyCode) 305);
      bool key = Input.GetKey((KeyCode) 120);
      if (_multiple && flag && !key)
      {
        if (Object.op_Equality((Object) _object, (Object) null) || this.hashSelectObject.Contains(_object) || this.hashSelectObject.Count != 0 && !_object.enableMaluti)
          return;
        this.AddObject(_object, this.hashSelectObject.Count == 0);
      }
      else
      {
        switch (Studio.Studio.optionSystem.selectedState)
        {
          case 0:
            this.StopSelectObject();
            break;
          case 1:
            GuideObject selectObject = this.selectObject;
            if (!Object.op_Equality((Object) selectObject, (Object) null))
            {
              if (!selectObject.isChild)
              {
                if (Object.op_Implicit((Object) _object) && _object.isChild)
                {
                  selectObject.SetActive(false, false);
                  break;
                }
                this.StopSelectObject();
                break;
              }
              selectObject.SetActive(false, false);
              break;
            }
            break;
        }
        this.hashSelectObject.Clear();
        if (Object.op_Implicit((Object) _object) && !_object.enables[this.m_Mode])
        {
          for (int index = 0; index < 3; ++index)
          {
            if (_object.enables[index])
            {
              this.mode = index;
              break;
            }
          }
        }
        this.AddObject(_object, true);
      }
      this.guideInput.guideObject = _object;
    }

    private void SetDeselectObject(GuideObject _object)
    {
      if (Object.op_Equality((Object) _object, (Object) null))
        return;
      bool isActive = _object.isActive;
      _object.isActive = false;
      Light _light = (Light) null;
      if (this.dicTransLight.TryGetValue(_object.transformTarget, out _light))
      {
        this.drawLightLine.Remove(_light);
        this.dicGuideLight.Remove(_object);
      }
      this.hashSelectObject.Remove(_object);
      this.guideInput.deselectObject = _object;
      if (this.hashSelectObject.Count <= 0 || !isActive)
        return;
      this.selectObject.isActive = true;
    }

    private void StopSelectObject()
    {
      foreach (GuideObject key in this.hashSelectObject)
      {
        key.isActive = false;
        Light _light = (Light) null;
        if (this.dicGuideLight.TryGetValue(key, out _light))
        {
          this.drawLightLine.Remove(_light);
          this.dicGuideLight.Remove(key);
        }
      }
    }

    private void AddObject(GuideObject _object, bool _active = true)
    {
      if (Object.op_Equality((Object) _object, (Object) null))
        return;
      if (_active)
        _object.isActive = true;
      Light _light = (Light) null;
      if (this.dicTransLight.TryGetValue(_object.transformTarget, out _light))
      {
        this.drawLightLine.Add(_light);
        this.dicGuideLight.Add(_object, _light);
      }
      this.hashSelectObject.Add(_object);
    }

    protected override void Awake()
    {
      if (!this.CheckInstance())
        return;
      Object.DontDestroyOnLoad((Object) ((Component) this).get_gameObject());
      if (Object.op_Equality((Object) this.transformWorkplace, (Object) null))
        this.transformWorkplace = ((Component) this).get_transform();
      this.operationTarget = (GuideObject) null;
    }
  }
}
