using System.Collections.Generic;
using CapaDatos;
using CapaEntidad;

namespace CapaNegocio
{
    public class InventarioNegocio
    {
        private InventarioDatos objetoDatos = new InventarioDatos();

        public bool DescontarStock(int idProducto, int cantidad)
        {
            return objetoDatos.DescontarStock(idProducto, cantidad);
        }

        public List<Inventario> Listar()
        {
            List<Inventario> lista = objetoDatos.Listar();

            foreach (Inventario inventario in lista)
            {
                if (inventario.StockActual < inventario.StockMinimo)
                {
                    inventario.Estado = "Stock bajo";
                }
                else
                {
                    inventario.Estado = "Normal";
                }
            }

            return lista;
        }

        public bool Registrar(Inventario inventario)
        {
            return objetoDatos.Registrar(inventario);
        }

        public bool Editar(Inventario inventario)
        {
            return objetoDatos.Editar(inventario);
        }

        public bool Eliminar(int idInventario)
        {
            return objetoDatos.Eliminar(idInventario);
        }
    }
}