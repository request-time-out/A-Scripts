// Decompiled with JetBrains decompiler
// Type: Studio.VoiceRegistrationList
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using Manager;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Studio
{
  public class VoiceRegistrationList : MonoBehaviour
  {
    [SerializeField]
    private Button buttonClose;
    [SerializeField]
    private InputField inputName;
    [SerializeField]
    private Button buttonSave;
    [SerializeField]
    private Button buttonLoad;
    [SerializeField]
    private Button buttonImport;
    [SerializeField]
    private Transform transformRoot;
    [SerializeField]
    private GameObject prefabNode;
    [SerializeField]
    private Button buttonDelete;
    [SerializeField]
    private Sprite spriteDelete;
    [SerializeField]
    private VoiceControl voiceControl;
    private List<string> listPath;
    private Dictionary<int, StudioNode> dicNode;
    private int select;

    public VoiceRegistrationList()
    {
      base.\u002Ector();
    }

    public OCIChar ociChar { get; set; }

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
        if (!((Component) this).get_gameObject().get_activeSelf())
          return;
        this.InitList();
      }
    }

    private void OnClickClose()
    {
      ((Component) this).get_gameObject().SetActive(false);
    }

    private void OnEndEditName(string _text)
    {
      ((Selectable) this.buttonSave).set_interactable(!_text.IsNullOrEmpty());
    }

    private void OnClickSave()
    {
      this.ociChar.voiceCtrl.SaveList(this.inputName.get_text());
      this.InitList();
    }

    private void OnClickLoad()
    {
      this.ociChar.voiceCtrl.LoadList(this.listPath[this.select], false);
      this.voiceControl.InitList();
    }

    private void OnClickImport()
    {
      this.ociChar.voiceCtrl.LoadList(this.listPath[this.select], true);
      this.voiceControl.InitList();
    }

    private void OnClickDelete()
    {
      CheckScene.sprite = this.spriteDelete;
      // ISSUE: method pointer
      CheckScene.unityActionYes = new UnityAction((object) this, __methodptr(OnSelectDeleteYes));
      // ISSUE: method pointer
      CheckScene.unityActionNo = new UnityAction((object) this, __methodptr(OnSelectDeleteNo));
      Singleton<Scene>.Instance.LoadReserve(new Scene.Data()
      {
        levelName = "StudioCheck",
        isAdd = true
      }, false);
    }

    private void OnSelectDeleteYes()
    {
      Singleton<Scene>.Instance.UnLoad();
      File.Delete(this.listPath[this.select]);
      this.InitList();
    }

    private void OnSelectDeleteNo()
    {
      Singleton<Scene>.Instance.UnLoad();
    }

    private void OnClickSelect(int _no)
    {
      StudioNode studioNode = (StudioNode) null;
      if (this.dicNode.TryGetValue(this.select, out studioNode))
        studioNode.select = false;
      this.select = _no;
      if (this.dicNode.TryGetValue(this.select, out studioNode))
        studioNode.select = true;
      if (this.select == -1)
        return;
      ((Selectable) this.buttonLoad).set_interactable(true);
      ((Selectable) this.buttonImport).set_interactable(true);
      ((Selectable) this.buttonDelete).set_interactable(true);
    }

    private void InitList()
    {
      for (int index = 0; index < this.transformRoot.get_childCount(); ++index)
        Object.Destroy((Object) ((Component) this.transformRoot.GetChild(index)).get_gameObject());
      this.transformRoot.DetachChildren();
      this.select = -1;
      this.inputName.set_text(string.Empty);
      ((Selectable) this.buttonSave).set_interactable(false);
      ((Selectable) this.buttonLoad).set_interactable(false);
      ((Selectable) this.buttonImport).set_interactable(false);
      ((Selectable) this.buttonDelete).set_interactable(false);
      string[] files = System.IO.Directory.GetFiles(UserData.Create("studio/voicelist"), "*.dat");
      // ISSUE: reference to a compiler-generated field
      if (VoiceRegistrationList.\u003C\u003Ef__mg\u0024cache0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        VoiceRegistrationList.\u003C\u003Ef__mg\u0024cache0 = new Func<string, bool>(VoiceCtrl.CheckIdentifyingCode);
      }
      // ISSUE: reference to a compiler-generated field
      Func<string, bool> fMgCache0 = VoiceRegistrationList.\u003C\u003Ef__mg\u0024cache0;
      this.listPath = ((IEnumerable<string>) files).Where<string>(fMgCache0).ToList<string>();
      this.dicNode.Clear();
      for (int key = 0; key < this.listPath.Count; ++key)
      {
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        VoiceRegistrationList.\u003CInitList\u003Ec__AnonStorey0 listCAnonStorey0 = new VoiceRegistrationList.\u003CInitList\u003Ec__AnonStorey0();
        // ISSUE: reference to a compiler-generated field
        listCAnonStorey0.\u0024this = this;
        GameObject gameObject = (GameObject) Object.Instantiate<GameObject>((M0) this.prefabNode);
        gameObject.get_transform().SetParent(this.transformRoot, false);
        StudioNode component = (StudioNode) gameObject.GetComponent<StudioNode>();
        component.active = true;
        // ISSUE: reference to a compiler-generated field
        listCAnonStorey0.no = key;
        // ISSUE: method pointer
        component.addOnClick = new UnityAction((object) listCAnonStorey0, __methodptr(\u003C\u003Em__0));
        component.text = VoiceCtrl.LoadListName(this.listPath[key]);
        this.dicNode.Add(key, component);
      }
    }

    private void Awake()
    {
      // ISSUE: method pointer
      ((UnityEvent) this.buttonClose.get_onClick()).AddListener(new UnityAction((object) this, __methodptr(OnClickClose)));
      // ISSUE: method pointer
      ((UnityEvent<string>) this.inputName.get_onEndEdit()).AddListener(new UnityAction<string>((object) this, __methodptr(OnEndEditName)));
      // ISSUE: method pointer
      ((UnityEvent) this.buttonSave.get_onClick()).AddListener(new UnityAction((object) this, __methodptr(OnClickSave)));
      ((Selectable) this.buttonSave).set_interactable(false);
      // ISSUE: method pointer
      ((UnityEvent) this.buttonLoad.get_onClick()).AddListener(new UnityAction((object) this, __methodptr(OnClickLoad)));
      // ISSUE: method pointer
      ((UnityEvent) this.buttonImport.get_onClick()).AddListener(new UnityAction((object) this, __methodptr(OnClickImport)));
      // ISSUE: method pointer
      ((UnityEvent) this.buttonDelete.get_onClick()).AddListener(new UnityAction((object) this, __methodptr(OnClickDelete)));
    }
  }
}
