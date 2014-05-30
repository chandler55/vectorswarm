using UnityEngine;
using System.Collections;

// Based on code by n1ngimi

[AddComponentMenu("2D Toolkit/Sprite/tk2dRadialSprite")]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
[ExecuteInEditMode]
/// <summary>
/// Sprite implementation that implements radial clipping
/// </summary>
public class tk2dRadialSprite : tk2dBaseSprite
{
	Mesh mesh;
	Vector2[] meshUvs;
	Vector3[] meshVertices;
	Color32[] meshColors;
	Vector3[] meshNormals = null;
	Vector4[] meshTangents = null;
	int[] meshIndices;

	[SerializeField]
	protected bool _createBoxCollider = false;

	/// <summary>
	/// Create a trimmed box collider for this sprite
	/// </summary>
	public bool CreateBoxCollider {
		get { return _createBoxCollider; }
		set {
			if (_createBoxCollider != value) {
				_createBoxCollider = value;
				UpdateCollider();
			}
		}
	}

	public enum Direction {
		Clockwise,
		CounterClockwise
	}

	[SerializeField]
	protected Direction _rotationDirection = Direction.Clockwise;

	public Direction RotationDirection {
		get { return _rotationDirection; }
		set {  
			if (_rotationDirection != value) {
				_rotationDirection = value;
				UpdateGeometry();
			}
		}
	}
	
	[SerializeField] float _visibleAmount = 1;

	public float VisibleAmount {
		get {
			return _visibleAmount;
		}
		set {
			float cvalue = Mathf.Clamp01(value);
			if (cvalue != _visibleAmount) {
				_visibleAmount = cvalue;
				UpdateGeometry();
			}
		}
	}

	new void Awake()
	{
		base.Awake();
		
		// Create mesh, independently to everything else
		mesh = new Mesh();
		mesh.hideFlags = HideFlags.DontSave;
		GetComponent<MeshFilter>().mesh = mesh;
		
		// Cache box collider		
		if (boxCollider == null) {
			boxCollider = GetComponent<BoxCollider>();
		}
		#if !(UNITY_3_5 || UNITY_4_0 || UNITY_4_0_1 || UNITY_4_1 || UNITY_4_2)
		if (boxCollider2D == null) {
			boxCollider2D = GetComponent<BoxCollider2D>();
		}
		#endif
		
		// This will not be set when instantiating in code
		// In that case, Build will need to be called
		if (Collection)
		{
			// reset spriteId if outside bounds
			// this is when the sprite collection data is corrupt
			if (_spriteId < 0 || _spriteId >= Collection.Count)
				_spriteId = 0;
			
			Build();
		}
	}
	
	protected void OnDestroy()
	{
		if (mesh)
		{
			#if UNITY_EDITOR
			DestroyImmediate(mesh);
			#else
			Destroy(mesh);
			#endif
		}
	}
	
	new protected void SetColors(Color32[] dest)
	{
		tk2dSpriteGeomGen.SetSpriteColors (dest, 0, 16, _color, collectionInst.premultipliedAlpha);
	}
	
	// Calculated center and extents
	Vector3 boundsCenter = Vector3.zero, boundsExtents = Vector3.zero;
	
	protected void SetGeometry(Vector3[] vertices, Vector2[] uvs)
	{
		var sprite = CurrentSprite;
		tk2dRadialSpriteGeomGen.SetRadialSpriteGeom ( meshVertices, meshUvs, meshNormals, meshTangents, 0, sprite, scale );

		if (meshNormals.Length > 0 || meshTangents.Length > 0) {
			tk2dSpriteGeomGen.SetSpriteVertexNormals(meshVertices, meshVertices[0], meshVertices[15], sprite.normals, sprite.tangents, meshNormals, meshTangents);
		}
		
		if (sprite.positions.Length != 4 || sprite.complexGeometry)
		{
			for (int i = 0; i < vertices.Length; ++i)
				vertices[i] = Vector3.zero;
		}

		UpdateClipping();
	}
	
	void SetIndices()
	{
		meshIndices = new int[9 * 6];
		tk2dRadialSpriteGeomGen.SetRadialSpriteindices(meshIndices, 0, 0, CurrentSprite );
	}
	
