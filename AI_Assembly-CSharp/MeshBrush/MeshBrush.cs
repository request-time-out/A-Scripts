// Decompiled with JetBrains decompiler
// Type: MeshBrush.MeshBrush
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using UnityEngine;

namespace MeshBrush
{
  public class MeshBrush : MonoBehaviour
  {
    public const float version = 1.9f;
    public bool active;
    public string groupName;
    public bool[] layerMask;
    public float radius;
    public Color color;
    public Vector2 quantityRange;
    public bool useDensity;
    public Vector2 densityRange;
    public float delay;
    public Vector2 offsetRange;
    public Vector2 slopeInfluenceRange;
    public bool useSlopeFilter;
    public Vector2 angleThresholdRange;
    public bool inverseSlopeFilter;
    public Vector3 slopeReferenceVector;
    public Vector3 slopeReferenceVectorSampleLocation;
    public bool yAxisTangent;
    public bool strokeAlignment;
    public bool autoIgnoreRaycast;
    public Vector2 scatteringRange;
    public bool useOverlapFilter;
    public Vector2 minimumAbsoluteDistanceRange;
    public bool uniformRandomScale;
    public bool uniformAdditiveScale;
    public Vector2 randomScaleRange;
    public Vector2 randomScaleRangeX;
    public Vector2 randomScaleRangeY;
    public Vector2 randomScaleRangeZ;
    public Vector2 additiveScaleRange;
    public Vector3 additiveScaleNonUniform;
    public AnimationCurve randomScaleCurve;
    public float randomScaleCurveVariation;
    public Vector2 randomRotationRange;
    public bool positionBrushRandomizer;
    public bool rotationBrushRandomizer;
    public bool scaleBrushRandomizer;
    public KeyCode paintKey;
    public KeyCode deleteKey;
    public KeyCode combineKey;
    public KeyCode randomizeKey;
    public KeyCode increaseRadiusKey;
    public KeyCode decreaseRadiusKey;
    [SerializeField]
    private int maxQuantityLimit;
    [SerializeField]
    private float maxDelayLimit;
    [SerializeField]
    private float maxDensityLimit;
    [SerializeField]
    private float minOffsetLimit;
    [SerializeField]
    private float maxOffsetLimit;
    [SerializeField]
    private float maxMinimumAbsoluteDistanceLimit;
    [SerializeField]
    private float maxAdditiveScaleLimit;
    [SerializeField]
    private float maxRandomScaleLimit;
    public bool helpFoldout;
    public bool helpTemplatesFoldout;
    public bool helpGeneralUsageFoldout;
    public bool helpOptimizationFoldout;
    public bool meshesFoldout;
    public bool templatesFoldout;
    public bool keyBindingsFoldout;
    public bool brushFoldout;
    public bool slopesFoldout;
    public bool randomizersFoldout;
    public bool overlapFilterFoldout;
    public bool additiveScaleFoldout;
    public bool optimizationFoldout;
    [SerializeField]
    private bool globalPaintingMode;
    public bool collapsed;
    public bool stats;
    public bool lockSceneView;
    public bool classicUI;
    public float previewIconSize;
    public bool manualReferenceVectorSampling;
    public bool showReferenceVectorInSceneView;
    public bool autoStatic;
    public bool autoSelectOnCombine;
    private Transform cachedTransform;
    private Collider cachedCollider;
    private GameObject brush;
    private Transform brushTransform;
    private Transform holderObj;
    private const string minString = "min";
    private const string maxString = "max";
    private const string trueString = "true";
    private const string falseString = "false";
    private const string enabledString = "enabled";
    public Vector3 lastPaintLocation;
    public Vector3 brushStrokeDirection;
    [SerializeField]
    private List<GameObject> meshes;
    private List<Transform> paintedMeshes;
    private List<Transform> paintedMeshesInsideBrushArea;
    private float nextFeasibleStrokeTime;

    public MeshBrush()
    {
      List<GameObject> gameObjectList = new List<GameObject>(5);
      gameObjectList.Add((GameObject) null);
      this.meshes = gameObjectList;
      this.paintedMeshes = new List<Transform>(200);
      this.paintedMeshesInsideBrushArea = new List<Transform>(50);
      base.\u002Ector();
    }

    public Transform CachedTransform
    {
      get
      {
        if (Object.op_Equality((Object) this.cachedTransform, (Object) null))
          this.cachedTransform = ((Component) this).get_transform();
        return this.cachedTransform;
      }
    }

    public Collider CachedCollider
    {
      get
      {
        if (Object.op_Equality((Object) this.cachedCollider, (Object) null))
          this.cachedCollider = (Collider) ((Component) this).GetComponent<Collider>();
        return this.cachedCollider;
      }
    }

    public GameObject Brush
    {
      get
      {
        this.CheckBrush();
        return this.brush;
      }
    }

    public Transform BrushTransform
    {
      get
      {
        this.CheckBrush();
        return this.brushTransform;
      }
    }

    public Transform HolderObj
    {
      get
      {
        this.CheckHolder();
        return this.holderObj;
      }
    }

    public void OnValidate()
    {
      this.ValidateKeyBindings();
      this.ValidateRangeLimits();
      if (this.meshes.Count == 0)
        this.meshes.Add((GameObject) null);
      if (this.layerMask.Length != 32)
      {
        this.layerMask = new bool[32];
        for (int index = this.layerMask.Length - 1; index >= 0; --index)
          this.layerMask[index] = true;
      }
      if (this.layerMask[2])
        this.layerMask[2] = false;
      if ((double) this.radius < 0.00999999977648258)
        this.radius = 0.01f;
      this.radius = (float) Math.Round((double) this.radius, 3);
      VectorClampingUtility.ClampVector(ref this.quantityRange, 1f, (float) this.maxQuantityLimit, 1f, (float) this.maxQuantityLimit);
      VectorClampingUtility.ClampVector(ref this.densityRange, 0.1f, this.maxDensityLimit, 0.1f, this.maxDensityLimit);
      this.delay = Mathf.Clamp(this.delay, 0.03f, this.maxDelayLimit);
      this.randomScaleCurveVariation = Mathf.Clamp(this.randomScaleCurveVariation, 0.0f, 3f);
      VectorClampingUtility.ClampVector(ref this.offsetRange, this.minOffsetLimit, this.maxOffsetLimit, this.minOffsetLimit, this.maxOffsetLimit);
      VectorClampingUtility.ClampVector(ref this.scatteringRange, 0.0f, 100f, 0.0f, 100f);
      VectorClampingUtility.ClampVector(ref this.slopeInfluenceRange, 0.0f, 100f, 0.0f, 100f);
      VectorClampingUtility.ClampVector(ref this.angleThresholdRange, 1f, 180f, 1f, 180f);
      VectorClampingUtility.ClampVector(ref this.minimumAbsoluteDistanceRange, 0.0f, this.maxMinimumAbsoluteDistanceLimit, 0.0f, this.maxMinimumAbsoluteDistanceLimit);
      VectorClampingUtility.ClampVector(ref this.randomScaleRange, 0.01f, this.maxRandomScaleLimit, 0.0f, this.maxRandomScaleLimit);
      VectorClampingUtility.ClampVector(ref this.randomScaleRangeX, 0.01f, this.maxRandomScaleLimit, 0.0f, this.maxRandomScaleLimit);
      VectorClampingUtility.ClampVector(ref this.randomScaleRangeY, 0.01f, this.maxRandomScaleLimit, 0.0f, this.maxRandomScaleLimit);
      VectorClampingUtility.ClampVector(ref this.randomScaleRangeZ, 0.01f, this.maxRandomScaleLimit, 0.0f, this.maxRandomScaleLimit);
      VectorClampingUtility.ClampVector(ref this.randomRotationRange, 0.0f, 100f, 0.0f, 100f);
      VectorClampingUtility.ClampVector(ref this.additiveScaleRange, -0.9f, this.maxAdditiveScaleLimit, -0.9f, this.maxAdditiveScaleLimit);
      VectorClampingUtility.ClampVector(ref this.additiveScaleNonUniform, -0.9f, this.maxAdditiveScaleLimit, -0.9f, this.maxAdditiveScaleLimit, -0.9f, this.maxAdditiveScaleLimit);
    }

