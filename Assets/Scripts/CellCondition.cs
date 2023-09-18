using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellCondition
{   
    /*  Cell condition representa
     *  el contenido que tiene que tener una celda para poder hacer un movimiento
     *  el movimiento en si
     *  ambas
     *
     * si el cell content es de tipo condicion, de fallar esa condicion se cancelan todos los movimientos de esa capa
     * y no se continua con las capas siguiente
     *
     * si el cell content es de tipo movimiento, se evalua la condicion para esa celda especifica, y si falla la condicion
     * no se agrega el movimiento a esa celda, pero los demas movimientos de la capa no son afectados.
     * A menos que la cell condition este marcada como obligatory, en ese caso funciona como movimiento si pasa la condicion
     * y como condicion si no la pasa, anulando los demas movimientos de la capa y no continuando con la capa siguiente
     *
     * si endOnCapture es verdadero, y la condicion de la casilla es verdadera, se marca la casilla como posible movimiento.
     * pero si es una casilla donde sucede una captura, no se cancela la capa actual pero si las siguientes.
     * Sirve para las piezas que cortan su movimiento al encontrar una pieza del rival y no la pueden "saltar", como por ejemplo la torre
     * que puede moverse en linea recta hasta encontrar una pieza, y las casillas de atras, que serian las proximas capas, no son validas
     * para el movimiento
     */ 
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

