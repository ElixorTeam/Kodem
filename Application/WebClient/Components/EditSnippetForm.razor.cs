using Blazored.Toast.Services;
using Codem.Api.Controllers;
using Mapster;
using Microsoft.AspNetCore.Components;
using WebClient.Components.CodeEditor;
using Сodem.Shared.Models;
using WebClient.Models;
using Сodem.Shared.Dtos.File;
using Сodem.Shared.Dtos.Snippet;

namespace WebClient.Components;

public sealed partial class EditSnippetForm : ComponentBase
{
    [Inject] private IToastService ToastService { get; set; } = null!;
    [Inject] private SnippetController SnippetController { get; set; } = null!;
    [Parameter, EditorRequired] public CodeSnippet Model { get; set; } = null!;
    [Parameter, EditorRequired] public CodeFileManager CodeFileManager { get; set; } = null!;
    [Parameter, EditorRequired] public Guid SnippetId { get; set; }
    
    [Parameter] public EventCallback<string> ActiveLanguageChanged { get; set; }
    private List<ValueTypeModel<TimeSpan>> ExpireTimeList { get; } = new()
    {
        new ValueTypeModel<TimeSpan>("Never", TimeSpan.FromDays(365)),
        new ValueTypeModel<TimeSpan>("1 hour", TimeSpan.FromHours(1)),
        new ValueTypeModel<TimeSpan>("1 day", TimeSpan.FromDays(1)),
        new ValueTypeModel<TimeSpan>("1 week", TimeSpan.FromDays(7)),
        new ValueTypeModel<TimeSpan>("1 month", TimeSpan.FromDays(31))
    };
    
    private List<FileDto> ConvertToFileDto(IList<CodeFile> fileModelList)
    {
        return fileModelList.Select(file => 
            new FileDto { Data = file.Text, Name = file.Title }).ToList();
    }

    private async void HandleSubmit()
    {
        DateTime finalDate = DateTime.Now.Add(Model.ExpireTime);
        IList<CodeFile> files = CodeFileManager.GetAllFiles();
        List<FileDto> fileDtos = files.Adapt<List<FileDto>>();
        SnippetDto snippetDto = new()
        {
            Id = SnippetId,
            Title = Model.Title,
            IsPrivate = Model.IsPrivate,
            Password = Model.Password,
            Files = fileDtos,
        };
        
        try
        {
            await SnippetController.UpdateSnippet(snippetDto);
            ToastService.ShowSuccess("Successfully edited");
        }
        catch
        {
            ToastService.ShowError("Errors in form");
        }
    }
}