    private void ValidateRangeLimits()
    {
      this.maxQuantityLimit = Mathf.Clamp(this.maxQuantityLimit, 1, 1000);
      this.maxDensityLimit = Mathf.Clamp(this.maxDensityLimit, 1f, 1000f);
      this.maxDelayLimit = Mathf.Clamp(this.maxDelayLimit, 1f, 10f);
      this.minOffsetLimit = Mathf.Clamp(this.minOffsetLimit, -1000f, -1f);
      this.maxOffsetLimit = Mathf.Clamp(this.maxOffsetLimit, 1f, 1000f);
      this.maxMinimumAbsoluteDistanceLimit = Mathf.Clamp(this.maxMinimumAbsoluteDistanceLimit, 3f, 1000f);
      this.maxAdditiveScaleLimit = Mathf.Clamp(this.maxAdditiveScaleLimit, 3f, 1000f);
      this.maxRandomScaleLimit = Mathf.Clamp(this.maxRandomScaleLimit, 3f, 1000f);
    }

    private void ValidateKeyBindings()
    {
      if (this.paintKey == null)
        this.paintKey = (KeyCode) 112;
      if (this.deleteKey == null)
        this.deleteKey = (KeyCode) 108;
      if (this.randomizeKey == null)
        this.randomizeKey = (KeyCode) 106;
      if (this.combineKey == null)
        this.combineKey = (KeyCode) 107;
      if (this.increaseRadiusKey == null)
        this.increaseRadiusKey = (KeyCode) 111;
      if (this.decreaseRadiusKey != null)
        return;
      this.decreaseRadiusKey = (KeyCode) 105;
    }

    public void GatherPaintedMeshes()
    {
      this.paintedMeshes = ((IEnumerable<Transform>) ((Component) this.HolderObj).GetComponentsInChildren<Transform>()).ToList<Transform>();
    }

    public void CleanSetOfMeshesToPaint()
    {
      if (this.meshes.Count <= 1)
        return;
      for (int index = this.meshes.Count - 1; index >= 0; --index)
      {
        if (Object.op_Equality((Object) this.meshes[index], (Object) null))
          this.meshes.RemoveAt(index);
      }
      if (this.meshes.Count != 0)
        return;
      this.meshes.Add((GameObject) null);
    }

    private void GatherMeshesInsideBrushArea(RaycastHit brushLocation)
    {
      this.paintedMeshesInsideBrushArea.Clear();
      using (List<Transform>.Enumerator enumerator = this.paintedMeshes.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          Transform current = enumerator.Current;
          if (Object.op_Inequality((Object) current, (Object) null) && Object.op_Inequality((Object) current, (Object) this.BrushTransform) && (Object.op_Inequality((Object) current, (Object) this.HolderObj) && (double) Vector3.Distance(((RaycastHit) ref brushLocation).get_point(), current.get_position()) < (double) this.radius))
            this.paintedMeshesInsideBrushArea.Add(current);
        }
      }
    }

    public void PaintMeshes(RaycastHit brushLocation)
    {
      if ((double) this.nextFeasibleStrokeTime >= (double) Time.get_realtimeSinceStartup())
        return;
      this.nextFeasibleStrokeTime = Time.get_realtimeSinceStartup() + this.delay;
      this.CheckBrush();
      this.brushStrokeDirection = Vector3.op_Subtraction(((RaycastHit) ref brushLocation).get_point(), this.lastPaintLocation);
      int num1 = !this.useDensity ? (int) Random.Range((float) this.quantityRange.x, (float) (this.quantityRange.y + 1.0)) : (int) ((double) this.radius * (double) this.radius * 3.14159274101257 * (double) Random.Range((float) this.densityRange.x, (float) this.densityRange.y));
      if (num1 <= 0)
        num1 = 1;
      if (this.useOverlapFilter)
        this.GatherMeshesInsideBrushArea(brushLocation);
      bool flag = false;
      for (int index = num1; index > 0; --index)
      {
        float num2 = this.radius * 0.01f * Random.Range((float) this.scatteringRange.x, (float) this.scatteringRange.y);
        this.brushTransform.set_position(Vector3.op_Addition(((RaycastHit) ref brushLocation).get_point(), Vector3.op_Multiply(((RaycastHit) ref brushLocation).get_normal(), 0.5f)));
        this.brushTransform.set_rotation(Quaternion.LookRotation(((RaycastHit) ref brushLocation).get_normal()));
        this.brushTransform.set_up(this.brushTransform.get_forward());
        if (num1 > 1)
          this.brushTransform.Translate(Random.Range((float) -Random.get_insideUnitCircle().x * num2, (float) Random.get_insideUnitCircle().x * num2), 0.0f, Random.Range((float) -Random.get_insideUnitCircle().y * num2, (float) Random.get_insideUnitCircle().y * num2), (Space) 1);
        RaycastHit targetLocation;
        if ((!this.globalPaintingMode ? (this.CachedCollider.Raycast(new Ray(this.brushTransform.get_position(), Vector3.op_UnaryNegation(((RaycastHit) ref brushLocation).get_normal())), ref targetLocation, 2.5f) ? 1 : 0) : (Physics.Raycast(new Ray(this.brushTransform.get_position(), Vector3.op_UnaryNegation(((RaycastHit) ref brushLocation).get_normal())), ref targetLocation, 2.5f) ? 1 : 0)) != 0)
        {
          float num3 = !this.useSlopeFilter ? (!this.inverseSlopeFilter ? 0.0f : 180f) : Vector3.Angle(((RaycastHit) ref targetLocation).get_normal(), !this.manualReferenceVectorSampling ? Vector3.get_up() : this.slopeReferenceVector);
          if ((!this.inverseSlopeFilter ? ((double) num3 < (double) Random.Range((float) this.angleThresholdRange.x, (float) this.angleThresholdRange.y) ? 1 : 0) : ((double) num3 > (double) Random.Range((float) this.angleThresholdRange.x, (float) this.angleThresholdRange.y) ? 1 : 0)) != 0 && (!this.useOverlapFilter || !this.CheckOverlap(((RaycastHit) ref targetLocation).get_point())))
          {
            GameObject gameObject1 = (GameObject) Object.Instantiate<GameObject>((M0) this.meshes[Random.Range(0, this.meshes.Count)]);
            if (Object.op_Equality((Object) gameObject1, (Object) null))
            {
              if (!flag)
              {
                flag = true;
                Debug.LogError((object) "MeshBrush: one or more fields in the set of meshes to paint is null. Please assign all fields before painting (or remove empty ones).");
              }
            }
            else
            {
              if (this.autoIgnoreRaycast)
                gameObject1.set_layer(2);
              Transform transform1 = gameObject1.get_transform();
              this.OrientPaintedMesh(transform1, targetLocation);
              if ((double) Mathf.Abs((float) this.offsetRange.x) > 1.40129846432482E-45 || (double) Mathf.Abs((float) this.offsetRange.y) > 1.40129846432482E-45)
                MeshTransformationUtility.ApplyMeshOffset(transform1, Random.Range((float) this.offsetRange.x, (float) this.offsetRange.y), ((RaycastHit) ref brushLocation).get_normal());
              if (this.uniformRandomScale)
              {
                if ((double) Mathf.Abs((float) (this.randomScaleRange.x - 1.0)) > 1.40129846432482E-45 || (double) Mathf.Abs((float) (this.randomScaleRange.y - 1.0)) > 1.40129846432482E-45)
                  MeshTransformationUtility.ApplyRandomScale(transform1, this.randomScaleRange);
              }
              else if ((double) Mathf.Abs((float) (this.randomScaleRangeX.x - 1.0)) > 1.40129846432482E-45 || (double) Mathf.Abs((float) (this.randomScaleRangeX.y - 1.0)) > 1.40129846432482E-45 || ((double) Mathf.Abs((float) (this.randomScaleRangeY.x - 1.0)) > 1.40129846432482E-45 || (double) Mathf.Abs((float) (this.randomScaleRangeY.y - 1.0)) > 1.40129846432482E-45) || ((double) Mathf.Abs((float) (this.randomScaleRangeZ.x - 1.0)) > 1.40129846432482E-45 || (double) Mathf.Abs((float) (this.randomScaleRangeZ.y - 1.0)) > 1.40129846432482E-45))
                MeshTransformationUtility.ApplyRandomScale(transform1, this.randomScaleRangeX, this.randomScaleRangeY, this.randomScaleRangeZ);
              Transform transform2 = transform1;
              transform2.set_localScale(Vector3.op_Multiply(transform2.get_localScale(), Mathf.Abs(this.randomScaleCurve.Evaluate(Vector3.Distance(transform1.get_position(), ((RaycastHit) ref brushLocation).get_point()) / this.radius) + Random.Range(-this.randomScaleCurveVariation, this.randomScaleCurveVariation))));
              if (this.uniformAdditiveScale)
              {
                if ((double) Mathf.Abs((float) this.additiveScaleRange.x) > 1.40129846432482E-45 || (double) Mathf.Abs((float) this.additiveScaleRange.y) > 1.40129846432482E-45)
                  MeshTransformationUtility.AddConstantScale(transform1, this.additiveScaleRange);
              }
              else if ((double) Mathf.Abs((float) this.additiveScaleNonUniform.x) > 1.40129846432482E-45 || (double) Mathf.Abs((float) this.additiveScaleNonUniform.y) > 1.40129846432482E-45 || (double) Mathf.Abs((float) this.additiveScaleNonUniform.z) > 1.40129846432482E-45)
                MeshTransformationUtility.AddConstantScale(transform1, (float) this.additiveScaleNonUniform.x, (float) this.additiveScaleNonUniform.y, (float) this.additiveScaleNonUniform.z);
              if (this.randomRotationRange.x > 0.0 || this.randomRotationRange.y > 0.0)
                MeshTransformationUtility.ApplyRandomRotation(transform1, Random.Range((float) this.randomRotationRange.x, (float) this.randomRotationRange.y));
              transform1.set_parent(this.HolderObj);
              GameObject gameObject2 = gameObject1;
              gameObject2.set_isStatic(gameObject2.get_isStatic() | this.autoStatic);
              this.paintedMeshes.Add(transform1);
            }
          }
        }
      }
      this.lastPaintLocation = ((RaycastHit) ref brushLocation).get_point();
    }

