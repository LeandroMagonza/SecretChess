using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellCondition
{
    public CellContent cellContent;
    //public CellContent preConditionCellContent;
    // public CellContent endCellContent;
    public bool obligatory;
    public bool endOnCapture;

    public CellCondition(CellContent cellContent, 
        bool obligatory = false,
        bool endOnCapture = false
        )
    {
        this.cellContent = cellContent;
        this.obligatory = obligatory;
        this.endOnCapture = endOnCapture;
    }
    
}

