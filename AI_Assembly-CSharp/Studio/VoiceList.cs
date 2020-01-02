// Decompiled with JetBrains decompiler
// Type: Studio.VoiceList
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Studio
{
  public class VoiceList : MonoBehaviour
  {
    [SerializeField]
    private Transform transformRoot;
    [SerializeField]
    private GameObject objectPrefab;
    [SerializeField]
    private ScrollRect scrollRect;
    [SerializeField]
    private VoiceControl voiceControl;
    private int group;
    private int category;

    public VoiceList()
    {
      base.\u002Ector();
    }

    public bool active
    {
      get
      {
        return ((Component) this).get_gameObject().get_activeSelf();
      }
      set
      {
        if (((Component) this).get_gameObject().get_activeSelf() != value)
          ((Component) this).get_gameObject().SetActive(value);
        if (((Component) this).get_gameObject().get_activeSelf())
          return;
        this.group = -1;
        this.category = -1;
      }
    }

    public void InitList(int _group, int _category)
    {
      if (!Utility.SetStruct<int>(ref this.group, _group) && !Utility.SetStruct<int>(ref this.category, _category))
        return;
      int childCount = this.transformRoot.get_childCount();
      for (int index = 0; index < childCount; ++index)
        Object.Destroy((Object) ((Component) this.transformRoot.GetChild(index)).get_gameObject());
      this.transformRoot.DetachChildren();
      this.scrollRect.set_verticalNormalizedPosition(1f);
      foreach (KeyValuePair<int, Info.LoadCommonInfo> keyValuePair in Singleton<Info>.Instance.dicVoiceLoadInfo[_group][_category])
      {
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        VoiceList.\u003CInitList\u003Ec__AnonStorey0 listCAnonStorey0 = new VoiceList.\u003CInitList\u003Ec__AnonStorey0();
        // ISSUE: reference to a compiler-generated field
        listCAnonStorey0.\u0024this = this;
        GameObject gameObject = (GameObject) Object.Instantiate<GameObject>((M0) this.objectPrefab);
        if (!gameObject.get_activeSelf())
          gameObject.SetActive(true);
        gameObject.get_transform().SetParent(this.transformRoot, false);
        VoiceNode component = (VoiceNode) gameObject.GetComponent<VoiceNode>();
        // ISSUE: reference to a compiler-generated field
        listCAnonStorey0.no = keyValuePair.Key;
        // ISSUE: method pointer
        component.addOnClick = new UnityAction((object) listCAnonStorey0, __methodptr(\u003C\u003Em__0));
        component.text = keyValuePair.Value.name;
      }
      this.active = true;
      this.group = _group;
      this.category = _category;
    }

    private void OnSelect(int _no)
    {
      OCIChar[] array = ((IEnumerable<int>) Singleton<GuideObjectManager>.Instance.selectObjectKey).Select<int, OCIChar>((Func<int, OCIChar>) (v => Studio.Studio.GetCtrlInfo(v) as OCIChar)).Where<OCIChar>((Func<OCIChar, bool>) (v => v != null)).ToArray<OCIChar>();
      int length = array.Length;
      for (int index = 0; index < length; ++index)
        array[index].AddVoice(this.group, this.category, _no);
      this.voiceControl.InitList();
    }
  }
}
