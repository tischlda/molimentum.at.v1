<%@ Control Language="C#" Inherits="ViewUserControl<IPagedList>" %>
<%=Html.ActionLink("<<", null, new { page = 1}) %>
<%=Html.ActionLink("<", null, new { page = Math.Max(1, Model.Page - 1) })%>
<%=Model.Page %> / <%=Model.Pages %>
<%=Html.ActionLink(">", null, new { page = Math.Min(Model.Pages, Model.Page + 1) })%>
<%=Html.ActionLink(">>", null, new { page = Model.Pages })%>