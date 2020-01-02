// Decompiled with JetBrains decompiler
// Type: AIProject.DebugUtil.MarkerOutput
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using AIProject.Animal;
using System.Collections.Generic;
using UnityEngine;

namespace AIProject.DebugUtil
{
  [RequireComponent(typeof (Canvas))]
  [AddComponentMenu("YK/Debug/MarkerOutput")]
  public class MarkerOutput : MonoBehaviour
  {
    private Canvas _canvas;
    [SerializeField]
    private GameObject _markerPrefab;
    private List<GameObject> _markers;
    private List<GameObject> _animalMarkers;
    private List<GameObject> _merchantMarkers;

    public MarkerOutput()
    {
      base.\u002Ector();
    }

    public Transform CameraTransform { get; set; }

    private void Awake()
    {
      this._canvas = (Canvas) ((Component) this).GetComponent<Canvas>();
      if (!Object.op_Equality((Object) this._canvas, (Object) null) && !Object.op_Equality((Object) this._markerPrefab, (Object) null))
        return;
      ((Behaviour) this).set_enabled(false);
    }

    private void Start()
    {
    }

    private void OnUpdate()
    {
      if (Object.op_Equality((Object) this.CameraTransform, (Object) null))
        return;
      int num1 = AgentMarker.AgentMarkerTable.Count - this._markers.Count;
      if (num1 > 0)
      {
        for (int index = 0; index < num1; ++index)
        {
          GameObject gameObject = (GameObject) Object.Instantiate<GameObject>((M0) this._markerPrefab);
          gameObject.get_transform().SetParent(((Component) this._canvas).get_transform(), false);
          this._markers.Add(gameObject);
        }
      }
      else if (num1 < 0)
      {
        for (int index = 0; index < this._markers.Count; ++index)
          this._markers[index].SetActive(index < this._markers.Count - num1);
      }
      int num2 = 0;
      foreach (int key in AgentMarker.Keys)
      {
        AgentActor agentActor;
        if (AgentMarker.AgentMarkerTable.TryGetValue(key, out agentActor))
        {
          Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(Camera.get_main(), agentActor.Position);
          GameObject marker = this._markers[num2++];
          marker.get_transform().set_position(Vector2.op_Implicit(screenPoint));
          float num3 = Quaternion.Angle(Quaternion.LookRotation(Vector3.op_Subtraction(agentActor.Position, this.CameraTransform.get_position())), this.CameraTransform.get_rotation());
          marker.SetActive((double) num3 < 60.0);
        }
      }
      this.OnUpdateAnimalMarker();
      this.OnUpdateMerchantMarker();
    }

    private void OnUpdateAnimalMarker()
    {
      if (Object.op_Equality((Object) this.CameraTransform, (Object) null))
        return;
      int num1 = AnimalMarker.AnimalMarkerTable.Count - this._animalMarkers.Count;
      if (num1 > 0)
      {
        for (int index = 0; index < num1; ++index)
        {
          GameObject gameObject = (GameObject) Object.Instantiate<GameObject>((M0) this._markerPrefab);
          gameObject.get_transform().SetParent(((Component) this._canvas).get_transform(), false);
          this._animalMarkers.Add(gameObject);
        }
      }
      else if (num1 < 0)
      {
        for (int index = 0; index < this._animalMarkers.Count; ++index)
          this._animalMarkers[index].SetActive(index < this._animalMarkers.Count + num1);
      }
      int num2 = 0;
      foreach (int key in AnimalMarker.Keys)
      {
        AnimalBase animalBase;
        if (AnimalMarker.AnimalMarkerTable.TryGetValue(key, out animalBase))
        {
          Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(Camera.get_main(), animalBase.Position);
          GameObject animalMarker = this._animalMarkers[num2++];
          animalMarker.get_transform().set_position(Vector2.op_Implicit(screenPoint));
          float num3 = Quaternion.Angle(Quaternion.LookRotation(Vector3.op_Subtraction(animalBase.Position, this.CameraTransform.get_position())), this.CameraTransform.get_rotation());
          animalMarker.SetActive((double) num3 < 60.0);
        }
      }
    }

    private void OnUpdateMerchantMarker()
    {
      if (Object.op_Equality((Object) this.CameraTransform, (Object) null))
        return;
      int num1 = MerchantMarker.MerchantMarkerTable.Count - this._merchantMarkers.Count;
      if (0 < num1)
      {
        for (int index = 0; index < num1; ++index)
        {
          GameObject gameObject = (GameObject) Object.Instantiate<GameObject>((M0) this._markerPrefab);
          gameObject.get_transform().SetParent(((Component) this._canvas).get_transform(), false);
          this._merchantMarkers.Add(gameObject);
        }
      }
      else if (num1 < 0)
      {
        for (int index = 0; index < this._merchantMarkers.Count; ++index)
          this._merchantMarkers[index].SetActive(index < this._merchantMarkers.Count + num1);
      }
      int num2 = 0;
      foreach (int key in MerchantMarker.Keys)
      {
        MerchantActor merchantActor = (MerchantActor) null;
        if (MerchantMarker.MerchantMarkerTable.TryGetValue(key, out merchantActor))
        {
          Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(Camera.get_main(), merchantActor.Position);
          GameObject merchantMarker = this._merchantMarkers[num2++];
          merchantMarker.get_transform().set_position(Vector2.op_Implicit(screenPoint));
          float num3 = Quaternion.Angle(Quaternion.LookRotation(Vector3.op_Subtraction(merchantActor.Position, this.CameraTransform.get_position())), this.CameraTransform.get_rotation());
          merchantMarker.SetActive((double) num3 < 60.0);
        }
      }
    }
  }
}
