using UnityEngine;

public class Coin : MonoBehaviour
{
	static public Pooler coinPool;
    public bool isPremium = false;

    [SerializeField]
    Mesh[] coinMeshVariants;

    private void OnEnable()
    {
        transform.GetChild(0).GetComponent<MeshFilter>().mesh = coinMeshVariants[TrackManager.instance.section];
    }
}
