using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MaterialFlag
{
    None,
    CheckerPattern = 1,
    HasTexture = 2,
}

[Serializable]
public struct RayTracingMaterial_Editor
{
    public Color albedo;
    public Vector3 specular;
    public float smoothness;
    public Vector3 emission;
    public float specularChance;
    public MaterialFlag flag;

    public RayTracingMaterial GetMaterialForShader()
    {
        return new RayTracingMaterial()
        {
            albedo = new Vector3(albedo.r, albedo.g, albedo.b),
            specular = this.specular,
            smoothness = this.smoothness,
            emission = this.emission,
            specularChance = this.specularChance,
            flag = this.flag
        };
    }
}

public struct RayTracingMaterial
{
    public Vector3 albedo;
    public Vector3 specular;
    public float smoothness;
    public Vector3 emission;
    public float specularChance;
    public MaterialFlag flag;
    
}

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
public class RayTracingObject : MonoBehaviour
{
    public RayTracingMaterial_Editor Material;

    [SerializeField, HideInInspector] int materialId;
    [SerializeField, HideInInspector] bool materialInited;

    private void OnValidate()
    {
        MeshRenderer renderer = GetComponent<MeshRenderer>();
        if (renderer != null)
        {
            if (materialId != gameObject.GetInstanceID())
            {
                renderer.sharedMaterial = new Material(renderer.sharedMaterial);
                materialId = gameObject.GetInstanceID();
            }

            renderer.sharedMaterial.color = Material.albedo;
        }
    }

    private void OnEnable()
    {
        RayTracingManager.RegisterObject(this);
    }

    private void OnDisable()
    {
        RayTracingManager.UnregisterObject(this);
    }

    private void Update()
    {
        if (transform.hasChanged)
        {
            RayTracingManager.RegisterObject(this);
            RayTracingManager.UnregisterObject(this);
            transform.hasChanged = false;
        }
    }
}
