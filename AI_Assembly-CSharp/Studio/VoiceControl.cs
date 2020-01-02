// Decompiled with JetBrains decompiler
// Type: Studio.VoiceControl
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Studio
{
  public class VoiceControl : MonoBehaviour
  {
    [SerializeField]
    private GameObject objectNode;
    [SerializeField]
    private Transform transformRoot;
    [SerializeField]
    private ScrollRect scrollRect;
    [SerializeField]
    private Button buttonRepeat;
    [SerializeField]
    private Sprite[] spriteRepeat;
    [SerializeField]
    private Button buttonStop;
    [SerializeField]
    private Button buttonPlay;
    [SerializeField]
    private Image imagePlayNow;
    [SerializeField]
    private Button buttonPlayAll;
    [SerializeField]
    private Button buttonStopAll;
    [SerializeField]
    private Button buttonExpansion;
    [SerializeField]
    private Sprite[] spriteExpansion;
    [SerializeField]
    private GameObject objBeneath;
    [SerializeField]
    private Button buttonSave;
    [SerializeField]
    private Button buttonDeleteAll;
    [SerializeField]
    private VoiceRegistrationList voiceRegistrationList;
    private OCIChar m_OCIChar;
    private List<VoicePlayNode> listNode;
    private int select;

    public VoiceControl()
    {
      base.\u002Ector();
    }

    public OCIChar ociChar
    {
      set
      {
        this.m_OCIChar = value;
        if (this.m_OCIChar == null)
          return;
        this.InitList();
      }
    }

    public bool active
    {
      get
      {
        return ((Component) this).get_gameObject().get_activeSelf();
      }
      set
      {
        if (((Component) this).get_gameObject().get_activeSelf() == value)
          return;
        ((Component) this).get_gameObject().SetActive(value);
      }
    }

    public void InitList()
    {
      for (int index = 0; index < this.transformRoot.get_childCount(); ++index)
        Object.Destroy((Object) ((Component) this.transformRoot.GetChild(index)).get_gameObject());
      this.transformRoot.DetachChildren();
      this.select = -1;
      this.listNode.Clear();
      foreach (VoiceCtrl.VoiceInfo voiceInfo1 in this.m_OCIChar.voiceCtrl.list)
      {
        Info.LoadCommonInfo voiceInfo2 = Singleton<Info>.Instance.GetVoiceInfo(voiceInfo1.group, voiceInfo1.category, voiceInfo1.no);
        if (voiceInfo2 == null)
          Debug.Log((object) string.Format("情報が取れない G[{0}] : C[{1}] : N[{2}]", (object) voiceInfo1.group, (object) voiceInfo1.category, (object) voiceInfo1.no));
        else
          this.AddNode(voiceInfo2.name);
      }
      this.scrollRect.set_verticalNormalizedPosition(1f);
      ((Behaviour) this.imagePlayNow).set_enabled(this.m_OCIChar != null && this.m_OCIChar.voiceCtrl.isPlay);
      ((Selectable) this.buttonRepeat).get_image().set_sprite(this.spriteRepeat[(int) this.m_OCIChar.voiceRepeat]);
      this.voiceRegistrationList.ociChar = this.m_OCIChar;
    }

    private void AddNode(string _name)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      VoiceControl.\u003CAddNode\u003Ec__AnonStorey0 nodeCAnonStorey0 = new VoiceControl.\u003CAddNode\u003Ec__AnonStorey0();
      // ISSUE: reference to a compiler-generated field
      nodeCAnonStorey0.\u0024this = this;
      GameObject gameObject = (GameObject) Object.Instantiate<GameObject>((M0) this.objectNode);
      gameObject.get_transform().SetParent(this.transformRoot, false);
      // ISSUE: reference to a compiler-generated field
      nodeCAnonStorey0.vpn = (VoicePlayNode) gameObject.GetComponent<VoicePlayNode>();
      // ISSUE: reference to a compiler-generated field
      nodeCAnonStorey0.vpn.active = true;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: method pointer
      nodeCAnonStorey0.vpn.addOnClick = new UnityAction((object) nodeCAnonStorey0, __methodptr(\u003C\u003Em__0));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: method pointer
      nodeCAnonStorey0.vpn.addOnClickDelete = new UnityAction((object) nodeCAnonStorey0, __methodptr(\u003C\u003Em__1));
      // ISSUE: reference to a compiler-generated field
      nodeCAnonStorey0.vpn.text = _name;
      // ISSUE: reference to a compiler-generated field
      this.listNode.Add(nodeCAnonStorey0.vpn);
    }

    private void OnClickRepeat()
    {
      int index = (int) (this.m_OCIChar.voiceRepeat + 1) % Enum.GetNames(typeof (VoiceCtrl.Repeat)).Length;
      this.m_OCIChar.voiceRepeat = (VoiceCtrl.Repeat) index;
      ((Selectable) this.buttonRepeat).get_image().set_sprite(this.spriteRepeat[index]);
    }

    private void OnClickStop()
    {
      this.m_OCIChar.StopVoice();
    }

    private void OnClickPlay()
    {
      ((Behaviour) this.imagePlayNow).set_enabled(this.m_OCIChar.PlayVoice(this.select));
    }

    private void OnClickPlayAll()
    {
      OCIChar[] array = ((IEnumerable<int>) Singleton<GuideObjectManager>.Instance.selectObjectKey).Select<int, OCIChar>((Func<int, OCIChar>) (v => Studio.Studio.GetCtrlInfo(v) as OCIChar)).Where<OCIChar>((Func<OCIChar, bool>) (v => v != null)).ToArray<OCIChar>();
      int length = array.Length;
      for (int index = 0; index < length; ++index)
        array[index].PlayVoice(0);
    }

    private void OnClickStopAll()
    {
      OCIChar[] array = ((IEnumerable<int>) Singleton<GuideObjectManager>.Instance.selectObjectKey).Select<int, OCIChar>((Func<int, OCIChar>) (v => Studio.Studio.GetCtrlInfo(v) as OCIChar)).Where<OCIChar>((Func<OCIChar, bool>) (v => v != null)).ToArray<OCIChar>();
      int length = array.Length;
      for (int index = 0; index < length; ++index)
        array[index].StopVoice();
    }

    private void OnClickExpansion()
    {
      bool flag = !this.objBeneath.get_activeSelf();
      this.objBeneath.SetActive(flag);
      ((Selectable) this.buttonExpansion).get_image().set_sprite(this.spriteExpansion[!flag ? 0 : 1]);
    }

    private void OnClickSave()
    {
      this.voiceRegistrationList.active = !this.voiceRegistrationList.active;
      if (!this.voiceRegistrationList.active)
        return;
      this.voiceRegistrationList.ociChar = this.m_OCIChar;
    }

    private void OnClickDeleteAll()
    {
      int count = this.listNode.Count;
      for (int index = 0; index < count; ++index)
        this.listNode[index].Destroy();
      this.listNode.Clear();
      this.scrollRect.set_verticalNormalizedPosition(1f);
      this.m_OCIChar.DeleteAllVoice();
    }

    private void OnClickSelect(VoicePlayNode _vpn)
    {
      if (MathfEx.RangeEqualOn<int>(0, this.select, this.listNode.Count))
        this.listNode[this.select].select = false;
      this.select = this.listNode.FindIndex((Predicate<VoicePlayNode>) (v => Object.op_Equality((Object) v, (Object) _vpn)));
      this.listNode[this.select].select = true;
    }

    private void OnClickDelete(VoicePlayNode _vpn)
    {
      int index = this.listNode.FindIndex((Predicate<VoicePlayNode>) (v => Object.op_Equality((Object) v, (Object) _vpn)));
      this.listNode.RemoveAt(index);
      _vpn.Destroy();
      this.scrollRect.set_verticalNormalizedPosition(1f);
      this.m_OCIChar.DeleteVoice(index);
      if (this.select != index)
        return;
      this.select = -1;
    }

    private void Start()
    {
      // ISSUE: method pointer
      ((UnityEvent) this.buttonRepeat.get_onClick()).AddListener(new UnityAction((object) this, __methodptr(OnClickRepeat)));
      // ISSUE: method pointer
      ((UnityEvent) this.buttonStop.get_onClick()).AddListener(new UnityAction((object) this, __methodptr(OnClickStop)));
      // ISSUE: method pointer
      ((UnityEvent) this.buttonPlay.get_onClick()).AddListener(new UnityAction((object) this, __methodptr(OnClickPlay)));
      // ISSUE: method pointer
      ((UnityEvent) this.buttonPlayAll.get_onClick()).AddListener(new UnityAction((object) this, __methodptr(OnClickPlayAll)));
      // ISSUE: method pointer
      ((UnityEvent) this.buttonStopAll.get_onClick()).AddListener(new UnityAction((object) this, __methodptr(OnClickStopAll)));
      // ISSUE: method pointer
      ((UnityEvent) this.buttonExpansion.get_onClick()).AddListener(new UnityAction((object) this, __methodptr(OnClickExpansion)));
      // ISSUE: method pointer
      ((UnityEvent) this.buttonSave.get_onClick()).AddListener(new UnityAction((object) this, __methodptr(OnClickSave)));
      // ISSUE: method pointer
      ((UnityEvent) this.buttonDeleteAll.get_onClick()).AddListener(new UnityAction((object) this, __methodptr(OnClickDeleteAll)));
    }

    private void Update()
    {
      if (!((Behaviour) this.imagePlayNow).get_enabled())
        return;
      ((Behaviour) this.imagePlayNow).set_enabled(this.m_OCIChar != null && this.m_OCIChar.voiceCtrl.isPlay);
    }
  }
}