    public void RandomizeMeshes(RaycastHit brushLocation)
    {
      if ((double) this.nextFeasibleStrokeTime >= (double) Time.get_realtimeSinceStartup())
        return;
      this.nextFeasibleStrokeTime = Time.get_realtimeSinceStartup() + this.delay;
      this.GatherMeshesInsideBrushArea(brushLocation);
      for (int index = this.paintedMeshesInsideBrushArea.Count - 1; index >= 0; --index)
      {
        Transform targetTransform = this.paintedMeshesInsideBrushArea[index];
        if (Object.op_Inequality((Object) targetTransform, (Object) null))
        {
          if (this.positionBrushRandomizer)
          {
            float num = this.radius * 0.01f * Random.Range((float) this.scatteringRange.x, (float) this.scatteringRange.y);
            this.brushTransform.set_position(Vector3.op_Addition(((RaycastHit) ref brushLocation).get_point(), Vector3.op_Multiply(((RaycastHit) ref brushLocation).get_normal(), 0.5f)));
            this.brushTransform.set_rotation(Quaternion.LookRotation(((RaycastHit) ref brushLocation).get_normal()));
            this.brushTransform.set_up(this.brushTransform.get_forward());
            this.brushTransform.Translate(Random.Range((float) -Random.get_insideUnitCircle().x * num, (float) Random.get_insideUnitCircle().x * num), 0.0f, Random.Range((float) -Random.get_insideUnitCircle().y * num, (float) Random.get_insideUnitCircle().y * num), (Space) 1);
            RaycastHit targetLocation;
            if ((!this.globalPaintingMode ? (this.CachedCollider.Raycast(new Ray(this.brushTransform.get_position(), Vector3.op_UnaryNegation(((RaycastHit) ref brushLocation).get_normal())), ref targetLocation, 2.5f) ? 1 : 0) : (Physics.Raycast(new Ray(this.brushTransform.get_position(), Vector3.op_UnaryNegation(((RaycastHit) ref brushLocation).get_normal())), ref targetLocation, 2.5f) ? 1 : 0)) != 0)
              this.OrientPaintedMesh(targetTransform, targetLocation);
            if ((double) Mathf.Abs((float) this.offsetRange.x) > 1.40129846432482E-45 || (double) Mathf.Abs((float) this.offsetRange.y) > 1.40129846432482E-45)
              MeshTransformationUtility.ApplyMeshOffset(targetTransform, Random.Range((float) this.offsetRange.x, (float) this.offsetRange.y), ((RaycastHit) ref brushLocation).get_normal());
          }
          if (this.rotationBrushRandomizer && (this.randomRotationRange.x > 0.0 || this.randomRotationRange.y > 0.0))
            MeshTransformationUtility.ApplyRandomRotation(targetTransform, Random.Range((float) this.randomRotationRange.x, (float) this.randomRotationRange.y));
          if (this.scaleBrushRandomizer)
          {
            if (this.uniformRandomScale)
            {
              if ((double) Mathf.Abs((float) (this.randomScaleRange.x - 1.0)) > 1.40129846432482E-45 || (double) Mathf.Abs((float) (this.randomScaleRange.y - 1.0)) > 1.40129846432482E-45)
                MeshTransformationUtility.ApplyRandomScale(targetTransform, this.randomScaleRange);
            }
            else if ((double) Mathf.Abs((float) (this.randomScaleRangeX.x - 1.0)) > 1.40129846432482E-45 || (double) Mathf.Abs((float) (this.randomScaleRangeX.y - 1.0)) > 1.40129846432482E-45 || ((double) Mathf.Abs((float) (this.randomScaleRangeY.x - 1.0)) > 1.40129846432482E-45 || (double) Mathf.Abs((float) (this.randomScaleRangeY.y - 1.0)) > 1.40129846432482E-45) || ((double) Mathf.Abs((float) (this.randomScaleRangeZ.x - 1.0)) > 1.40129846432482E-45 || (double) Mathf.Abs((float) (this.randomScaleRangeZ.y - 1.0)) > 1.40129846432482E-45))
              MeshTransformationUtility.ApplyRandomScale(targetTransform, this.randomScaleRangeX, this.randomScaleRangeY, this.randomScaleRangeZ);
            Transform transform = targetTransform;
            transform.set_localScale(Vector3.op_Multiply(transform.get_localScale(), Mathf.Abs(this.randomScaleCurve.Evaluate(Vector3.Distance(targetTransform.get_position(), ((RaycastHit) ref brushLocation).get_point()) / this.radius) + Random.Range(-this.randomScaleCurveVariation, this.randomScaleCurveVariation))));
          }
        }
      }
    }

    public void DeleteMeshes(RaycastHit brushLocation)
    {
      if ((double) this.nextFeasibleStrokeTime >= (double) Time.get_realtimeSinceStartup())
        return;
      this.nextFeasibleStrokeTime = Time.get_realtimeSinceStartup() + this.delay;
      this.GatherMeshesInsideBrushArea(brushLocation);
      for (int index = this.paintedMeshesInsideBrushArea.Count - 1; index >= 0; --index)
      {
        this.paintedMeshes.Remove(this.paintedMeshesInsideBrushArea[index]);
        Object.Destroy((Object) ((Component) this.paintedMeshesInsideBrushArea[index]).get_gameObject());
      }
    }

    public void CombineMeshes(RaycastHit brushLocation)
    {
      if ((double) this.nextFeasibleStrokeTime >= (double) Time.get_realtimeSinceStartup())
        return;
      this.nextFeasibleStrokeTime = Time.get_realtimeSinceStartup() + this.delay;
      this.GatherMeshesInsideBrushArea(brushLocation);
      if (this.paintedMeshesInsideBrushArea.Count <= 0)
        return;
      ((MeshBrushParent) ((Component) this.HolderObj).GetComponent<MeshBrushParent>()).CombinePaintedMeshes(this.autoSelectOnCombine, ((IEnumerable<Transform>) this.paintedMeshesInsideBrushArea).Select<Transform, MeshFilter>((Func<Transform, MeshFilter>) (mesh => (MeshFilter) ((Component) mesh).GetComponent<MeshFilter>())).ToArray<MeshFilter>());
    }

