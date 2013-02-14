<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Molimentum.Model.PostComment>" %>
<fieldset>
    <div class="editor-label">
        <%: Html.LabelFor(m => m.Author) %>:
    </div>
    <div class="editor-field">
        <%: Html.TextBoxFor(m => m.Author, new { size = 30 }) %>
        <%: Html.ValidationMessageFor(m => m.Author, "*") %>
    </div>
    <div class="editor-label">
        <%: Html.LabelFor(m => m.Website) %>:
    </div>
    <div class="editor-field">
        <%: Html.TextBoxFor(m => m.Website, new { size = 30 }) %>
        <%: Html.ValidationMessageFor(m => m.Website, "*") %>
    </div>
    <div class="editor-label">
        <%: Html.LabelFor(m => m.Email) %>:
    </div>
    <div class="editor-field">
        <%: Html.TextBoxFor(m => m.Email, new { size = 30 }) %>
        (wird nicht veröffentlicht)
        <%: Html.ValidationMessageFor(m => m.Email, "*") %>
    </div>
    <div class="editor-label">
        <%: Html.LabelFor(m => m.Title) %>:
    </div>
    <div class="editor-field">
        <%: Html.TextBoxFor(m => m.Title, new { size = 30 }) %>
        <%: Html.ValidationMessageFor(m => m.Title, "*") %>
    </div>
    <div class="editor-label">
        <%: Html.LabelFor(m => m.Body) %>:
    </div>
    <div class="editor-field">
        <%: Html.TextAreaFor(m => m.Body, new { cols = 60, rows = 10 }) %>
        <%: Html.ValidationMessageFor(m => m.Body, "*") %>
    </div>
</fieldset>