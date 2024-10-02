using System.ComponentModel.DataAnnotations;

namespace FinanMan.BlazorUi.Services;

public enum Operator
{
    [Display(Name = "+")]
    Add,
    [Display(Name = "-")]
    Subtract,
    [Display(Name = "*")]
    Multiply,
    [Display(Name = "/")]
    Divide
}