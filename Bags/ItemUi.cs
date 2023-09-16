using UnityEngine;
using UnityEngine.UI;

public class ItemUi : MonoBehaviour
{
    public int amount;
    //public int id;
    //public Item.ItemType type;
    public Item item;
    public float scaleSpeed = 4f;

    private float targetScale = 1f;
    private Vector3 animationScale = new Vector3(1.4f, 1.4f, 1.4f);
    private Image image;
    private Text text;

    public Image GetImage
    {
        get{
            if (image == null) image = GetComponent<Image>();
            return image;
        }
    }
    public Text GetText
    {
        get
        {
            if (text == null) text = GetComponentInChildren<Text>();
            return text;
        }
    }

    /// <summary>
    /// 设置item
    /// </summary>
    public void SetItem(Item item, int amount = 1)
    {
        this.item = item;
        this.amount = amount;
        GetImage.sprite = Resources.Load<Sprite>(item.Sprite);
        if (amount == 1) GetText.text = "";
        else GetText.text = amount.ToString();
        transform.localScale = animationScale;
    }

    /// <summary>
    /// ui界面上数量增加
    /// </summary>
    public void AddItem(int amount = 1, bool isBaoLiu = false) 
    {
        this.amount += amount;
        if (this.amount > 1) GetText.text = this.amount.ToString();
        else if (this.amount == 0 && !isBaoLiu) Destroy(this.gameObject);
        else GetText.text = "";
        transform.localScale = animationScale;
    }

    public void Show(Item item, int amount)
    {
        SetItem(item, amount);
        gameObject.SetActive(true);
    }

    public void Hide() 
    { 
        gameObject.SetActive(false); 
    }

    public void SetPosition(Vector3 v3)
    {
        transform.position = v3;
    }

    private void Update()
    {
        if (transform.localScale.x != targetScale)
        {
            float scale = Mathf.Lerp(transform.localScale.x, targetScale, scaleSpeed * Time.deltaTime);
            transform.localScale = new Vector3(scale, scale, scale);
            if (Mathf.Abs(transform.localScale.x - targetScale) <= 0.02f)
                transform.localScale = new Vector3(targetScale, targetScale, targetScale);
        }
    }

}
