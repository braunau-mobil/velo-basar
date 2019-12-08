﻿using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using BraunauMobil.VeloBasar.Logic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BraunauMobil.VeloBasar.Pages.Acceptances
{
    public class AcceptParameter
    {
        public int SellerId { get; set; }
    }
    public class AcceptModel : PageModel
    {
        private readonly IVeloContext _context;
        private readonly ITransactionContext _transactionContext;
        private readonly IProductContext _productContext;


        public AcceptModel(IVeloContext veloContext, ITransactionContext transactionContext, IProductContext productContext)
        {
            _context = veloContext;
            _transactionContext = transactionContext;
            _productContext = productContext;
        }

        public async Task<IActionResult> OnGetAsync(AcceptParameter parameter)
        {
            Contract.Requires(parameter != null);

            var products = Request.Cookies.GetAcceptanceProducts();
            _productContext.AttachRelations(products);
            var acceptance = await _transactionContext.AcceptProductsAsync(_context.Basar, parameter.SellerId, products);

            Response.Cookies.ClearAcceptanceProducts();
            return this.RedirectToPage<DetailsModel>(new DetailsParameter { AcceptanceId = acceptance.Id, OpenDocument = true, ShowSuccess = true });
        }
    }
}