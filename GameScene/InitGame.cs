using System.Collections;
using System.Collections.Generic;
using TPSShoot;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace TPSShoot.GameScene
{
    public class InitGame : MonoBehaviour
    {
        public GameObject initCamera;
        private void Awake()
        {
            InitScene();
        }
        // Start is called before the first frame update
        void Start()
        {
            Events.AllAddressablesLoaded += OnAllLoaded;
        }
        void Update()
        {

        }

        private void OnAllLoaded()
        {
            if (initCamera != null)
            {
                Destroy(initCamera);
            }
        }
        private void OnDestroy()
        {
            Events.AllAddressablesLoaded -= OnAllLoaded;
        }
        private async void InitScene()
        {
            var t1 = Addressables.InstantiateAsync(MyAddressablesStr.player);

            var t2 = Addressables.InstantiateAsync(MyAddressablesStr.map);
            var t3 = Addressables.InstantiateAsync(MyAddressablesStr.gameManager);

            var t4 = Addressables.InstantiateAsync(MyAddressablesStr.deskTopInput);
            var t5 = Addressables.InstantiateAsync(MyAddressablesStr.mobileInput);


            await t1.Task;
            Events.PlayerLoaded.Call();
            await t2.Task;
            
            await t4.Task;
            await t5.Task;

            await t3.Task;

            

            Events.AllAddressablesLoaded.Call();
        }
    }
}
