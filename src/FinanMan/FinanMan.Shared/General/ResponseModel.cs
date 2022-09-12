using Microsoft.AspNetCore.Components;
using System.Text;
using System.Text.Json.Serialization;

namespace FinanMan.Shared.General;

public class ResponseModelBase
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public List<string>? ErrorMessages { get; set; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public List<Exception>? Exceptions { get; set; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool WasError => (ErrorMessages?.Any() ?? false) || (Exceptions?.Any() ?? false);
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public int RecordCount { get; set; }

    [JsonIgnore]
    public RenderFragment RenderError => builder =>
    {
        var curElem = 0;
        if (ErrorMessages?.Any() ?? false)
        {
            builder.OpenElement(curElem++, "ul");
            builder.AddAttribute(curElem++, "class", "text-danger");
            foreach (var curErrMsg in ErrorMessages)
            {
                builder.OpenElement(curElem++, "li");
                builder.AddContent(curElem++, curErrMsg);
                builder.CloseElement();
            }
            builder.CloseElement();
        }

        if (Exceptions?.Any() ?? false)
        {
            var curExInd = 0;
            foreach (var curEx in Exceptions)
            {
                builder.OpenElement(curElem++, "div");
                builder.AddAttribute(curElem++, "id", $"ex-{curExInd++}");
                builder.AddAttribute(curElem++, "class", "exception-block");

                builder.OpenElement(curElem++, "p");
                builder.AddAttribute(curElem++, "class", "exception-message");
                builder.AddContent(curElem++, curEx.Message);
                builder.CloseElement();

                builder.OpenElement(curElem++, "pre");
                builder.AddAttribute(curElem++, "class", "stack-trace");
                builder.AddContent(curElem++, curEx.StackTrace);
                builder.CloseElement();

                builder.CloseElement();
            }
        }
    };

    public string AsHtml()
    {
        var outputStr = new StringBuilder();
        if (ErrorMessages?.Any() ?? false)
        {
            outputStr.AppendLine("<ul class=\"text-danger\">");
            foreach (var curErrMsg in ErrorMessages)
            {
                outputStr.AppendLine($"  <li>{curErrMsg}</li>");
            }
            outputStr.AppendLine("</ul>");
        }

        if (Exceptions?.Any() ?? false)
        {
            var curExInd = 0;
            foreach (var curEx in Exceptions)
            {
                outputStr.AppendLine($"<div id=\"ex-{curExInd++}\" class=\"exception-block\">");
                outputStr.AppendLine($"  <p class=\"exception-message\">{curEx.Message}</p>");
                outputStr.AppendLine($"  <pre class=\"stack-trace\">{curEx.StackTrace}</pre>");
                outputStr.AppendLine("</div>");
            }
        }
        return outputStr.ToString();
    }

    public virtual void ClearErrors()
    {
        ErrorMessages = null;
        Exceptions = null;
    }

    public void AddError(string message)
    {
        if (ErrorMessages is null) { ErrorMessages = new List<string>(); }
        ErrorMessages.Add(message);
    }

    public void AddError(Exception ex)
    {
        if (Exceptions is null) { Exceptions = new List<Exception>(); }
        Exceptions.Add(ex);
    }

    public void AddErrors(List<string>? messages)
    {
        if (!(messages?.Any() ?? false)) { return; }
        if (ErrorMessages is null) { ErrorMessages = new List<string>(); }
        ErrorMessages.AddRange(messages);
    }

    public void AddErrors(List<Exception>? exceptions)
    {
        if (!(exceptions?.Any() ?? false)) { return; }
        if (Exceptions is null) { Exceptions = new List<Exception>(); }
        Exceptions.AddRange(exceptions);
    }

}

// ResponseModelBase`1.cs or ResponseModelBase-TKey.cs
public class ResponseModelBase<TKey> : ResponseModelBase
{
    public TKey? RecordId { get; set; }
}

// ResponseModel`1 or ResponseModel-T.cs
public class ResponseModel<T> : ResponseModelBase
  where T : class
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public T? ReturnObject { get; set; }
}

// ResponseModel`2 or ResponseModel-T-TKey.cs
public class ResponseModel<T, TKey> : ResponseModelBase<TKey>
  where T : class
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public T? ReturnObject { get; set; }
}
