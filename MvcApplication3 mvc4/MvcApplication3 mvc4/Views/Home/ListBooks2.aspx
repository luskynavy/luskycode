<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<IEnumerable<MvcApplication3_mvc4.Book>>" %>
<%@ Import namespace="MvcApplication3_mvc4.Resources" %>
<!DOCTYPE html>

<html>
<head runat="server">
    <meta name="viewport" content="width=device-width" />
    <title><%=Resources.BooksListOld%></title>
</head>
<body>
    <p>
        <%: Html.ActionLink(Resources.CreateNew, "Create") %>
    </p>
    <table>
        <tr>
            <th>
                <%: Html.DisplayNameFor(model => model.name) %>
            </th>
            <th>
                <%: Html.DisplayNameFor(model => model.note) %>
            </th>
            <th></th>
        </tr>
    
    <% foreach (var item in Model) { %>
        <tr>
            <td>
                <%: Html.DisplayFor(modelItem => item.name) %>
            </td>
            <td>
                <%: Html.DisplayFor(modelItem => item.note) %>
            </td>
            <td>
                <%: Html.ActionLink(Resources.Edit, "EditBook", new { id = item.id })%> |
                <%: Html.ActionLink(Resources.Details, "DetailsBook", new { id = item.id })%> |
                <%: Html.ActionLink(Resources.Delete, "DeleteBook", new { id = item.id })%>
            </td>
        </tr>
    <% } %>
    
    </table>
</body>
</html>