    public void SampleReferenceVector(Vector3 referenceVector, Vector3 sampleLocation)
    {
      this.slopeReferenceVector = referenceVector;
      this.slopeReferenceVectorSampleLocation = sampleLocation;
    }

    private void OrientPaintedMesh(Transform targetTransform, RaycastHit targetLocation)
    {
      targetTransform.set_position(((RaycastHit) ref targetLocation).get_point());
      targetTransform.set_rotation(Quaternion.LookRotation(((RaycastHit) ref targetLocation).get_normal()));
      Vector3 vector3_1 = Vector3.Lerp(!this.yAxisTangent ? Vector3.get_up() : targetTransform.get_up(), targetTransform.get_forward(), Random.Range((float) this.slopeInfluenceRange.x, (float) this.slopeInfluenceRange.y) * 0.01f);
      Vector3 vector3_2 = !this.strokeAlignment || !Vector3.op_Inequality(this.brushStrokeDirection, Vector3.get_zero()) || !Vector3.op_Inequality(this.lastPaintLocation, Vector3.get_zero()) ? targetTransform.get_forward() : this.brushStrokeDirection;
      Vector3.OrthoNormalize(ref vector3_1, ref vector3_2);
      targetTransform.set_rotation(Quaternion.LookRotation(vector3_2, vector3_1));
    }

