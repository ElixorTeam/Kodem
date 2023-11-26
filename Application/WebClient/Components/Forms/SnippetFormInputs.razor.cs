using Microsoft.AspNetCore.Components;
using WebClient.Models;
using Сodem.Shared.Enums;

namespace WebClient.Components.Forms;

public sealed partial class SnippetFormInputs
{
    [Parameter, EditorRequired] public CodeSnippetModel Model { get; set; } = null!;
    
    private bool IsPasswordVisible { get; set; } = false;
    private static Array VisibilityList { get; } = Enum.GetValues(typeof(SnippetVisibilityEnum));
    private static Array ExpireTimeList { get; } = Enum.GetValues(typeof(SnippetExpiration));

    private void SwitchPasswordVisibility() => IsPasswordVisible = !IsPasswordVisible;
    
    private void HandleVisibilityChangeClearPassword()
    {
        if (Model.Visibility != SnippetVisibilityEnum.ByLink & !string.IsNullOrEmpty(Model.Password))
            Model.Password = string.Empty;
    }
}