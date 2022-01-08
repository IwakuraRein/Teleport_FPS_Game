using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelCreateandDestroy : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject Parent;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InstantiatePanel(GameObject prefab)
    {
        GameObject panel;
        panel=Instantiate(prefab, Parent.transform);
        StartCoroutine(PanelCoroutine(panel));
        
    }
    IEnumerator PanelCoroutine(GameObject panel)
    {
        //yield on a new YieldInstruction that waits for 5 seconds.
        Animator animator = panel.GetComponent<Animator>();
        yield return new WaitForSeconds(1f);
        animator.SetBool("Open", true);
    }

    public void DestroyPanel(GameObject panel)
    {
        Destroy(panel, 4);

    }
}
