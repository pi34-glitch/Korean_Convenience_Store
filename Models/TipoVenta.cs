namespace Korean_Convenience_Store.Models
{
    /// <summary>
    /// Modalidad de venta de un producto:
    /// - Unidad: producto empaquetado que se vende por pieza (ramen, bebidas, snacks).
    /// - PesoGramos: producto preparado/autoservicio que se cobra por peso en gramos.
    /// </summary>
    public enum TipoVenta
    {
        Unidad,
        PesoGramos
    }
}