	// returns true if value is close enough to compValue, by within 1% of scale
	bool NearEnough(float value, float compValue, float scale) {
		float diff = Mathf.Abs(value - compValue);
		return Mathf.Abs(diff / scale) < 0.01f;
	}

	
	public override void Build()
	{		
		var spriteDef = CurrentSprite;
		
		meshUvs = new Vector2[16];
		meshVertices = new Vector3[16];
		meshColors = new Color32[16];
		meshNormals = new Vector3[0];
		meshTangents = new Vector4[0];
		if (spriteDef.normals != null && spriteDef.normals.Length > 0) {
			meshNormals = new Vector3[16];
		}
		if (spriteDef.tangents != null && spriteDef.tangents.Length > 0) {
			meshTangents = new Vector4[16];
		}
		SetIndices();
		SetGeometry(meshVertices, meshUvs);
		SetColors(meshColors);
		
		if (mesh == null)
		{
			mesh = new Mesh();
			mesh.hideFlags = HideFlags.DontSave;
		}
		else
		{
			mesh.Clear();
		}

		mesh.vertices = meshVertices;
		mesh.colors32 = meshColors;
		mesh.uv = meshUvs;
		mesh.normals = meshNormals;
		mesh.tangents = meshTangents;
		mesh.triangles = meshIndices;
		mesh.RecalculateBounds();
		mesh.bounds = AdjustedMeshBounds( mesh.bounds, renderLayer );
		
		GetComponent<MeshFilter>().mesh = mesh;

		UpdateCollider();
		UpdateMaterial();
	}
	
	protected override void UpdateGeometry() { UpdateGeometryImpl(); }
	protected override void UpdateColors() { UpdateColorsImpl(); }
	protected override void UpdateVertices() { UpdateGeometryImpl(); }
	void UpdateIndices() {
		if (mesh != null) {
			SetIndices();
			mesh.triangles = meshIndices;
		}
	}
	
	protected void UpdateColorsImpl()
	{
		#if UNITY_EDITOR
		// This can happen with prefabs in the inspector
		if (meshColors == null || meshColors.Length == 0)
			return;
		#endif
		if (meshColors == null || meshColors.Length == 0) {
			Build();
		}
		else {
			SetColors(meshColors);
			mesh.colors32 = meshColors;
		}
	}
	
	protected void UpdateGeometryImpl()
	{
		#if UNITY_EDITOR
		// This can happen with prefabs in the inspector
		if (mesh == null)
			return;
		#endif
		if (meshVertices == null || meshVertices.Length == 0) {
			Build();
		}
		else {
			SetGeometry(meshVertices, meshUvs);
			mesh.vertices = meshVertices;
			mesh.uv = meshUvs;
			mesh.normals = meshNormals;
			mesh.tangents = meshTangents;
			mesh.RecalculateBounds();
			mesh.bounds = AdjustedMeshBounds( mesh.bounds, renderLayer );
			
			UpdateCollider();
		}
	}
	
	#region Collider
	protected override void UpdateCollider()
	{
		if (CreateBoxCollider) {
			if (CurrentSprite.physicsEngine == tk2dSpriteDefinition.PhysicsEngine.Physics3D) {
				if (boxCollider != null) {
					boxCollider.size = 2 * boundsExtents;
					boxCollider.center = boundsCenter;
				}
			}
			else if (CurrentSprite.physicsEngine == tk2dSpriteDefinition.PhysicsEngine.Physics2D) {
				#if !(UNITY_3_5 || UNITY_4_0 || UNITY_4_0_1 || UNITY_4_1 || UNITY_4_2)
				if (boxCollider2D != null) {
					boxCollider2D.size = 2 * boundsExtents;
					boxCollider2D.center = boundsCenter;
				}
				#endif
			}
		}
	}
	
	#if UNITY_EDITOR
	void OnDrawGizmos() {
		if (mesh != null) {
			Bounds b = mesh.bounds;
			Gizmos.color = Color.clear;
			Gizmos.matrix = transform.localToWorldMatrix;
			Gizmos.DrawCube(b.center, b.extents * 2);
			Gizmos.matrix = Matrix4x4.identity;
			Gizmos.color = Color.white;
		}
	}
	#endif
	
	protected override void CreateCollider() {
		UpdateCollider();
	}
	
	#if UNITY_EDITOR
	public override void EditMode__CreateCollider() {
		if (CreateBoxCollider) {
			base.CreateSimpleBoxCollider();
		}
		
		UpdateCollider();
	}
	#endif
	#endregion
	
	protected override void UpdateMaterial()
	{
		if (renderer.sharedMaterial != collectionInst.spriteDefinitions[spriteId].materialInst)
			renderer.material = collectionInst.spriteDefinitions[spriteId].materialInst;
	}
	
