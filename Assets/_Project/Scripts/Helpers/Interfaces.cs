using UnityEngine;

public interface IDestructable
{
    void RequestDestroy();
    void DestroyImmediately();
}