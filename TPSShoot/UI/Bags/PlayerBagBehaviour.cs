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
        [Header("������һЩ����")]
        [Tooltip("��ק�������ٶ�")]public float dragSpeed = 8f;

        [Header("һЩ����ĸ�ֵ")]
        [Tooltip("��������")] public Transform bags;
        [Tooltip("������ʱ�ƶ�����λ��")] public Transform bagsMoveTo;
        [Tooltip("��ʾ��Ϣ��")] public GameObject tipTool;
        [Tooltip("��ʾ��Ϣ�ģ��Աȣ�")] public GameObject tipToolContrast;
        [Tooltip("��ק�Ķ���")] public ItemUi dragItem;
        [Tooltip("װ��")] public GameObject character;
        [Tooltip("����")] public GameObject attribute;
        [Tooltip("������ʾ������")] public Text attributeText;
        [Tooltip("����")] public GameObject knapsack;


        private List<Item> _items;
        private CanvasGroup _characterCanvas;
        private CanvasGroup _attributeCanvas;
        private Image _attributeImage;

        private static PlayerBagBehaviour _instance; // ����
        private bool _isOpenBag; // �����Ƿ��
        private float _width, _height; // ��Ļ�Ŀ��
        private int _increase, _reduce; // ���Ե����¸���

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
        // �����еĶ������������֮��ִ��
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
                Debug.Log("�ı��˿��");
                _width = Screen.width;
                _height = Screen.height;

                // �ı��˿�ߵļ���
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
        /// ��ʼ������
        /// </summary>
        private void OnInitBag()
        {
            gameObject.SetActive(false);
        }

        /// <summary>
        /// �򿪱�����رձ���������
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

        #region �򿪺͹رձ���
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

        #region ����¼�
        public void OnClickAttribute()
        {
            _attributeCanvas.DOFade(1, 1);
            _attributeImage.raycastTarget = true;
            _characterCanvas.DOFade(0, 1);
            // ���¼�����������
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
