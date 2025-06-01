using Microsoft.AspNetCore.Mvc;
using SalesWebMvc.Models.ViewModels;
using SalesWebMvc.Services;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SalesWebMvc.Controllers
{

    public class SalesRecordsController : Controller
    {
        private readonly SalesRecordService _salesRecordService;
        public SalesRecordsController(SalesRecordService salesRecordService)
        {
            _salesRecordService = salesRecordService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> SimpleSearch(DateTime? minDate, DateTime? maxDate, int page = 1)
        {
            if (!minDate.HasValue)
                minDate = new DateTime(DateTime.Now.Year, 1, 1);

            if (!maxDate.HasValue)
                maxDate = DateTime.Now;

            int pageSize = 5;


            ViewData["minDate"] = minDate.Value.ToString("yyyy-MM-dd");
            ViewData["maxDate"] = maxDate.Value.ToString("yyyy-MM-dd");

            var (records, totalCount) = await _salesRecordService.FindByDatePageAsync(minDate, maxDate, page, pageSize);

            var viewModel = new SalesRecordViewModel
            {
                SalesRecords = records,
                CurrentPage = page,
                TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize),
                MinDate = minDate,
                MaxDate = maxDate
            };

            return View(viewModel);
        }

        // VERSÃO SEM PAGINAÇÃO
        //public async Task<IActionResult> SimpleSearch(DateTime? minDate, DateTime? maxDate)
        //{
        //    if (!minDate.HasValue)
        //    {
        //        minDate = new DateTime(DateTime.Now.Year, 1, 1);
        //    }
        //    if (!maxDate.HasValue)
        //    {
        //        maxDate = DateTime.Now;
        //    }

        //    ViewData["minDate"] = minDate.Value.ToString("yyyy-MM-dd");
        //    ViewData["maxDate"] = maxDate.Value.ToString("yyyy-MM-dd");

        //    var result = await _salesRecordService.FindByDateAsync(minDate, maxDate);
        //    return View(result);
        //}


        // VERSÃO SEM SCROLL DINÂMICO
        //public async Task<IActionResult> GroupingSearch(DateTime? minDate, DateTime? maxDate)
        //{
        //    if (!minDate.HasValue)
        //    {
        //        minDate = new DateTime(DateTime.Now.Year, 1, 1);
        //    }
        //    if (!maxDate.HasValue)
        //    {
        //        maxDate = DateTime.Now;
        //    }

        //    ViewData["minDate"] = minDate.Value.ToString("yyyy-MM-dd");
        //    ViewData["maxDate"] = maxDate.Value.ToString("yyyy-MM-dd");

        //    var result = await _salesRecordService.FindByDateGroupingAsync(minDate, maxDate);
        //    return View(result);
        //}

        // ACTION PRINCIPAL: CARREGA O LAYOUT E AS DATAS
        public IActionResult GroupingSearch(DateTime? minDate, DateTime? maxDate)
        {
            if (!minDate.HasValue) minDate = new DateTime(DateTime.Now.Year, 1, 1);
            if (!maxDate.HasValue) maxDate = DateTime.Now;

            ViewData["minDate"] = minDate.Value.ToString("yyyy-MM-dd");
            ViewData["maxDate"] = maxDate.Value.ToString("yyyy-MM-dd");

            return View(); // view vazia, só estrutura, carregará por JS
        }

        // ACTION PARCIAL: CHAMA VIA AJAX
        public async Task<IActionResult> LoadGroupedSales(DateTime? minDate, DateTime? maxDate, int skip = 0, int take = 1)
        {
            var pageGroups = await _salesRecordService.FindByDateGroupingPagedAsync(minDate, maxDate, skip, take);

            ViewData["minDate"] = minDate.Value.ToString("yyyy-MM-dd");
            ViewData["maxDate"] = maxDate.Value.ToString("yyyy-MM-dd");

            // Pega o último grupo carregado para evitar repetição (quando a primeira vez não envia o departamento)
            var previousDepartment = (skip == 0 ?"" :pageGroups.FirstOrDefault()?.Key.Name);
            ViewData["lastDepartment"] = previousDepartment;

            return PartialView("GroupedSalesPartial", pageGroups);
        }

    }
}
