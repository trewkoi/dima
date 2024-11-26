using Dima.Core.Common.Extensions;
using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Transactions;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Dima.Web.Pages.Transactions;

public class ListTransactionsPage : ComponentBase
{
    #region Properties

    public bool IsBusy { get; set; }
    public List<Transaction> Transactions { get; set; } = [];
    public string SearchTerm { get; set; } = string.Empty;
    public int CurrentYear { get; set; } = DateTime.Today.Year;
    public int CurrentMonth { get; set; } = DateTime.Today.Month;
    public int[] Tears { get; set; } =
    {
        DateTime.Now.Year,
        DateTime.Now.AddYears(-1).Year,
        DateTime.Now.AddYears(-2).Year,
        DateTime.Now.AddYears(-3).Year
    };

    #endregion

    #region Services
    
    [Inject]
    public ISnackbar Snackbar { get; set; } = null!;
    
    [Inject]
    public IDialogService DialogService { get; set; } = null!;
    
    [Inject]
    public ITransactionHandler Handler { get; set; } = null!;
    
    #endregion

    
    #region Private Methods

    private async Task GetTransactions()
    {
        IsBusy = true;

        try
        {
            var request = new GetTransactionsByPeriodRequest
            {
                StartDate = DateTime.Today.GetFirstDay(CurrentYear, CurrentMonth),
                EndDate = DateTime.Now.GetFirstDay(CurrentYear, CurrentMonth),
                PageNumber = 1,
                PageSize = 1000,
            };
            var result = await Handler.GetByPeriodAsync(request);
            
            if (result is { IsSuccess: true })
                Transactions = result.Data ?? [];
        }
        catch (Exception ex)
        {
            Snackbar.Add(ex.Message, Severity.Error);
        }
        finally
        {
            IsBusy = false;
        }
    }
    
    #endregion
    
    #region Public Methods

    public Func<Transaction, bool> Filter => transaction =>
    {
        if (string.IsNullOrEmpty(SearchTerm))
            return true;

        return transaction.Id.ToString().Contains(SearchTerm, StringComparison.OrdinalIgnoreCase)
               || transaction.Title.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase);
    };
    
    #endregion
    
    #region Overrides

    protected override async Task OnInitializedAsync() 
        => await GetTransactions();

    #endregion
}