using Microsoft.EntityFrameworkCore;
using Online_Shop.Dtos;

namespace Online_Shop.Utilities
{
    public class PaginatedList<T> : List<T>
    {
        public int PaginaActual { get; private set; }

        public int PaginasTotales { get; private set; }

        public int Inicio { get; private set; }

        public int Final { get; private set; }

        public PaginatedList(List<T> items, int totalRegistros, int paginaActual, int cantidadRegistros)
        {
            PaginaActual = paginaActual;
            PaginasTotales = (int)Math.Ceiling(totalRegistros / (double)cantidadRegistros);

            (Inicio, Final) = GetBounds(PaginaActual, PaginasTotales);

            this.AddRange(items);
        }

        public bool PaginasAnteriores => PaginaActual > 1;
        public bool PaginasSiguientes => PaginaActual < PaginasTotales;

        public static async Task<PaginatedList<T>> Paginar(IQueryable<T> datos, int paginaActual, int cantidadRegistros)
        {
            var totalRegistros = await datos.CountAsync();
            var items = await datos.Skip((paginaActual - 1) * cantidadRegistros).Take(cantidadRegistros).ToListAsync();
            return new PaginatedList<T>(items, totalRegistros, paginaActual, cantidadRegistros);
        }

        public static (int, int) GetBounds(int paginaActual, int paginasTotales)
        {
            int inicio = paginaActual;
            int final = paginasTotales;

            if (paginasTotales >= 5)
            {
                if (paginaActual == 1)
                {
                    inicio = 1;
                }
                else if (paginaActual == 2)
                {
                    inicio -= 1;
                }
                else if (paginaActual == paginasTotales)
                {
                    inicio -= 4;
                }
                else if (paginaActual == (paginasTotales - 1))
                {
                    inicio -= 3;
                }
                else
                {
                    inicio -= 2;
                }
                final = inicio + 4;
            }
            else
            {
                inicio = 1;
            }

            return (inicio, final);
        }
    }
}