	protected override int GetCurrentVertexCount()
	{
		#if UNITY_EDITOR
		if (meshVertices == null)
			return 0;
		#endif
		return 16;
	}


	void UpdateClipping()
	{
		float _value = 1 - Mathf.Clamp01(_visibleAmount);
		float radianAngle = (float)(Mathf.PI * 2f * _value );
		float uvX = (float)(Mathf.Min(Mathf.Abs(Mathf.Tan(radianAngle)), 1) * Mathf.Sign(Mathf.Sin(radianAngle)));
		float uvY = (float)(Mathf.Min(Mathf.Abs(Mathf.Tan(radianAngle + Mathf.PI * 0.5f)), 1) * Mathf.Sign(Mathf.Cos(radianAngle)));

		float drawWidth = ( meshVertices[3] - meshVertices[0] ).magnitude;
		float drawHeight = ( meshVertices[12] - meshVertices[0] ).magnitude;

		Vector3 point;
		if( _rotationDirection == Direction.Clockwise )
		{
			point.x = meshVertices[5].x + uvX * drawWidth * 0.5f;
		}
		else
		{
			point.x = meshVertices[5].x + -uvX * drawWidth * 0.5f;
		}
		point.y = meshVertices[5].y + uvY * drawHeight * 0.5f;
		point.z = meshVertices[5].z;

		if( _rotationDirection == Direction.Clockwise )
		{
			if( _value <= 0.125f )
			{
				meshVertices[14] = point;
				meshUvs[14] += ( meshUvs[15] - meshUvs[14] ) * uvX;
			}
			else if ( _value <= 0.25f )
			{
				meshVertices[15] = point;
				meshVertices[14] = meshVertices[15];

				meshUvs[15] += ( meshUvs[11] - meshUvs[15] ) * ( 1 - uvY );
				meshUvs[14] = meshUvs[15];
			}
			else if ( _value <= 0.375f )
			{
				meshVertices[14] = meshVertices[15] = meshVertices[11];
				meshVertices[7] = point;

				meshUvs[14] = meshUvs[15] = meshUvs[11];
				meshUvs[7] += ( meshUvs[3] - meshUvs[7] ) * Mathf.Abs ( uvY );
			}
			else if ( _value <= 0.5f )
			{
				meshVertices[14] = meshVertices[15] = meshVertices[11];
				meshVertices[3] = point;
				meshVertices[7] = meshVertices[3];

				meshUvs[14] = meshUvs[15] = meshUvs[11];
				meshUvs[3] += ( meshUvs[2] - meshUvs[3] ) * ( 1 - uvX );
				meshUvs[7] = meshUvs[3];
			}
			else if ( _value <= 0.625f )
			{
				meshVertices[14] = meshVertices[15] = meshVertices[11];
				meshVertices[7] = meshVertices[3] = meshVertices[2];
				meshVertices[1] = point;

				meshUvs[14] = meshUvs[15] = meshUvs[11];
				meshUvs[7] = meshUvs[3] = meshUvs[2];
				meshUvs[1] += ( meshUvs[0] - meshUvs[1] ) * Mathf.Abs ( uvX );
			}
			else if ( _value <= 0.75f )
			{
				meshVertices[14] = meshVertices[15] = meshVertices[11];
				meshVertices[7] = meshVertices[3] = meshVertices[2];
				meshVertices[0] = point;
				meshVertices[1] = meshVertices[0];

				meshUvs[14] = meshUvs[15] = meshUvs[11];
				meshUvs[7] = meshUvs[3] = meshUvs[2];
				meshUvs[0] += ( meshUvs[4] - meshUvs[0]) * ( 1 - Mathf.Abs( uvY ) );
				meshUvs[1] = meshUvs[0];
			}
			else if ( _value <= 0.875f )
			{
				meshVertices[14] = meshVertices[15] = meshVertices[11];
				meshVertices[7] = meshVertices[3] = meshVertices[2];
				meshVertices[1] = meshVertices[0] = meshVertices[4];
				meshVertices[8] = point;

				meshUvs[14] = meshUvs[15] = meshUvs[11];
				meshUvs[7] = meshUvs[3] = meshUvs[2];
				meshUvs[1] = meshUvs[0] = meshUvs[4];
				meshUvs[8] += ( meshUvs[12] - meshUvs[8] ) * uvY;
			}
			else
			{
				meshVertices[14] = meshVertices[15] = meshVertices[11];
				meshVertices[7] = meshVertices[3] = meshVertices[2];
				meshVertices[1] = meshVertices[0] = meshVertices[4];
				meshVertices[12] = point;
				meshVertices[8] = meshVertices[12];

				meshUvs[14] = meshUvs[15] = meshUvs[11];
				meshUvs[7] = meshUvs[3] = meshUvs[2];
				meshUvs[1] = meshUvs[0] = meshUvs[4];
				meshUvs[12] += ( meshUvs[13] - meshUvs[12] ) * ( 1 - Mathf.Abs ( uvX ) );
				meshUvs[8] = meshUvs[12];
			}
		}
		else if( _rotationDirection == Direction.CounterClockwise )
		{
			if( _value <= 0.125f )
			{
				meshVertices[13] = point;
				meshUvs[13] += ( meshUvs[12] - meshUvs[13] ) * uvX;
			}
			else if ( _value <= 0.25f )
			{
				meshVertices[12] = point;
				meshVertices[13] = meshVertices[12];
				
				meshUvs[12] += ( meshUvs[8] - meshUvs[12] ) * ( 1 - uvY );
				meshUvs[13] = meshUvs[12];
			}
			else if ( _value <= 0.375f )
			{
				meshVertices[13] = meshVertices[12] = meshVertices[8];
				meshVertices[4] = point;
				
				meshUvs[13] = meshUvs[12] = meshUvs[8];
				meshUvs[4] += ( meshUvs[0] - meshUvs[4] ) * Mathf.Abs ( uvY );
			}
			else if ( _value <= 0.5f )
			{
				meshVertices[13] = meshVertices[12] = meshVertices[8];
				meshVertices[0] = point;
				meshVertices[4] = meshVertices[0];
				
				meshUvs[13] = meshUvs[12] = meshUvs[8];
				meshUvs[0] += ( meshUvs[1] - meshUvs[0] ) * ( 1 - uvX );
				meshUvs[4] = meshUvs[0];
			}
			else if ( _value <= 0.625f )
			{
				meshVertices[13] = meshVertices[12] = meshVertices[8];
				meshVertices[4] = meshVertices[0] = meshVertices[1];
				meshVertices[2] = point;
				
				meshUvs[13] = meshUvs[12] = meshUvs[8];
				meshUvs[4] = meshUvs[0] = meshUvs[1];
				meshUvs[2] += ( meshUvs[3] - meshUvs[2] ) * Mathf.Abs ( uvX );
			}
			else if ( _value <= 0.75f )
			{
				meshVertices[13] = meshVertices[12] = meshVertices[8];
				meshVertices[4] = meshVertices[0] = meshVertices[1];
				meshVertices[3] = point;
				meshVertices[2] = meshVertices[3];
				
				meshUvs[13] = meshUvs[12] = meshUvs[8];
				meshUvs[4] = meshUvs[0] = meshUvs[1];
				meshUvs[3] += ( meshUvs[7] - meshUvs[3]) * ( 1 - Mathf.Abs( uvY ) );
				meshUvs[2] = meshUvs[3];
			}
			else if ( _value <= 0.875f )
			{
				meshVertices[13] = meshVertices[12] = meshVertices[8];
				meshVertices[4] = meshVertices[0] = meshVertices[1];
				meshVertices[2] = meshVertices[3] = meshVertices[7];
				meshVertices[11] = point;
				
				meshUvs[13] = meshUvs[12] = meshUvs[8];
				meshUvs[4] = meshUvs[0] = meshUvs[1];
				meshUvs[2] = meshUvs[3] = meshUvs[7];
				meshUvs[11] += ( meshUvs[15] - meshUvs[11] ) * uvY;
			}
			else
			{
				meshVertices[13] = meshVertices[12] = meshVertices[8];
				meshVertices[4] = meshVertices[0] = meshVertices[1];
				meshVertices[2] = meshVertices[3] = meshVertices[7];
				meshVertices[15] = point;
				meshVertices[11] = meshVertices[15];
				
				meshUvs[13] = meshUvs[12] = meshUvs[8];
				meshUvs[4] = meshUvs[0] = meshUvs[1];
				meshUvs[2] = meshUvs[3] = meshUvs[7];
				meshUvs[15] += ( meshUvs[14] - meshUvs[15] ) * ( 1 - Mathf.Abs ( uvX ) );
				meshUvs[11] = meshUvs[15];
			}
		}
	}
}
