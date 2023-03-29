using UnityEngine;

public static class TextCreator
{
    public static GameObject[,] textMeshObjects;

    public static void SetTextArraySize(int x, int z)
    {
        textMeshObjects = new GameObject[x, z];
    }

    public static TextMesh CreateWorldText(
        int textMeshObjectArrayX,
        int textMeshObjectArrayZ,
        Transform parent,
        string text,
        Vector3 localPosition,
        Color color,
        TextAnchor textAnchor,
        TextAlignment textAlignment,
        int sortingOrder
    )
    {
        GameObject gameObject = new GameObject("World_Text", typeof(TextMesh));
        textMeshObjects[textMeshObjectArrayX, textMeshObjectArrayZ] = gameObject;
        Transform transform = gameObject.transform;
        gameObject.transform.rotation = Quaternion.Euler(90, 0, 0);
        transform.SetParent(parent, false);
        transform.localPosition = localPosition;
        TextMesh textMesh = gameObject.GetComponent<TextMesh>();
        textMesh.anchor = textAnchor;
        textMesh.alignment = textAlignment;
        textMesh.text = text;
        textMesh.fontSize = 355;
        textMesh.color = color;
        textMesh.characterSize = .02f;
        textMesh.GetComponent<MeshRenderer>().sortingOrder = sortingOrder;
        return textMesh;
    }
}
