using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Row : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    // private int? rowNumber = null;
    private int rowNumber;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
      public void SetRowNumber(int rowNumber){
        // if (rowNumber == null)
        // {
            this.rowNumber = rowNumber;
        // }
    }
}
