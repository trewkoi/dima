using Dima.Core.Handlers;
using Dima.Core.Requests.Categories;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Dima.Web.Pages.Categories;

public partial class EditCategoryPage : ComponentBase
{
    #region Properties

    public bool IsBusy { get; set; } = false;
    public UpdateCategoryRequest InputModel { get; set; } = new();
    
    #endregion
    
    #region Parameters
    
    [Parameter]
    public string Id { get; set; } = string.Empty;
    
    #endregion
    
    #region Services
    
    [Inject]
    public NavigationManager NavigationManager { get; set; } = null!;
    
    [Inject]
    public ICategoryHandler Handler { get; set; } = null!;
    
    [Inject]
    public ISnackbar Snackbar { get; set; } = null!;
    
    #endregion
    
    #region Overrides

    protected override async Task OnInitializedAsync()
    {
        GetCategoryByIdRequest? request = null;
        
        try
        {
            request = new GetCategoryByIdRequest { Id = long.Parse(Id) };
        }
        catch (Exception ex)
        {
            Snackbar.Add(ex.Message, Severity.Error);
        }
        
        if (request is null)
            return;

        IsBusy = true;
        
        try
        {
            var result = await Handler.GetByIdAsync(request);

            if (result is { IsSuccess: true, Data: not null })
                InputModel = new UpdateCategoryRequest
                {
                    Id = result.Data.Id,
                    Title = result.Data.Title,
                    Description = result.Data.Description,
                };
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
    
    #region Methods

    public async Task OnValidSubmitAsync()
    {
        IsBusy = true;

        try
        {
            var result = await Handler.UpdateAsync(InputModel);
            if (result is { IsSuccess: true })
            {
                if (result.Message != null) 
                    Snackbar.Add(result.Message, Severity.Success);
                NavigationManager.NavigateTo("/categories");
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
    
}