// Decompiled with JetBrains decompiler
// Type: ADV.EventCG.Data
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIChara;
using Cinemachine;
using Illusion.Extensions;
using Illusion.Game.Elements.EasyLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace ADV.EventCG
{
  public class Data : MonoBehaviour
  {
    public const string ParentNameCamera = "camPos";
    public const string ParentNameChara = "chaPos";
    public const string ParentNamePlayer = "playerPos";
    private Transform _camRoot;
    public Data.Scene[] scenes;
    private Transform _chaRoot;
    private Camera _targetCamera;
    private Dictionary<Transform, Tuple<Vector3, Quaternion>> backupDic;

    public Data()
    {
      base.\u002Ector();
    }

    public Transform camRoot
    {
      get
      {
        return this._camRoot;
      }
      set
      {
        if (((Component) this).get_transform().get_childCount() == 0)
          return;
        Transform child = ((Component) this).get_transform().GetChild(0);
        if (((Object) child).get_name() != "camPos")
          return;
        this._camRoot = value;
        this.backupDic[this._camRoot] = Tuple.Create<Vector3, Quaternion>(this._camRoot.get_position(), this._camRoot.get_rotation());
        this.cameraData = (CameraData) ((Component) child).GetComponent<CameraData>();
        ((Behaviour) this.cameraData).set_enabled(true);
        CinemachineVirtualCamera component1 = (CinemachineVirtualCamera) ((Component) this._camRoot).GetComponent<CinemachineVirtualCamera>();
        if (Object.op_Inequality((Object) component1, (Object) null))
        {
          CinemachineVirtualCamera cinemachineVirtualCamera = (CinemachineVirtualCamera) ((Component) child).get_gameObject().AddComponent<CinemachineVirtualCamera>();
          ((CinemachineVirtualCameraBase) cinemachineVirtualCamera).set_Priority(((CinemachineVirtualCameraBase) component1).get_Priority() + 1);
          (^(LensSettings&) ref cinemachineVirtualCamera.m_Lens).FieldOfView = (__Null) (double) this.cameraData.fieldOfView;
          ((CinemachineVirtualCameraBase) cinemachineVirtualCamera).set_LookAt((Transform) null);
          this.cameraData.SetCameraData((Component) component1);
          (^(LensSettings&) ref component1.m_Lens).FieldOfView = (__Null) (double) this.cameraData.fieldOfView;
          ((CinemachineVirtualCameraBase) component1).set_LookAt((Transform) null);
        }
        else
        {
          Camera component2 = (Camera) ((Component) this._camRoot).GetComponent<Camera>();
          this.cameraData.SetCameraData((Component) component2);
          component2.set_fieldOfView(this.cameraData.fieldOfView);
          this._camRoot.SetPositionAndRotation(child.get_position(), child.get_rotation());
          ObservableExtensions.Subscribe<Unit>(Observable.Take<Unit>(Observable.Where<Unit>(Observable.TakeUntilDestroy<Unit>((IObservable<M0>) ObservableTriggerExtensions.UpdateAsObservable((Component) child), (Component) this._camRoot), (Func<M0, bool>) (_ => this.cameraData.initialized)), 1), (Action<M0>) (_ => this._camRoot.SetPositionAndRotation(child.get_position(), child.get_rotation())));
        }
      }
    }

    public static bool IsCharaPos(Object child)
    {
      return child.get_name().IndexOf("chaPos") == 0;
    }

    public Dictionary<int, List<GameObject>> itemList { get; private set; }

    public void SetChaRoot(Transform root, Dictionary<int, CharaData> charaDataDic)
    {
      this._chaRoot = root;
      this.itemList = new Dictionary<int, List<GameObject>>();
      List<Transform> transformList1 = ((Component) this).get_transform().Children();
      List<Transform> transformList2 = transformList1;
      // ISSUE: reference to a compiler-generated field
      if (Data.\u003C\u003Ef__mg\u0024cache0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Data.\u003C\u003Ef__mg\u0024cache0 = new Func<Transform, bool>(Data.IsCharaPos);
      }
      // ISSUE: reference to a compiler-generated field
      Func<Transform, bool> fMgCache0 = Data.\u003C\u003Ef__mg\u0024cache0;
      // ISSUE: object of a compiler-generated type is created
      Dictionary<int, Transform> dictionary = ((IEnumerable<Transform>) transformList2).Where<Transform>(fMgCache0).Select<Transform, \u003C\u003E__AnonType1<Transform, int>>((Func<Transform, int, \u003C\u003E__AnonType1<Transform, int>>) ((t, i) => new \u003C\u003E__AnonType1<Transform, int>(t, i))).ToDictionary<\u003C\u003E__AnonType1<Transform, int>, int, Transform>((Func<\u003C\u003E__AnonType1<Transform, int>, int>) (v => v.i), (Func<\u003C\u003E__AnonType1<Transform, int>, Transform>) (v => v.t));
      Transform transform1 = transformList1.Find((Predicate<Transform>) (p => ((Object) p).get_name() == "playerPos"));
      int index = -1;
      if (Object.op_Inequality((Object) transform1, (Object) null))
        dictionary[index] = transform1;
      foreach (KeyValuePair<int, CharaData> keyValuePair in charaDataDic)
      {
        int key = !keyValuePair.Value.data.isHeroine ? index : keyValuePair.Key;
        Transform transform2;
        if (dictionary.TryGetValue(key, out transform2))
        {
          keyValuePair.Value.backup.Set();
          Transform transform3 = keyValuePair.Value.backup.transform;
          this.backupDic[transform3] = Tuple.Create<Vector3, Quaternion>(transform3.get_position(), transform3.get_rotation());
          transform3.SetPositionAndRotation(transform2.get_position(), transform2.get_rotation());
          this.itemList.Add(key, new List<GameObject>());
        }
      }
    }

    public Transform chaRoot
    {
      get
      {
        return this._chaRoot;
      }
    }

    public Camera targetCamera
    {
      get
      {
        return ((object) this).GetCacheObject<Camera>(ref this._targetCamera, new Func<Camera>(((Component) this).GetComponentInChildren<Camera>));
      }
    }

    public CameraData cameraData { get; private set; }

    public void Restore()
    {
      using (Dictionary<Transform, Tuple<Vector3, Quaternion>>.Enumerator enumerator = this.backupDic.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          KeyValuePair<Transform, Tuple<Vector3, Quaternion>> current = enumerator.Current;
          if (!Object.op_Equality((Object) current.Key, (Object) null))
          {
            if (Object.op_Equality((Object) current.Key, (Object) this._camRoot) && Object.op_Inequality((Object) this.cameraData, (Object) null))
              this.cameraData.RepairCameraData((Component) this._camRoot);
            current.Key.SetPositionAndRotation(current.Value.Item1, current.Value.Item2);
          }
        }
      }
    }

    public void ItemClear()
    {
      using (Dictionary<int, List<GameObject>>.ValueCollection.Enumerator enumerator = this.itemList.Values.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          List<GameObject> current = enumerator.Current;
          current.ForEach((Action<GameObject>) (item => Object.Destroy((Object) item)));
          current.Clear();
        }
      }
    }

    public List<Transform> GetCharaPosChildren
    {
      get
      {
        List<Transform> transformList = ((Component) this).get_transform().Children();
        if (Data.\u003C\u003Ef__mg\u0024cache1 == null)
          Data.\u003C\u003Ef__mg\u0024cache1 = new Func<Transform, bool>(Data.IsCharaPos);
        Func<Transform, bool> fMgCache1 = Data.\u003C\u003Ef__mg\u0024cache1;
        return ((IEnumerable<Transform>) transformList).Where<Transform>(fMgCache1).ToList<Transform>();
      }
    }

    public void Next(int index, Dictionary<int, CharaData> charaDataDic)
    {
      Data.Scene scene1 = this.scenes.SafeGet<Data.Scene>(index);
      if (scene1 == null)
        return;
      Data.Scene scene2 = this.scenes.SafeGet<Data.Scene>(index - 1);
      foreach (KeyValuePair<int, CharaData> keyValuePair in charaDataDic)
      {
        Data.Scene.Chara get = scene1.FindGet(keyValuePair.Key);
        if (get != null)
        {
          Data.Scene.Chara chara = (Data.Scene.Chara) null;
          MotionIK motionIK = (MotionIK) null;
          YureCtrlEx yureCtrl = (YureCtrlEx) null;
          bool flag = false;
          Data.Scene.Chara.MotionAndItem motionAndItem = get.motionAndItem;
          if (motionAndItem.ik.bundle.IsNullOrEmpty() && scene2 != null)
          {
            chara = scene2.FindGet(keyValuePair.Key);
            flag = true;
            if (chara != null)
              motionIK = chara.motionAndItem.ik.motionIK;
          }
          if (motionAndItem.yure.bundle.IsNullOrEmpty())
          {
            if (!flag && scene2 != null)
              chara = scene2.FindGet(keyValuePair.Key);
            if (chara != null)
              yureCtrl = chara.motionAndItem.yure.yureCtrl;
          }
          motionAndItem.ik.Create(keyValuePair.Value.chaCtrl, motionIK, (MotionIK[]) Array.Empty<MotionIK>());
          motionAndItem.yure.Create(keyValuePair.Value.chaCtrl, yureCtrl);
        }
      }
      foreach (KeyValuePair<int, CharaData> keyValuePair in charaDataDic)
        scene1.FindGet(keyValuePair.Key)?.Change(keyValuePair.Value.chaCtrl, this.itemList[keyValuePair.Key]);
    }

    [Serializable]
    public class Scene
    {
      public Data.Scene.Chara[] charas;

      public Data.Scene.Chara this[int index]
      {
        get
        {
          return this.charas.SafeGet<Data.Scene.Chara>(index);
        }
      }

      public Data.Scene.Chara FindGet(int no)
      {
        foreach (Data.Scene.Chara chara in this.charas)
        {
          if (chara.no == no)
            return chara;
        }
        return (Data.Scene.Chara) null;
      }

      [Serializable]
      public class Chara
      {
        public Data.Scene.Chara.MotionAndItem motionAndItem = new Data.Scene.Chara.MotionAndItem();
        public int no;

        public void Change(ChaControl chaCtrl, List<GameObject> itemList)
        {
          this.motionAndItem.Change(chaCtrl, itemList);
        }

        [Serializable]
        public class MotionAndItem
        {
          public Motion motion = new Motion();
          public IKMotion ik = new IKMotion();
          public YureMotion yure = new YureMotion();
          public Data.Scene.Chara.MotionAndItem.ItemSet[] items;
          public Data.Scene.Chara.MotionAndItem.ItemRemove[] removes;

          public void Change(ChaControl chaCtrl, List<GameObject> itemList)
          {
            Animator animBody = chaCtrl.animBody;
            if (this.motion.Setting(animBody))
            {
              this.motion.Play(animBody);
              this.ik.Setting(chaCtrl, this.motion.state);
              this.yure.Setting(chaCtrl, this.motion.state);
            }
            foreach (Data.Scene.Chara.MotionAndItem.ItemSet itemSet in this.items)
              itemSet.Execute(chaCtrl, itemList);
            foreach (Data.Scene.Chara.MotionAndItem.ItemRemove remove in this.removes)
              remove.Execute(chaCtrl, itemList);
          }

          [Serializable]
          public class ItemSet
          {
            public Illusion.Game.Elements.EasyLoader.Item.Data data = new Illusion.Game.Elements.EasyLoader.Item.Data();
            public string name;

            public void Execute(ChaControl chaCtrl, List<GameObject> itemList)
            {
              GameObject gameObject = this.data.Load(chaCtrl);
              ((Object) gameObject).set_name(this.name);
              itemList.Add(gameObject);
            }
          }

          [Serializable]
          public class ItemRemove
          {
            public string name;

            public bool Execute(ChaControl chaCtrl, List<GameObject> itemList)
            {
              int index = itemList.FindIndex((Predicate<GameObject>) (p => ((Object) p).get_name() == this.name));
              GameObject gameObject = itemList.SafeGet<GameObject>(index);
              if (Object.op_Equality((Object) gameObject, (Object) null))
              {
                Debug.LogError((object) (this.name + " to Remove Item Faild"));
                return false;
              }
              itemList.RemoveAt(index);
              Object.Destroy((Object) gameObject);
              return true;
            }
          }
        }
      }
    }
  }
}
