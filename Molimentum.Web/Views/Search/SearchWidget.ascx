<%@ Control Language="C#" Inherits="ViewUserControl<IEnumerable<TagSummary>>" %>
<%
    var min = Model.Min(tag => tag.Count);
    var max = Model.Max(tag => tag.Count);
%>
<div class="widget">
    <form action="<%=ResolveClientUrl("~/Search") %>" id="cse-search-box">
      <div>
        <input type="hidden" name="cx" value="partner-pub-3483513874676139:j765bgmm30q" />
        <input type="hidden" name="cof" value="FORID:9" />
        <input type="hidden" name="ie" value="ISO-8859-1" />
        <input type="text" name="q" size="20" />
        <input type="submit" name="sa" value="Suche" />
      </div>
    </form>
    <script type="text/javascript" src="/cse/brand?form=cse-search-box&amp;lang=de"></script>

    <div>
<%  foreach (var tag in Model) { %>
    <% var s = Math.Log(tag.Count, max) * 170 + 80; // Math.Max((250 * (tag.Count - min)) / (max - min), 80); %>
    <%=Html.ActionLink(tag.Tag, "Query", "Posts", new { tag = tag.Tag }, new { style = "font-size: " + s + "%"} )%>
<%  } %>
    </div>
</div>