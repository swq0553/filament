//------------------------------------------------------------------------------
// Material parameters
//------------------------------------------------------------------------------

// These variables should be in a struct but some GPU drivers ignore the
// precision qualifier on individual struct members
HIGHP mat3  shading_tangentToWorld;   // TBN matrix
HIGHP vec3  shading_position;         // position of the fragment in world space
      vec3  shading_view;             // normalized vector from the fragment to the eye
      vec3  shading_normal;           // normalized normal, in world space
      vec3  shading_reflected;        // reflection of view about normal
      float shading_NoV;              // dot(normal, view), always strictly > 0

#if defined(MATERIAL_HAS_CLEAR_COAT)
      vec3  shading_clearCoatNormal;  // normalized clear coat layer normal, in world space
#endif

/** @public-api */
HIGHP mat3 getWorldTangentFrame() {
    return shading_tangentToWorld;
}

/** @public-api */
HIGHP vec3 getWorldPosition() {
    return shading_position;
}

/** @public-api */
vec3 getWorldViewVector() {
    return shading_view;
}

/** @public-api */
vec3 getWorldNormalVector() {
    return shading_normal;
}

/** @public-api */
vec3 getWorldReflectedVector() {
    return shading_reflected;
}

/** @public-api */
float getNdotV() {
    return shading_NoV;
}

struct MaterialInputs {
    vec4  baseColor;
#if !defined(SHADING_MODEL_UNLIT)
    float roughness;
#if !defined(SHADING_MODEL_CLOTH)
    float metallic;
    float reflectance;
#endif
    float ambientOcclusion;
#endif
    vec4  emissive;

    float clearCoat;
    float clearCoatRoughness;

    float anisotropy;
    vec3  anisotropyDirection;

#if defined(SHADING_MODEL_SUBSURFACE)
    float thickness;
    float subsurfacePower;
    vec3  subsurfaceColor;
#endif

#if defined(SHADING_MODEL_CLOTH)
    vec3  sheenColor;
#if defined(MATERIAL_HAS_SUBSURFACE_COLOR)
    vec3  subsurfaceColor;
#endif
#endif

#if defined(MATERIAL_HAS_NORMAL)
    vec3  normal;
#endif
#if defined(MATERIAL_HAS_CLEAR_COAT) && defined(MATERIAL_HAS_CLEAR_COAT_NORMAL)
    vec3  clearCoatNormal;
#endif
};

void initMaterial(out MaterialInputs material) {
    material.baseColor = vec4(1.0);
#if !defined(SHADING_MODEL_UNLIT)
    material.roughness = 1.0;
#if !defined(SHADING_MODEL_CLOTH)
    material.metallic = 0.0;
    material.reflectance = 0.5;
#endif
    material.ambientOcclusion = 1.0;
#endif
    material.emissive = vec4(0.0);

#if defined(MATERIAL_HAS_CLEAR_COAT)
    material.clearCoat = 1.0;
    material.clearCoatRoughness = 0.0;
#endif

#if defined(MATERIAL_HAS_ANISOTROPY)
    material.anisotropy = 0.0;
    material.anisotropyDirection = vec3(1.0, 0.0, 0.0);
#endif

#if defined(SHADING_MODEL_SUBSURFACE)
    material.thickness = 0.5;
    material.subsurfacePower = 12.234;
    material.subsurfaceColor = vec3(1.0);
#endif

#if defined(SHADING_MODEL_CLOTH)
    material.sheenColor = sqrt(material.baseColor.rgb);
#if defined(MATERIAL_HAS_SUBSURFACE_COLOR)
    material.subsurfaceColor = vec3(0.0);
#endif
#endif

#if defined(MATERIAL_HAS_NORMAL)
    material.normal = vec3(0.0, 0.0, 1.0);
#endif
#if defined(MATERIAL_HAS_CLEAR_COAT) && defined(MATERIAL_HAS_CLEAR_COAT_NORMAL)
    material.clearCoatNormal = vec3(0.0, 0.0, 1.0);
#endif
}
