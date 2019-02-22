<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<IEnumerable<MvcApplication3_mvc4.Book>>" %>

<!DOCTYPE html>

<html>
<head runat="server">
    <meta name="viewport" content="width=device-width" />
    <title>ListBooks2</title>
</head>
<body>
    <p>
        <%: Html.ActionLink("Create New", "Create") %>
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
                <%: Html.ActionLink("Edit", "EditBook", new { id = item.id })%> |
                <%: Html.ActionLink("Details", "DetailsBook", new { id = item.id })%> |
                <%: Html.ActionLink("Delete", "DeleteBook", new { id = item.id })%>
            </td>
        </tr>
    <% } %>
    
    </table>
</body>
</html>
