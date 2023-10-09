using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TPSShoot.Manger;
using UnityEngine;
using UnityEngine.UI;

namespace TPSShoot.Bags
{
    public partial class PlayerBagBehaviour : MonoBehaviour
    {
        [Header("背包的一些属性")]
        [Tooltip("拖拽背包的速度")]public float dragSpeed = 8f;

        [Header("一些组件的赋值")]
        [Tooltip("整个背包")] public Transform bags;
        [Tooltip("背包打开时移动到的位置")] public Transform bagsMoveTo;
        [Tooltip("显示信息的")] public GameObject tipTool;
        [Tooltip("显示信息的（对比）")] public GameObject tipToolContrast;
        [Tooltip("拖拽的对象")] public ItemUi dragItem;
        [Tooltip("装备")] public GameObject character;
        [Tooltip("属性")] public GameObject attribute;
        [Tooltip("属性显示的文字")] public Text attributeText;
        [Tooltip("背包")] public GameObject knapsack;


        private List<Item> _items;
        private CanvasGroup _characterCanvas;
        private CanvasGroup _attributeCanvas;
        private Image _attributeImage;

        private static PlayerBagBehaviour _instance; // 单例
        private bool _isOpenBag; // 背包是否打开
        private float _width, _height; // 屏幕的宽高
        private int _increase, _reduce; // 属性的上下浮动

        private Character _character;
        private Knapsack _knapsack;

        public Character GetCharacter() { return _character; }
        public Knapsack GetKnapsack() { return _knapsack; }
        public float Width { get => _width; }
        public float Height { get => _height; }
        public static PlayerBagBehaviour Instance { get { return _instance; } }
        public bool IsOpenBag { get => _isOpenBag; }
        public List<Item> Items { get => _items;  }
        private void Awake()
        {
            _instance = this;
            _width = Screen.width;
            _height = Screen.height;

            _attributeCanvas = attribute.GetComponent<CanvasGroup>();
            _attributeImage = attribute.GetComponent<Image>();
            _characterCanvas = character.GetComponent<CanvasGroup>();
            AwakeInitTool();

            _character = new Character(this,character);
            _knapsack = new Knapsack(this, knapsack);
            SubScribe();
        }
        private void Start()
        {
            if (bagsMoveTo == null)
            {
                GameObject go = new GameObject("BagsMoveTo");
                Instantiate(go);
                go.transform.parent = transform.parent;
                bagsMoveTo = go.transform;
                bagsMoveTo.position = new Vector3(0, 0, 0);
            }

            dragItem.Hide();
        }
        // 当所有的东西都加载完成之后执行
        public void OnAllLoad()
        {
            _increase = MonsterDroppedManager.Instance.increase;
            _reduce = MonsterDroppedManager.Instance.reduce;
            OnInitBag();
        }
        private void Update()
        {
            if (_width != Screen.width || _height != Screen.height)
            {
                Debug.Log("改变了宽高");
                _width = Screen.width;
                _height = Screen.height;

                // 改变了宽高的监听
            }
            UpdateTipShow();
        }

        private void OnDestroy()
        {
            UnSubScribe();
        }
        private void SubScribe()
        {
            Events.BagRequest += OnBagsRequest;
            Events.ItemsJsonLoaded += OnInitItemTypeId;
            Events.BagsComputeAttribute += _character.ComputeAttribute;
            Events.BagsShowTip += ShowTip;
            Events.BagsHideTip += HideTip;
            Events.BagsKnapsackPutOn += _knapsack.PutOn;

            Events.AllAddressablesLoaded += OnAllLoad;
        }
        private void UnSubScribe()
        {
            Events.BagRequest -= OnBagsRequest;
            Events.ItemsJsonLoaded -= OnInitItemTypeId;
            Events.BagsComputeAttribute -= _character.ComputeAttribute;
            Events.BagsShowTip -= ShowTip;
            Events.BagsHideTip -= HideTip;
            Events.BagsKnapsackPutOn -= _knapsack.PutOn;

            Events.AllAddressablesLoaded -= OnAllLoad;
        }

        /// <summary>
        /// 初始化背包
        /// </summary>
        private void OnInitBag()
        {
            gameObject.SetActive(false);
        }

        /// <summary>
        /// 打开背包或关闭背包的请求
        /// </summary>
        private void OnBagsRequest()
        {
            if (!_isOpenBag)
            {
                OpenBag();
            }
            else
            {
                CloseBag();
            }
        }

        #region 打开和关闭背包
        private void OpenBag()
        {
            gameObject.SetActive(true);
            _isOpenBag = true;
            bags.DOMove(bagsMoveTo.position, 1);
            Events.PlayerOpenBag.Call();
        }

        private void CloseBag()
        {
            _isOpenBag = false;
            bags.DOMove(new Vector3(_width * 2, -_height * 2), 1).OnComplete(()=>
            {
                gameObject.SetActive(false);
                Events.PlayerCloseBag.Call();
            });
        }

        #endregion

        public Item GetItemById(int id)
        {
            return _items.Find(a => a.Id == id);
        }

        #region 点击事件
        public void OnClickAttribute()
        {
            _attributeCanvas.DOFade(1, 1);
            _attributeImage.raycastTarget = true;
            _characterCanvas.DOFade(0, 1);
            // 重新计算以下属性
            Events.BagsComputeAttribute.Call();
        }

        public void OnClickCharacter()
        {
            _attributeCanvas.DOFade(0, 1);
            _attributeImage.raycastTarget = false;
            _characterCanvas.DOFade(1, 1);

            Events.BagsComputeAttribute.Call();
        }
        #endregion
    }
}
