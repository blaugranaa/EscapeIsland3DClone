using System.Linq;
using UnityEngine;

public class Line : MonoBehaviour
{
    public StickmanTypes stickmanType;
    internal Transform[] lineCells;
    internal Stickman[] stickmans =new Stickman[4];
    internal bool isFull;
    private void Awake()
    {
        lineCells = GetComponentsInChildren<Transform>().Skip(1).ToArray();
    }
    void Start()
    {
        if (stickmanType == StickmanTypes.None) return;

        SetStickmansToLine();
    }

    private void SetStickmansToLine()
    {
        for (int i = 0; i < lineCells.Length; i++)
        {
            var stickman = PoolingSystem.Instance.InstantiateAPS($"{stickmanType}Stickman", lineCells[i].position, Quaternion.identity);
            stickman.transform.SetParent(lineCells[i], true);
            stickman.transform.localEulerAngles = Vector3.zero;
            stickmans[i] = stickman.GetComponent<Stickman>();
            
        }
        isFull = true;
    }
}
