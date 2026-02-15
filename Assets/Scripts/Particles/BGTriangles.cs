using UnityEngine;

public class BGTriangles : MonoBehaviour
{
    void Awake()
    {
        var ps = GetComponent<ParticleSystem>();
        ps.Simulate(12f, true, true, true);
        ps.Play();
    }
}
