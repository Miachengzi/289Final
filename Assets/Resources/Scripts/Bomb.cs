using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bomb : MonoBehaviour
{
    #region 
    [Header("Explosion")]
    private Animator animator;
    public GameObject exlposionEffect1;
    public GameObject exlposionEffect2;

    [SerializeField] private float bombRadius;
    [SerializeField] private float bombDamage;
    public LayerMask layerMask;
    #endregion

    private void Start()
    {
        Physics2D.queriesStartInColliders = false;

        animator = GetComponent<Animator>();

        StartCoroutine(Boom());
    }

    IEnumerator Boom()
    {
        yield return new WaitForSeconds(1.2f);
        animator.SetTrigger("Active");
    }

    public void Explosion()
    {
        Instantiate(exlposionEffect1, transform.position, Quaternion.identity);//炸弹特效 1
        Instantiate(exlposionEffect2, transform.position, Quaternion.identity);//炸弹特效 2

        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, bombRadius, layerMask);

        foreach (Collider2D _collider in colliders)
        {
            ITakenDamage damagable = _collider.GetComponent<ITakenDamage>();

            if (damagable != null)
            {
                damagable.TakenDamage(bombDamage);
            }

            Debug.Log(_collider.gameObject.name);
        }

        Destroy(gameObject);//MARKER 销毁炸弹本身
        FindObjectOfType<CameraController>().ScreenShake(0.75f);//相机震动
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, bombRadius);
        Gizmos.color = Color.red;
    }
}
