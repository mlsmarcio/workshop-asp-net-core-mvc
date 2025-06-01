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

        // VERSÃO COM PAGINAÇÃO
        public async Task<(List<SalesRecord> Records, int TotalCount)> FindByDatePageAsync(DateTime? minDate, DateTime? maxDate, int pageNumber, int pageSize)  // ? para argumentos opcionais
        {
            var result = _context.SalesRecord.AsQueryable();

            if (minDate.HasValue)
                result = result.Where(x => x.Date >= minDate.Value);

            if (maxDate.HasValue)
                result = result.Where(x => x.Date <= maxDate.Value);

            var totalCount = await result.CountAsync();

            var records = await result
                .Include(x => x.Seller)
                .ThenInclude(s => s.Department)
                //.OrderByDescending(x => x.Date)
                .OrderBy(x => x.Date)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (records, totalCount);
        }

        //  VERSÃO SEM PAGINAÇÃO
        public async Task<List<SalesRecord>> FindByDateAsync(DateTime? minDate, DateTime? maxDate)
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

        }

        // VERSÃO SEM PAGINAÇÃO
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

        // VERSÃO COM PAGINAÇÃO
        public async Task<List<IGrouping<Department, SalesRecord>>> FindByDateGroupingPagedAsync(
            DateTime? minDate, DateTime? maxDate, int skip, int take)
        {
            var query = _context.SalesRecord
               .Include(x => x.Seller)
               .ThenInclude(s => s.Department)
               .AsQueryable();

            if (minDate.HasValue)
                query = query.Where(x => x.Date >= minDate.Value);
            if (maxDate.HasValue)
                query = query.Where(x => x.Date <= maxDate.Value);

            var all = await query
                .OrderBy(x => x.Seller.Department.Name)
                .ThenBy(x => x.Date)
                .ToListAsync();

            var allGrouped = all
                .GroupBy(x => x.Seller.Department)
                .ToList();

            var pagedItems = new List<SalesRecord>();
            int skipped = 0;
            foreach (var group in allGrouped)
            {
                foreach (var item in group)
                {
                    if (skipped < skip)
                    {
                        skipped++;
                        continue;
                    }

                    if (pagedItems.Count >= take)
                        break;

                    pagedItems.Add(item);
                }

                if (pagedItems.Count >= take)
                    break;
            }

            // Agrupar os itens que foram paginados
            var result = pagedItems
                .GroupBy(x => x.Seller.Department)
                .ToList();
            return result;
        }

    }
}
