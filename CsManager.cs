using UnityEngine;

namespace TPSShoot
{
    public class CsManager : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            Events.PlayerLoaded.Call();
            Events.AllAddressablesLoaded.Call();
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
