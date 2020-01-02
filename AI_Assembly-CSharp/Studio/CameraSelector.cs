// Decompiled with JetBrains decompiler
// Type: Studio.CameraSelector
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Studio
{
  public class CameraSelector : MonoBehaviour
  {
    [SerializeField]
    private TMP_Dropdown dropdown;
    private List<OCICamera> listCamera;
    private int index;

    public CameraSelector()
    {
      base.\u002Ector();
    }

    private void OnValueChanged(int _index)
    {
      if (_index == 0)
        Singleton<Studio.Studio>.Instance.ChangeCamera(Singleton<Studio.Studio>.Instance.ociCamera, false, false);
      else
        Singleton<Studio.Studio>.Instance.ChangeCamera(this.listCamera[_index - 1], true, false);
    }

    public void SetCamera(OCICamera _ocic)
    {
      this.dropdown.set_value((_ocic != null ? this.listCamera.FindIndex((Predicate<OCICamera>) (c => c == _ocic)) : -1) + 1);
    }

    public void NextCamera()
    {
      if (this.listCamera.IsNullOrEmpty<OCICamera>())
        return;
      this.index = (this.index + 1) % (this.listCamera.Count + 1);
      this.OnValueChanged(this.index);
    }

    public void Init()
    {
      this.dropdown.ClearOptions();
      List<ObjectInfo> objectInfoList = ObjectInfoAssist.Find(5);
      this.listCamera = objectInfoList.Select<ObjectInfo, OCICamera>((Func<ObjectInfo, OCICamera>) (i => Studio.Studio.GetCtrlInfo(i.dicKey) as OCICamera)).ToList<OCICamera>();
      this.index = 0;
      List<TMP_Dropdown.OptionData> optionDataList = !objectInfoList.IsNullOrEmpty<ObjectInfo>() ? objectInfoList.Select<ObjectInfo, TMP_Dropdown.OptionData>((Func<ObjectInfo, TMP_Dropdown.OptionData>) (c => new TMP_Dropdown.OptionData((Studio.Studio.GetCtrlInfo(c.dicKey) as OCICamera).name))).ToList<TMP_Dropdown.OptionData>() : new List<TMP_Dropdown.OptionData>();
      optionDataList.Insert(0, new TMP_Dropdown.OptionData("-"));
      this.dropdown.set_options(optionDataList);
      ((Selectable) this.dropdown).set_interactable(!objectInfoList.IsNullOrEmpty<ObjectInfo>());
      this.SetCamera(Singleton<Studio.Studio>.Instance.ociCamera);
    }

    private void Awake()
    {
      // ISSUE: method pointer
      ((UnityEvent<int>) this.dropdown.get_onValueChanged()).AddListener(new UnityAction<int>((object) this, __methodptr(OnValueChanged)));
      ((Selectable) this.dropdown).set_interactable(false);
    }
  }
}
