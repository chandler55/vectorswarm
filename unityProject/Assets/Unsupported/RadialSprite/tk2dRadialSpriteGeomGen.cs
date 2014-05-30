using UnityEngine;
using System.Collections;

public class tk2dRadialSpriteGeomGen
{
	// Radial sprite
	public static void SetRadialSpriteindices ( int[] indices, int offset, int vStart, tk2dSpriteDefinition spriteDef )
	{
		if( spriteDef.positions.Length == 4 )
		{
			int[] inds = new int[9 * 6] {
				0, 4, 1, 1, 4, 5,
				1, 1, 1, 2, 2, 2,
				2, 6, 3, 3, 6, 7,
				4, 4, 4, 5, 5, 5,
				6, 6, 6, 7, 7, 7,
				8, 12, 9, 9, 12, 13,
				9, 9, 9, 10, 10, 10,
				10, 14, 11, 11, 14, 15,
				5, 5, 5, 5, 5, 5 // middle bit
			};
			int n = inds.Length;
			for( int i = 0;i < n; ++i) {
				indices[offset + i] = vStart + inds[i];
			}
		}
	}


	public static void SetRadialSpriteGeom(Vector3[] pos, Vector2[] uv, Vector3[] norm, Vector4[] tang, int offset, tk2dSpriteDefinition spriteDef, Vector3 scale)
	{

		Vector3 center = ( spriteDef.positions[3] - spriteDef.positions[0] ) * 0.5f + spriteDef.positions[0];
		Vector3 vecX = ( Vector3.Scale ( ( spriteDef.positions[1] - spriteDef.positions[0] ), scale ) ) * 0.5f;
		Vector3 vecY = ( Vector3.Scale ( ( spriteDef.positions[2] - spriteDef.positions[0] ), scale ) ) * 0.5f;

		pos[5] = pos[6] = pos[9] = pos[10] = center;

		pos[0] = Vector3.Scale( spriteDef.positions[0],  scale);
		pos[3] = Vector3.Scale( spriteDef.positions[1],  scale);
		pos[12] = Vector3.Scale( spriteDef.positions[2],  scale);
		pos[15] = Vector3.Scale( spriteDef.positions[3],  scale);

		pos[1] = pos[2] = center - vecY;
		pos[4] = pos[8] = center - vecX;
		pos[7] = pos[11] = center + vecX;
		pos[13] = pos[14] = center + vecY;

		Vector2 uvX = ( spriteDef.uvs[1] - spriteDef.uvs[0] ) * 0.5f;
		Vector2 uvY = ( spriteDef.uvs[2] - spriteDef.uvs[0] ) * 0.5f;

		uv[5] = uv[6] = uv[9] = uv[10] = spriteDef.uvs[0] + uvX + uvY;

		uv[0] = spriteDef.uvs[0];
		uv[3] = spriteDef.uvs[1];
		uv[12] = spriteDef.uvs[2];
		uv[15] = spriteDef.uvs[3];

		uv[1] = uv[2] = uv[5] - uvY;
		uv[4] = uv[8] = uv[5] - uvX;
		uv[7] = uv[11] = uv[5] + uvX;
		uv[13] = uv[14] = uv[5] + uvY;



		if (norm != null && spriteDef.normals != null)
		{
			for (int i = 0; i < spriteDef.normals.Length; ++i)
			{
				norm[offset + i] = spriteDef.normals[i];
			}
		}
		if (tang != null && spriteDef.tangents != null)
		{
			for (int i = 0; i < spriteDef.tangents.Length; ++i)
			{
				tang[offset + i] = spriteDef.tangents[i];
			}
		}
	}
}