    private bool CheckOverlap(Vector3 objPos)
    {
      if (this.paintedMeshes == null || this.paintedMeshes.Count < 1)
        return false;
      using (List<Transform>.Enumerator enumerator = this.paintedMeshes.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          Transform current = enumerator.Current;
          if (Object.op_Inequality((Object) current, (Object) null) && Object.op_Inequality((Object) current, (Object) this.BrushTransform) && (Object.op_Inequality((Object) current, (Object) this.HolderObj) && (double) Vector3.Distance(objPos, current.get_position()) < (double) Random.Range((float) this.minimumAbsoluteDistanceRange.x, (float) this.minimumAbsoluteDistanceRange.y)))
            return true;
        }
      }
      return false;
    }

    private void CheckHolder()
    {
      MeshBrushParent[] componentsInChildren = (MeshBrushParent[]) ((Component) this).GetComponentsInChildren<MeshBrushParent>();
      if (componentsInChildren.Length > 0)
      {
        this.holderObj = (Transform) null;
        for (int index = 0; index < componentsInChildren.Length; ++index)
        {
          if (Object.op_Inequality((Object) componentsInChildren[index], (Object) null) && string.CompareOrdinal(((Object) componentsInChildren[index]).get_name(), this.groupName) == 0)
            this.holderObj = ((Component) componentsInChildren[index]).get_transform();
        }
        if (!Object.op_Equality((Object) this.holderObj, (Object) null))
          return;
        this.CreateHolder();
      }
      else
        this.CreateHolder();
    }

    private void CheckBrush()
    {
      this.CheckHolder();
      this.brushTransform = this.holderObj.Find("Brush");
      if (!Object.op_Equality((Object) this.brushTransform, (Object) null))
        return;
      this.CreateBrush();
    }

    private void CreateHolder()
    {
      GameObject gameObject = new GameObject(this.groupName);
      gameObject.AddComponent<MeshBrushParent>();
      gameObject.get_transform().set_rotation(this.CachedTransform.get_rotation());
      gameObject.get_transform().set_parent(this.CachedTransform);
      gameObject.get_transform().set_localPosition(Vector3.get_zero());
      this.holderObj = gameObject.get_transform();
    }

    private void CreateBrush()
    {
      this.brush = new GameObject("Brush");
      this.brushTransform = this.brush.get_transform();
      this.brushTransform.set_position(this.CachedTransform.get_position());
      this.brushTransform.set_parent(this.holderObj);
    }

    public void ResetKeyBindings()
    {
      this.paintKey = (KeyCode) 112;
      this.deleteKey = (KeyCode) 108;
      this.combineKey = (KeyCode) 107;
      this.randomizeKey = (KeyCode) 106;
      this.increaseRadiusKey = (KeyCode) 111;
      this.decreaseRadiusKey = (KeyCode) 105;
    }

    public void ResetSlopeSettings()
    {
      this.slopeInfluenceRange = new Vector2(95f, 100f);
      this.angleThresholdRange = new Vector2(25f, 30f);
      this.useSlopeFilter = false;
      this.inverseSlopeFilter = false;
      this.manualReferenceVectorSampling = false;
      this.showReferenceVectorInSceneView = true;
    }

    public void ResetRandomizers()
    {
      this.randomScaleRange = Vector2.get_one();
      this.randomScaleRangeX = this.randomScaleRangeY = this.randomScaleRangeZ = Vector2.get_one();
      this.randomScaleCurve = AnimationCurve.Linear(0.0f, 1f, 1f, 1f);
      this.randomScaleCurveVariation = 0.0f;
      this.randomRotationRange = new Vector2(0.0f, 5f);
      this.positionBrushRandomizer = false;
      this.rotationBrushRandomizer = true;
      this.scaleBrushRandomizer = true;
    }

    public void ResetAdditiveScale()
    {
      this.uniformRandomScale = true;
      this.additiveScaleRange = Vector2.get_zero();
      this.additiveScaleNonUniform = Vector3.get_zero();
    }

    public void ResetOverlapFilterSettings()
    {
      this.useOverlapFilter = false;
      this.minimumAbsoluteDistanceRange = new Vector2(0.25f, 0.5f);
    }

    public XDocument SaveTemplate(string filePath)
    {
      XDocument xdocument = new XDocument(new object[1]
      {
        (object) new XElement((XName) "meshBrushTemplate", new object[13]
        {
          (object) new XAttribute((XName) "version", (object) 1.9f),
          (object) new XElement((XName) "instance", new object[4]
          {
            (object) new XElement((XName) "active", (object) this.active),
            (object) new XElement((XName) "name", (object) this.groupName),
            (object) new XElement((XName) "stats", (object) this.stats),
            (object) new XElement((XName) "lockSceneView", (object) this.lockSceneView)
          }),
          (object) new XElement((XName) "meshes", (object) new XElement((XName) "ui", new object[2]
          {
            (object) new XElement((XName) "style", !this.classicUI ? (object) "modern" : (object) "classic"),
            (object) new XElement((XName) "iconSize", (object) this.previewIconSize)
          })),
          (object) new XElement((XName) "keyBindings", new object[6]
          {
            (object) new XElement((XName) "paint", (object) this.paintKey),
            (object) new XElement((XName) "delete", (object) this.deleteKey),
            (object) new XElement((XName) "combine", (object) this.combineKey),
            (object) new XElement((XName) "randomize", (object) this.randomizeKey),
            (object) new XElement((XName) "increaseRadius", (object) this.increaseRadiusKey),
            (object) new XElement((XName) "decreaseRadius", (object) this.decreaseRadiusKey)
          }),
          (object) new XElement((XName) "brush", new object[10]
          {
            (object) new XElement((XName) "radius", (object) this.radius),
            (object) new XElement((XName) "color", new object[4]
            {
              (object) new XElement((XName) "r", (object) (float) this.color.r),
              (object) new XElement((XName) "g", (object) (float) this.color.g),
              (object) new XElement((XName) "b", (object) (float) this.color.b),
              (object) new XElement((XName) "a", (object) (float) this.color.a)
            }),
            (object) new XElement((XName) "quantity", new object[2]
            {
              (object) new XElement((XName) "min", (object) (float) this.quantityRange.x),
              (object) new XElement((XName) "max", (object) (float) this.quantityRange.y)
            }),
            (object) new XElement((XName) "useDensity", (object) this.useDensity),
            (object) new XElement((XName) "density", new object[2]
            {
              (object) new XElement((XName) "min", (object) (float) this.densityRange.x),
              (object) new XElement((XName) "max", (object) (float) this.densityRange.y)
            }),
            (object) new XElement((XName) "offset", new object[2]
            {
              (object) new XElement((XName) "min", (object) (float) this.offsetRange.x),
              (object) new XElement((XName) "max", (object) (float) this.offsetRange.y)
            }),
            (object) new XElement((XName) "scattering", new object[2]
            {
              (object) new XElement((XName) "min", (object) (float) this.scatteringRange.x),
              (object) new XElement((XName) "max", (object) (float) this.scatteringRange.y)
            }),
            (object) new XElement((XName) "delay", (object) this.delay),
            (object) new XElement((XName) "yAxisTangent", (object) this.yAxisTangent),
            (object) new XElement((XName) "strokeAlignment", (object) this.strokeAlignment)
          }),
          (object) new XElement((XName) "slopes", new object[2]
          {
            (object) new XElement((XName) "slopeInfluence", new object[2]
            {
              (object) new XElement((XName) "min", (object) (float) this.slopeInfluenceRange.x),
              (object) new XElement((XName) "max", (object) (float) this.slopeInfluenceRange.y)
            }),
            (object) new XElement((XName) "slopeFilter", new object[7]
            {
              (object) new XElement((XName) "enabled", (object) this.useSlopeFilter),
              (object) new XElement((XName) "inverse", (object) this.inverseSlopeFilter),
              (object) new XElement((XName) "angleThreshold", new object[2]
              {
                (object) new XElement((XName) "min", (object) (float) this.angleThresholdRange.x),
                (object) new XElement((XName) "max", (object) (float) this.angleThresholdRange.y)
              }),
              (object) new XElement((XName) "manualReferenceVectorSampling", (object) this.manualReferenceVectorSampling),
              (object) new XElement((XName) "showReferenceVectorInSceneView", (object) this.showReferenceVectorInSceneView),
              (object) new XElement((XName) "referenceVector", new object[3]
              {
                (object) new XElement((XName) "x", (object) (float) this.slopeReferenceVector.x),
                (object) new XElement((XName) "y", (object) (float) this.slopeReferenceVector.y),
                (object) new XElement((XName) "z", (object) (float) this.slopeReferenceVector.z)
              }),
              (object) new XElement((XName) "referenceVectorSampleLocation", new object[3]
              {
                (object) new XElement((XName) "x", (object) (float) this.slopeReferenceVectorSampleLocation.x),
                (object) new XElement((XName) "y", (object) (float) this.slopeReferenceVectorSampleLocation.y),
                (object) new XElement((XName) "z", (object) (float) this.slopeReferenceVectorSampleLocation.z)
              })
            })
          }),
          (object) new XElement((XName) "randomizers", new object[3]
          {
            (object) new XElement((XName) "scale", new object[4]
            {
              (object) new XElement((XName) "scaleUniformly", (object) this.uniformRandomScale),
              (object) new XElement((XName) "uniform", new object[2]
              {
                (object) new XElement((XName) "min", (object) (float) this.randomScaleRange.x),
                (object) new XElement((XName) "max", (object) (float) this.randomScaleRange.y)
              }),
              (object) new XElement((XName) "nonUniform", new object[3]
              {
                (object) new XElement((XName) "x", new object[2]
                {
                  (object) new XElement((XName) "min", (object) (float) this.randomScaleRangeX.x),
                  (object) new XElement((XName) "max", (object) (float) this.randomScaleRangeX.y)
                }),
                (object) new XElement((XName) "y", new object[2]
                {
                  (object) new XElement((XName) "min", (object) (float) this.randomScaleRangeY.x),
                  (object) new XElement((XName) "max", (object) (float) this.randomScaleRangeY.y)
                }),
                (object) new XElement((XName) "z", new object[2]
                {
                  (object) new XElement((XName) "min", (object) (float) this.randomScaleRangeZ.x),
                  (object) new XElement((XName) "max", (object) (float) this.randomScaleRangeZ.y)
                })
              }),
              (object) new XElement((XName) "curve", new object[2]
              {
                (object) new XElement((XName) "variation", (object) this.randomScaleCurveVariation),
                (object) new XElement((XName) "keys", (object) ((IEnumerable<Keyframe>) this.randomScaleCurve.get_keys()).Select<Keyframe, XElement>((Func<Keyframe, XElement>) (key => new XElement((XName) nameof (key), new object[4]
                {
                  (object) new XElement((XName) "time", (object) ((Keyframe) ref key).get_time()),
                  (object) new XElement((XName) "value", (object) ((Keyframe) ref key).get_value()),
                  (object) new XElement((XName) "inTangent", (object) ((Keyframe) ref key).get_inTangent()),
                  (object) new XElement((XName) "outTangent", (object) ((Keyframe) ref key).get_outTangent())
                }))))
              })
            }),
            (object) new XElement((XName) "rotation", new object[2]
            {
              (object) new XElement((XName) "min", (object) (float) this.randomRotationRange.x),
              (object) new XElement((XName) "max", (object) (float) this.randomRotationRange.y)
            }),
            (object) new XElement((XName) "randomizerBrush", new object[3]
            {
              (object) new XElement((XName) "position", (object) this.positionBrushRandomizer),
              (object) new XElement((XName) "rotation", (object) this.rotationBrushRandomizer),
              (object) new XElement((XName) "scale", (object) this.scaleBrushRandomizer)
            })
          }),
          (object) new XElement((XName) "overlapFilter", new object[2]
          {
            (object) new XElement((XName) "enabled", (object) this.useOverlapFilter),
            (object) new XElement((XName) "minimumAbsoluteDistance", new object[2]
            {
              (object) new XElement((XName) "min", (object) (float) this.minimumAbsoluteDistanceRange.x),
              (object) new XElement((XName) "max", (object) (float) this.minimumAbsoluteDistanceRange.y)
            })
          }),
          (object) new XElement((XName) "additiveScale", new object[3]
          {
            (object) new XElement((XName) "scaleUniformly", (object) this.uniformAdditiveScale),
            (object) new XElement((XName) "uniform", new object[2]
            {
              (object) new XElement((XName) "min", (object) (float) this.additiveScaleRange.x),
              (object) new XElement((XName) "max", (object) (float) this.additiveScaleRange.y)
            }),
            (object) new XElement((XName) "nonUniform", new object[3]
            {
              (object) new XElement((XName) "x", (object) (float) this.additiveScaleNonUniform.x),
              (object) new XElement((XName) "y", (object) (float) this.additiveScaleNonUniform.y),
              (object) new XElement((XName) "z", (object) (float) this.additiveScaleNonUniform.z)
            })
          }),
          (object) new XElement((XName) "optimization", new object[3]
          {
            (object) new XElement((XName) "autoIgnoreRaycast", (object) this.autoIgnoreRaycast),
            (object) new XElement((XName) "autoSelectOnCombine", (object) this.autoSelectOnCombine),
            (object) new XElement((XName) "autoStatic", (object) this.autoStatic)
          }),
          (object) new XElement((XName) "rangeLimits", new object[7]
          {
            (object) new XElement((XName) "quantity", (object) new XElement((XName) "max", (object) this.maxQuantityLimit)),
            (object) new XElement((XName) "density", (object) new XElement((XName) "max", (object) this.maxDensityLimit)),
            (object) new XElement((XName) "offset", new object[2]
            {
              (object) new XElement((XName) "min", (object) this.minOffsetLimit),
              (object) new XElement((XName) "max", (object) this.maxOffsetLimit)
            }),
            (object) new XElement((XName) "delay", (object) new XElement((XName) "max", (object) this.maxDelayLimit)),
            (object) new XElement((XName) "minimumAbsoluteDistance", (object) new XElement((XName) "max", (object) this.maxMinimumAbsoluteDistanceLimit)),
            (object) new XElement((XName) "randomScale", (object) new XElement((XName) "max", (object) this.maxRandomScaleLimit)),
            (object) new XElement((XName) "additiveScale", (object) new XElement((XName) "max", (object) this.maxAdditiveScaleLimit))
          }),
          (object) new XElement((XName) "inspectorFoldouts", new object[13]
          {
            (object) new XElement((XName) "help", (object) this.helpFoldout),
            (object) new XElement((XName) "templatesHelp", (object) this.helpTemplatesFoldout),
            (object) new XElement((XName) "generalUsageHelp", (object) this.helpGeneralUsageFoldout),
            (object) new XElement((XName) "optimizationHelp", (object) this.helpOptimizationFoldout),
            (object) new XElement((XName) "meshes", (object) this.meshesFoldout),
            (object) new XElement((XName) "templates", (object) this.templatesFoldout),
            (object) new XElement((XName) "keyBindings", (object) this.keyBindingsFoldout),
            (object) new XElement((XName) "brush", (object) this.brushFoldout),
            (object) new XElement((XName) "slopes", (object) this.slopesFoldout),
            (object) new XElement((XName) "randomizers", (object) this.randomizersFoldout),
            (object) new XElement((XName) "overlapFilter", (object) this.overlapFilterFoldout),
            (object) new XElement((XName) "additiveScale", (object) this.additiveScaleFoldout),
            (object) new XElement((XName) "optimization", (object) this.optimizationFoldout)
          }),
          (object) new XElement((XName) "globalPaintingMode", new object[2]
          {
            (object) new XElement((XName) "enabled", (object) this.globalPaintingMode),
            (object) new XElement((XName) "layerMask", (object) ((IEnumerable<bool>) this.layerMask).Select<bool, XElement>((Func<bool, int, XElement>) ((layer, index) => new XElement((XName) nameof (layer), new object[2]
            {
              (object) new XAttribute((XName) nameof (index), (object) index),
              (object) layer
            }))))
          })
        })
      });
      xdocument.Save(filePath);
      return xdocument;
    }

    public bool LoadTemplate(string filePath)
    {
      if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
      {
        Debug.LogError((object) "MeshBrush: the specified template file path is invalid or does not exist! Cancelling loading procedure...");
        return false;
      }
      XDocument xdocument = XDocument.Load(filePath);
      if (xdocument == null)
      {
        Debug.LogError((object) "MeshBrush: the specified template file couldn't be loaded.");
        return false;
      }
      foreach (XElement element in xdocument.Root.Elements())
      {
        string localName1 = element.Name.LocalName;
        if (localName1 != null)
        {
          // ISSUE: reference to a compiler-generated field
          if (MeshBrush.MeshBrush.\u003C\u003Ef__switch\u0024map5 == null)
          {
            // ISSUE: reference to a compiler-generated field
            MeshBrush.MeshBrush.\u003C\u003Ef__switch\u0024map5 = new Dictionary<string, int>(12)
            {
              {
                "instance",
                0
              },
              {
                "meshes",
                1
              },
              {
                "keyBindings",
                2
              },
              {
                "brush",
                3
              },
              {
                "slopes",
                4
              },
              {
                "randomizers",
                5
              },
              {
                "overlapFilter",
                6
              },
              {
                "additiveScale",
                7
              },
              {
                "optimization",
                8
              },
              {
                "rangeLimits",
                9
              },
              {
                "inspectorFoldouts",
                10
              },
              {
                "globalPaintingMode",
                11
              }
            };
          }
          int num;
          // ISSUE: reference to a compiler-generated field
          if (MeshBrush.MeshBrush.\u003C\u003Ef__switch\u0024map5.TryGetValue(localName1, out num))
          {
            switch (num)
            {
              case 0:
                using (IEnumerator<XElement> enumerator = element.Elements().GetEnumerator())
                {
                  while (enumerator.MoveNext())
                  {
                    XElement current = enumerator.Current;
                    switch (current.Name.LocalName)
                    {
                      case "active":
                        this.active = string.CompareOrdinal(current.Value, "true") == 0;
                        continue;
                      case "name":
                        this.groupName = current.Value;
                        continue;
                      case "stats":
                        this.stats = string.CompareOrdinal(current.Value, "true") == 0;
                        continue;
                      case "lockSceneView":
                        this.lockSceneView = string.CompareOrdinal(current.Value, "true") == 0;
                        continue;
                      default:
                        continue;
                    }
                  }
                  continue;
                }
              case 1:
                using (IEnumerator<XElement> enumerator = element.Elements().GetEnumerator())
                {
                  while (enumerator.MoveNext())
                  {
                    XElement current = enumerator.Current;
                    switch (current.Name.LocalName)
                    {
                      case "ui":
                        this.classicUI = string.CompareOrdinal(current.Element((XName) "style").Value, "classic") == 0;
                        this.previewIconSize = float.Parse(current.Element((XName) "iconSize").Value);
                        continue;
                      default:
                        continue;
                    }
                  }
                  continue;
                }
              case 2:
                using (IEnumerator<XElement> enumerator = element.Elements().GetEnumerator())
                {
                  while (enumerator.MoveNext())
                  {
                    XElement current = enumerator.Current;
                    switch (current.Name.LocalName)
                    {
                      case "paint":
                        this.paintKey = (KeyCode) Enum.Parse(typeof (KeyCode), current.Value);
                        continue;
                      case "delete":
                        this.deleteKey = (KeyCode) Enum.Parse(typeof (KeyCode), current.Value);
                        continue;
                      case "combine":
                        this.combineKey = (KeyCode) Enum.Parse(typeof (KeyCode), current.Value);
                        continue;
                      case "randomize":
                        this.randomizeKey = (KeyCode) Enum.Parse(typeof (KeyCode), current.Value);
                        continue;
                      case "increaseRadius":
                        this.increaseRadiusKey = (KeyCode) Enum.Parse(typeof (KeyCode), current.Value);
                        continue;
                      case "decreaseRadius":
                        this.decreaseRadiusKey = (KeyCode) Enum.Parse(typeof (KeyCode), current.Value);
                        continue;
                      default:
                        continue;
                    }
                  }
                  continue;
                }
              case 3:
                using (IEnumerator<XElement> enumerator = element.Elements().GetEnumerator())
                {
                  while (enumerator.MoveNext())
                  {
                    XElement current = enumerator.Current;
                    string localName2 = current.Name.LocalName;
                    if (localName2 != null)
                    {
                      // ISSUE: reference to a compiler-generated field
                      if (MeshBrush.MeshBrush.\u003C\u003Ef__switch\u0024map0 == null)
                      {
                        // ISSUE: reference to a compiler-generated field
                        MeshBrush.MeshBrush.\u003C\u003Ef__switch\u0024map0 = new Dictionary<string, int>(10)
                        {
                          {
                            "radius",
                            0
                          },
                          {
                            "color",
                            1
                          },
                          {
                            "quantity",
                            2
                          },
                          {
                            "useDensity",
                            3
                          },
                          {
                            "density",
                            4
                          },
                          {
                            "offset",
                            5
                          },
                          {
                            "scattering",
                            6
                          },
                          {
                            "delay",
                            7
                          },
                          {
                            "yAxisTangent",
                            8
                          },
                          {
                            "strokeAlignment",
                            9
                          }
                        };
                      }
                      // ISSUE: reference to a compiler-generated field
                      if (MeshBrush.MeshBrush.\u003C\u003Ef__switch\u0024map0.TryGetValue(localName2, out num))
                      {
                        switch (num)
                        {
                          case 0:
                            this.radius = float.Parse(current.Value);
                            continue;
                          case 1:
                            this.color = new Color(float.Parse(current.Element((XName) "r").Value), float.Parse(current.Element((XName) "g").Value), float.Parse(current.Element((XName) "b").Value), float.Parse(current.Element((XName) "a").Value));
                            continue;
                          case 2:
                            this.quantityRange = new Vector2(float.Parse(current.Element((XName) "min").Value), float.Parse(current.Element((XName) "max").Value));
                            continue;
                          case 3:
                            this.useDensity = string.CompareOrdinal(current.Value, "true") == 0;
                            continue;
                          case 4:
                            this.densityRange = new Vector2(float.Parse(current.Element((XName) "min").Value), float.Parse(current.Element((XName) "max").Value));
                            continue;
                          case 5:
                            this.offsetRange = new Vector2(float.Parse(current.Element((XName) "min").Value), float.Parse(current.Element((XName) "max").Value));
                            continue;
                          case 6:
                            this.scatteringRange = new Vector2(float.Parse(current.Element((XName) "min").Value), float.Parse(current.Element((XName) "max").Value));
                            continue;
                          case 7:
                            this.delay = float.Parse(current.Value);
                            continue;
                          case 8:
                            this.yAxisTangent = string.CompareOrdinal(current.Value, "true") == 0;
                            continue;
                          case 9:
                            this.strokeAlignment = string.CompareOrdinal(current.Value, "true") == 0;
                            continue;
                          default:
                            continue;
                        }
                      }
                    }
                  }
                  continue;
                }
              case 4:
                using (IEnumerator<XElement> enumerator = element.Descendants().GetEnumerator())
                {
                  while (enumerator.MoveNext())
                  {
                    XElement current = enumerator.Current;
                    string localName2 = current.Name.LocalName;
                    if (localName2 != null)
                    {
                      // ISSUE: reference to a compiler-generated field
                      if (MeshBrush.MeshBrush.\u003C\u003Ef__switch\u0024map1 == null)
                      {
                        // ISSUE: reference to a compiler-generated field
                        MeshBrush.MeshBrush.\u003C\u003Ef__switch\u0024map1 = new Dictionary<string, int>(8)
                        {
                          {
                            "slopeInfluence",
                            0
                          },
                          {
                            "enabled",
                            1
                          },
                          {
                            "inverse",
                            2
                          },
                          {
                            "angleThreshold",
                            3
                          },
                          {
                            "manualReferenceVectorSampling",
                            4
                          },
                          {
                            "showReferenceVectorInSceneView",
                            5
                          },
                          {
                            "referenceVector",
                            6
                          },
                          {
                            "referenceVectorSampleLocation",
                            7
                          }
                        };
                      }
                      // ISSUE: reference to a compiler-generated field
                      if (MeshBrush.MeshBrush.\u003C\u003Ef__switch\u0024map1.TryGetValue(localName2, out num))
                      {
                        switch (num)
                        {
                          case 0:
                            this.slopeInfluenceRange = new Vector2(float.Parse(current.Element((XName) "min").Value), float.Parse(current.Element((XName) "max").Value));
                            continue;
                          case 1:
                            this.useSlopeFilter = string.CompareOrdinal(current.Value, "true") == 0;
                            continue;
                          case 2:
                            this.inverseSlopeFilter = string.CompareOrdinal(current.Value, "true") == 0;
                            continue;
                          case 3:
                            this.angleThresholdRange = new Vector2(float.Parse(current.Element((XName) "min").Value), float.Parse(current.Element((XName) "max").Value));
                            continue;
                          case 4:
                            this.manualReferenceVectorSampling = string.CompareOrdinal(current.Value, "true") == 0;
                            continue;
                          case 5:
                            this.showReferenceVectorInSceneView = string.CompareOrdinal(current.Value, "true") == 0;
                            continue;
                          case 6:
                            this.slopeReferenceVector = new Vector3(float.Parse(current.Element((XName) "x").Value), float.Parse(current.Element((XName) "y").Value), float.Parse(current.Element((XName) "z").Value));
                            continue;
                          case 7:
                            this.slopeReferenceVectorSampleLocation = new Vector3(float.Parse(current.Element((XName) "x").Value), float.Parse(current.Element((XName) "y").Value), float.Parse(current.Element((XName) "z").Value));
                            continue;
                          default:
                            continue;
                        }
                      }
                    }
                  }
                  continue;
                }
              case 5:
                using (IEnumerator<XElement> enumerator1 = element.Elements().GetEnumerator())
                {
                  while (enumerator1.MoveNext())
                  {
                    XElement current1 = enumerator1.Current;
                    switch (current1.Name.LocalName)
                    {
                      case "scale":
                        using (IEnumerator<XElement> enumerator2 = current1.Descendants().GetEnumerator())
                        {
                          while (enumerator2.MoveNext())
                          {
                            XElement current2 = enumerator2.Current;
                            string localName2 = current2.Name.LocalName;
                            if (localName2 != null)
                            {
                              // ISSUE: reference to a compiler-generated field
                              if (MeshBrush.MeshBrush.\u003C\u003Ef__switch\u0024map2 == null)
                              {
                                // ISSUE: reference to a compiler-generated field
                                MeshBrush.MeshBrush.\u003C\u003Ef__switch\u0024map2 = new Dictionary<string, int>(7)
                                {
                                  {
                                    "scaleUniformly",
                                    0
                                  },
                                  {
                                    "uniform",
                                    1
                                  },
                                  {
                                    "x",
                                    2
                                  },
                                  {
                                    "y",
                                    3
                                  },
                                  {
                                    "z",
                                    4
                                  },
                                  {
                                    "variation",
                                    5
                                  },
                                  {
                                    "keys",
                                    6
                                  }
                                };
                              }
                              // ISSUE: reference to a compiler-generated field
                              if (MeshBrush.MeshBrush.\u003C\u003Ef__switch\u0024map2.TryGetValue(localName2, out num))
                              {
                                switch (num)
                                {
                                  case 0:
                                    this.uniformRandomScale = string.CompareOrdinal(current2.Value, "true") == 0;
                                    continue;
                                  case 1:
                                    this.randomScaleRange = new Vector2(float.Parse(current2.Element((XName) "min").Value), float.Parse(current2.Element((XName) "max").Value));
                                    continue;
                                  case 2:
                                    this.randomScaleRangeX = new Vector2(float.Parse(current2.Element((XName) "min").Value), float.Parse(current2.Element((XName) "max").Value));
                                    continue;
                                  case 3:
                                    this.randomScaleRangeY = new Vector2(float.Parse(current2.Element((XName) "min").Value), float.Parse(current2.Element((XName) "max").Value));
                                    continue;
                                  case 4:
                                    this.randomScaleRangeZ = new Vector2(float.Parse(current2.Element((XName) "min").Value), float.Parse(current2.Element((XName) "max").Value));
                                    continue;
                                  case 5:
                                    this.randomScaleCurveVariation = float.Parse(current2.Value);
                                    continue;
                                  case 6:
                                    this.randomScaleCurve = new AnimationCurve(current2.Descendants((XName) "key").Select<XElement, Keyframe>((Func<XElement, Keyframe>) (key => new Keyframe(float.Parse(key.Element((XName) "time").Value), float.Parse(key.Element((XName) "value").Value), float.Parse(key.Element((XName) "inTangent").Value), float.Parse(key.Element((XName) "outTangent").Value)))).ToArray<Keyframe>());
                                    continue;
                                  default:
                                    continue;
                                }
                              }
                            }
                          }
                          continue;
                        }
                      case "rotation":
                        if (string.CompareOrdinal(current1.Parent.Name.LocalName, "randomizerBrush") != 0)
                        {
                          this.randomRotationRange = new Vector2(float.Parse(current1.Element((XName) "min").Value), float.Parse(current1.Element((XName) "max").Value));
                          continue;
                        }
                        continue;
                      case "randomizerBrush":
                        XElement xelement1 = current1.Element((XName) "position");
                        if (xelement1 != null)
                          this.positionBrushRandomizer = string.CompareOrdinal(xelement1.Value, "true") == 0;
                        XElement xelement2 = current1.Element((XName) "rotation");
                        if (xelement2 != null)
                          this.rotationBrushRandomizer = string.CompareOrdinal(xelement2.Value, "true") == 0;
                        XElement xelement3 = current1.Element((XName) "scale");
                        if (xelement3 != null)
                        {
                          this.scaleBrushRandomizer = string.CompareOrdinal(xelement3.Value, "true") == 0;
                          continue;
                        }
                        continue;
                      default:
                        continue;
                    }
                  }
                  continue;
                }
              case 6:
                using (IEnumerator<XElement> enumerator = element.Elements().GetEnumerator())
                {
                  while (enumerator.MoveNext())
                  {
                    XElement current = enumerator.Current;
                    switch (current.Name.LocalName)
                    {
                      case "enabled":
                        this.useOverlapFilter = string.CompareOrdinal(current.Value, "true") == 0;
                        continue;
                      case "minimumAbsoluteDistance":
                        this.minimumAbsoluteDistanceRange = new Vector2(float.Parse(current.Element((XName) "min").Value), float.Parse(current.Element((XName) "max").Value));
                        continue;
                      default:
                        continue;
                    }
                  }
                  continue;
                }
              case 7:
                using (IEnumerator<XElement> enumerator = element.Elements().GetEnumerator())
                {
                  while (enumerator.MoveNext())
                  {
                    XElement current = enumerator.Current;
                    switch (current.Name.LocalName)
                    {
                      case "scaleUniformly":
                        this.uniformAdditiveScale = string.CompareOrdinal(current.Value, "true") == 0;
                        continue;
                      case "uniform":
                        this.additiveScaleRange = new Vector2(float.Parse(current.Element((XName) "min").Value), float.Parse(current.Element((XName) "max").Value));
                        continue;
                      case "nonUniform":
                        this.additiveScaleNonUniform = new Vector3(float.Parse(current.Element((XName) "x").Value), float.Parse(current.Element((XName) "y").Value), float.Parse(current.Element((XName) "z").Value));
                        continue;
                      default:
                        continue;
                    }
                  }
                  continue;
                }
              case 8:
                using (IEnumerator<XElement> enumerator = element.Elements().GetEnumerator())
                {
                  while (enumerator.MoveNext())
                  {
                    XElement current = enumerator.Current;
                    switch (current.Name.LocalName)
                    {
                      case "autoIgnoreRaycast":
                        this.autoIgnoreRaycast = string.CompareOrdinal(current.Value, "true") == 0;
                        continue;
                      case "autoSelectOnCombine":
                        this.autoSelectOnCombine = string.CompareOrdinal(current.Value, "true") == 0;
                        continue;
                      case "autoStatic":
                        this.autoStatic = string.CompareOrdinal(current.Value, "true") == 0;
                        continue;
                      default:
                        continue;
                    }
                  }
                  continue;
                }
              case 9:
                using (IEnumerator<XElement> enumerator = element.Elements().GetEnumerator())
                {
                  while (enumerator.MoveNext())
                  {
                    XElement current = enumerator.Current;
                    string localName2 = current.Name.LocalName;
                    if (localName2 != null)
                    {
                      // ISSUE: reference to a compiler-generated field
                      if (MeshBrush.MeshBrush.\u003C\u003Ef__switch\u0024map3 == null)
                      {
                        // ISSUE: reference to a compiler-generated field
                        MeshBrush.MeshBrush.\u003C\u003Ef__switch\u0024map3 = new Dictionary<string, int>(7)
                        {
                          {
                            "quantity",
                            0
                          },
                          {
                            "density",
                            1
                          },
                          {
                            "offset",
                            2
                          },
                          {
                            "delay",
                            3
                          },
                          {
                            "minimumAbsoluteDistance",
                            4
                          },
                          {
                            "randomScale",
                            5
                          },
                          {
                            "additiveScale",
                            6
                          }
                        };
                      }
                      // ISSUE: reference to a compiler-generated field
                      if (MeshBrush.MeshBrush.\u003C\u003Ef__switch\u0024map3.TryGetValue(localName2, out num))
                      {
                        switch (num)
                        {
                          case 0:
                            this.maxQuantityLimit = int.Parse(current.Element((XName) "max").Value);
                            continue;
                          case 1:
                            this.maxDensityLimit = float.Parse(current.Element((XName) "max").Value);
                            continue;
                          case 2:
                            this.minOffsetLimit = float.Parse(current.Element((XName) "min").Value);
                            this.maxOffsetLimit = float.Parse(current.Element((XName) "max").Value);
                            continue;
                          case 3:
                            this.maxDelayLimit = float.Parse(current.Element((XName) "max").Value);
                            continue;
                          case 4:
                            this.maxMinimumAbsoluteDistanceLimit = float.Parse(current.Element((XName) "max").Value);
                            continue;
                          case 5:
                            this.maxRandomScaleLimit = float.Parse(current.Element((XName) "max").Value);
                            continue;
                          case 6:
                            this.maxAdditiveScaleLimit = float.Parse(current.Element((XName) "max").Value);
                            continue;
                          default:
                            continue;
                        }
                      }
                    }
                  }
                  continue;
                }
              case 10:
                using (IEnumerator<XElement> enumerator = element.Elements().GetEnumerator())
                {
                  while (enumerator.MoveNext())
                  {
                    XElement current = enumerator.Current;
                    string localName2 = current.Name.LocalName;
                    if (localName2 != null)
                    {
                      // ISSUE: reference to a compiler-generated field
                      if (MeshBrush.MeshBrush.\u003C\u003Ef__switch\u0024map4 == null)
                      {
                        // ISSUE: reference to a compiler-generated field
                        MeshBrush.MeshBrush.\u003C\u003Ef__switch\u0024map4 = new Dictionary<string, int>(13)
                        {
                          {
                            "help",
                            0
                          },
                          {
                            "templatesHelp",
                            1
                          },
                          {
                            "generalUsageHelp",
                            2
                          },
                          {
                            "optimizationHelp",
                            3
                          },
                          {
                            "meshes",
                            4
                          },
                          {
                            "templates",
                            5
                          },
                          {
                            "keyBindings",
                            6
                          },
                          {
                            "brush",
                            7
                          },
                          {
                            "slopes",
                            8
                          },
                          {
                            "randomizers",
                            9
                          },
                          {
                            "overlapFilter",
                            10
                          },
                          {
                            "additiveScale",
                            11
                          },
                          {
                            "optimization",
                            12
                          }
                        };
                      }
                      // ISSUE: reference to a compiler-generated field
                      if (MeshBrush.MeshBrush.\u003C\u003Ef__switch\u0024map4.TryGetValue(localName2, out num))
                      {
                        switch (num)
                        {
                          case 0:
                            this.helpFoldout = string.CompareOrdinal(current.Value, "true") == 0;
                            continue;
                          case 1:
                            this.helpTemplatesFoldout = string.CompareOrdinal(current.Value, "true") == 0;
                            continue;
                          case 2:
                            this.helpGeneralUsageFoldout = string.CompareOrdinal(current.Value, "true") == 0;
                            continue;
                          case 3:
                            this.helpOptimizationFoldout = string.CompareOrdinal(current.Value, "true") == 0;
                            continue;
                          case 4:
                            this.meshesFoldout = string.CompareOrdinal(current.Value, "true") == 0;
                            continue;
                          case 5:
                            this.templatesFoldout = string.CompareOrdinal(current.Value, "true") == 0;
                            continue;
                          case 6:
                            this.keyBindingsFoldout = string.CompareOrdinal(current.Value, "true") == 0;
                            continue;
                          case 7:
                            this.brushFoldout = string.CompareOrdinal(current.Value, "true") == 0;
                            continue;
                          case 8:
                            this.slopesFoldout = string.CompareOrdinal(current.Value, "true") == 0;
                            continue;
                          case 9:
                            this.randomizersFoldout = string.CompareOrdinal(current.Value, "true") == 0;
                            continue;
                          case 10:
                            this.overlapFilterFoldout = string.CompareOrdinal(current.Value, "true") == 0;
                            continue;
                          case 11:
                            this.additiveScaleFoldout = string.CompareOrdinal(current.Value, "true") == 0;
                            continue;
                          case 12:
                            this.optimizationFoldout = string.CompareOrdinal(current.Value, "true") == 0;
                            continue;
                          default:
                            continue;
                        }
                      }
                    }
                  }
                  continue;
                }
              case 11:
                this.globalPaintingMode = string.CompareOrdinal(element.Element((XName) "enabled").Value, "true") == 0;
                this.layerMask = element.Descendants((XName) "layer").Select<XElement, bool>((Func<XElement, bool>) (layerElement => string.CompareOrdinal(layerElement.Value, "false") != 0)).ToArray<bool>();
                continue;
              default:
                continue;
            }
          }
        }
      }
      return true;
    }
  }
}
