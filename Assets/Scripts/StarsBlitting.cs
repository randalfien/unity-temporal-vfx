using UnityEngine;

public class StarsBlitting : MonoBehaviour
{
    public StarsManager Ref;

    private void OnPreRender()
    {
        Ref.OnPreRender();
    }
}
