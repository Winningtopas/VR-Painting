using UnityEngine;

public class Paint
{
    public Matrix4x4 paintMatrix;
    public Vector4 channelMask;
    public Vector4 scaleBias;
    public Brush brush;
}

[System.Serializable]
public class Brush
{
    public Texture2D splatTexture;
    public int splatsX = 1;
    public int splatsY = 1;
    public int splatIndex = -1;

    public float splatScale = 1.0f;
    public float splatRandomScaleMin = 1.0f;
    public float splatRandomScaleMax = 1.0f;

    public float splatRotation = 0f;
    public float splatRandomRotation = 180f;

    public Color paintColor = new Color(1, 1, 1, 0);
    public bool useMeshMaterialColor = true;
    public Vector4 GetPaintColor(Color meshMaterialColor)
    {
        if (useMeshMaterialColor)
            paintColor = meshMaterialColor;
        return paintColor;
    }

    public Vector4 GetTile()
    {
        float splatscaleX = 1.0f / splatsX;
        float splatscaleY = 1.0f / splatsY;

        int index = splatIndex;
        if (index >= splatsX * splatsY)
        {
            splatIndex = 0;
            index = 0;
        }

        if (splatIndex == -1) index = Random.Range(0, splatsX * splatsY);

        float splatsBiasX = splatscaleX * (index % splatsX);
        float splatsBiasY = splatscaleY * (index / splatsX);

        return new Vector4(splatscaleX, splatscaleY, splatsBiasX, splatsBiasY);
    }
}