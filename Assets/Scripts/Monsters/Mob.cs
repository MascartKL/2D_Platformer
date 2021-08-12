using UnityEngine;
using UnityEngine.UI;

public class Mob : MonoBehaviour
{
	[Header("Parametrs mob")]
	[SerializeField] protected float hp;
	[SerializeField] protected float damage;
	[SerializeField] protected float speed;
	[SerializeField] protected float distanceAgro;
	[SerializeField] protected float distanceAttack;

	//Mob components
	protected Rigidbody2D rb;
    protected bool isLeftScale = false;
    protected Animator anim;

    [Header("Resistance")]
    [SerializeField] protected float physRes;
    [SerializeField] protected float spelRes;

    [Header("UI")]
    [SerializeField] protected Image hpBar;
    [SerializeField] protected Image hpBarEffect;
    [SerializeField] protected float hpSpeed = 0.002f;
    [SerializeField] protected Text valueDamage;
    [SerializeField] protected Canvas canvas;
 

    [Header("Player info")]
    protected GameObject player;
    protected float distanceToPlayer;
    [SerializeField] protected LayerMask playerLayer;

    public delegate void DeathDelegate(GameObject go);

    public static event DeathDelegate Death;

	public void TakeDamage(float damage, bool creat)
    {
        hp -= (damage - damage*(physRes/100));
        hpBar.fillAmount = hp * 0.01f;

        RandomGenerationDamageValue(damage.ToString(), creat);

        if (hp <= 0)
        {
            Destroy(gameObject);
           // Death(this.gameObject);
        }
    }

    protected void HpBarChange()
	{
        if (hpBar.fillAmount < hpBarEffect.fillAmount)
            hpBarEffect.fillAmount -= hpSpeed;
        else
            hpBarEffect.fillAmount = hpBar.fillAmount;
    }

    protected virtual void Persuit()
	{
        AttackFlip();

        float tempVelocity_x = player.transform.position.x - transform.position.x;

        rb.velocity = new Vector2
        (speed * (tempVelocity_x / Mathf.Abs(tempVelocity_x)),
        0);
    }

    protected virtual void Attack()
	{
        AttackFlip();
	}

    protected void Flip()
    {
        Vector3 Scaler = transform.localScale;
        Scaler.x *= -1;
        transform.localScale = Scaler;

        Vector3 CanvasScaler = canvas.transform.localScale;
        CanvasScaler.x *= -1;
        canvas.transform.localScale = CanvasScaler;
    }

    private void AttackFlip()
	{
        if (transform.position.x > player.transform.position.x && isLeftScale == true)
        {
            Flip();
            isLeftScale = false;
        }

        if (transform.position.x < player.transform.position.x && isLeftScale == false)
        {
            Flip();
            isLeftScale = true;
        }
    }

    private void RandomGenerationDamageValue(string damage, bool creat)
	{
        if (!valueDamage.IsActive())
            valueDamage.enabled = true;

        
        if (!creat)
		{
            valueDamage.text = damage.ToString();
            valueDamage.fontSize = Random.Range(60, 80);
            valueDamage.color = Color.red;

            valueDamage.rectTransform.localPosition = new Vector3(Random.Range(-100, 100), Random.Range(100, 200), 0);
            valueDamage.fontSize += 6;
        }
		else
		{
            valueDamage.text = damage.ToString();
            valueDamage.fontSize = Random.Range(100, 120);
            valueDamage.color = Color.yellow;

            valueDamage.rectTransform.localPosition = new Vector3(Random.Range(-100, 100), Random.Range(100, 200), 0);
            valueDamage.fontSize += 6;
        }

        Invoke("disableText", 1f);
    }

    private void disableText()
	{
        if (valueDamage.IsActive())
            valueDamage.enabled = false;
    }
}