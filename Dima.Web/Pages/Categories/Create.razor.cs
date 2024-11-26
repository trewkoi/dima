using Dima.Core.Handlers;
using Dima.Core.Requests.Categories;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Dima.Web.Pages.Categories;

public class CreateCategoryPage : ComponentBase
{
    #region Methods

    public async Task OnValidSubmitAsync()
    {
        IsBusy = true;


        try
        {
            var result = await Handler.CreateAsync(InputModel);
            if (result.IsSuccess)
            {
                if (result.Message != null) 
                    Snackbar.Add(result.Message, Severity.Success);
                NavigationManager.NavigateTo("/categories");
            }
            else
            {
                if (result.Message != null) 
                    Snackbar.Add(result.Message, Severity.Error);
            }
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

    #region Properties

    public bool IsBusy { get; set; }
    public CreateCategoryRequest InputModel { get; set; } = new();

    #endregion

    #region Services

    [Inject] public ICategoryHandler Handler { get; set; } = null!;

    [Inject] public NavigationManager NavigationManager { get; set; } = null!;

    [Inject] public ISnackbar Snackbar { get; set; } = null!;

    #endregion
}