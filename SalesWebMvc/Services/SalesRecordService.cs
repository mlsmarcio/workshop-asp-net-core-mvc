using Microsoft.EntityFrameworkCore;
using SalesWebMvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesWebMvc.Services
{
    public class SalesRecordService
    {
        private readonly SalesWebMvcContext _context;

        public SalesRecordService(SalesWebMvcContext context)
        {
            _context = context;
        }

        public async Task<List<SalesRecord>> FindByDateAsync(DateTime? minDate, DateTime? maxDate)  // ? para argumentos opcionais
        {
            // CONSTRUINDO UM OBJETO IQueryable - permite construir as consultas em cima dele, apartir do DBContext usando link
            var result = from obj in _context.SalesRecord select obj;

            // SE A DATA MINIMA FOI INFORMADA
            if (minDate.HasValue)
            {
                result = result.Where(x => x.Date >= minDate.Value);
            }
            if (maxDate.HasValue)
            {
                result = result.Where(x => x.Date <= maxDate.Value);
            }

            return await result
                .Include(x => x.Seller) // faz o join com a tabela de vendedores
                .Include(x => x.Seller.Department) // faz o join com a tabela de departamentos
                .OrderByDescending(x => x.Date) // ORDENA POR DATA DECRESCENTE
                .ToListAsync();
                //.ToList();
        }

        public async Task<List<IGrouping<Department, SalesRecord>>> FindByDateGroupingAsync(DateTime? minDate, DateTime? maxDate)  // ? para argumentos opcionais
        {
            // CONSTRUINDO UM OBJETO IQueryable - permite construir as consultas em cima dele, apartir do DBContext usando link
            var result = from obj in _context.SalesRecord select obj;

            // SE A DATA MINIMA FOI INFORMADA
            if (minDate.HasValue)
            {
                result = result.Where(x => x.Date >= minDate.Value);
            }
            if (maxDate.HasValue)
            {
                result = result.Where(x => x.Date <= maxDate.Value);
            }

            return await result
                .Include(x => x.Seller) // faz o join com a tabela de vendedores
                .Include(x => x.Seller.Department) // faz o join com a tabela de departamentos
                .OrderByDescending(x => x.Date) // ORDENA POR DATA DECRESCENTE
                .GroupBy(x => x.Seller.Department)  // Agrupa os resultados - mudando o tipo de retorno para IGrouping
                .ToListAsync();
            //.ToList();
        }
    }
}
