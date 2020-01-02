// Decompiled with JetBrains decompiler
// Type: SaveLoadUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SaveLoadUI : Singleton<SaveLoadUI>
{
  private List<GameObject> savedatas = new List<GameObject>();
  public const string savePath = "/in-house/Scripts/Game/Scene/Map/Craft/SaveData";
  public Button save;
  public Button load;
  public Button close;
  public Button dataLoad;
  public Button loadCancel;
  public Button loadEndOK;
  public Button saveEndOK;
  public Button prevPage;
  public Button nextPage;
  public Text NowPageCnt;
  public GameObject savedata;
  public GameObject saveloadPanel;
  public Transform loadPanel;
  public GameObject loadCheckPanel;
  public GameObject loadEnd;
  public GameObject saveEnd;
  public RectTransform saveDataArea;
  public int nSaveID;
  public List<string> saveFiles;
  private int nNowPageCnt;
  private int nMaxPageCnt;
  private int widthnum;
  private int heightnum;

  private void Start()
  {
    this.nNowPageCnt = 1;
    this.nMaxPageCnt = 1;
    this.NowPageCnt.set_text(string.Format("{0}", (object) this.nNowPageCnt));
    this.widthnum = 0;
    this.heightnum = 0;
    if (Object.op_Inequality((Object) this.load, (Object) null))
    {
      // ISSUE: method pointer
      ((UnityEvent) this.load.get_onClick()).AddListener(new UnityAction((object) this, __methodptr(LoadSetUp)));
    }
    if (Object.op_Inequality((Object) this.close, (Object) null))
    {
      // ISSUE: method pointer
      ((UnityEvent) this.close.get_onClick()).AddListener(new UnityAction((object) this, __methodptr(Close)));
    }
    if (Object.op_Inequality((Object) this.loadCancel, (Object) null))
    {
      // ISSUE: method pointer
      ((UnityEvent) this.loadCancel.get_onClick()).AddListener(new UnityAction((object) this, __methodptr(LoadCancel)));
    }
    if (Object.op_Inequality((Object) this.loadEndOK, (Object) null))
    {
      // ISSUE: method pointer
      ((UnityEvent) this.loadEndOK.get_onClick()).AddListener(new UnityAction((object) this, __methodptr(LoadEndOkDel)));
    }
    if (Object.op_Inequality((Object) this.saveEndOK, (Object) null))
    {
      // ISSUE: method pointer
      ((UnityEvent) this.saveEndOK.get_onClick()).AddListener(new UnityAction((object) this, __methodptr(SaveEndOkDel)));
    }
    if (Object.op_Inequality((Object) this.prevPage, (Object) null))
    {
      // ISSUE: method pointer
      ((UnityEvent) this.prevPage.get_onClick()).AddListener(new UnityAction((object) this, __methodptr(\u003CStart\u003Em__0)));
    }
    if (!Object.op_Inequality((Object) this.nextPage, (Object) null))
      return;
    // ISSUE: method pointer
    ((UnityEvent) this.nextPage.get_onClick()).AddListener(new UnityAction((object) this, __methodptr(\u003CStart\u003Em__1)));
  }

  private void LoadSetUp()
  {
    this.saveloadPanel.SetActive(true);
    this.nNowPageCnt = 1;
    this.NowPageCnt.set_text(string.Format("{0}", (object) this.nNowPageCnt));
    this.saveFiles = ((IEnumerable<string>) Directory.GetFiles(Application.get_dataPath() + "/in-house/Scripts/Game/Scene/Map/Craft/SaveData", "*.png")).ToList<string>();
    float x1 = (float) ((RectTransform) this.savedata.GetComponent<RectTransform>()).get_sizeDelta().x;
    float y1 = (float) ((RectTransform) this.savedata.GetComponent<RectTransform>()).get_sizeDelta().y;
    float x2 = (float) this.saveDataArea.get_sizeDelta().x;
    float y2 = (float) this.saveDataArea.get_sizeDelta().y;
    this.widthnum = Mathf.FloorToInt(x2 / x1);
    this.heightnum = Mathf.FloorToInt(y2 / y1);
    this.nMaxPageCnt = this.saveFiles.Count / (this.widthnum * this.heightnum);
    if (this.saveFiles.Count % (this.widthnum * this.heightnum) != 0)
      ++this.nMaxPageCnt;
    if (this.saveFiles.Count == 0)
      this.nMaxPageCnt = 1;
    for (int index = 0; index < this.saveFiles.Count; ++index)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      SaveLoadUI.\u003CLoadSetUp\u003Ec__AnonStorey0 setUpCAnonStorey0 = new SaveLoadUI.\u003CLoadSetUp\u003Ec__AnonStorey0();
      // ISSUE: reference to a compiler-generated field
      setUpCAnonStorey0.\u0024this = this;
      GameObject gameObject = (GameObject) Object.Instantiate<GameObject>((M0) this.savedata);
      gameObject.get_transform().SetParent(this.loadPanel, false);
      Vector3 localPosition = gameObject.get_transform().get_localPosition();
      ref Vector3 local1 = ref localPosition;
      local1.x = (__Null) (local1.x + (double) x1 * (double) (index % this.widthnum));
      ref Vector3 local2 = ref localPosition;
      local2.y = (__Null) (local2.y - (double) y1 * (double) (index / this.widthnum % this.heightnum));
      gameObject.get_transform().set_localPosition(localPosition);
      ((RawImage) gameObject.GetComponentInChildren<RawImage>()).set_texture((Texture) PngAssist.LoadTexture(this.saveFiles[index]));
      // ISSUE: reference to a compiler-generated field
      setUpCAnonStorey0.ID = index;
      // ISSUE: method pointer
      ((UnityEvent) ((Button) gameObject.GetComponent<Button>()).get_onClick()).AddListener(new UnityAction((object) setUpCAnonStorey0, __methodptr(\u003C\u003Em__0)));
      this.savedatas.Add(gameObject);
    }
    this.SaveDataChangeActive();
    ((Selectable) this.prevPage).set_interactable(false);
    ((Selectable) this.nextPage).set_interactable(true);
    if (this.nNowPageCnt != this.nMaxPageCnt)
      return;
    ((Selectable) this.nextPage).set_interactable(false);
  }

  private void loadCheckPanelSetUp(int saveId)
  {
    this.nSaveID = saveId;
    this.loadCheckPanel.SetActive(true);
  }

  private void Close()
  {
    this.saveloadPanel.SetActive(false);
    using (List<GameObject>.Enumerator enumerator = this.savedatas.GetEnumerator())
    {
      while (enumerator.MoveNext())
        Object.Destroy((Object) enumerator.Current);
    }
    this.savedatas.Clear();
  }

  private void LoadCancel()
  {
    this.loadCheckPanel.SetActive(false);
  }

  private void LoadEndOkDel()
  {
    this.loadEnd.SetActive(false);
    this.LoadCancel();
  }

  private void SaveEndOkDel()
  {
    this.saveEnd.SetActive(false);
  }

  private void ChangePage(int direction)
  {
    switch (direction)
    {
      case 0:
        --this.nNowPageCnt;
        if (this.nNowPageCnt <= 1)
        {
          ((Selectable) this.prevPage).set_interactable(false);
          this.nNowPageCnt = 1;
        }
        else if (!((Selectable) this.prevPage).get_interactable())
          ((Selectable) this.prevPage).set_interactable(true);
        ((Selectable) this.nextPage).set_interactable(true);
        break;
      case 1:
        ++this.nNowPageCnt;
        if (this.nNowPageCnt >= this.nMaxPageCnt)
        {
          ((Selectable) this.nextPage).set_interactable(false);
          this.nNowPageCnt = this.nMaxPageCnt;
        }
        else if (!((Selectable) this.nextPage).get_interactable())
          ((Selectable) this.nextPage).set_interactable(true);
        ((Selectable) this.prevPage).set_interactable(true);
        break;
    }
    this.NowPageCnt.set_text(string.Format("{0}", (object) this.nNowPageCnt));
    this.SaveDataChangeActive();
  }

  private void SaveDataChangeActive()
  {
    for (int index = 0; index < this.savedatas.Count; ++index)
    {
      if (index / this.widthnum / this.heightnum == this.nNowPageCnt - 1)
        this.savedatas[index].SetActive(true);
      else
        this.savedatas[index].SetActive(false);
    }
  }
}
