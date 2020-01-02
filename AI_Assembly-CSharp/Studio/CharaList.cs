// Decompiled with JetBrains decompiler
// Type: Studio.CharaList
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using AIChara;
using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Studio
{
  public class CharaList : MonoBehaviour
  {
    [SerializeField]
    private int sex;
    [SerializeField]
    private GameObject objectNode;
    [SerializeField]
    private RawImage imageChara;
    [SerializeField]
    private CharaFileSort charaFileSort;
    [SerializeField]
    private Button buttonLoad;
    [SerializeField]
    private Button buttonChange;
    private bool isDelay;

    public CharaList()
    {
      base.\u002Ector();
    }

    public bool isInit { get; private set; }

    public void InitCharaList(bool _force = false)
    {
      if (this.isInit && !_force)
        return;
      this.charaFileSort.DeleteAllNode();
      if (this.sex == 1)
        this.InitFemaleList();
      else
        this.InitMaleList();
      int count = this.charaFileSort.cfiList.Count;
      for (int index = 0; index < count; ++index)
      {
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        CharaList.\u003CInitCharaList\u003Ec__AnonStorey0 listCAnonStorey0 = new CharaList.\u003CInitCharaList\u003Ec__AnonStorey0()
        {
          \u0024this = this,
          info = this.charaFileSort.cfiList[index]
        };
        // ISSUE: reference to a compiler-generated field
        listCAnonStorey0.info.index = index;
        GameObject gameObject = (GameObject) Object.Instantiate<GameObject>((M0) this.objectNode);
        if (!gameObject.get_activeSelf())
          gameObject.SetActive(true);
        gameObject.get_transform().SetParent(this.charaFileSort.root, false);
        // ISSUE: reference to a compiler-generated field
        listCAnonStorey0.info.node = (ListNode) gameObject.GetComponent<ListNode>();
        // ISSUE: reference to a compiler-generated field
        listCAnonStorey0.info.button = (Button) gameObject.GetComponent<Button>();
        // ISSUE: reference to a compiler-generated field
        // ISSUE: method pointer
        listCAnonStorey0.info.node.AddActionToButton(new UnityAction((object) listCAnonStorey0, __methodptr(\u003C\u003Em__0)));
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        listCAnonStorey0.info.node.text = listCAnonStorey0.info.name;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: method pointer
        listCAnonStorey0.info.node.listEnterAction.Add(new UnityAction((object) listCAnonStorey0, __methodptr(\u003C\u003Em__1)));
      }
      ((Graphic) this.imageChara).set_color(Color.get_clear());
      this.charaFileSort.Sort(0, false);
      ((Selectable) this.buttonLoad).set_interactable(false);
      ((Selectable) this.buttonChange).set_interactable(false);
      this.isInit = true;
    }

    private void OnSelectChara(int _idx)
    {
      if (this.charaFileSort.select == _idx)
        return;
      this.charaFileSort.select = _idx;
      ((Selectable) this.buttonLoad).set_interactable(true);
      OCIChar ctrlInfo = Studio.Studio.GetCtrlInfo(Singleton<Studio.Studio>.Instance.treeNodeCtrl.selectNode) as OCIChar;
      ((Selectable) this.buttonChange).set_interactable(ctrlInfo != null && ctrlInfo.oiCharInfo.sex == this.sex);
      this.isDelay = true;
      DisposableExtensions.AddTo<IDisposable>((M0) ObservableExtensions.Subscribe<long>((IObservable<M0>) Observable.Timer(TimeSpan.FromMilliseconds(250.0)), (Action<M0>) (_ => this.isDelay = false)), (Component) this);
    }

    private void LoadCharaImage(int _idx)
    {
      if (this.isDelay)
        return;
      this.imageChara.set_texture((Texture) PngAssist.LoadTexture(this.charaFileSort.cfiList[_idx].file));
      ((Graphic) this.imageChara).set_color(Color.get_white());
      Resources.UnloadUnusedAssets();
      GC.Collect();
    }

    public void OnSort(int _type)
    {
      this.charaFileSort.select = -1;
      ((Selectable) this.buttonLoad).set_interactable(false);
      ((Selectable) this.buttonChange).set_interactable(false);
      this.charaFileSort.Sort(_type);
    }

    public void LoadCharaFemale()
    {
      Singleton<Studio.Studio>.Instance.AddFemale(this.charaFileSort.selectPath);
    }

    public void ChangeCharaFemale()
    {
      OCIChar[] array = ((IEnumerable<int>) Singleton<GuideObjectManager>.Instance.selectObjectKey).Select<int, OCIChar>((System.Func<int, OCIChar>) (v => Studio.Studio.GetCtrlInfo(v) as OCIChar)).Where<OCIChar>((System.Func<OCIChar, bool>) (v => v != null)).Where<OCIChar>((System.Func<OCIChar, bool>) (v => v.oiCharInfo.sex == 1)).ToArray<OCIChar>();
      int length = array.Length;
      for (int index = 0; index < length; ++index)
        array[index].ChangeChara(this.charaFileSort.selectPath);
    }

    private void InitFemaleList()
    {
      List<string> files = new List<string>();
      Illusion.Utils.File.GetAllFiles(UserData.Path + "chara/female", "*.png", ref files);
      this.charaFileSort.cfiList.Clear();
      foreach (string str in files)
      {
        ChaFileControl chaFileControl = new ChaFileControl();
        if (chaFileControl.LoadCharaFile(str, (byte) 1, true, true))
          this.charaFileSort.cfiList.Add(new CharaFileInfo(string.Empty, string.Empty)
          {
            file = str,
            name = chaFileControl.parameter.fullname,
            time = System.IO.File.GetLastWriteTime(str)
          });
      }
    }

    public void LoadCharaMale()
    {
      Singleton<Studio.Studio>.Instance.AddMale(this.charaFileSort.selectPath);
    }

    public void ChangeCharaMale()
    {
      OCIChar[] array = ((IEnumerable<int>) Singleton<GuideObjectManager>.Instance.selectObjectKey).Select<int, OCIChar>((System.Func<int, OCIChar>) (v => Studio.Studio.GetCtrlInfo(v) as OCIChar)).Where<OCIChar>((System.Func<OCIChar, bool>) (v => v != null)).Where<OCIChar>((System.Func<OCIChar, bool>) (v => v.oiCharInfo.sex == 0)).ToArray<OCIChar>();
      int length = array.Length;
      for (int index = 0; index < length; ++index)
        array[index].ChangeChara(this.charaFileSort.selectPath);
    }

    private void InitMaleList()
    {
      List<string> files = new List<string>();
      Illusion.Utils.File.GetAllFiles(UserData.Path + "chara/male", "*.png", ref files);
      this.charaFileSort.cfiList.Clear();
      foreach (string str in files)
      {
        ChaFileControl chaFileControl = new ChaFileControl();
        if (chaFileControl.LoadCharaFile(str, (byte) 0, true, true))
          this.charaFileSort.cfiList.Add(new CharaFileInfo(string.Empty, string.Empty)
          {
            file = str,
            name = chaFileControl.parameter.fullname,
            time = System.IO.File.GetLastWriteTime(str)
          });
      }
    }

    private void OnSelect(TreeNodeObject _node)
    {
      if (Object.op_Equality((Object) _node, (Object) null) || !Singleton<Studio.Studio>.IsInstance())
        return;
      ObjectCtrlInfo objectCtrlInfo = (ObjectCtrlInfo) null;
      if (!Singleton<Studio.Studio>.Instance.dicInfo.TryGetValue(_node, out objectCtrlInfo))
        ((Selectable) this.buttonChange).set_interactable(false);
      else if (objectCtrlInfo.kind != 0)
        ((Selectable) this.buttonChange).set_interactable(false);
      else if (!(objectCtrlInfo is OCIChar ociChar) || ociChar.oiCharInfo.sex != this.sex)
      {
        ((Selectable) this.buttonChange).set_interactable(false);
      }
      else
      {
        if (this.charaFileSort.select == -1)
          return;
        ((Selectable) this.buttonChange).set_interactable(true);
      }
    }

    private void OnDeselect(TreeNodeObject _node)
    {
      if (Object.op_Equality((Object) _node, (Object) null) || !Singleton<Studio.Studio>.IsInstance())
        return;
      ((Selectable) this.buttonChange).set_interactable(!((IList<OCIChar>) ((IEnumerable<int>) Singleton<GuideObjectManager>.Instance.selectObjectKey).Select<int, OCIChar>((System.Func<int, OCIChar>) (v => Studio.Studio.GetCtrlInfo(v) as OCIChar)).Where<OCIChar>((System.Func<OCIChar, bool>) (v => v != null)).Where<OCIChar>((System.Func<OCIChar, bool>) (v => v.oiCharInfo.sex == this.sex)).ToArray<OCIChar>()).IsNullOrEmpty<OCIChar>());
    }

    private void OnDelete(ObjectCtrlInfo _info)
    {
      if (_info == null || _info.kind != 0 || (!(_info is OCIChar ociChar) || ociChar.oiCharInfo.sex != this.sex) || this.charaFileSort.select == -1)
        return;
      ((Selectable) this.buttonChange).set_interactable(false);
    }

    private void Awake()
    {
      this.isInit = false;
      this.InitCharaList(false);
      Singleton<Studio.Studio>.Instance.treeNodeCtrl.onSelect += new Action<TreeNodeObject>(this.OnSelect);
      Singleton<Studio.Studio>.Instance.onDelete += new Action<ObjectCtrlInfo>(this.OnDelete);
      Singleton<Studio.Studio>.Instance.treeNodeCtrl.onDeselect += new Action<TreeNodeObject>(this.OnDeselect);
    }
  }
}
